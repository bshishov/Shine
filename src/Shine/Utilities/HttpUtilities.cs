﻿using System.Collections.Specialized;
using System.Net;

namespace Shine.Utilities
{
    public class HttpUtilities
    {
        public static NameValueCollection ParseQueryString(string queryString)
        {
            var queryParameters = new NameValueCollection();
            var querySegments = queryString.Split('&');
            foreach (var segment in querySegments)
            {
                var parts = segment.Split('=');
                if (parts.Length <= 1) continue;
                var key = parts[0].Trim('?', ' ');
                var val = WebUtility.UrlDecode(parts[1].Trim());

                queryParameters.Add(key, val);
            }
            return queryParameters;
        }
    }
}