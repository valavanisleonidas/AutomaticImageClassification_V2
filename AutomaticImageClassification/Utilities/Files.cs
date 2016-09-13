using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Utilities
{
    public class Files
    {
        public static string[] GetFilesFrom(string searchFolder, string[] filters = null, bool isRecursive = true)
        {
            if (filters == null)
                filters = new[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };

            List<string> filesFound = new List<string>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, string.Format("*.{0}", filter), searchOption));
            }
            return filesFound.ToArray();
        }

        public static string[] GetFilesFrom(string searchFolder,int filesFromEachSubFolder, string[] filters = null, bool isRecursive = true)
        {
            if (filters == null)
                filters = new[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };

            string[] filesInDirectory = Directory.GetDirectories(searchFolder);
            List<string> filesFound = new List<string>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (string subfolder in filesInDirectory)
            {
                filesFound.AddRange(Directory.GetFiles(subfolder, string.Format("*.{0}", filters), searchOption).Take(filesFromEachSubFolder));
            }
            return filesFound.ToArray();
        }

        public static void WriteFile(string fileToWrite,List<double[]> contentList )
        {
            string content = "";
            foreach (double[] feature in contentList)
            {
                content += string.Join(" ", feature);
                content += "\r\n";
            }
            // Write the string to a file.
            File.WriteAllText(fileToWrite, content,Encoding.UTF8);
        }

        public static List<double[]> ReadFileToList(string path)
        {
            return ReadFileToArray(path).ToList();
        }

        public static double[][] ReadFileToArray(string path)
        {
            return File.ReadAllLines(path)
                .Select(l => l.Split(new [] { ' '},StringSplitOptions.RemoveEmptyEntries).Select(i => double.Parse(i, CultureInfo.InvariantCulture)).ToArray())
                .ToArray();
            
        }
    }
}
