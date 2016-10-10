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
        //default for pictures
        public static string[] GetFilesFrom(string searchFolder, string[] filters = null, bool isRecursive = true)
        {
            if (filters == null)
                filters = new[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };

            var filesFound = new List<string>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, string.Format("*.{0}", filter), searchOption));
            }
            return filesFound.ToArray();
        }
        //default for pictures
        public static string[] GetFilesFrom(string searchFolder,int filesFromEachSubFolder, string[] filters = null, bool isRecursive = true)
        {
            if (filters == null)
                filters = new[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };

            var filesInDirectory = Directory.GetDirectories(searchFolder);
            var filesFound = new List<string>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var subfolder in filesInDirectory)
            {
                filesFound.AddRange(Directory.GetFiles(subfolder, string.Format("*.{0}", filters), searchOption).Take(filesFromEachSubFolder));
            }
            return filesFound.ToArray();
        }

        public static void WriteFile<T>(string fileToWrite,List<T[]> contentList )
        {
            var content = "";
            foreach (var feature in contentList)
            {
                content += string.Join(" ", feature);
                content += "\r\n";
            }
            // Write the string to a file.
            File.WriteAllText(fileToWrite, content,Encoding.UTF8);
        }

        public static void WriteFile<T>(string fileToWrite, List<T> contentList)
        {
            var content = "";
            foreach (var feature in contentList)
            {
                content += string.Join(" ", feature);
                content += @"\r\n";
            }
            // Write the string to a file.
            File.WriteAllText(fileToWrite, content, Encoding.UTF8);
        }

        public static List<T[]> ReadFileToListArrayList<T>(string path)
        {
            return ReadFileTo2DArray<T>(path).ToList();
        }

        public static List<T> ReadFileToList<T>(string path)
        {
            return ReadFileTo1DArray<T>(path).ToList();
        }

        public static T[][] ReadFileTo2DArray<T>(string path)
        {
            return File.ReadAllLines(path)
                .Select(l => l.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(i => (T)Convert.ChangeType(i, typeof(T))).ToArray())
                .ToArray();
        }

        public static T[] ReadFileTo1DArray<T>(string path)
        {

            return File.ReadAllLines(path).Select(i => (T)Convert.ChangeType(i, typeof(T))).ToArray();
        }



    }
}
