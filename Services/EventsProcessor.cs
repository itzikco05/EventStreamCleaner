using EventStreamCleaner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EventStreamCleaner.Services
{
    public class EventsProcessor
    {
        Dictionary<string, (int,TrafficEvent)> timeBucket = new Dictionary<string, (int, TrafficEvent)>();
        Dictionary<string, HashSet<string>> aggregation = new Dictionary<string,HashSet<string>>();
        int totalEvents = 0;
        int invalidEvents=0;
        public int duplicateEvents = 0;
        int uniqueValidEvents = 0;
        public List<TrafficEvent> Process(List<TrafficEvent> events)
        {
            totalEvents = events.Count;
            foreach(TrafficEvent trafficEvent in events)
            {
                if (!ValidateEvent(trafficEvent))
                {
                    invalidEvents++;
                    continue;
                }

                DateTime timestamp = DateTime.Parse(trafficEvent.timestamp);
                DateTime rounded=new DateTime(timestamp.Year,timestamp.Month,timestamp.Day,timestamp.Hour,timestamp.Minute,0,DateTimeKind.Utc);
                string key = trafficEvent.road_segment_id + trafficEvent.event_type + rounded.ToString();
                if (timeBucket.ContainsKey(key))
                {
                    duplicateEvents++;
                    DateTime timestamp2 = DateTime.Parse(timeBucket[key].Item2.timestamp);
                    if(timestamp2<timestamp)
                        timeBucket[key] = (timeBucket[key].Item1+1, trafficEvent);

                }
                else
                {
                    timeBucket[key] = ( 1, trafficEvent);
                }

                aggregation[trafficEvent.road_segment_id].Add(trafficEvent.event_type);

            }
            List<TrafficEvent> list = new List<TrafficEvent>();
            foreach (var value in timeBucket.Values)
            {
                if(value.Item1 == 1)
                {
                    uniqueValidEvents++;
                }
                list.Add(value.Item2);
            }
            return list;
        }
        public void PrintSummary()
        {
            Console.WriteLine("1.Total events received: "+ totalEvents);
            Console.WriteLine("2. Total invalid events: "+ invalidEvents);
            Console.WriteLine("3. Total duplicates removed: "+ duplicateEvents);
            Console.WriteLine("4. Unique valid events:" + uniqueValidEvents);
        }

        public void PrintAggregation()
        {
            foreach (var key in aggregation.Keys)
            {
                Console.WriteLine("For key:"+key+" unique value number:"+ aggregation[key].Count);
            }
        }
        private bool ValidateEvent(TrafficEvent trafficEvent)
        {

            //var isInt = int.TryParse(trafficEvent.severity, out int severity);
            //if (isInt && severity >= 1 && severity <= 5)
            if (trafficEvent.severity >= 1 && trafficEvent.severity <= 5)
            {
                var isISO = DateTime.TryParse(trafficEvent.timestamp, out DateTime timestamp);
                if (isISO) 
                    return true;
                else return false;
            }
            return false;
        }

    }
    
}
