using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cervine
{
    /// <summary>
    /// SaveGame class responsible for loading and saving game into file
    /// </summary>
    public class SaveGame
    {
        public void LoadScores()
        {
            var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                string highscores = "";
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    highscores += temp.GetString(b);
                }
            }

            throw new NotImplementedException();
        }
    }
}
