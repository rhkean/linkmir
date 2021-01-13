namespace linkmir.Models
{
    public class LinkStatsDTO : LinkDTO
    {
        public string Domain { get; set; }
        public string Subdomain { get; set; }
        public int SubmissionCount { get; set; }
        public int AccessCount { get; set; }
    }
}