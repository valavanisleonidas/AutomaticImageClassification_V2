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
            if (!Directory.Exists(searchFolder))
            {
                throw new ArgumentException("Folder : " + searchFolder + " does not exist.Give valid folder to search");
            }

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
        public static string[] GetFilesFrom(string searchFolder, int filesFromEachSubFolder, string[] filters = null,
            bool isRecursive = true)
        {
            if (!Directory.Exists(searchFolder))
            {
                throw new ArgumentException("Folder : " + searchFolder + " does not exist.Give valid folder to search");
            }

            if (filters == null)
                filters = new[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };

            var filesInDirectory = Directory.GetDirectories(searchFolder);
            var filesFound = new List<string>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var subfolder in filesInDirectory)
            {
                filesFound.AddRange(
                    Directory.GetFiles(subfolder, string.Format("*.{0}", filters), searchOption)
                        .OrderBy(x => Guid.NewGuid())
                        .Take(filesFromEachSubFolder));
            }
            return filesFound.ToArray();
        }

        public static string[] GetSubFolders(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new ArgumentException("Folder : " + path + " does not exist.Give valid folder to search");
            }
            return Directory.GetDirectories(path).Select(a => a.Remove(0, path.Length).Replace("\\", "")).ToArray();
        }

        public static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public static string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        public static Dictionary<string, int> MapCategoriesToNumbers(string path)
        {
            return MapCategoriesToNumbers(GetSubFolders(path));
        }

        public static Dictionary<string, int> MapCategoriesToNumbers(string[] subfolders)
        {
            var categoriesNumbers = new Dictionary<string, int>();
            for (var i = 0; i < subfolders.Length; i++)
            {
                categoriesNumbers.Add(subfolders[i], i + 1);
            }
            return categoriesNumbers;
        }

        //write each line of List into a new line in file
        public static void WriteFile<T>(string fileToWrite, List<T[]> contentList)
        {
            var encoderShouldEmitUtf8Identifier = false;
            using (
                StreamWriter sw = new StreamWriter(File.Open(fileToWrite, FileMode.Create),
                    new UTF8Encoding(encoderShouldEmitUtf8Identifier)))
            {
                foreach (var features in contentList)
                {
                    sw.WriteLine(string.Join(" ", features.Select(p => p.ToString()).ToArray()));
                }
            }
        }

        //write each line of List into a new line in file
        public static void WriteFile<T>(string fileToWrite, List<T> contentList, bool writeInOneLine = false)
        {
            var encoderShouldEmitUtf8Identifier = false;
            using (
                StreamWriter sw = new StreamWriter(File.Open(fileToWrite, FileMode.Create),
                    new UTF8Encoding(encoderShouldEmitUtf8Identifier)))
            {
                var separator = Environment.NewLine;
                if (writeInOneLine)
                {
                    separator = " ";
                }
                sw.Write(string.Join(separator, contentList.Select(p => p.ToString()).ToArray()));
            }
        }

        //appends array to end of line in file or if file does not exists it creates a new file
        public static void WriteAppendFile<T>(string fileToWrite, T[] contentArray)
        {
            var encoderShouldEmitUtf8Identifier = false;
            using (
                StreamWriter sw = new StreamWriter(File.Open(fileToWrite, FileMode.Append),
                    new UTF8Encoding(encoderShouldEmitUtf8Identifier)))
            {
                sw.WriteLine(string.Join(" ", contentArray.Select(p => p.ToString()).ToArray()));
            }
        }

        //appends array to end of line in file or if file does not exists it creates a new file
        //public static void WriteAppendBinaryFile<T>(string fileToWrite, T[] contentArray)
        //{
        //    var encoderShouldEmitUtf8Identifier = false;
        //    using (BinaryWriter sw = new BinaryWriter(File.Open(fileToWrite, FileMode.Append), new UTF8Encoding(encoderShouldEmitUtf8Identifier)))
        //    {
        //        sw.Write(string.Join(" ", contentArray.Select(p => p.ToString()).ToArray()) + "\r\n");
        //    }
        //}

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
                .Select(
                    l =>
                        l.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(i => (T)Convert.ChangeType(i, typeof(T),CultureInfo.CurrentCulture))
                            .ToArray())
                .ToArray();
        }

        public static T[] ReadFileTo1DArray<T>(string path)
        {
            return File.ReadAllLines(path).Select(i => (T)Convert.ChangeType(i, typeof(T))).ToArray();
        }

    }
}