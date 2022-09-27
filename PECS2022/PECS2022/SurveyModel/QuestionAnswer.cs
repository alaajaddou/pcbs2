using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022
{
    public class QuestionAnswer
    {
        public QuestionAnswer()
        {

           
            SingleValues = new List<SingleValue>();
           // ListValues = new List<ListValues>();
        }

        public Guid QId { get; set; }
        public List<SingleValue> SingleValues { get; set; }
        public List<MatrixValue> MatrixValues { get; set; }
        
         public List<ListItem> ListItems { get; set; }

    }
}
