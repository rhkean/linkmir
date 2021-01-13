using System;
using System.Collections.Generic;
using ES.XxHash;

// Original code found at:  https://weblog.west-wind.com/posts/2012/Apr/24/Getting-a-base-Domain-from-a-Domain
// Modified to also handle subdomain parsing and correcting for multiple sub-domain links
// Also added extension method to generate the linkmir unique_path

// By taking the submitted link and generating a Uri object, we can cheat on some required code logic
// * automatically converts the hostname to lower case
// * automatically test for and differentiate between DNS names and IP address hostnames
// * easily test that the protocol is http/https
public static class NetworkUtils
{
    // string containing 64 (2^6) valid URL characters.  Valid ASCII characters for use in URL's
    // are:  [a-zA-Z0-9], "-", ".", "_", and "~"
    // This totals 66 characters.  Since we only need 64, exclude "." and "~", since they are less
    // user friendly to the eye.
    private const string validUrlCharacters = "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ0123456789_-";

    /// <summary>
    /// Retrieves a base domain name from a full domain name.
    /// For example: www.west-wind.com produces west-wind.com
    /// </summary>
    /// <param name="domainName">Dns Domain name as a string</param>
    /// <returns></returns>
    private static string GetBaseDomain(string domainName)
    {
            var tokens = domainName.Split('.');

            // ** REMOVED from original code.  It is possible 
            // ** for a Uri to have multiple sub-domains
            // // only split 3 segments like www.west-wind.com
            // if (tokens == null || tokens.Length != 3)
            //     return domainName;

            // if there are less than 3 tokens return domainName
            // like west-wind.com
            // This if-block replaces the one above
            if (tokens == null || tokens.Length < 3)
                return domainName;

            var tok  = new List<string>(tokens);
            var remove = tokens.Length - 2;
            tok.RemoveRange(0, remove);

            return tok[0] + "." + tok[1]; ;                                
    }

    /// <summary>
    /// Retrieves a sub domain name from a full domain name.
    /// For example: www.west-wind.com produces www
    /// </summary>
    /// <param name="domainName">Dns Domain name as a string</param>
    /// <returns></returns>
    private static string GetSubDomain(string domainName)
    {
            var tokens = domainName.Split('.');

            // if there are less than 3 tokens return empty string
            // ex. west-wind.com would return ""
            if (tokens == null || tokens.Length < 3)
                return string.Empty;

            var tok  = new List<string>(tokens);
            // remove domain from List
            var remove = tokens.Length - 2;
            tok.RemoveRange(remove, 2);

            return string.Join('.', tok);                                
    }

    /// <summary>
    /// Returns the base domain from a domain name
    /// Example: http://www.west-wind.com returns west-wind.com
    /// This is an extension method to the Uri class.
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static string GetBaseDomain(this Uri uri)
    {
        if (uri.HostNameType == UriHostNameType.Dns)                        
            return GetBaseDomain(uri.DnsSafeHost);
        
        return uri.Host;
    }

    /// <summary>
    /// Returns the sub domain from a domain name
    /// Example: http://www.west-wind.com returns www
    /// This is an extension method to the Uri class.
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static string GetSubDomain(this Uri uri)
    {
        if (uri.HostNameType == UriHostNameType.Dns)                        
            return GetSubDomain(uri.DnsSafeHost);
        
        return uri.Host;
    }
 
    /// <summary>
    /// Returns Unique URL path for linkmir project
    /// This will return an 11 character unique path
    /// This is an extension method to the Uri class.
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static string GetUniqueUrlPath(this Uri uri)
    {
        string toReturn = string.Empty;

        // use the AbosolutUri property since it already
        // translates the hostname to lowercase.  Since DNS names
        // are not case-sensitive, this will reduce duplicate entries
        // in the data.
        //
        // Generate a 64-bit hash of the Uri using xxHash, which is a
        // very fast hashing algorithm that produces a lot frequency of
        // collisions.  more here:  https://cyan4973.github.io/xxHash/
        //
        // Using a 64-bit hash allows for 2^64 unique links of 11 characters
        // in length.
        // This could be expanded, if needed, using a 128-bit hash algorithm,
        // however, the unique link would be 22 characters long... not very short :)
        var hash = uri.AbsoluteUri.XxHash();

        // using a 6-bit mask, the while-loop loops until the the mask reaches
        // zero by shifting the mask 6 bits at a time.
        ulong mask = 0xFC00000000000000;
        int shiftAmount = 64;
        while (mask > 0)
        {
            // determine how far to shift the workingBits to get a single byte-size
            // result to use as an index in the validUrlCharacters string
            shiftAmount -= 6;
            // if shiftAmount is negative (in the final loop, it will be -2, bc we are
            // shifting 6 bits at a time, which is not a factor of 64), set shiftAmount
            // to 0
            if(shiftAmount < 0)
                shiftAmount = 0;

            // zero-out all bits except the 6 we need for this loop
            ulong workingBits = hash & mask;
            // shift the 6 bits to the least-significant byte and cast to an int
            int index = (int)(workingBits >> shiftAmount);
            // use shifted bits as an index into the validUrlCharacters string and
            // concatenate the resulting character to the result.
            toReturn += validUrlCharacters[index];
            // shift mask for next loop
            mask>>=6;
        }

        return toReturn;
    }

    public static string GetSubReddit(this Uri uri)
    {
        return string.Empty;
    }
}