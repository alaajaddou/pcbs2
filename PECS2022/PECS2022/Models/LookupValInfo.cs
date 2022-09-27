using SQLite;

using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.Models
{
    [Table("LookupValues")]
    public class LookupVal
    {

        [PrimaryKey]
        public Guid KID { get; set; }
        public string LookUpCode { get; set; }
        public int LookUpId { get; set; }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public string FieldOne { get; set; }
        public string FieldTwo { get; set; }
        public string FieldThree { get; set; }


        [Ignore]
        public string FullDescription
        {
            get
            {
                return $"{Code} - {Description}";
            }
        }
    }
}
