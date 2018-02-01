using System;
using System.Collections.Generic;

namespace Test.COM.Comparer
{
    /// <summary>
    /// 字符串排序
    /// 先数字后字母
    /// 1,3,4A,4C,L1,L3
    /// </summary>
    public class StrNumberComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            int i1, i2;
            if (int.TryParse(x, out i1) && int.TryParse(y, out i2))
                return i1.CompareTo(i2);

            var len1 = x.Length;
            var len2 = y.Length;
            var marker1 = 0;
            var marker2 = 0;

            while (marker1 < len1 && marker2 < len2)
            {
                var ch1 = x[marker1];
                var ch2 = y[marker2];

                var space1 = new char[len1];
                var loc1 = 0;
                var space2 = new char[len2];
                var loc2 = 0;

                do
                {
                    space1[loc1++] = ch1;
                    marker1++;

                    if (marker1 < len1)
                        ch1 = x[marker1];
                    else
                        break;
                } while (char.IsDigit(ch1) == char.IsDigit(space1[0]));

                do
                {
                    space2[loc2++] = ch2;
                    marker2++;

                    if (marker2 < len2)
                        ch2 = y[marker2];
                    else
                        break;
                } while (char.IsDigit(ch2) == char.IsDigit(space2[0]));

                var str1 = new string(space1);
                var str2 = new string(space2);

                int result;

                if (char.IsDigit(space1[0]) && char.IsDigit(space2[0]))
                {
                    var thisNumericChunk = int.Parse(str1);
                    var thatNumericChunk = int.Parse(str2);
                    result = thisNumericChunk.CompareTo(thatNumericChunk);
                }
                else
                {
                    result = string.Compare(str1, str2, StringComparison.Ordinal);
                }

                if (result != 0) return result;
            }

            return x.Length - y.Length;
        }
    }
}
