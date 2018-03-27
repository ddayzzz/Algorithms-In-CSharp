using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.StringDemo.Sort
{
    class SortTest
    {
        public static void Main()
        {
            Alphabet alphabet = Alphabet.LOWERCASE;
            int length = 1000;
            int count = 100000;
            //String[] t1 = RandomStrings.GenerateRandomStrings(alphabet, length, count);
            String[] t1 = RandomStrings.GenerateRandomStrings_DifferentLength(alphabet, count, length);
            String[] t2 = t1;
            String[] t3 = t1;
            //CodeTime.CodeTimer.Time("低位优先：", 1, () =>//LSD 不适用于长的不规则的字符串之间
            //{
            //    LSD.Sort(t1, length, alphabet);
            //});
            //CodeTime.CodeTimer.Time("高位优先：", 1, () =>
            //{
            //    MSD.Sort(t2, alphabet);
            //});
            CodeTime.CodeTimer.Time("Array.Sort：", 1, () =>
            {
                Array.Sort(t2);
            });
            CodeTime.CodeTimer.Time("三向快速排序：", 1, () =>
            {
                TripleDirectedSort.Sort(t3, alphabet);
            });
            //输出排序结果
            //Console.WriteLine("{0,20}{1,20}{2,20}", "LSD", "Array.Sort", "TRI");
            //for(int i=0;i<count;++i)
            //{
            //    Console.WriteLine("{0,20}{1,20}{2,20}", t1[i], t2[i], t3[i]);
            //}
            bool res = true;
            for(int i=0;i<count && res;++i)
            {
                if (t1[i].Equals(t2[i]) && t1[i].Equals(t3[i]))
                    res = res && true;
                else
                    res = res && false;
            }
            Console.WriteLine("{0}", res);
            Console.ReadKey();
        }
    }
}
