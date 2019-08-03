// namespace for grouping like objects to be neat and tidy
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// can also use reference individually
namespace BackEnd.Data {
    //inside = part of the names space not restricted to one file 
    //for use in Entity framework to build database table
    public class Speaker : ConferenceDTO.Speaker{
        public virtual ICollection<SessionSpeaker> SessionSpeakers { get; set; } = new List<SessionSpeaker>();
 
        // public int ID { get; set; }

        // [Required]
        // [StringLength(200)]
        // public string Name { get; set; }
        // //Required property Makes name a required object, String lenght to limit num of digits

        // [StringLength(4000)]
        // public string Bio { get; set; }

        // [StringLength(1000)]
        // public virtual string WebSite { get; set; }   
        // //virtual allows override from inheritance
    }
}