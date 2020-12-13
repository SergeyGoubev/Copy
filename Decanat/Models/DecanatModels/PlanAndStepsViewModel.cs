using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decanat.Models.DecanatModels
{
    public class PlanAndStepsViewModel
    {
        public Plan plan;
        public List<Step> steps;

        public PlanAndStepsViewModel(Plan plan, List<Step> steps)
        {
            this.plan = plan;
            this.steps = steps;
        }

        public PlanAndStepsViewModel()
        {

        }
    }
}