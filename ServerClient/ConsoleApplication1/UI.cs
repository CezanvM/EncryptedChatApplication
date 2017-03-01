using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientNSP;

namespace ConsoleApplication1
{
    public partial class UI : Form
    {
        Client client = new Client();
        public UI()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Text != "")
            {
                client.sentMessage(MessageBox.Text);
            }
        }
    }
}
