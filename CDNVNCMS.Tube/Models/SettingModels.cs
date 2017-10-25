using System.ComponentModel.DataAnnotations;

namespace CDNVNCMS.Tube.Models
{
    public class SettingModels
    {
        public string WebName { get; set; }
        [DataType(DataType.MultilineText)]
        public string MetaKeyword { get; set; }
        [DataType(DataType.MultilineText)]
        public string MetaDescription { get; set; }
        public string LogoUrl { get; set; }
        public string IconUrl { get; set; }
        public string Count { get; set; }
    }
}