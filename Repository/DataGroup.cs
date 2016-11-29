using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EdisonBrick.Repository
{
    public class DataGroup
    {
        
        public DataGroup(Guid id, string name, DateTime startDate, TimeSpan? duration = null, string description = "")
        {
            if (id == Guid.Empty)
                id = Guid.NewGuid(); 

            Id = id;
            Name = name;
            StartDateUTC = startDate;
            Duration = duration;
            Description = description; 
        }

        public Guid Id; 

        [Required]
        public string Name;
        [Required]
        public DateTime StartDateUTC;

        public TimeSpan? Duration;

        public DateTime? EndDateUTC { get
            {
                TimeSpan? duration = Duration;
                if (duration == null)
                    return null;
                else 
                    return StartDateUTC + duration; 
            }
        }
        
        public string Description = string.Empty;  
    }


}
