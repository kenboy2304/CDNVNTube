﻿using System.ComponentModel.DataAnnotations;

namespace CDNVNCMS.Tube.Entities
{
    public class GeneralDescription:General
    {
        public string Name { get; set; }
        [DataType(DataType.Html)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string MetaKeyword { get; set; }
        [DataType(DataType.MultilineText)]
        public string MetaDescription { get; set; }
    }
}
