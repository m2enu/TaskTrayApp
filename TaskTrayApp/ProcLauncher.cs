/*******************************************************************************
 * Copyright (c) 2020 m2enu
 * Released under the MIT License
 * https://github.com/m2enu/TaskTrayApp/blob/master/LICENSE.txt
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TaskTrayApp
{
    public class LauncherConfig : IProcConfig
    {

        public Dictionary<string, LauncherConfigItem> Launcher { get; set; }
            = new Dictionary<string, LauncherConfigItem>();

        public void Load()
        {
            var ret = JsonLoader.FromFile<LauncherConfig>("config/Launcher.json");
            this.Launcher = ret.Launcher;
        }

        public ToolStripMenuItem MenuItem()
        {
            var topMenu = new ToolStripMenuItem
            {
                Text = "Launcher",
            };
            topMenu.DropDownItems.AddRange(this.Launcher.Values
                .Select(x => new ProcToolStripMenuItem<LauncherConfigItem>(x))
                .ToArray());
            return topMenu;
        }

    }

    public class LauncherConfigItem : IProcConfigItem
    {

        public string Title { get; set; } = "";
        public string FileName { get; set; } = "";
        public IList<string> Arguments { get; set; } = new List<string>();

        public string MenuText
        {
            get
            {
                return this.Title;
            }
        }

        public void Execute()
        {
            var arg = string.Join(" ", this.Arguments);
            try
            {
                Process.Start(this.FileName, arg);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }

}
