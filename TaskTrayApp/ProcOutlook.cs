﻿/*******************************************************************************
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
using Outlook = Microsoft.Office.Interop.Outlook;

namespace TaskTrayApp
{
    public class OutlookConfig : IProcConfig
    {

        public Dictionary<string, OutlookConfigItem> Outlook { get; set; }
            = new Dictionary<string, OutlookConfigItem>();

        public void Load()
        {
            var ret = JsonLoader.FromFile<OutlookConfig>("config/Outlook.json");
            this.Outlook = ret.Outlook;
        }

        public ToolStripMenuItem MenuItem()
        {
            var topMenu = new ToolStripMenuItem
            {
                Text = "Outlook",
            };
            topMenu.DropDownItems.AddRange(this.Outlook.Values
                .Select(x => new ProcToolStripMenuItem<OutlookConfigItem>(x))
                .ToArray());
            return topMenu;
        }

    }

    public class OutlookConfigItem : IProcConfigItem
    {

        public string Title { get; set; } = "";
        public IList<string> To { get; set; } = new List<string>();
        public IList<string> Cc { get; set; } = new List<string>();
        public string Subject { get; set; } = "";
        public IList<string> Body { get; set; } = new List<string>();

        public string MenuText
        {
            get
            {
                return this.Title;
            }
        }

        public void Execute()
        {
            var app = new Outlook.Application();
            if (!(app.CreateItem(Outlook.OlItemType.olMailItem) is Outlook.MailItem mail))
            {
                return;
            }
            mail.To = string.Join("; ", this.To);
            mail.CC = string.Join("; ", this.Cc);
            mail.Recipients.ResolveAll();
            mail.Subject = ParseCommandString(this.Subject);
            mail.Body = ParseCommandString(string.Join("\r\n", this.Body));
            mail.BodyFormat = Outlook.OlBodyFormat.olFormatPlain;
            mail.Display(false);
        }

        protected static string ParseCommandString(string msg)
        {
            // TODO: Only ${DATE} command is available.
            var _now = DateTime.Now;
            var ret = msg.Replace("${DATE}", _now.ToString("MM/dd"));
            return ret;
        }

    }

}
