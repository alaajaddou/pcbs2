using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PECS2022
{
    public class LookUpValueInfo
    {
        public int Id { get; set; }
        public int AnswerId { get; set; }
        public string AnswerCode { get; set; }
        public string Description { get; set; }
        public bool NeedComments { get; set; }
        public bool IsActive { get; set; }

        public string FullDescription { get { return $"{AnswerCode} - {Description}"; } }

    }
}
