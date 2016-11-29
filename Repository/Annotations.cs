using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdisonBrick.Repository
{
    public class Annotation
    {
        public Annotation(DateTime DateTimeUTC, string Name, string Type, string Description)
        {
            this.DateTimeUTC = DateTimeUTC;
            this.Name = Name;
            this.Type = Type;
            this.Description = Description;
        }


        [Required]
        public DateTime DateTimeUTC = DateTime.UtcNow;

        public string Name;

        public string Type;

        public string Description;
    }
}
