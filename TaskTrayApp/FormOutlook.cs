/*******************************************************************************
 * Copyright (c) 2020 m2enu
 * Released under the MIT License
 * https://github.com/m2enu/TaskTrayApp/blob/master/LICENSE.txt
 ******************************************************************************/
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace TaskTrayApp
{
    public partial class FormOutlook : ITaskTrayWrapper
    {

        private readonly OutlookConfig config = new OutlookConfig();

        public ToolStripMenuItem MenuItem()
        {
            var topMenu = new ToolStripMenuItem
            {
                Text = "Outlook",
            };
            foreach (var item in config.Items)
            {
                var subMenu = new ToolStripMenuItem
                {
                    Text = item.Key,
                    TextAlign = ContentAlignment.MiddleLeft,
                };
                subMenu.Click += new EventHandler(OnClicked);
                topMenu.DropDownItems.Add(subMenu);
            }
            return topMenu;

        }

        public void SetUp()
        {
            config.Load();
        }

        public void TearDown()
        {
        }

        public void OnClicked(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            var item = config.Items[menu.Text];
            item.CreateNewMail();
        }
    }

    public class OutlookConfig
    {

        public Dictionary<string, OutlookConfigItem> Items { get; set; }

        public bool Load()
        {
            var ret = Load("config/outlook.json");
            var cfg = ret.Item2;
            this.Items = cfg.Items;
            return ret.Item1;
        }

        private static Tuple<bool, OutlookConfig> Load(string filename)
        {
            var jsonStr = File.ReadAllText(filename);
            var cfg = JsonSerializer.Deserialize<OutlookConfig>(jsonStr);
            return Tuple.Create(false, cfg);
        }
    }

    public class OutlookConfigItem
    {
        public IList<string> To { get; set; }
        public IList<string> Cc { get; set; }
        public string Subject { get; set; }
        public IList<string> Body { get; set; }

        public void CreateNewMail()
        {
            var app = new Outlook.Application();
            if (!(app.CreateItem(OlItemType.olMailItem) is MailItem mail))
            {
                return;
            }
            var to = mail.Recipients.Add(string.Join("; ", this.To));
            to.Type = (int)OlMailRecipientType.olTo;
            var cc = mail.Recipients.Add(string.Join("; ", this.Cc));
            cc.Type = (int)OlMailRecipientType.olCC;
            mail.Recipients.ResolveAll();
            mail.Subject = ParseCommandString(this.Subject);
            mail.Body = ParseCommandString(string.Join("\r\n", this.Body));
            mail.Display(false);
        }

        public static string ParseCommandString(string msg)
        {
            // TODO: Only ${DATE} command is available.
            var _now = DateTime.Now;
            var ret = msg.Replace("${DATE}", _now.ToString("MM/dd"));
            return ret;
        }

    }
}
