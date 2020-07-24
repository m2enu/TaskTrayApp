/*******************************************************************************
 * Copyright (c) 2020 m2enu
 * Released under the MIT License
 * https://github.com/m2enu/TaskTrayApp/blob/master/LICENSE.txt
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskTrayApp
{
    public partial class FormMain : Form
    {

        private readonly TaskTrayFormContainer container =
            new TaskTrayFormContainer(
                new FormClipboard()
            );

        public FormMain()
        {
            InitializeComponent();

            container.SetUp();
            var n = 0;
            foreach (var i in container.MenuItem())
            {
                contextMenuStrip1.Items.Insert(n++, i);
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CloseReason.ApplicationExitCall != e.CloseReason)
            {
                e.Cancel = true;
                this.Visible = false;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            container.TearDown();
            notifyIcon1.Dispose();
            Application.Exit();
        }
    }
}
