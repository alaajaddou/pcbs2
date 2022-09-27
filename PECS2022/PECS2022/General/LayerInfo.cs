using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PECS2022
{
    public class LayerInfo
    {

        public string TableName { get; set; }
        public string DisplayName { get; set; }
        public float InitialOpicity { get; set; }

        public int Order { get; set; }

        public bool EnableLabeling { get; set; }
        public string FieldName { get; set; }
        public Color BorderLineColor { get; set; }
        public Color FontColor { get; set; }
        public float FontSize { get; set; }
        public float BorderLineSize { get; set; }





    }
}
