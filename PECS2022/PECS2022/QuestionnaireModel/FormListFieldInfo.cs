using System;

namespace PECS2022
{
    public class FormListFieldInfo
    {
        public System.Guid Id { get; set; }
        public System.Guid FormListId { get; set; }
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
        public Nullable<System.Guid> ListID { get; set; }
        public Nullable<int> MinValue { get; set; }
        public Nullable<int> MaxValue { get; set; }
        public bool IsReadOnly { get; set; }
        public int OrderId { get; set; }
        public bool IsKey { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public System.DateTime LastModifiedDate { get; set; }

    }
}