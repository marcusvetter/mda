using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;

namespace ATM
{
    public partial class ATMUI : Form
    {
        private readonly Sender _atmSender;

        public ATMUI(Sender atmSender)
        {
            InitializeComponent();

            _atmSender = atmSender;
        }

        //delegate to post to UI thread
        private delegate void ShowMessageDelegate(string message);

        //Callback for message receive
        public void Print(String message)
        {
            var s = new ShowMessageDelegate(richTextBox1.AppendText);

            this.Invoke(s, message + Environment.NewLine);
        }

        private void buttonValidCard_Click(object sender, EventArgs e)
        {
            _atmSender.SendEvent("cardentered_t");
        }

        private void buttonInvalidCard_Click(object sender, EventArgs e)
        {
            _atmSender.SendEvent("cardentered_f");
        }

        private void buttonValidPIN_Click(object sender, EventArgs e)
        {
            _atmSender.SendEvent("pinentered_t");
        }

        private void buttonInvalidPIN_Click(object sender, EventArgs e)
        {
            _atmSender.SendEvent("pinentered_f");
        }
    }
}
