/*******************************************************************************
 * Copyright (c) 2020 m2enu
 * Released under the MIT License
 * https://github.com/m2enu/TaskTrayApp/blob/master/LICENSE.txt
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace TaskTrayApp
{

    public interface IProcWrapper
    {
        ToolStripMenuItem MenuItem();
        void SetUp();
        void TearDown();
    }

    public interface IProcConfig
    {

        void Load();
        ToolStripMenuItem MenuItem();

    }

    public interface IProcConfigItem
    {

        string MenuText { get; }
        void Execute();

    }

    public class ProcBase<T> : IProcWrapper
        where T : IProcConfig, new()
    {

        private readonly T Config = new T();

        public ToolStripMenuItem MenuItem()
        {
            return Config.MenuItem();
        }

        public void SetUp()
        {
            Config.Load();
        }

        public void TearDown()
        {
        }

    }

    public class ProcToolStripMenuItem<T> : ToolStripMenuItem
        where T : IProcConfigItem
    {

        private readonly T Item;

        public ProcToolStripMenuItem(T item)
        {
            this.Item = item;
            this.Text = this.Item.MenuText;
            this.TextAlign = ContentAlignment.MiddleLeft;
            this.Click += new EventHandler(OnClicked);
        }

        private void OnClicked(object sender, EventArgs e)
        {
            this.Item.Execute();
        }

    }

    public static class JsonLoader
    {
        public static T FromFile<T>(string FileName)
            where T: IProcConfig, new()
        {
            try
            {
                var jsonStr = File.ReadAllText(FileName);
                var Config = JsonSerializer.Deserialize<T>(jsonStr);
                return Config;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // TODO: implement procedure when failed deserialize.
            return new T();
        }
    }

    public class ProcContainer
    {

        private readonly ReadOnlyCollection<IProcWrapper> members;

        public ProcContainer(params IProcWrapper[] source)
        {
            if (0 == source.Length)
            {
                throw new ArgumentException("Pass at least 1 item");
            }
            members = new ReadOnlyCollection<IProcWrapper>(source.ToList());
        }

        public IEnumerable<ToolStripMenuItem> MenuItem()
        {
            return members.Select(x => x.MenuItem());
        }

        public void SetUp()
        {
            foreach (var member in members)
            {
                member.SetUp();
            }
        }

        public void TearDown()
        {
            foreach (var member in members)
            {
                member.TearDown();
            }
        }

    }

}
