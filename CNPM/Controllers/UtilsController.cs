using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CNPM.Utilities;

namespace CNPM.Controllers
{
    public class UtilsController : Controller
    {
        public FileResult Download(string file)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Utils.GetAbsolutePath(file));
            var response = new FileContentResult(fileBytes, "application/octet-stream");
            response.FileDownloadName = file;
            return response;
        }

        /// <summary>
        /// download an detail
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public FileResult Detail(string file)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Utils.GetAbsoluteDetailPath(file));
            var response = new FileContentResult(fileBytes, "application/octet-stream");
            response.FileDownloadName = file;
            return response;
        }

    }
}