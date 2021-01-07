using System.ComponentModel.DataAnnotations;

namespace linkmir.DbModels
{
    public class LinkmirLink
    {
        [Key]
        public string ShortLink { get; set; }
        public string OriginalUri { get; set; }
        public string Domain { get; set; }
        public string Subdomain { get; set; }
        public int SubmissionCount { get; set; }
        public int AccessCount { get; set; }
    }
}