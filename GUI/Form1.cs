﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientNSP;

namespace GUI
{
    public partial class Form1 : Form
    {
        Client client = new Client();
        public Form1()
        {
            InitializeComponent();
            
            client.init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.sentMessage("HEllo");
        }
    }
}
