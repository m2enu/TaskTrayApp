/*******************************************************************************
 * Copyright (c) 2020 m2enu
 * Released under the MIT License
 * https://github.com/m2enu/TaskTrayApp/blob/master/LICENSE.txt
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TaskTrayApp
{

    public interface IProcWrapper
    {
        public ToolStripMenuItem MenuItem();
        public void SetUp();
        public void TearDown();
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
