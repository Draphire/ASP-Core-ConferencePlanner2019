 using System;
 using System.Collections.Generic;
 using System.Text;

 namespace ConferenceDTO
 {
     public class SpeakerResponse : Speaker
     {
         //object carry info for the view presentation, View Model
         // TODO: Set order of JSON properties so this shows up last not first
         public ICollection<Session> Sessions { get; set; } = new List<Session>();
     }
 }