using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.StringDemo.Sort
{
    /// <summary>
    /// 低位优先的排序算法：适合等长的小数组。
    /// </summary>
    class LSD
    {
        /// <summary>
        /// 通过前W个字符将a[] 排序
        /// </summary>
        /// <param name="a"></param>
        /// <param name="W"></param>
        /// <param name="alphabet">可以指定一个字符表。</param>
        public static void Sort(String[] a, int W, Alphabet alphabet)
        {
            int N = a.Length;
            int R = alphabet.R();
            String[] aux = new string[N];//辅助数组
            for(int d=W-1;d>=0;--d)
            {
                //按照d 位的顺序排序
                //获取这个字符的扩展ascii 的值。确定索引
                int[] count = new int[R+ 1];//统计频率。具有相同的字符的数量
                for (int i = 0; i < N; ++i)
                    ++count[alphabet.ToIndex(a[i][d]) + 1];//注意是统计给下一位的。
                //统计每一组（键相同的）第一个元素的位置
                for (int i = 0; i < R; ++i)
                    count[i + 1] += count[i];
                //元素分类
                for (int i = 0; i < N; ++i)
                    aux[count[alphabet.ToIndex(a[i][d])]++] = a[i];//将第i个字符复制到按照d位的字符分组的位置
                for (int i = 0; i < N; ++i)
                    a[i] = aux[i];//写回。下次按高位排序

            }
        }
        public static void Main()
        {
            Alphabet alphabet = Alphabet.LOWERCASE;
            String[] pokes = new string[]
            {
                "sheuu","sells","seash","byiri","theiu","seaio",
                "shore","thehe","shell","sherw","sells","arehh","surel","seash"
            };
            Sort(pokes, 5, alphabet);
            foreach (var s in pokes)
                Console.WriteLine(s);
            Console.ReadKey();

        }
    }
}
