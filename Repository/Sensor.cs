using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;
using Json.Net; 

namespace EdisonBrick.Repository
{
    public class Sensor
    {
        //Name for this sensor. Must be unique. 
        public string Name;
        /// <summary>
        /// Type of sensor, e.g. temperature
        /// </summary>
        public string Type;
        /// <summary>
        /// Physical minimum for this sensor. A measurement outside these bounds will be considered invalid. 
        /// </summary>
        public double AbsMin;
        /// <summary>
        /// Physical maximum for this sensor. Any value above this will be considered invalid. 
        /// </summary>
        public double AbsMax;
        /// <summary>
        /// unit of measurement
        /// </summary>
        public string Unit; 


    }
}
