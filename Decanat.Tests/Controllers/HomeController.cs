using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Decanat.Models.DecanatModels;
using Decanat.DAO;

namespace Decanat.Tests.Controllers
{
    class HomeController
    {
        [SetUp]
        public void setMarkTest()
        {
            int id = 3;
            int mark = 5;
            AnswerDAO aDAO = new AnswerDAO();
            Answer actual = aDAO.getInfo(id);
            int temp = actual.status;
            actual.mark = mark;
            actual.status = 2;
            aDAO.setMark(id, mark);
            Answer expect = aDAO.getInfo(id);
            Assert.AreEqual(expect.mark, actual.mark);
            Assert.AreEqual(expect.status, actual.status);
            aDAO.setStatus(id, temp);
            
        }
        [SetUp]
        public void setstatusTest()
        {
            int id = 3;
            int status = 3;
            PlanDAO pDAO = new PlanDAO();
            Plan actual = pDAO.showPlanInfo(id);
            int temp = actual.status;
            actual.status = status;
            pDAO.setStatus(id, status);
            Plan expect = pDAO.showPlanInfo(id);
            Assert.AreEqual(actual.status, expect.status);
            pDAO.setStatus(id, temp);
        }

    }
}



