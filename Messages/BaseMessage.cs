using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdisonBrick.Messages
{
    public class BaseMessage
    {
        public  const string GetAnnotations = "GetAnnotations";
        public const string GetDataGroup = "GetDataGroups";


        [Required]
        public string Type;

        [Required]
        public int Id; 
    }
}
