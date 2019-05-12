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
    public class StudentController : Controller
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
        public StudentController()
        {
        }

        public StudentController(AppUserManager userManager)
        {
            this._userManager = userManager;
        }

        public StudentController(AppUserManager userManager, AppSignInManager signInManager)
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
            var courses = (from stc in db.student_course
                           join c in db.courses on stc.course_id equals c.id
                           join s in db.subjects on c.subject_id equals s.id
                           where stc.student_id == id
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
            var user = UserManager.FindById(User.Identity.GetUserId());
            int sid = user.id;
            var c = db.courses.FirstOrDefault(m => m.id == id);
            ViewBag.CourseCode = c.code;
            ViewBag.CourseName = c.subject.name;
            ViewBag.CourseID = id;
            ViewBag.StudentID = sid;
            return View();
        }

        public ActionResult GetCourseMenu()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            int id = user.id;
            var courses = (from stc in db.student_course
                           join c in db.courses on stc.course_id equals c.id
                           join s in db.subjects on c.subject_id equals s.id
                           where stc.student_id == id
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

        public ActionResult ManageGroup(int courseID)
        {
            var c = db.courses.FirstOrDefault(m => m.id == courseID);
            ViewBag.CourseCode = c.code;
            ViewBag.CourseName = c.subject.name;
            ViewBag.courseID = courseID;
            var user = UserManager.FindById(User.Identity.GetUserId());
            int sid = user.id;
            var group = (from sg in db.student_group
                         join g in db.groups on sg.group_id equals g.id
                         join t in db.topics on g.topic_id equals t.id
                         where sg.student_id == sid && g.course_id == courseID
                         select new ManageGroupVM {
                             topic_id = t.id,
                             topic_name = t.name,
                             description = t.description,
                             file_detail = t.file_detail,
                             group_id = g.id,
                             group_name = g.name,
                             members = (from st in db.student_group
                                        join u in db.users on st.student_id equals u.id
                                        where st.group_id == g.id
                                        select u.name).ToList()
                         }).FirstOrDefault();
            return View(group);
        }

        public ActionResult AllTopics(int courseID)
        {
            var c = db.courses.FirstOrDefault(m => m.id == courseID);
            ViewBag.CourseCode = c.code;
            ViewBag.CourseName = c.subject.name;
            ViewBag.courseID = courseID;

            var user = UserManager.FindById(User.Identity.GetUserId());
            var x = (from sg in db.student_group
                     join g in db.groups on sg.group_id equals g.id
                     join t in db.topics on g.topic_id equals t.id
                     where sg.student_id == user.id && g.course_id == courseID
                     select new
                     {
                         topic_id = t.id,
                         topic_name = t.name
                     }).FirstOrDefault();
            if (x != null)
            {
                ViewBag.topic_id = x.topic_id;
                ViewBag.topic_name = x.topic_name;
            }

            var topicByCourse = (from t in db.topics
                                 where t.course_id == courseID
                                 select new TopicViewModel
                                 {
                                     id = t.id,
                                     course_id = t.course_id,
                                     description = t.description,
                                     name = t.name,
                                     file_detail = t.file_detail,
                                     totalGroup = db.topics.FirstOrDefault(m => m.id == t.id).groups.Count
                                 }).ToList();
            return View(topicByCourse);
        }

        public ActionResult RegisterTopic(int id)
        {
            var topic = db.topics.Where(m => m.id == id).Select(m => new RegisterTopicVM
            {
                id = m.id,
                name = m.name,
                course_id = m.course_id,
                description = m.description,
                file_detail = m.file_detail,
                topic_id = m.id,
                groups = db.groups.Where(g => g.topic_id == id).Select(g => new GroupViewModel {
                    id = g.id,
                    name = g.name,
                    topic_id = g.topic_id,
                    members = (from st in db.student_group
                               join u in db.users on st.student_id equals u.id
                               where st.group_id == g.id
                               select u.name).ToList()
                }).ToList()
            }).FirstOrDefault();
            var c = db.courses.FirstOrDefault(m => m.id == topic.course_id);
            ViewBag.CourseCode = c.code;
            ViewBag.CourseName = c.subject.name;
            ViewBag.courseID = c.id;
            return View(topic);
        }

        public ActionResult JoinGroup(int id)
        {
            var stg = new student_group
            {
                student_id = UserManager.FindById(User.Identity.GetUserId()).id,
                group_id = id
            };
            var cId = db.groups.Where(m => m.id == id).Select(m => m.course_id).FirstOrDefault();
            db.student_group.Add(stg);
            db.SaveChanges();
            return RedirectToAction("ManageGroup", new { courseID = cId });
            // pass parameter to registertopic action
        }

        public ActionResult CreateGroup(string group_name, int topic_id, int course_id)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var new_group = new group
            {
                name = group_name,
                course_id = course_id,
                topic_id = topic_id
            };
            db.groups.Add(new_group);

            var new_std_gr = new student_group
            {
                student_id = user.id,
                group_id = new_group.id
            };
            db.student_group.Add(new_std_gr);
            db.SaveChanges();
            return RedirectToAction("AllTopics", new { courseID = course_id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelTopic(int courseID)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var query1 = (from sg in db.student_group
                     join g in db.groups on sg.group_id equals g.id
                     where sg.student_id == user.id && g.course_id == courseID
                     select sg).FirstOrDefault();
            var query2 = (from sg in db.student_group
                          where sg.group_id == query1.group_id
                          select sg).ToList();
            db.student_group.Remove(query1);
            if(query2.Count == 1)
            {
                var query3 = (from g in db.groups
                              where g.id == query1.group_id
                              select g).FirstOrDefault();
                db.groups.Remove(query3);
            }
            db.SaveChanges();
            var cId = courseID;
            return RedirectToAction("AllTopics", new { courseID = cId});
        }
    }
}