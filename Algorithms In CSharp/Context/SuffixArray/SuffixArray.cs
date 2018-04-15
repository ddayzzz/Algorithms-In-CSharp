using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Context.SuffixArray
{
    class SuffixArray
    {
        private readonly string[] suffixes;//后缀数组
        private readonly int N;// 字符串（和数组的长度）
        public SuffixArray(string s)
        {
            N = s.Length;//确定长度
            suffixes = new string[N];//后缀
            for (int i = 0; i < N; ++i)
                suffixes[i] = s.Substring(i);//不能太大了
            Array.Sort(suffixes);//排序
        }
        public int Length { get { return N; } }
        public string Select(int i) { return suffixes[i]; }
        public int Index(int i) { return N - suffixes[i].Length;/*原始的位置，原始的顺序是子字符串长度的递减。所以i位置匹配那么后面的一样会匹配*/ }
        private int LCP(string s, string t)
        {
            int N = Math.Min(s.Length, t.Length);
            for (int i = 0; i < N; ++i)
                if (s[i] != t[i])
                    return i;//站到一个字串的部分
            return N;
        }
        /// <summary>
        /// 返回键在有序后的位置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int Rank(string key)
        {
            int lo = 0, hi = N - 1;
            while(lo <= hi)
            {
                int mid = lo + (hi - lo) / 2;
                int cmp = key.CompareTo(suffixes[mid]);
                if (cmp < 0)
                    hi = mid - 1;
                else if (cmp > 0)
                    lo = mid + 1;
                else
                    return mid;
            }
            return lo;
        }
        /// <summary>
        /// 返回与前一个字串的公共长度
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int LCP(int i)
        {
            return LCP(suffixes[i], suffixes[i - 1]);
        }
    }
}
