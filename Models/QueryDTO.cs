using System.Collections.Generic;

namespace linkmir.Models
{
    public class QueryDTO
    {
        public string Domain { get; set; }
        public string SubDomain { get; set; }

        public List<LinkStatsDTO> Links { get; set; }

        public int MatchingLinksCount { get; set; }

        public int TotalSubmissionCount { get; set; }

        public int TotalAccessCount { get; set; }
    }
}