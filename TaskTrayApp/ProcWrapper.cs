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

    public interface ITaskTrayWrapper
    {
        public ToolStripMenuItem MenuItem();
        public void SetUp();
        public void TearDown();
    }

    public class TaskTrayFormContainer
    {

        private readonly ReadOnlyCollection<ITaskTrayWrapper> forms;

        public TaskTrayFormContainer(params ITaskTrayWrapper[] source)
        {
            if (0 == source.Length)
            {
                throw new ArgumentException("Pass at least 1 item");
            }
            forms = new ReadOnlyCollection<ITaskTrayWrapper>(source.ToList());
        }

        public IEnumerable<ToolStripMenuItem> MenuItem()
        {
            return forms.Select(x => x.MenuItem());
        }

        public void SetUp()
        {
            foreach (var f in forms)
            {
                f.SetUp();
            }
        }

        public void TearDown()
        {
            foreach (var f in forms)
            {
                f.TearDown();
            }
        }

    }

}
