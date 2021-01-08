using System;
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

        private Uri _uri;

        public LinkmirLink() {}

        public LinkmirLink(string link)
        {
            if (Uri.TryCreate(link, UriKind.Absolute, out _uri))
            {
                if(IsValid())
                {
                    ShortLink = _uri.GetUniqueUrlPath();
                    OriginalUri = _uri.AbsoluteUri;
                    Domain = _uri.GetBaseDomain();
                    Subdomain = _uri.GetSubDomain();
                    SubmissionCount = 1;
                    AccessCount = 0;
                }
            }
        }

        public bool IsValid()
        {
            return (_uri != null && (_uri.Scheme == "http" || _uri.Scheme == "https"));
        }

    }
}