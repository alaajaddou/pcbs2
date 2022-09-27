using System;

namespace PECS2022
{
    public class QuestionInfo
    {
        public System.Guid Id { get; set; }
   
        public string Code { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public Nullable<int> TypeId { get; set; }
        public Nullable<int> FormateId { get; set; }
        public int DisplayTypeId { get; set; }
        public Nullable<int> LookUpId { get; set; }
        public bool IsOptional { get; set; }
        public bool SingleValueMatrix { get; set; }
        public Nullable<int> RowLookUpId { get; set; }
        public Nullable<int> ColumnLookUpId { get; set; }
        public Nullable<System.Guid> ListId { get; set; }
        public Nullable<int> MinValue { get; set; }
        public Nullable<int> MaxValue { get; set; }
        public bool IsReadOnly { get; set; }
        public int OrderId { get; set; }
        public Nullable<int> DisplayActionId { get; set; }
        public Nullable<int> ConditionGroupId { get; set; }
        public bool IsActive { get; set; }
   
 

        
      
    }
}
