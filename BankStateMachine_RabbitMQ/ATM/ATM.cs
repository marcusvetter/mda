using System;
using System.Text;
using System.Windows.Forms;
using Common;

namespace ATM
{
    internal class ATM
    {
        private readonly Sender _bankSender;
        private readonly Sender _atmSender;
        private readonly Receiver _receiver;
        public string BankHostName = "localhost";
        public string BankQueueName = "bankEventQueue";
        public string HostName = "localhost";
        public string QueueName = "atmEventQueue";

        private State _state = State.CardEntry;
        private readonly ATMUI _atmUi;
        private bool _cardValid = true;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new ATM();
        }

        public ATM()
        {
            // Instantiate the sender and receiver
            _receiver = new Receiver(HostName, QueueName);
            _bankSender = new Sender(BankHostName, BankQueueName);
            _atmSender = new Sender(HostName, QueueName);

            _atmUi = new ATMUI(_atmSender);

            //listen for events
            _receiver.OnMessageReceived += HandleEvent;

            //start receiving events
            _receiver.StartReceiving();

            Application.Run(_atmUi);
        }

        // Handle the event
        public void HandleEvent(byte[] message)
        {
            string evt = Encoding.UTF8.GetString(message);
            _atmUi.Print("Received event: " + evt);
            switch (_state)
            {
                case State.CardEntry:
                    if (evt == "cardentered_t")
                    {
                        _cardValid = true;
                        _state = State.PinEntry;
                    } else if (evt == "cardentered_f")
                    {
                        _cardValid = false;
                        _state = State.PinEntry;
                    }
                        
                    break;
                case State.PinEntry:
                    if (evt == "pinentered_t")
                    {
                        if (_cardValid)
                        {
                            _atmUi.Print("Send valid card and valid PIN to bank");
                            _bankSender.SendEvent("verifypin_tt");
                        }
                        else
                        {
                            _atmUi.Print("Send invalid card and valid PIN to bank");
                            _bankSender.SendEvent("verifypin_ft");
                        }
                        _state = State.Verification;
                    }
                    else if (evt == "pinentered_f")
                    {
                        if (_cardValid)
                        {
                            _atmUi.Print("Send valid card and invalid PIN to bank");
                            _bankSender.SendEvent("verifypin_tf");
                        }
                        else
                        {
                            _atmUi.Print("Send invalid card and invalid PIN to bank");
                            _bankSender.SendEvent("verifypin_ff");
                        }
                        _state = State.Verification;
                    }
                    break;
                case State.Verification:
                    switch (evt)
                    {
                        case "reenterpin":
                            _state = State.PinEntry;
                            break;
                        case "abort":
                            _state = State.ReturningCard;
                            _atmUi.Print("Send 'done' to bank");
                            _bankSender.SendEvent("done");
                            _state = State.CardEntry;
                            break;
                        case "pinverified":
                            _state = State.AmountEntry;
                            _atmSender.SendEvent("internal_dispense");
                            break;
                    }
                    break;
                case State.AmountEntry:
                    if (evt == "internal_dispense")
                    {
                        _state = State.Counting;
                        // Counting()
                        _state = State.Dispensing;
                        _state = State.ReturningCard;
                        _atmUi.Print("ATM: Send 'done' to bank");
                        _bankSender.SendEvent("done");
                        _state = State.CardEntry;
                        _atmUi.Print("Dispensing money");
                    }
                    break;
            }
        }
    }


    internal enum State
    {
        CardEntry,
        PinEntry,
        Verification,
        ReturningCard,
        AmountEntry,
        Counting,
        Dispensing
    }
}