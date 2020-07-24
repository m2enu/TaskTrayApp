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
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace TaskTrayApp
{
    public partial class FormClipboard : Form, ITaskTrayWrapper
    {

        private readonly ClipboardConfig config = new ClipboardConfig();

        public FormClipboard()
        {
            InitializeComponent();
        }

        public ToolStripMenuItem MenuItem()
        {
            var topMenu = new ToolStripMenuItem
            {
                Text = "Clipboard",
            };
            foreach (var text in config.Text)
            {
                var subMenu = new ToolStripMenuItem
                {
                    Text = text,
                    TextAlign = ContentAlignment.MiddleLeft,
                };
                subMenu.Click += new EventHandler(MenuClicked);
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

        public void MenuClicked(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            Clipboard.SetText(menu.Text + "\r\n");
        }
    }

    public class ClipboardConfig
    {

        public IList<string> Text { get; set; }

        public bool Load()
        {
            var ret = LoadInternal("config/clipboard.json");
            var cfg = ret.Item2;
            this.Text = cfg.Text;
            return ret.Item1;
        }

        private static Tuple<bool, ClipboardConfig> LoadInternal(string filename)
        {
            if (!File.Exists(filename))
            {
                var ret = new ClipboardConfig
                {
                    Text = new List<string> { "Config file load failed." },
                };
                return Tuple.Create(true, ret);
            }
            var jsonStr = File.ReadAllText(filename);
            var cfg = JsonSerializer.Deserialize<ClipboardConfig>(jsonStr);
            return Tuple.Create(false, cfg);
        }

    }

}
