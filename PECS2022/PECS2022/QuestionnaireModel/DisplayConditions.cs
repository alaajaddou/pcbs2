using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022
{
   public class DisplayConditions
    {
        public System.Guid QuestionId { get; set; }
        public System.Guid ConditionQuestionId { get; set; }
        public int OperatorId { get; set; }
        public string Value { get; set; }
    }
}
