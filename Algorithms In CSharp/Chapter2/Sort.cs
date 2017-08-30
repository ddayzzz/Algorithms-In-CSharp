
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Algorithms_In_CSharp.Chapter2
{
    namespace  SortDemos
    {
        //扩展方法：显示数组元素
        public static class Extensions
        {
            public static void Show<T>(this T[] a)
            {
                for (int i = 0; i < a.Length; ++i)
                    Console.Write(a[i] + " ");
                Console.Write("\n");
            }
        }
        //所有比较的模板类
        class SortTemplate
        {
            //排序的接口
            public static void Sort<T>(T[] a) { }
            //less方法用于对元素的比较
            public static bool Less<T>(T v, T w)
                where T:IComparable<T>
            {
                return v.CompareTo(w) < 0;
            }
            //exchange方法交换元素的位置
            public static void Exchange<T>(T[] a,int i,int j)
            {
                T temp = a[i];
                a[i] = a[j];
                a[j] = temp;
            }
            //打印数组
            public static void Show<T>(T[] a)
            {
                for(int i=0;i<a.Length;++i)
                {
                    Console.Write(a[i] + " ");

                }
                Console.Write("\n");
            }
            //数组是否有序
            public static bool IsSorted<T>(T[] a)
                where T : IComparable<T>
            {
                for (int i = 1; i < a.Length; ++i)
                    if (Less(a[i], a[i - 1]))
                        return false;
                return true;
            }
        }
        //希尔排序
        class Shell: SortTemplate
        {
            //覆盖
            public new static void Sort<T>(T[] a)
                where T : IComparable<T>
            {
                int N = a.Length;
                int h = 1;
                while (h < N / 3)
                    h = 3 * h + 1;//选择间隔。这个间隔没有固定的值
                while(h>=1)
                {
                    for(int i=h;i<N;++i)
                    {
                        for(int j =i;j>=h && Less(a[j],a[j-h]);j-=h)
                        {
                            Exchange(a, j, j - h);
                        }
                        //一组以h为间隔的数组排序完成
                    }
                    h = h / 3;
                }
            }
        }
        //归并排序
        class Merge:SortTemplate
        {
            private static int[] aux;//辅助数组
            //假设数组a[lo,...,mid]和a[mid+1,hi]已经是有序的，那么就需要将两个元素按照从大到小的顺序选择放入目标数组a
            private static void DoMerge(int[] a,int lo,int mid,int hi)
            {
                int i = lo;
                int j = mid + 1;
                for (int k = lo; k <= hi; ++k)
                    aux[k] = a[k];//拷贝需要划分的数组到辅助数组
                for(int k=lo;k<=hi;++k)
                {
                    if (i > mid)
                        a[k] = aux[j++];//如果第一个数组不足了，就从右边选取元素（这个数组已经排过序了，且第一个数组至少小于第二个数组的第一个没有使用的元素）
                    else if (j > hi)
                        a[k] = aux[i++];//同上
                    else if (Less(aux[j], aux[i]))
                        a[k] = aux[j++];
                    else
                        a[k] = aux[i++];
                }
            }
            //将数组a[lo,...,hi]合并
            private static void Sort(int[] a,int lo,int hi)
            {
                if (hi <= lo)
                    return;//基本情况，已经将数组划分为连续的两个元素，并且排了序
                int mid = (hi - lo) / 2 + lo;
                Sort(a, lo, mid);
                Sort(a, mid + 1, hi);
                //假设两段子数组a[lo...mid]和[mid+1...hi]已经排序了
                if (!Less(a[mid+1], a[mid]))
                    return;//如果已经是有序（第一个数组的最大<=第二个最小使用!Less可以判断）
                DoMerge(a, lo, mid, hi);
            }
            public static void Sort(int[] a)
            {
                aux = new int[a.Length];
                Sort(a, 0, a.Length - 1);
            }
            //自底向上归并排序：划分子数组，然后两个两个合并
            public static void SortBU(int[] a)
            {
                int N = a.Length;
                aux = new int[N];
                for (int sz = 1; sz < N; sz = sz + sz)
                {
                    //每个子数组的元素个数为2^sz
                    for (int lo = 0; lo < N - sz/*最后一个数组的其实位置*/; lo += sz + sz/*跳转到当前长度的下一组*/)
                    {
                        DoMerge(a, lo, lo + sz - 1, Math.Min(lo + sz + sz - 1, N - 1)/*如果N不是2^N，那么可能会溢出*/);
                    }
                }
            }
        }
        //测试程序
        class App
        {
            static void Main()
            {
                int[] a = Common.GenerateRadomData(10000, 0, 5000);
                a.Show();
                //Shell.Sort(a);
                //Console.WriteLine(Shell.IsSorted(a));
                Merge.SortBU(a);
                //Merge.Sort(a);//如果a是有序的，排序只消耗线性事件
                Console.WriteLine(Merge.IsSorted(a));
                a.Show();
                Console.ReadKey();
            }
        }
    }
}