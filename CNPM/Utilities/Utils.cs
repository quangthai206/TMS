using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CNPM.Utilities
{
    public class Utils
    {
        public static string GetAbsolutePath(string relativePath)
        {
            return Path.Combine(GetWorkspace(), relativePath);
        }

        public static string GetWorkspace()
        {
            return Config.GetWorkspace();
        }

        public static string GetAbsoluteDetailPath(string relativePath)
        {
            return Path.Combine(Config.GetAbsoluteDetailFolderPath(), relativePath);
        }

        /// <summary>
        /// Save posted file to local, if filename was existed, auto append index to the end
        /// e.g., if postedName = "question.docx", but it was existed, check if name "question1.docx" is available to save with this name
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="directory"></param>
        /// <returns>relative path</returns>
        public static string SaveFile(HttpPostedFileBase postedFile, string directory)
        {
            string fileName = GetAvailableFileName(postedFile.FileName, directory);
            string temp = Path.Combine(directory, fileName);
            postedFile.SaveAs(temp);
            return fileName;
        }

        /// <summary>
        /// if filename was existed, auto append index to the end
        /// e.g., if fileName = "question.docx", but it was existed, check if name "question1.docx" is available
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static string GetAvailableFileName(string fileName, string directory)
        {
            string temp = Path.Combine(directory, fileName);
            if (File.Exists(temp))
            {
                int dotIndex = fileName.LastIndexOf(".");
                string fileNameWithoutExtension = dotIndex < 0 ? fileName : fileName.Substring(0, dotIndex);
                string extension = dotIndex < 0 ? "" : fileName.Substring(dotIndex + 1);
                return GetAvailableFileName(fileNameWithoutExtension, extension, 1, directory);
            }
            else
            {
                return fileName;
            }
        }

        public static string GetAvailableFileName(string fileNameWithoutExtension, string extension, int index, string directory)
        {
            string fileName = fileNameWithoutExtension + index + "." + extension;
            string temp = Path.Combine(directory, fileName);
            if (File.Exists(temp))
                return GetAvailableFileName(fileNameWithoutExtension, extension, index + 1, directory);
            else
                return fileName;
        }
    }
}