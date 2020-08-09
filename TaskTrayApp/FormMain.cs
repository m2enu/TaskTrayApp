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

        private readonly ProcContainer container = new ProcContainer(
                new ProcBase<ClipboardConfig>(),
                new ProcBase<OutlookConfig>(),
                new ProcBase<LauncherConfig>()
            );

        public FormMain()
        {
            InitializeComponent();

            SetupMenu();
        }

        private void SetupMenu()
        {
            // Remain misc menu (Reload, Exit)
            while (contextMenuStripFile.Items.Count > 2)
            {
                contextMenuStripFile.Items.RemoveAt(0);
            }

            container.SetUp();
            var n = 0;
            foreach (var i in container.MenuItem())
            {
                contextMenuStripFile.Items.Insert(n++, i);
            }
        }

        private void OnFormMainLoad(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void OnFormMainClosing(object sender, FormClosingEventArgs e)
        {
            if (CloseReason.ApplicationExitCall != e.CloseReason)
            {
                e.Cancel = true;
                this.Visible = false;
            }
        }

        private void OnClickedExitMenu(object sender, EventArgs e)
        {
            container.TearDown();
            notifyIcon1.Dispose();
            Application.Exit();
        }

        private void OnClickedUpdateMenu(object sender, EventArgs e)
        {
            SetupMenu();
        }
    }
}
