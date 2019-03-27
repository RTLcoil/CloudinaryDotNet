using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Implement generic utility functions
    /// </summary>
    internal static partial class Utils
    {
        internal static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts DateTime to Unix epoch time in seconds
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Epoch time in seconds</returns>
        internal static long ToUnixTimeSeconds(DateTime date)
        {
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }

        /// <summary>
        /// Converts Unix epoch time in seconds to DateTime
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns>Datetime</returns>
        public static DateTime FromUnixTimeSeconds(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        /// <summary>
        /// Unix epoch time now in seconds
        /// </summary>
        /// <returns>total seconds since epoch</returns>
        internal static long UnixTimeNowSeconds()
        {
            return ToUnixTimeSeconds(DateTime.UtcNow);
        }
        /// <summary>
        /// Concatenates items using provided separator, escaping separtor character in each item
        /// </summary>
        /// <param name="separator"> The string to use as a separator</param>
        /// <param name="items">IEnumerable to join</param>
        /// <returns></returns>
        internal static string SafeJoin(string separator, IEnumerable<string> items)
        {
            return String.Join(separator, items.Select(item => Regex.Replace(item, $"([{separator}])", "\\$1")));
        }

        internal static bool IsRemoteFile(string filePath)
        {
            return Regex.IsMatch(
                filePath, 
                @"^((ftp|https?|s3|gs):.*)|data:([\w-]+/[\w-]+)?(;[\w-]+=[\w-]+)*;base64,([a-zA-Z0-9/+\n=]+)");
        }

        /// <summary>
        /// Encode string to URL-safe Base64 string
        /// </summary>
        internal static string EncodeUrlSafe(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            return EncodeUrlSafe(bytes);
        }

        /// <summary>
        /// Encode bytes to URL-safe Base64 string
        /// </summary>
        internal static string EncodeUrlSafe(byte[] bytes)
        {
            return Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_');
        }

        /// <summary>
        /// Escape Url with a custom set of escaped characters.
        /// </summary>
        /// <param name="url">Url to escape.</param>
        /// <param name="reUnsafe">Regular expression with characters to escape.</param>
        /// <param name="toUpper">Whether to return escaped characters in upper case. Default: true.</param>
        internal static string EscapeUrlUnsafe(string url, string reUnsafe = "([^a-zA-Z0-9_.\\-\\/:]+)", bool toUpper = true)
        {
            Regex r = new Regex(reUnsafe, RegexOptions.Compiled | RegexOptions.RightToLeft);
            var sbUrl = new StringBuilder(url);    
            foreach (Match itemMatch in r.Matches(sbUrl.ToString()))
            {
                var escapedItem = string.Join("%", itemMatch.Value.Select(c => Convert.ToByte(c).ToString("x2")));
                sbUrl.Remove(itemMatch.Index, itemMatch.Length);
                sbUrl.Insert(itemMatch.Index, "%" + (toUpper ? escapedItem.ToUpper() : escapedItem.ToLower()));
            }
            return sbUrl.ToString();
        }


        internal static byte[] ComputeHash(string s)
        {
            using (var sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(Encoding.UTF8.GetBytes(s));
            }
        }
    }
}
