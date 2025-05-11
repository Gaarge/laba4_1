using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Models
{
    public class PerformanceMetrics
    {
        public TimeSpan ExecutionTime { get; set; }
        public long MemoryUsedBytes { get; set; }
        public int ElementCount { get; set; }
    }
}
