using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.StringDemo.Match
{
    /// <summary>
    /// KMP 算法：使用了经过 DFA 处理的状态数组。参考博客 https://blog.csdn.net/congduan/article/details/45459963
    /// </summary>
    class KMP: ISearch
    {
        private string pattern;//模式串
        private int[,] dfa;//状态机
        private Alphabet alphabet;
        public KMP(string pattern, Alphabet alphabet)
        {
            this.pattern = pattern;
            int R = alphabet.R();
            this.alphabet = alphabet;
            //构建DFA自动机
            int M = pattern.Length;
            dfa = new int[R, M];
            dfa[alphabet.ToIndex(pattern[0]), 0] = 1;//当前输入是模式串的时候，模式指针向前移动
            for (int X = 0, j = 1; j < M; ++j)
            {
                // j 表示的是模式字符的位置
                for (int c = 0; c < R; ++c)
                {
                    dfa[c, j] = dfa[c, X];//拷贝X列到j的位置
                }
                int rj = alphabet.ToIndex(pattern[j]);//真实的对于R范围内的缩索引
                dfa[rj, j] = j + 1;//具有相同的输入的位置
                X = dfa[rj, X];
            }
        }
        public int Search(string txt)
        {
            int i, j, N = txt.Length,M=pattern.Length;
            for(i=0,j=0;i<N && j < M;++i)
            {
                j = dfa[alphabet.ToIndex(txt[i]), j];//输入进行向上转移
            }
            if (j == M)
                return i - M;
            else
                return N;//没有匹配
        }
        public static void Main()
        {
            Console.WriteLine("模式串(小写英文字母)：");
            string pat, txt;
            pat = Console.ReadLine();
            Console.WriteLine("待匹配串(小写英文字母)：");
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
