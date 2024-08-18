using System;
using System.Collections.Generic;
using System.Linq;

namespace Location.Models
{
    /// <summary>
    /// Represents the quality metrics for an IP location provider.
    /// </summary>
    public class ProviderQualityMetrics
    {
        public string ProviderName { get; set; }
        public int RequestCount { get; set; } = 0;
        public int ErrorCount { get; set; } = 0;
        public double AvgResponseTime { get; set; } = 0;
        public DateTime LastResetTime { get; set; } = DateTime.Now;
        public List<double> ResponseTimes { get; set; } = new List<double>();
    }
}
