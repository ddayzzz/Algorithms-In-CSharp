using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.StringDemo
{
    //ref https://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Alphabet.java.html
    class Alphabet
    {
        private Char[] m_alphabet;//保存所有的字母
        private int[] m_inverse;//保存索引。[m] 表示第m的字符的索引
        private readonly int m_r;//基数
        /// <summary>
        /// 从字符串s中创建一个字符表
        /// </summary>
        /// <param name="s">字符串</param>
        public Alphabet(String s)
        {
            var unicode = new Boolean[Char.MaxValue];//表示C# 中unicode 最大的能够表示的字符的大小
            for (int i = 0; i < s.Length; ++i)
            {
                char c = s[i];
                if (unicode[c])
                    throw new ArgumentException(string.Format("非法的字符：{0} 重复了！", c));
                unicode[c] = true;//去重标记
            }
            this.m_alphabet = s.ToCharArray();
            this.m_r = this.m_alphabet.Length;
            this.m_inverse = new int[char.MaxValue];
            //设置索引
            for (int c = 0; c < m_inverse.Length; ++c)
                m_inverse[c] = -1;
            for (int c = 0; c < m_r; ++c)
                m_inverse[this.m_alphabet[c]] = c;
        }
        /// <summary>
        /// 指定了基数。在这个基数范围内的所有字符都会有索引号
        /// </summary>
        /// <param name="R">索引</param>
        public Alphabet(int R)
        {
            m_r = R;
            m_alphabet = new char[R];
            m_inverse = new int[R];
            for (int i = 0; i < R; ++i)
            {
                m_alphabet[i] = (char)i;
                m_inverse[i] = i;
            }
        }
        /// <summary>
        /// 默认创建的是扩展ASCII的字母表
        /// </summary>
        public Alphabet() : this(256) { }
        /// <summary>
        /// 获取索引位置的元素，转换为字符
        /// </summary>
        /// <param name="index">元素的索引</param>
        /// <returns></returns>
        public Char ToChar(int index)
        {
            if (index < 0 || index >= m_r)
                throw new ArgumentException("索引介于 [0,R) 之间。");
            return m_alphabet[index];
        }
        /// <summary>
        /// 获得一个索引
        /// </summary>
        /// <param name="c">字符</param>
        /// <returns></returns>
        public int ToIndex(Char c)
        {
            if (c >= m_inverse.Length || m_inverse[c] == -1)
                throw new ArgumentException(String.Format("字符:'{0}'不在字符表中", c));
            return m_inverse[c];

        }
        /// <summary>
        /// 字符是否包含在字母表里
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public Boolean Contains(Char c)
        {
            return m_inverse[c] != -1;
        }
        /// <summary>
        /// 获取基数
        /// </summary>
        /// <returns></returns>
        public Int32 R()
        {
            return m_r;
        }
        /// <summary>
        /// 表示一个索引需要的比特数
        /// </summary>
        /// <returns></returns>
        public int LgR()
        {
            int s = m_r;
            int l = 0;
            while (s >= 1)
            {
                ++l;
                s /= 2;
            }
            return l;

        }
        /// <summary>
        /// 获取字符串中的所有字符的索引
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public int[] ToIndices(String s)
        {
            int[] target = new int[s.Length];
            for (int i = 0; i < target.Length; ++i)
                target[i] = ToIndex(s[i]);
            return target;
        }
        /// <summary>
        /// 获取所有的索引对应的字符串
        /// </summary>
        /// <param name="indices">索引号集合</param>
        /// <returns></returns>
        public String ToChars(int[] indices)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < indices.Length; ++i)
                sb.Append(ToChar(indices[i]));
            return sb.ToString();
        }
        /**
        *  The binary alphabet { 0, 1 }.
        */
        public static readonly Alphabet BINARY = new Alphabet("01");

        /**
         *  The octal alphabet { 0, 1, 2, 3, 4, 5, 6, 7 }.
         */
        public static readonly Alphabet OCTAL = new Alphabet("01234567");

        /**
         *  The decimal alphabet { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }.
         */
        public static readonly Alphabet DECIMAL = new Alphabet("0123456789");

        /**
         *  The hexadecimal alphabet { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, A, B, C, D, E, F }.
         */
        public static readonly Alphabet HEXADECIMAL = new Alphabet("0123456789ABCDEF");

        /**
         *  The DNA alphabet { A, C, T, G }.
         */
        public static readonly Alphabet DNA = new Alphabet("ACGT");

        /**
         *  The lowercase alphabet { a, b, c, ..., z }.
         */
        public static readonly Alphabet LOWERCASE = new Alphabet("abcdefghijklmnopqrstuvwxyz");

        /**
         *  The uppercase alphabet { A, B, C, ..., Z }.
         */

        public static readonly Alphabet UPPERCASE = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

        /**
         *  The protein alphabet { A, C, D, E, F, G, H, I, K, L, M, N, P, Q, R, S, T, V, W, Y }.
         */
        public static readonly Alphabet PROTEIN = new Alphabet("ACDEFGHIKLMNPQRSTVWY");

        /**
         *  The base-64 alphabet (64 characters).
         */
        public static readonly Alphabet BASE64 = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/");

        /**
         *  The ASCII alphabet (0-127).
         */
        public static readonly Alphabet ASCII = new Alphabet(128);

        /**
         *  The extended ASCII alphabet (0-255).
         */
        public static readonly Alphabet EXTENDED_ASCII = new Alphabet(256);
        public static void Main()
        {
            int[] encoded1 = Alphabet.BASE64.ToIndices("NowIsTheTimeForAllGoodMen");
            String decoded1 = Alphabet.BASE64.ToChars(encoded1);
            Console.WriteLine(decoded1);

            int[] encoded2 = Alphabet.DNA.ToIndices("AACGAACGGTTTACCCCG");
            String decoded2 = Alphabet.DNA.ToChars(encoded2);
            Console.WriteLine(decoded2);

            int[] encoded3 = Alphabet.DECIMAL.ToIndices("01234567890123456789");
            String decoded3 = Alphabet.DECIMAL.ToChars(encoded3);
            Console.WriteLine(decoded3);
        }
    }
}
