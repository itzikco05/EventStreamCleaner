using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStreamCleaner.Models
{
    public class TrafficEvent
    {
        public string event_id { get; set; }
        public string timestamp { get; set; }
        public string road_segment_id { get; set; }
        public string event_type { get; set; }
        public int severity {  get; set; }
        public string source { get; set; }


    }
}
