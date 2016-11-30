using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace EdisonBrick.Messages
{
    public class BaseMessage
    {

        /// <summary>
        /// This is a function that identifies the message and takes appropriate responce
        /// </summary>
        /// <param name="message">Message recieved through WebSocket</param>
        /// <returns>message to be sent back</returns>
        public delegate string ProsessMessage(BaseMessage message);

        /// <summary>
        /// This is a collection of messageTypes (as key) and functions that prossess them
        /// </summary>
        public static readonly ReadOnlyDictionary<string, ProsessMessage> MessageActions;

        [Required]
        public string Type;

        [Required]
        public int Id; 

        static BaseMessage(){
            var messageActions = new Dictionary<string, ProsessMessage>{
                ["GetAnnotations"] = (BaseMessage input) =>  
                    {
                        var responce =  new AnnotationsDTO{
                            Id = input.Id,
                            Type = input.Type,
                            Annotations = DbAccess.AnnotationsList
                        };
                        return JsonConvert.SerializeObject(responce); 
                    },
                ["GetDataGroups"] = (BaseMessage input) =>  
                    {
                        var responce = new DataGroupsDTO{
                            Id = input.Id,
                            Type = input.Type,
                            DataGroups = DbAccess.DataGroupsList
                        };
                        return JsonConvert.SerializeObject(responce);
                    }
            };
            MessageActions = new ReadOnlyDictionary<string, ProsessMessage>(messageActions); 
        }
        public  const string GetAnnotations = "GetAnnotations";
    }

    public class DataGroupsDTO : BaseMessage{
        public Repository.DataGroup[] DataGroups; 
    }

    public class AnnotationsDTO : BaseMessage{
        public Repository.Annotation[] Annotations; 
    }
}
