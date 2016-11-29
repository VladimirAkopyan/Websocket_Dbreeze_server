using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBreeze;

namespace EdisonBrick
{
    /// <summary>
    /// This class contains string values used in table names in the repository
    /// </summary>
    public static class TableType{
        const string TempDatapoints = "TempData"; //Temporary datapoints that have not yet been averaged
        const string SensorsAndValues = "SensValues"; //Sensors and their values
        const string DataGroup = "DataGroup"; // Also known as crop runs
        const string Events = "Events"; //Also known as annotations
    }

    public class Repository
    {
        public static Repository Instance = new Repository();
        private static DBreezeEngine dbEngine = new DBreeze.DBreezeEngine(@"./DBreeze");

        

        /// <summary>
        /// LoadsAllData on intiation
        /// </summary>
        public Repository()
        {

        }

        public bool AddDatapoint()
        {
        }

    }
}
