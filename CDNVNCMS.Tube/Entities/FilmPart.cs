using System.ComponentModel.DataAnnotations.Schema;

namespace CDNVNCMS.Tube.Entities
{
    public class FilmPart:General
    {
        [Column("YoutubeId")]
        public string VideoData { get; set; }
        public string Image { get; set; }
        [Column("Url")]
        public string VideoType { get; set; }
        
        public bool isError { get; set; }

        public int FilmId { get; set; }
        public virtual Film Film { get; set; }
        
    }
}
