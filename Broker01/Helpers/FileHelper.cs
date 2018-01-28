using System;
using System.IO;

namespace BrokerAlgo.Helpers
{
    class FileHelper
    {
        public static string GetAppdataFolder()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BrokerAlgo");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
    }
}
