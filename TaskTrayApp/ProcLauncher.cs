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
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace TaskTrayApp
{
    public partial class ProcLauncher : IProcWrapper
    {

        public readonly LauncherConfig config = new LauncherConfig();

        public ToolStripMenuItem MenuItem()
        {
            var topMenu = new ToolStripMenuItem
            {
                Text = "Launcher",
            };
            foreach (var item in config.Launcher.Values)
            {
                var subMenu = new ToolStripMenuItemForLauncher(item)
                {
                    Text = item.Title,
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
            var menu = sender as ToolStripMenuItemForLauncher;
            menu.CreateNewProcess();
        }

    }

    public class LauncherConfig
    {

        public Dictionary<string, LauncherConfigItem> Launcher { get; set; }

        public bool Load()
        {
            var ret = Load("config/launcher.json");
            var cfg = ret.Item2;
            this.Launcher = cfg.Launcher;
            return ret.Item1;
        }

        private static Tuple<bool, LauncherConfig> Load(string filename)
        {
            var jsonStr = File.ReadAllText(filename);
            var cfg = JsonSerializer.Deserialize<LauncherConfig>(jsonStr);
            return Tuple.Create(false, cfg);
        }
    }

    public class LauncherConfigItem
    {

        public string Title { get; set; }
        public string FileName { get; set; }
        public IList<string> Arguments { get; set; }

        public void CreateNewProcess()
        {
            var arg = string.Join(" ", this.Arguments);
            Process.Start(this.FileName, arg);
        }

    }

    public class ToolStripMenuItemForLauncher : ToolStripMenuItem
    {

        private readonly LauncherConfigItem launcher;

        public ToolStripMenuItemForLauncher(LauncherConfigItem item)
        {
            this.launcher = item;
        }

        public void CreateNewProcess()
        {
            this.launcher.CreateNewProcess();
        }

    }
}
