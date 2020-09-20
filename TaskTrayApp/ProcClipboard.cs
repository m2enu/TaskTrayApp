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
using System.Windows.Forms;

namespace TaskTrayApp
{
    public class ClipboardConfig : IProcConfig
    {

        public IList<string> Clipboard { get; set; } = new List<string>();

        public void Load()
        {
            var ret = JsonLoader.FromFile<ClipboardConfig>("config/Clipboard.json");
            this.Clipboard = ret.Clipboard;
        }

        public ToolStripMenuItem MenuItem()
        {
            var topMenu = new ToolStripMenuItem
            {
                Text = "Clipboard",
            };
            topMenu.DropDownItems.AddRange(this.Clipboard
                .Select(x => new ClipboardConfigItem(x))
                .Select(x => new ProcToolStripMenuItem<ClipboardConfigItem>(x))
                .ToArray());
            return topMenu;
        }

    }

    public class ClipboardConfigItem : IProcConfigItem
    {

        public ClipboardConfigItem(string txt)
        {
            this.Text = txt;
        }

        public string Text { get; set; }

        public string MenuText
        {
            get
            {
                return this.Text;
            }
        }

        public void Execute()
        {
            Clipboard.SetText(this.Text + "\r\n");
        }

    }

}
