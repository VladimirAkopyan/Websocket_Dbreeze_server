using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBreeze;
using Newtonsoft.Json;
using DBreeze.DataTypes;

namespace EdisonBrick
{
    /// <summary>
    /// This class contains string values used in table names in the repository
    /// </summary>
    public static class TableType{
        public const string TempDatapoints = "TempData"; //Temporary datapoints that have not yet been averaged
        public const string SensorsAndValues = "SensValues"; //Sensors and their values
        public const string DataGroup = "DataGroup"; // Also known as crop runs
        public const string Annotation = "Annotation"; //Also known as annotations
    }

    public static class DbAccess
    {
        private static DBreezeEngine _dbEngine;  

        

        private static SortedDictionary<DateTime, Repository.Annotation> AnnotationsSet = new SortedDictionary<DateTime, Repository.Annotation>();
        private static SortedDictionary<string, Repository.Sensor> SensorsSet = new SortedDictionary<string, Repository.Sensor>();
        private static SortedDictionary<Guid, Repository.DataGroup> DataGroupsSet = new SortedDictionary<Guid, Repository.DataGroup>(); 

        /// <summary>
        /// LoadsAllData on intiation
        /// </summary>
        static DbAccess()
        {
            DBreeze.Utils.CustomSerializator.Serializator = JsonConvert.SerializeObject;
            DBreeze.Utils.CustomSerializator.Deserializator = JsonConvert.DeserializeObject;
            _dbEngine = new DBreeze.DBreezeEngine(@"./DBreeze");

            //LoadAllValues
            using (var tran = _dbEngine.GetTransaction())
            {
                foreach (var row in tran.SelectForward<Guid, DbCustomSerializer<Repository.DataGroup>>(TableType.DataGroup))
                {
                    DataGroupsSet.Add(row.Key, row.Value.Get);
                }
                foreach (var row in tran.SelectForward<DateTime, DbCustomSerializer<Repository.Annotation>>(TableType.Annotation))
                {
                    AnnotationsSet.Add(row.Key, row.Value.Get);
                }


                /*
                foreach(var row in tran.SelectForward<string, DbCustomSerializer<Repository.Sensor>>(TableType.SensorsAndValues))
                {
                    Sensors.Add(row.Key, row.Value.Get);
                }*/
            }
        }

        public static bool AddAnnotation(Repository.Annotation annotation)
        {
            if (AnnotationsSet.ContainsKey(annotation.DateTimeUTC))
                return false;

            try
            {
                using (var tran = _dbEngine.GetTransaction())
                {
                    tran.Insert<DateTime, DbCustomSerializer<Repository.Annotation>>(TableType.Annotation, annotation.DateTimeUTC,annotation);
                    tran.Commit(); 
                }
                AnnotationsSet.Add(annotation.DateTimeUTC, annotation); 
                return true; 
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool AddDataGroup(Repository.DataGroup datagroup)
        {
            if (DataGroupsSet.ContainsKey(datagroup.Id))
                return false;

            try
            {
                using (var tran = _dbEngine.GetTransaction())
                {
                    tran.Insert<Guid, DbCustomSerializer<Repository.DataGroup>>(TableType.DataGroup, datagroup.Id, datagroup);
                    tran.Commit();
                }
                DataGroupsSet.Add(datagroup.Id, datagroup);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static Repository.DataGroup[] DataGroups
        {
            get
            {
                return DataGroupsSet.Select(item => item.Value).ToArray(); 
            }
        }

        public static Repository.Annotation[] Annotations
        {
            get
            {
                return AnnotationsSet.Select(item => item.Value).ToArray();
            }
        }



        /*
        public bool AddDatapoint()
        {
        }
        */
    }
}
