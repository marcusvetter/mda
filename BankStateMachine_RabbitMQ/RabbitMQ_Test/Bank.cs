using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BankStateMachine_RabbitMQ;
using Common;

namespace BankStateMachine_RabbitMQ
{
    internal class Bank
    {
        public string HostName = "localhost";
        public string QueueName = "bankEventQueue";

        public string AtmHostName = "localhost";
        public string AtmQueueName = "atmEventQueue";

        private readonly Receiver _receiver;
        private readonly Sender _sender;
        private readonly Sender _atmSender;

        private State _state = State.Idle;

        private Thread _orth1Thread;
        private Thread _orth2Thread;
        private readonly VerifyingState _vState;

        public static int NumIncorrect = 0;
        private const int MaxNumIncorrect = 2;
        public static bool  CardValid = true;
        public static bool  PinValid = true;

        private readonly Output _output;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var output = new Output();
            new Bank(output);

            Application.Run(output);
        }

        public Bank(Output output)
        {
            _output = output;

            // Instantiate the verifying state object
            _vState = new VerifyingState();

            // Instantiate the sender and receiver
            _sender = new Sender(HostName, QueueName);
            _receiver = new Receiver(HostName, QueueName);
            _atmSender = new Sender(AtmHostName, AtmQueueName);

            //listen for events
            _receiver.OnMessageReceived += HandleEvent;

            //start receiving events
            _receiver.StartReceiving();
        }

        // Handle the event
        public void HandleEvent(byte[] message)
        {
            var evt = Encoding.UTF8.GetString(message);
            _output.Print("Received event: " + evt);
            switch (_state)
            {
                case State.Idle:
                    // Reset the verifying state properties
                    _vState.CardValid = "unknown";
                    _vState.PinCorrect = "unknown";

                    if (evt == "verifypin_tt")
                    {
                        Bank.CardValid = true;
                        Bank.PinValid = true;
                    }
                    else if (evt == "verifypin_ft")
                    {
                        Bank.CardValid = false;
                        Bank.PinValid = true;
                    }
                    else if (evt == "verifypin_tf")
                    {
                        Bank.CardValid = true;
                        Bank.PinValid = false;
                    }
                    else if (evt == "verifypin_ff")
                    {
                        Bank.CardValid = false;
                        Bank.PinValid = false;
                    }

                    if (evt == "verifypin_tt" || evt == "verifypin_ft" || evt == "verifypin_tf" || evt == "verifypin_ff")
                    {
                        // Start two threads for orthogonal state
                        _orth1Thread = new Thread(new OrthogonalLine1(_sender, _output).Run);
                        _orth2Thread = new Thread(new OrthogonalLine2(_sender, _output).Run);
                        _state = State.Verifying;
                        _orth1Thread.Start();
                        _orth2Thread.Start();
                    }
                    break;
                case State.Verifying:
                    switch (evt)
                    {
                        case "internal_abort":
                            // Kill threads
                            _orth1Thread.Abort();
                            _orth2Thread.Abort();
                            _orth1Thread = null;
                            _orth2Thread = null;
                            _atmSender.SendEvent("abort");
                            _output.Print("Sent 'abort' to atm");
                            _state = State.Idle;
                            break;
                        case "internal_cardvalid":
                            _vState.CardValid = "true";
                            CheckSychronizingPseudoState();
                            break;
                        case "internal_pincorrect":
                            _vState.PinCorrect = "true";
                            CheckSychronizingPseudoState();
                            break;
                        case "internal_pinincorrect":
                            _vState.PinCorrect = "false";
                            CheckSychronizingPseudoState();
                            break;
                    }
                    break;
            }
        }

        private void CheckSychronizingPseudoState()
        {
            if (_vState.CardValid == "true" && _vState.PinCorrect == "true")
            {
                _atmSender.SendEvent("pinverified");
                _output.Print("Sent 'pinverified' to atm");
                _state = State.Idle;
            } else if (_vState.CardValid == "true" && _vState.PinCorrect == "false")
            {
                if (NumIncorrect < MaxNumIncorrect)
                {
                    NumIncorrect++;
                    _atmSender.SendEvent("reenterpin");
                    _output.Print("Sent 'reenterpin' to atm");
                    _state = State.Idle;
                }
                else
                {
                    CardValid = false;
                    _atmSender.SendEvent("abort");
                    _output.Print("Sent 'abort' to atm");
                    _state = State.Idle;
                }
            }
        }
    }


    internal class VerifyingState
    {
        public String CardValid { get; set; }
        public String PinCorrect { get; set; }
    }

    internal class OrthogonalLine1
    {
        private readonly Sender _sender;
        private readonly Output _output;

        public OrthogonalLine1(Sender sender, Output output)
        {
            _sender = sender;
            _output = output;
        }

        public void Run()
        {
            _output.Print("CardProcessor: Verifying card ...");
            Thread.Sleep(2000);

            if (Bank.CardValid)
            {
                _output.Print("CardProcessor: Card valid");
                _sender.SendEvent("internal_cardvalid");
            }
            else
            {
                _output.Print("CardProcessor: Card invalid");
                _sender.SendEvent("internal_abort");
            }
        }
    }

    internal class OrthogonalLine2
    {
        private readonly Sender _sender;
        private readonly Output _output;

        public OrthogonalLine2(Sender sender, Output output)
        {
            _sender = sender;
            _output = output;
        }

        public void Run()
        {
            _output.Print("PINProcessor: Verifying PIN ...");
            if (Bank.PinValid)
            {
                _output.Print("PINProcessor: PIN correct");
                Bank.NumIncorrect = 0;
                _sender.SendEvent("internal_pincorrect");
            }
            else
            {
                _output.Print("PINProcessor: PIN incorrect");
                _sender.SendEvent("internal_pinincorrect");
            }
        }
    }

    internal enum State
    {
        Idle,
        Verifying
    }
}