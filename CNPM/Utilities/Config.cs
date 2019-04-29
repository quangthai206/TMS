using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CNPM.Utilities
{
    public class Config
    {
        public static string GetWorkspace()
        {
            return "D:\\CNPM\\TMS_Solution";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>relative folder</returns>
        public static string GetDetailFolder()
        {
            return "file_detail";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>absolute answers folder</returns>
        public static string GetAbsoluteDetailFolderPath()
        {
            return Path.Combine(GetWorkspace(), GetDetailFolder());
        }

    }
}