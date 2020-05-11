﻿using System.Collections.Generic;

namespace DiscordBot.DiscordBot.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Splits the given string into a list of substrings, while outputting the splitting
        /// delimiters (each in its own string) as well. It's just like String.Split() except
        /// the delimiters are preserved. No empty strings are output.</summary>
        /// <param name="s">String to parse. Can be null or empty.</param>
        /// <param name="delimiters">The delimiting characters. Can be an empty array.</param>
        /// <returns></returns>
        public static IList<string> SplitAndKeepDelimiters(this string s, params char[] delimiters)
        {
            var parts = new List<string>();
            if (!string.IsNullOrEmpty(s))
            {
                int iFirst = 0;
                do
                {
                    int iLast = s.IndexOfAny(delimiters, iFirst);
                    if (iLast >= 0)
                    {
                        if (iLast > iFirst)
                        {
                            parts.Add(s[iFirst..(iLast + 1)]); //part we want to find
                        }

                        iFirst = iLast + 1;
                        continue;
                    }

                    //No delimiters were found, but at least one character remains. Add the rest and stop.
                    parts.Add(s[iFirst..]);
                    break;

                } while (iFirst < s.Length);
            }

            return parts;
        }
    }
}
