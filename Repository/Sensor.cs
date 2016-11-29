using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;
using Newtonsoft.Json;
using System.Collections.Concurrent;

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
        public double AbsMin = -40;
        /// <summary>
        /// Physical maximum for this sensor. Any value above this will be considered invalid. 
        /// </summary>
        public double AbsMax = 100;
        /// <summary>
        /// unit of measurement
        /// </summary>
        public string Unit;

        /// <summary>
        /// The data from this sensor will be roudned to this accuracy
        /// </summary>
        public float Accuracy = 0.5f; 

        [JsonIgnore]
        public SortedSet<Datapoint> HistoricalData = new SortedSet<Datapoint>(new DatapointComparer()); 
    }

    public class DatapointComparer : IComparer<Datapoint>
    {
        public int Compare(Datapoint x, Datapoint y)
        {
            if (x.TimeStampUTC > y.TimeStampUTC)
                return 1;
            else if (x.TimeStampUTC < y.TimeStampUTC)
                return -1;
            else
                return 0; 
        }
    }

    public struct Datapoint
    {
        /// <summary>
        /// Creates a datapoint
        /// </summary>
        /// <param name="unixTimestamp">unix timestamp to use</param>
        /// <param name="Value">The value</param>
        public Datapoint(UInt32 unixTimestamp, float Value)
        {
            this._UnixTimestamp = unixTimestamp;
            this.Value = Value; 
        }

        /// <summary>
        /// Creates a datapoint
        /// </summary>
        /// <param name="TimeStampUTC">Must be later than 1970, before 2050</param>
        /// <param name="Value">The value to assign</param>
        public Datapoint(DateTime TimeStampUTC, float Value)
        {
            TimeSpan difference = TimeStampUTC - _Epoc; 
            this._UnixTimestamp = (UInt32)difference.TotalSeconds;
            this.Value = Value;
        }

        private readonly UInt32 _UnixTimestamp;
        public readonly float Value;

        private static readonly System.DateTime _Epoc = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

        public DateTime TimeStampUTC{ 
            get  {
                // Unix timestamp is seconds past epoch
                var dtDateTime = _Epoc.AddSeconds(_UnixTimestamp).ToLocalTime();
                return dtDateTime;
            }
        }
    }

    
}
