using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022
{
 public    class VisitLog
    {

        [PrimaryKey]

        public Guid Id { get; set; }
        public string ID00 { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public bool NeedSend { get; set; }
        public string Researcher { get; set; }
        public bool IsComplete { get; set; }
        public double? S_X { get; set; }
        public double? S_Y { get; set; }
        public double? E_X { get; set; }
        public double? E_Y { get; set; }

        public int? QC2 { get; set; }

    }
}
