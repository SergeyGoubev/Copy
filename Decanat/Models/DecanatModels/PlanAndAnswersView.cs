using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decanat.Models.DecanatModels
{
    public class StepAndAnswersView
    {
        public Step step;
        public List<Answer> answers;

        public StepAndAnswersView(Step step)
        {
            this.step = step;
        }
    }
}