/*******************************************************************************
 * Copyright (c) 2020 m2enu
 * Released under the MIT License
 * https://github.com/m2enu/TaskTrayApp/blob/master/LICENSE.txt
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace TaskTrayApp
{
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
}
