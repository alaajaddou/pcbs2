using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022
{
    [Table("Individual")]
    public class Individual
    {

        [PrimaryKey]
        public System.Guid ID { get; set; }
        public string ID00 { get; set; }
        public int D1 { get; set; }
        public string D2 { get; set; }
        public int D3 { get; set; }
        public int D4 { get; set; }
        public int? D5_D { get; set; }
        public int? D5_M { get; set; }
        public int? D5_Y { get; set; }
        public int? D6 { get; set; }
        public int? D7 { get; set; }
        public int? D8 { get; set; }
        public int? D9 { get; set; }
        public int? D10_1 { get; set; }
        public int? D10_2 { get; set; }
        public int? D10_3 { get; set; }
        public int? D10_4 { get; set; }
        public int? D10_5 { get; set; }
        public int? D11 { get; set; }
        public int? D12_1 { get; set; }
        public int? D12_2 { get; set; }
        public int? D12_3 { get; set; }
        public int? D12_4 { get; set; }
        public int? D12_5 { get; set; }
        public int? D12_6 { get; set; }
        public int? D13 { get; set; }
        public int? D14 { get; set; }
        public int? D15 { get; set; }
        public int? D16 { get; set; }
        public int? D17 { get; set; }
        public string D17_CMNT { get; set; }
        public int? D18 { get; set; }
        public int? D19 { get; set; }
        public int? D20 { get; set; }
        public int? D21 { get; set; }
        public int? D22 { get; set; }
        public int? D23 { get; set; }
        public int? D24 { get; set; }
        public int? D25 { get; set; }
        public int? D26 { get; set; }
        public string D26_CMNT { get; set; }
        public int? L1 { get; set; }
        public string L1_CMNT { get; set; }
        public int? L2 { get; set; }
        public int? L3_A { get; set; }
        public int? L3_B { get; set; }
        public int? L3_C { get; set; }
        public int? L4 { get; set; }
        public int? L5 { get; set; }
        public int? L6 { get; set; }
        public int? L7 { get; set; }
        public int? L8 { get; set; }
        public int? L9 { get; set; }
        public int? L10 { get; set; }
        public int? L11 { get; set; }
        public string L12 { get; set; }
        public string L12_Desc { get; set; }
        public string L13 { get; set; }
        public string L13_Desc { get; set; }
        public int? L14_1 { get; set; }
        public int? L14_2 { get; set; }
        public int? L14_3 { get; set; }
        public int? L14_4 { get; set; }
        public int? L14_5 { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public bool? IsNew { get; set; }


    }

}
