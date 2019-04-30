using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CNPM.Models;
using CNPM.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CNPM.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        #region attributes
        private AppSignInManager _signInManager;
        private AppUserManager _userManager;
        private TMSdbEntities db = new TMSdbEntities();
        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public AppSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<AppSignInManager>();
            }
            private set { _signInManager = value; }
        }
        public TeacherController()
        {
        }

        public TeacherController(AppUserManager userManager)
        {
            this._userManager = userManager;
        }

        public TeacherController(AppUserManager userManager, AppSignInManager signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        #endregion
        // GET: Teacher
        public ActionResult Index()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            int id = user.id;
            var courses = (from c in db.courses
                           join s in db.subjects on c.subject_id equals s.id
                           where c.teacher_id == id
                           select new CourseViewModel
                           {
                               id = c.id,
                               code = c.code,
                               subject_id = c.subject_id,
                               teacher_id = c.teacher_id.Value,
                               subject_name = s.name
                           }).ToList();
            return View(courses);
        }

        public ActionResult Course(int id)
        {
            var c = db.courses.FirstOrDefault(m => m.id == id);
            ViewBag.CourseCode = c.code;
            ViewBag.CourseName = c.subject.name;
            ViewBag.CourseID = id;
            return View();
        }

        public ActionResult GetTopics(int id)
        {
            var topicByCourse = (from t in db.topics
                                 where t.course_id == id
                                 select new TopicViewModel
                                 {
                                     id = t.id,
                                     course_id = t.course_id,
                                     description = t.description,
                                     name = t.name,
                                     file_detail = t.file_detail
                                 }).ToList();
            return Json(new { data = topicByCourse}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManageTopic(int courseID)
        {
            var c = db.courses.FirstOrDefault(m => m.id == courseID);
            ViewBag.CourseCode = c.code;
            ViewBag.CourseName = c.subject.name;
            ViewBag.courseID = courseID;
            return View();
        }

        [HttpGet]
        public ActionResult AddTopic(int courseID)
        {
                var newTopic = new AddTopicVM();
                newTopic.course_id = courseID;
                return View(newTopic);
        }

        [HttpPost]
        public ActionResult AddTopic(AddTopicVM e)
        {
            if(ModelState.IsValid)
            {
                    var newTopic = new topic
                    {
                        name = e.name,
                        file_detail = e.file_detail == null ? null : SaveFile(e.file_detail),
                        course_id = e.course_id,
                        description = e.description
                    };
                    db.topics.Add(newTopic);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Thêm mới thành công" });
            }
            return Json(new { success = false, message = "Đã có lỗi xảy ra" });
        }
        
        private string SaveFile(HttpPostedFileBase postFile)
        {
            return Utils.SaveFile(postFile, Config.GetAbsoluteDetailFolderPath());
        }

        [HttpPost]
        public ActionResult DeleteTopic(int id)
        {
            var deleteTopic = db.topics.FirstOrDefault(m => m.id == id);
            if (deleteTopic.groups.Count > 0)
            {
                return Json(new { success = false, message = "Không thể xóa đề tài ngay lúc này" });
            }
            else
            {
                db.topics.Remove(deleteTopic);
                db.SaveChanges();
                return Json(new { success = true, message = "Xóa thành công" });
            }
        }

        [HttpGet]
        public ActionResult EditTopic(int id)
        {
            var topic = db.topics.Where(m => m.id == id)
                .Select(t => new TopicViewModel
                {
                    id = id,
                    name = t.name,
                    course_id = t.course_id,
                    description = t.description,
                    file_detail = t.file_detail
                }).FirstOrDefault();
            if (topic == null)
                return View("Error");
            return View(topic);
        }

        [HttpPost]
        public ActionResult EditTopic(TopicViewModel topic, string check)
        {
            if(ModelState.IsValid)
            {
                var old_topic = db.topics.FirstOrDefault(t => t.id == topic.id);

                old_topic.name = topic.name;
                old_topic.description = topic.description;
                var files = Request.Files;
                if (files["file_detail"] != null && !string.IsNullOrEmpty(files["file_detail"].FileName))
                    old_topic.file_detail = SaveFile(files["file_detail"]);
                if(check == "Không có tệp nào được chọn")
                {
                    old_topic.file_detail = null;
                }
                db.SaveChanges();
                return Json(new { success = true, message = "Sửa đổi thành công" });
            }
            return Json(new { success = false, message = "Đã có lỗi xảy ra" });
        }
    }
}