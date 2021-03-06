﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBreeze;
using Newtonsoft.Json;
using DBreeze.DataTypes;
using System.Collections.Concurrent; 

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

        

        private static SortedDictionary<DateTime, Repository.Annotation> _AnnotationsSet = new SortedDictionary<DateTime, Repository.Annotation>();
        private static SortedDictionary<string, Repository.Sensor> _SensorsSet = new SortedDictionary<string, Repository.Sensor>();
        private static SortedDictionary<Guid, Repository.DataGroup> _DataGroupsSet = new SortedDictionary<Guid, Repository.DataGroup>(); 

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
                    _DataGroupsSet.Add(row.Key, row.Value.Get);
                }
                foreach (var row in tran.SelectForward<DateTime, DbCustomSerializer<Repository.Annotation>>(TableType.Annotation))
                {
                    _AnnotationsSet.Add(row.Key, row.Value.Get);
                }


                /*
                foreach(var row in tran.SelectForward<string, DbCustomSerializer<Repository.Sensor>>(TableType.SensorsAndValues))
                {
                    Sensors.Add(row.Key, row.Value.Get);
                }*/
            }
        }

        public static bool AddOrUpdateAnnotations(Repository.Annotation annotation)
        {
            lock (_AnnotationsSet)
            {
                try
                {
                    using (var tran = _dbEngine.GetTransaction())
                    {
                        tran.Insert<DateTime, DbCustomSerializer<Repository.Annotation>>(TableType.Annotation, annotation.DateTimeUTC, annotation);
                        tran.Commit();
                    }
                     if (_AnnotationsSet.ContainsKey(annotation.DateTimeUTC))
                        _AnnotationsSet[annotation.DateTimeUTC] = annotation;
                    else
                        _AnnotationsSet.Add(annotation.DateTimeUTC, annotation);                    
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }
        }

        public static bool DeleteAnnotation(Repository.Annotation annotation)
        {
            lock (_AnnotationsSet)
            {
                if (_AnnotationsSet.ContainsKey(annotation.DateTimeUTC) == false)
                    return false;
                try
                {
                    using (var tran = _dbEngine.GetTransaction())
                    {
                        tran.RemoveKey(TableType.Annotation, annotation.DateTimeUTC); 
                        tran.Commit();
                    }
                    _AnnotationsSet.Remove(annotation.DateTimeUTC);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool AddOrUpdateDatagroup(Repository.DataGroup datagroup)
        {               
            lock (_DataGroupsSet)
            {
                try
                {
                    using (var tran = _dbEngine.GetTransaction())
                    {
                        tran.Insert<Guid, DbCustomSerializer<Repository.DataGroup>>(TableType.DataGroup, datagroup.Id, datagroup);
                        tran.Commit();
                    }
                    if (_DataGroupsSet.ContainsKey(datagroup.Id))
                        _DataGroupsSet[datagroup.Id]= datagroup;
                    else
                     _DataGroupsSet.Add(datagroup.Id, datagroup);                     
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true; 
            }
        }

        public static bool DeleteDataGroup(Repository.DataGroup dataGroup)
        {
            lock (_DataGroupsSet)
            {
                if (_DataGroupsSet.ContainsKey(dataGroup.Id) == false)
                    return false;
                try
                {
                    using (var tran = _dbEngine.GetTransaction())
                    {
                        tran.RemoveKey(TableType.DataGroup, dataGroup.Id); 
                        tran.Commit();
                    }
                    _DataGroupsSet.Remove(dataGroup.Id);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public static Repository.DataGroup[] DataGroupsList
        {
            get
            {
                lock (_DataGroupsSet)
                {
                    return _DataGroupsSet.Select(item => item.Value).ToArray();
                }
            }
        }

        public static Repository.Annotation[] AnnotationsList
        {
            get
            {
                lock (_AnnotationsSet)
                {
                    return _AnnotationsSet.Select(item => item.Value).ToArray();
                }
                
            }
        }



        /*
        public bool AddDatapoint()
        {
        }
        */
    }
}
