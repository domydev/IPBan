﻿/*
MIT License

Copyright (c) 2012-present Digital Ruby, LLC - https://www.digitalruby.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalRuby.IPBanCore
{
    /// <summary>
    /// Range of ipv6 addresses
    /// </summary>
    public struct IPV6Range : IComparable<IPV6Range>
    {
        /// <summary>
        /// Begin ip address
        /// </summary>
        public UInt128 Begin;

        /// <summary>
        /// End ip address
        /// </summary>
        public UInt128 End;

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (int)(Begin.GetHashCode() + End.GetHashCode());
            }
        }

        /// <summary>
        /// Check for equality
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>True if equal, false otherwise</returns>

        public override bool Equals(object obj)
        {
            if (!(obj is IPV6Range range))
            {
                return false;
            }
            return Begin == range.Begin && End == range.End;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="range">IPAddressRange</param>
        /// <exception cref="InvalidOperationException">Invalid address family</exception>
        public IPV6Range(IPAddressRange range)
        {
            if (range.Begin.AddressFamily != System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                throw new InvalidOperationException("Wrong address family for an ipv4 range");
            }
            Begin = range.Begin.ToUInt128();
            End = range.End.ToUInt128();
        }

        /// <summary>
        /// Conver to an ip address range
        /// </summary>
        /// <returns>IPAddressRange</returns>
        public IPAddressRange ToIPAddressRange() => new IPAddressRange(Begin.ToIPAddress(), End.ToIPAddress());

        /// <summary>
        /// IComparer against another IPV4Range
        /// </summary>
        /// <param name="other">Other range</param>
        /// <returns></returns>
        public int CompareTo(IPV6Range other)
        {
            int cmp = End.CompareTo(other.Begin);
            if (cmp < 0)
            {
                return cmp;
            }
            cmp = Begin.CompareTo(other.End);
            if (cmp > 0)
            {
                return cmp;
            }

            // inside range
            return 0;
        }
    }
}
