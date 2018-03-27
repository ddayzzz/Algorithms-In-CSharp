using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.StringDemo.Sort
{
    /// <summary>
    /// 高位排序
    /// </summary>
    class MSD
    {
        private static int R;//保存基数
        private static readonly int M = 6;//小数组的判断阈值。如果小于这个数字，使用插入排序。
        private static string[] aux;//辅助数组
        private static Alphabet alphabet;//保存字母表
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
        /// <summary>
        /// 递归函数
        /// </summary>
        /// <param name="a">要排序的字符串</param>
        /// <param name="lo">下界</param>
        /// <param name="hi">上界</param>
        /// <param name="d">哪一位（从最高位）</param>
        private static void Sort(String[] a, int lo, int hi, int d)
        {
            if (hi <= lo + M)
            {
                //Console.WriteLine("[{0}, {1}]使用了快速排序", lo, hi);
                //使用插入排序
                Chapter2.SortDemos.QuickSort.Sort(a);
                return;
            }
            int[] count = new int[R + 2];//多一个。因为0的位置表示长度为d的字符串子数组的其实索引。
            for (int i = lo; i <= hi; ++i)
            {
                count[CharAt(a[i], d) + 2]++;//统计字符出现的频率
            }
            //转换索引
            for (int r = 0; r < R + 1; ++r)
            {
                count[r + 1] += count[r];
            }
            //将排序结果写入
            for (int i = lo; i <= hi; ++i)
                aux[count[CharAt(a[i], d) + 1]++] = a[i];
            for (int r = 0; r < R; ++r)
                Sort(a, lo + count[r], lo + count[r + 1] - 1, d + 1);// count[1] 的值可以确定要跳过只有d个长度的字符串；r+1是确定最后一组的相同键的最后一个元素的后一个位置，减去1就是上边界。;但是具有相同的键的一组满足>=1个元素就可以进入循环
        }
        /// <summary>
        /// 排序多个字符串
        /// </summary>
        /// <param name="a">字符串数组</param>
        /// <param name="alphabet">字母表</param>
        public static void Sort(String[] a, Alphabet alphabet)
        {
            R = alphabet.R();
            MSD.alphabet = alphabet;
            aux = new String[a.Length];
            Sort(a, 0, a.Length - 1, 0);
        }
        public static void Main()
        {
            Alphabet alphabet = Alphabet.LOWERCASE;
            String[] pokes = new string[]
            {
                "she","sells","seashells","by","the","sea",
                "shore","the","shells","she","sells","are","surely","seashells"
            };
            Sort(pokes, alphabet);
            foreach (var s in pokes)
                Console.WriteLine(s);
            Console.ReadKey();
        }
    }
}
