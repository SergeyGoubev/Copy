using Decanat.DAO;
using Decanat.Models.DecanatModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Decanat.Controllers
{
    public class HomeController : Controller
    {
        VkrDAO vDAO = new VkrDAO();
        StudentDAO sDAO = new StudentDAO();
        AnswerDAO aDAO = new AnswerDAO();
        TeacherDAO tDAO = new TeacherDAO();
        GruppaDAO gDAO = new GruppaDAO();
        PlanDAO pDAO = new PlanDAO();
        StepDAO stepDAO = new StepDAO();
        KafedraDAO kDAO = new KafedraDAO();
        VkrDAO vkrDAO = new VkrDAO();
        UserDAO uDAO = new UserDAO();

        //Стартовая страница
        public ActionResult Index()
        {

            if (User.IsInRole("Student"))
            {
                int id = sDAO.getStudentId(User.Identity.Name);
                Console.WriteLine(id);
                VKR vkr = vDAO.getVKRbyStudent(id);
                ViewAnswerAndVKR vav = new ViewAnswerAndVKR(vkr, id);
                List<Answer> answers = aDAO.getAnswersByVKR(vkr.id);
                vav.answers = answers;
                return View(vav);

            }
            else if (User.IsInRole("Teacher"))
            {
                int id = tDAO.getTeacherId(User.Identity.Name);
                ViewAnswerAndVKR vav = new ViewAnswerAndVKR(aDAO.getLastAnswers(id), id);
                return View(vav);
            } else 
            
                if (User.IsInRole("Decan"))
                {
                List<Plan> plans = pDAO.showPlansByStatus(6);
                ViewAnswerAndVKR vav = new ViewAnswerAndVKR(plans);
                List<Kafedra> kafedras = kDAO.getAllKafedras();
                vav.kafedras = kafedras;
                return View(vav);
                }
                else
                {
                if (User.IsInRole("Leiter")) {
                    List<Kafedra> kafedras = kDAO.getAllKafedras();
                    ViewAnswerAndVKR vav = new ViewAnswerAndVKR(kafedras);
                    aDAO.checkAllAnswers();
                    return View(vav); 
                } else return View();
                }



        }
        //******************************************************************************************************************************
        //Добавления
        //******************************************************************************************************************************


        //Добавление студента
        public ActionResult AddStudent()
        {
            SelectList users = new SelectList(uDAO.getUnregistegUser(), "email");
            ViewBag.Users = users;
            return View();
        }

        [Authorize(Roles = "Leiter,Decan")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddStudent([Bind(Exclude = "ID")] Student st, int groupId)
        {
            Student student = st;
            student.gruppaId = groupId;
            if (sDAO.add(st)) return RedirectToAction("GetGroupInfo", new { id = groupId });
            else return View("AddStudent");
        }

        //Добавление учителя
        public ActionResult AddTeacher()
        {
            SelectList users = new SelectList(uDAO.getUnregistegUser(), "email");
            ViewBag.Users = users;
            return View();
        }

        [Authorize(Roles = "Leiter,Decan")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddTeacher([Bind(Exclude = "ID")] Teacher teach, int kafedraid)
        {
            Teacher teacher = teach;
            teacher.kafedraId = kafedraid;
            if (tDAO.add(teach)) return RedirectToAction("Index");
            else return View("AddTeacher");
        }

        //Добавление группы
        public ActionResult AddGroup()
        {
            return View();
        }
        [Authorize(Roles = "Leiter,Decan")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddGroup([Bind(Exclude = "ID")] Gruppa grup, int id)
        {
            Gruppa group = grup;
            group.kafedra = id;
            if (gDAO.add(grup)) return RedirectToAction("getKafedraInfo", new { id = id});
            else return View("AddGroup");
        }

        //Добавление этапа П-Г
        public ActionResult AddStep()
        {
            return View();
        }

        [Authorize(Roles = "Leiter,Decan, Student")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddStep([Bind(Exclude = "ID")] Step step, int id)
        {
            Step st = step;
            st.planId = id;
            if (stepDAO.Add(st)) {
                int tId = id;
                return RedirectToAction("ShowPlanInfo", new { id = tId}); 
            }
            else return View("AddStep");
        }

        //Добавление плана-графика
        public ActionResult AddPlan(int groupId)
        {
            Plan plan = new Plan(groupId);
            pDAO.add(plan);
            gDAO.sepPlanStatus(true, groupId);
            plan = pDAO.showPlanInfoByGropId(groupId);
            //List<Step> steps = new List<Step>();
            List<Step> steps = stepDAO.getStepsByPlanId(plan.id);
            PlanAndStepsViewModel pASVM = new PlanAndStepsViewModel(plan, steps);
            
            return RedirectToAction("ShowPlanInfo", new { id = plan.id });
        }

        [Authorize(Roles = "Leiter,Decan")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddPlan([Bind(Exclude = "ID")] Plan plan )
        {
            if (pDAO.add(plan)) return RedirectToAction("Index");
            else return View("AddPlan");
        }

        //Добавление кафедры
        public ActionResult AddKafedra()
        {
            return View();
        }

        [Authorize(Roles = "Leiter,Decan")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddKafedra([Bind(Exclude = "ID")] Kafedra kafedra)
        {
            if (kDAO.add(kafedra)) return View("Index"); else return View("AddKafedra"); ;
        }

        //Добавление ВКР
        public ActionResult addVKR()
        {
            return View();
        }
        [Authorize(Roles = "Teacher")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult addVKR([Bind(Exclude = "ID")] VKR vkr, int studentId, int gruppaId)
        {
            VKR newVKR = vkr;
            newVKR.studentId = studentId;
            int groupId = gDAO.getGroupInfo(gruppaId).id;
            int teacherid = tDAO.getTeacherId(User.Identity.Name);
            int planId = pDAO.showPlanInfoByGropId(groupId).id;
            newVKR.teacherId = teacherid;
            newVKR.planId = planId;
            if (vDAO.add(newVKR))
            {
                return RedirectToAction("getStudentInfo", new { id = studentId });
            }
            else
            {
                return View("addVKR");
            }

        }

        //**********************************************************************************************************
        //Предоставление информации
        //**********************************************************************************************************

        //Подробная информация о студенте
        public ActionResult getStudentInfo(int id)
        {
            Student student = sDAO.getStudentInfo(id);
           Gruppa gruppa = gDAO.getGroupInfo(student.gruppaId);
           GroupAndStudentView gASV = new GroupAndStudentView(student, gruppa);
            return View(student);
        }
        //Подробная информация о представленном ответе
        [Authorize(Roles = "Teacher,Student, Leiter,Decan")]
        public ActionResult ShowAnswerInfo(int id)
        {
            Answer ans = aDAO.getInfo(id);
            return View(ans);

        }

        //Предоставление информации о ВКР
        public ActionResult showVKRInfo(int id)
        {
            VKR vkr = vkrDAO.getVKRbyId(id);
            List<Answer> answers = aDAO.getAnswersByVKR(id);
            ViewAnswerAndVKR vAVKR = new ViewAnswerAndVKR(answers, vkr);
            return View(vAVKR);
        }
        //GET: VKR
        //Просмотреть все документы
        public ActionResult getAllWorks()
        {
            return View(vDAO.getAllVKR());
        }

        //Просмотр информации о плане
        public ActionResult ShowPlanInfo(int id)
        {
            int tId = id;
            Plan plan = pDAO.showPlanInfo(tId);
            List<Step> steps = stepDAO.getStepsByPlanId(tId);
            PlanAndStepsViewModel pASVM = new PlanAndStepsViewModel(plan, steps);
            return View(pASVM);
        }

        //Просмор всех групп
        public ActionResult GetAllGrous()
        {
            List<Gruppa> groups = gDAO.getAllGroups();
            return View(groups);
        }
        //Информация об этапе
        public ActionResult getStepInfo(int id)
        {
            Step step = stepDAO.getStepsInfo(id);
            StepAndAnswersView sAAV = new StepAndAnswersView(step);
            if (step.isPlanAproved)
            {
                List<Answer> answers = aDAO.getAnswersByStep(step.id);
                sAAV.answers = answers;
            }

            return View(sAAV);
        }


        //Вывод информации о группе
        public ActionResult GetGroupInfo(int id)
        {
            List<Student> students = sDAO.getAllStudentInProup(id);
            Gruppa group = gDAO.getGroupInfo(id);
            StudentsInGroupView sIGV = new StudentsInGroupView(group, students);
            return View(sIGV);
        }
        //Вовыд информации о кафедре и группах
        public ActionResult getKafedraInfo(int id)
        {
            Kafedra kafedra = kDAO.getKafedraInfo(id);
            List<Gruppa> groups = gDAO.getAllGroupsByKafedra(id);
            GroupsInKafedra gIK = new GroupsInKafedra(groups,kafedra);
            return View(gIK);
        }
        //Все преподаватели кафедры
        public ActionResult getTeachersInKafedra(int id)
        {
            Kafedra kafedra = kDAO.getKafedraInfo(id);
            List<Teacher> teachers = tDAO.getAllTeachersByKafedra(id);
            TeachersInKafedraView tIKV = new TeachersInKafedraView(kafedra, teachers);
            return View(tIKV);
        }
        //********************************************************************************************************************
        //Изменение
        //********************************************************************************************************************
        public ActionResult SetPlanStatus(int id, int status)
        {
            if (pDAO.setStatus(id, status)) return RedirectToAction("ShowPlanInfo", new { id = id });
            else return View("Index");
        }

        //Предостаить ответ
        public ActionResult sendAnswer(int id)
        {
            Answer ans = aDAO.getInfo(id);
            return View(ans);
        }
        [Authorize(Roles = "student")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult sendAnswer(Answer ans, int id)
        {
            if (aDAO.sendAnswer(ans.id, ans.link)) return RedirectToAction("ShowAnswerInfo", new { id = ans.id });
            else return View();
        }


        //ПОставить оценку
        // GET: /Home/Edit 
        public ActionResult setMark(int id)
        {
            Answer ans = aDAO.getInfo(id);
            return View(ans);
        }

        [Authorize(Roles = "Teacher")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult setMark(Answer ans)
        {
            if (aDAO.setMark(ans.id, ans.mark)) { return RedirectToAction("Index"); }
            else return View();
        }

        //Отправить на исправление
        public ActionResult sendToEdit (int id)
        {
            if (aDAO.setStatus(id, 3)) return RedirectToAction("showAnswerInfo", new { id = id });
            else return RedirectToAction("Index"); //На страницу с ошибкой
        }
        //Добавить время для предоставление ответа
        public ActionResult giveMachTime(int id)
        {
            if (aDAO.setStatus(id, 5)) return RedirectToAction("showAnswerInfo", new { id = id });
            else return RedirectToAction("Index"); //На страницу с ошибкой
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult setAnswerStatus(int id, int status)
        {
            if (aDAO.setStatus(id, status)) return RedirectToAction("showAnswerInfo", new { id = id });
            else return RedirectToAction("Index"); //На страницу с ошибкой
        }
        public ActionResult editStep(int id)
        {
            Step step = stepDAO.getStepsInfo(id);
            return View(step);
        }
        [Authorize(Roles = "Leiter,Decan")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult editStep(Step step)
        {
            if (stepDAO.edit(step)) return RedirectToAction("ShowPlanInfo", new {id = step.planId }); else return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



    }
}