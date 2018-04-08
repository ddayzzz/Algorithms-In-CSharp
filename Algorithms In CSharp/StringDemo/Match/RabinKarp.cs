using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.StringDemo.Match
{
    class RabinKarp
    {
        private string pattern;
        private long patHash;//保存模式字符串的散列值
        private int M;//模式字符串的长度
        private long Q;//很大的素数
        private int R;//基数
        private long RM;//R^(M-1) % Q
        private Alphabet alphabet;
        public RabinKarp(string pat, Alphabet alphabet)
        {
            pattern = pat;
            this.M = pat.Length;
            this.alphabet = alphabet;
        }
    }
}
