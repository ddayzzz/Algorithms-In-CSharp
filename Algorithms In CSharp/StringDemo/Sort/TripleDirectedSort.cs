using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.StringDemo.Sort
{
    /// <summary>
    /// 三向快速字符串排序：适用于长字符
    /// </summary>
    class TripleDirectedSort
    {
        private static Alphabet alphabet;
        /// <summary>
        /// 返回字符在字符中相对于字母表的索引
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="d">最高位开始的位数</param>
        /// <returns>返回字符的位置或者不足返回-1</returns>
        private static int CharAt(String s, int d)
        {
            if (d < s.Length)
                return alphabet.ToIndex(s[d]);

            else
                return -1;
        }
        private static void Exchange(String[] a, int i, int j)
        {
            String s1 = a[i];
            a[i] = a[j];
            a[j] = s1;
        }
        /// <summary>
        /// 递归函数
        /// </summary>
        /// <param name="a">要排序的字符串</param>
        /// <param name="lo">下界</param>
        /// <param name="hi">上界</param>
        /// <param name="d">哪一位（从最高位）</param>
        private static void Sort(String[] a, int lo, int hi, int d)
        {
            //原理来自于快速排序
            if (hi <= lo)
                return;//不处理小数组（1）和超过边界的非法情况
            int low = lo;
            int upp = hi;
            //设置轴
            int pivot = CharAt(a[lo], d);//这个字符作为轴
            int i = lo + 1;
            while(i <= upp)
            {
                int t = CharAt(a[i], d);
                //  ... <pivot  ...  pivot>  ...
                if (t < pivot)
                    Exchange(a, low++, i++);//把小于 pivot 换到前面去
                else if (t > pivot)
                    Exchange(a, i, upp--);//从 upp +1 开始一定是比pivot 要大的元素
                else
                    //是相同类型的元素
                    ++i;
            }
            //lo的位置就是第一个 == pivot 的位置
            Sort(a, lo, low - 1, d);//< pivot 的排序
            if (pivot >= 0)
                Sort(a, low, upp, d + 1);
            Sort(a, upp + 1, hi, d);
        }
        /// <summary>
        /// 通过前W个字符将a[] 排序
        /// </summary>
        /// <param name="a"></param>
        /// <param name="alphabet">可以指定一个字符表。</param>
        public static void Sort(String[] a, Alphabet alphabet)
        {

            TripleDirectedSort.alphabet = alphabet;
            Sort(a, 0, a.Length - 1, 0);
        }
        public static void Main()
        {
            Alphabet alphabet = Alphabet.LOWERCASE;
            String[] pokes = new string[]
            {
                "sheuu","sells","seash","byiri","theiu","seaio",
                "shore","thehe","shell","sherw","sells","arehh","surel","seash"
            };
            Sort(pokes, alphabet);
            foreach (var s in pokes)
                Console.WriteLine(s);
            Console.ReadKey();

        }
    }
}
