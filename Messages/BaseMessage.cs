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
        public delegate string ProsessMessage(string message);

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
                ["GetAnnotations"] = (string input) =>  
                    {
                        var message = JsonConvert.DeserializeObject<Messages.AnnotationsDTO>(input); 
                        message.Annotations = DbAccess.AnnotationsList;                        
                        return JsonConvert.SerializeObject(message); 
                    },
                ["GetDataGroups"] = (string input) =>  
                    {
                        var message = JsonConvert.DeserializeObject<Messages.DataGroupsDTO>(input);
                        message.DataGroups = DbAccess.DataGroupsList;                         
                        return JsonConvert.SerializeObject(message);
                    },
                ["AddOrUpdateDatagroup"] = (string input) =>  
                    {
                        var message = JsonConvert.DeserializeObject<Messages.DataGroupsDTO>(input);
                        foreach(var dataGroup in message.DataGroups)
                        {
                            DbAccess.AddOrUpdateDatagroup(dataGroup);
                        }
                        return JsonConvert.SerializeObject(message);
                    },
                ["AddOrUpdateAnnotations"] = (string input) =>  
                    {
                        var message = JsonConvert.DeserializeObject<Messages.AnnotationsDTO>(input);
                        foreach(var annotation in message.Annotations)
                        {
                            DbAccess.AddOrUpdateAnnotations(annotation);
                        }
                        return JsonConvert.SerializeObject(message);
                    },
                ["DeleteDatagroups"] = (string input) =>  
                    {
                        var message = JsonConvert.DeserializeObject<Messages.DataGroupsDTO>(input);
                        foreach(var dataGroup in message.DataGroups)
                        {
                            DbAccess.DeleteDataGroup(dataGroup);
                        }
                        message.DataGroups = null;
                        return JsonConvert.SerializeObject(message);
                    },
                ["DeleteAnnotations"] = (string input) =>  
                    {
                        var message = JsonConvert.DeserializeObject<Messages.AnnotationsDTO>(input);
                        foreach(var annotation in message.Annotations)
                        {
                            DbAccess.DeleteAnnotation(annotation);
                        }
                        message.Annotations = null;
                        return JsonConvert.SerializeObject(message);
                    }                   
            };
            MessageActions = new ReadOnlyDictionary<string, ProsessMessage>(messageActions); 
        }

    }

    public class DataGroupsDTO : BaseMessage{
        public Repository.DataGroup[] DataGroups; 
    }

    public class AnnotationsDTO : BaseMessage{
        public Repository.Annotation[] Annotations; 
    }
}
