using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankStateMachine_RabbitMQ
{
    public partial class Output : Form
    {
        public Output()
        {
            InitializeComponent();
        }

        private void Output_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //delegate to post to UI thread
        private delegate void ShowMessageDelegate(string message);

        //Callback for message receive
        public void Print(String message)
        {
            var s = new ShowMessageDelegate(richTextBox1.AppendText);

            this.Invoke(s, message + Environment.NewLine);
        }

    }
}
