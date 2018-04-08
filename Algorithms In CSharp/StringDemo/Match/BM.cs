using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.StringDemo.Match
{
    /// <summary>
    /// Boyer-Moore 算法
    /// </summary>
    class BM: ISearch
    {
        private int[] right;//文本串当发生不匹配的时候，向右移动的距离
        private string pattern;
        private Alphabet alphabet;//字母表
        public BM(string pat, Alphabet alphabet)
        {
            this.pattern = pat;
            this.alphabet = alphabet;
            int M = pat.Length, R = alphabet.R();
            right = new int[R];
            for (int c = 0; c < R; ++c)
                right[c] = -1;
            for (int j = 0; j < M; ++j)
                right[alphabet.ToIndex(pat[j])] = j;//包含在模式中的字符表示为其中出现在最右侧的位置
        }
        public int Search(string txt)
        {
            int N = txt.Length;
            int M = pattern.Length;
            int skip;
            for (int i = 0; i <= N - M; i = i + skip)
            {
                skip = 0;
                for (int j = M - 1; j >= 0; --j)
                {
                    if (txt[i + j] != pattern[j])
                    {
                        //发生了不匹配
                        skip = j - right[alphabet.ToIndex(txt[i + j])];
                        if (skip < 1)
                            skip = 1;
                        //有几种情况：txt 未匹配的字符不在模式串中，那么i递增j+1
                        // txt 未匹配的字符在模式字符串中：i = i + j - right[字符]。注意这个字符可能出现在>=j的位置
                        //   当 right[字符] > j 的时候 : j - right[字符] < 0 , 就会将 i 左移，为了避免这种情况，需要 skip=1, 保证txt 向右了
                        //                  < j 的时候，j - right[字符] 就是 i 需要向右移动的距离
                        //                  = 不可能
                        break;
                    }
                }
                if (skip == 0)
                    return i;
            }
            return N;//不匹配
        }
        public static void Main()
        {
            Console.WriteLine("MP:");
            Console.Write("模式串(小写英文字母)：");
            string pat, txt;
            pat = Console.ReadLine();
            Console.Write("待匹配串(小写英文字母)：");
            txt = Console.ReadLine();
            KMP kmp = new KMP(pat, Alphabet.LOWERCASE);
            int pos = kmp.Search(txt);
            Console.Write("   Text:{0}\nPattern:", txt);
            if (pos > txt.Length)
                Console.WriteLine("<No pattern found>");
            else
                Console.WriteLine("{0}{1}", new string(' ', pos), pat);
            Console.ReadLine();
        }
    }
}
