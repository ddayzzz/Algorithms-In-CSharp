
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
            public static void Sort<T>(T[] a)
                where T : IComparable<T>
            { }
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
            //归并排序非递归算法
            public static void SortNoRecuring(int[] a)
            {
                //拷贝临时的数据
                aux = new int[a.Length];
                int N = a.Length;
                int seg, start;
                for(seg=1;seg < N;seg=seg+seg)
                {
                    // for(start=0;)
                }
            }
        }
        //快速排序
        class QuickSort:SortTemplate
        {
            public new static void Sort<T>(T[] a)
                where T : IComparable<T>
            {
                Sort(a, 0, a.Length - 1);
            }
            private static void Sort<T>(T[] a,int lo,int hi)
                where T:IComparable<T>
            {
                if (lo >= hi)
                    return;
                int j = partition(a, lo, hi);//将数组分为a[lo...j)和(j...hi]两个部分，a[j]是中间的值
                Sort(a, lo, j - 1);//插入位置j之前有序
                Sort(a, j + 1, hi);//j之后有序
            }
            private static int partition<T>(T[] a,int lo,int hi)
                where T:IComparable<T>
            {
                int pivot = lo ;//设置比较的元素的索引
                int i = pivot+1;
                int j = hi;
                while(true)
                {
                    for (; i <= hi && Less(a[i], a[pivot]); ++i)
                        ;
                    for (; j >= pivot && Less(a[pivot], a[j]); --j)
                        ;
                    if (i >= j)
                        break;
                    Exchange(a, i, j);
                }
                Exchange(a, pivot, j);
                return j;

            }
            //三向切分快速排序：适合于很多重复的元素的数组。降低了复杂度
            public static void Sort_Dijkstra<T>(T[] a,int lo,int hi)
                where T:IComparable<T>
            {
                if (hi <= lo)
                    return;
                int lt = lo, i = lo + 1, gt = hi;
                T pivot = a[lo];
                while(i<=gt)
                {
                    int cmp = a[i].CompareTo(pivot);//i与切分前的元素进行比较
                    if (cmp < 0)
                        Exchange(a, i++, lt++);//将比pivot小的元素放到<=lt的位置
                    else if (cmp > 0)
                        Exchange(a, i, gt--);//将大于pivot的元素放到>=gt的位置
                    else
                        ++i;
                }
                Sort_Dijkstra(a, lo, lt - 1);
                Sort_Dijkstra(a, lt + 1, hi);
            }
            //快速三向切分 Bently-Mcllroy算法

        }
        //实现一个最大二叉堆
        class MaxBinaryHeap<Key>
            where Key:IComparable
        {
            private Key[] pq;//基于堆的二叉树
            private int currentSize;//存储与pq[0...N]的数据
            public MaxBinaryHeap(int size)
            {
                currentSize = 0;
                pq = new Key[size+1];
            }
            //是否为空
            public bool IsEmpty() { return currentSize == 0; }
            //插入元素
            public void Insert(Key key)
            {
                if(pq.Length == currentSize)
                {
                    //调整元素
                    Key[] newPq = new Key[2*(pq.Length - 1)+1];
                    for (int i = 1; i <= currentSize; ++i)
                        newPq[i] = pq[i];
                    pq = newPq;
                }
                pq[++currentSize] = key;
                swim(currentSize);

            }
            //删除元素
            public Key DeleteMax()
            {
                if(IsEmpty()==false)
                {
                    Key data = pq[1];
                    Exchange(1, currentSize--);
                    sink(1);
                    //pq[currentSize--] = null;
                    //值类型不需要设置null
                    return data;
                }
                throw new Exception("没有元素可以删除");
            }
            //排序比较
            public bool Less(int i,int j)
            {
                return pq[i].CompareTo(pq[j]) < 0;
            }
            //交换元素
            public void Exchange(int i,int j)
            {
                if(i !=j)
                {
                    Key temp = pq[i];
                    pq[i] = pq[j];
                    pq[j] = temp;
                }
            }
            //下沉：将索引为k的元素下沉
            private void sink(int k)
            {
                //int minchild = k * 2;//左子节点
                while(k*2 <= currentSize)
                {
                    int minchild = k * 2;
                    if (minchild < currentSize && Less(minchild, minchild + 1))
                        ++minchild;
                    if (!Less(k, minchild))
                    {
                        break;
                    }
                    Exchange(minchild, k);
                    k = minchild;
                }
            }
            //上浮：将索引为k的元素上浮
            private void swim(int k)
            {
                
                while(k > 1 && Less(k/2,k))
                {
                    Exchange(k / 2, k);
                    k = k / 2;
                }
            }
        }
        //关联索引的泛型优先队列API
        //参考了：http://www.cnblogs.com/nullzx/p/6624731.html
        public class IndexMaxPriorityQueue<T>
            where T:class,IComparable//限定存储值类型。以后应该可以定义class类型的
        {
            private int[] pq;//保存索引
            private int[] qp;//逆序：qp[pq[i]] = pq[qp[i]] = i
            private int currentSize;//元素的数量
            private T[] keys;//右优先级之分的元素
            //初始化大小
            public IndexMaxPriorityQueue(int size)
            {
                keys = new T[size + 1];
                pq = new int[size + 1];
                qp = new int[size + 1];
                for (int i = 0; i <= size; ++i)
                    qp[i] = -1;
            }
            //插入元素
            public void Insert(int k,T key)
            {
                currentSize++;
                qp[k] = currentSize;//保存k索引在pq的位置
                pq[currentSize] = k;
                keys[k] = key;
                swim(currentSize);
            }
            //将索引k的元素设置为item
            public void Change(int k,T item)
            {
                //调整周围的元素
                keys[k] = item;
                swim(qp[k]);
                sink(qp[k]);
            }
            //检查索引是否包含元素
            public bool Contains(int k) { return qp[k] != -1; }
            //删除索引k以及元素
            public void Delete(int k)
            {
                int index = qp[k];
                Exchange(index, currentSize--);
                swim(index);
                sink(index);
                qp[k] = -1;
            }
            //返回最小的元素
            public T Min()
            {
                return keys[pq[1]];
            }
            //返回最小元素的索引
            public int MinIndex() { return pq[1]; }
            //删除最小的元素，并返回索引
            public int DeleteMin()
            {
                int indexMin = pq[1];
                Exchange(1, currentSize--);
                sink(1);
                qp[pq[currentSize + 1]] = -1;
                return indexMin;
            }
            //优先队列是否为空
            public bool IsEmpty()
            {
                return currentSize == 0;
            }
            //优先队列的元素数量
            public int Size() { return currentSize; }
            private void sink(int k)
            {
                //int minchild = k * 2;//左子节点
                while (k * 2 <= currentSize)
                {
                    int minchild = k * 2;
                    if (minchild < currentSize && Less(minchild, minchild + 1))
                        ++minchild;
                    if (!Less(k, minchild))
                    {
                        break;
                    }
                    Exchange(minchild, k);
                    k = minchild;
                }
            }
            //上浮：将索引为k的元素上浮
            private void swim(int k)
            {

                while (k > 1 && Less(k / 2, k))
                {
                    Exchange(k / 2, k);
                    k = k / 2;
                }
            }
            //交换
            private void Exchange(int i,int j)
            {
                int temp = pq[i];
                pq[i] = pq[j];
                pq[j] = temp;
                qp[pq[i]] = i;
                qp[pq[j]] = j;
            }
            //比较
            private bool Less(int i,int j)
            {
                //Less需要比较keys中保存的变量
                return keys[pq[i]].CompareTo(keys[pq[j]]) < 0;
            }
            //使用有些队列多项归并
            public class Multiway
            {
                public static void Merge(StreamReader[] streams)
                {
                    int N = streams.Length;
                    IndexMaxPriorityQueue<string> pq = new IndexMaxPriorityQueue<string>(N);//最大堆，从大到小排序
                    for (int i = 0; i < N; ++i)
                        if (!streams[i].EndOfStream)
                            pq.Insert(i, streams[i].ReadLine());//i是保存的元素的流在数组的索引

                    while (!pq.IsEmpty())
                    {
                        Console.WriteLine(pq.Min());
                        int i = pq.DeleteMin();
                        if (!streams[i].EndOfStream)
                        {
                            pq.Insert(i, streams[i].ReadLine());//替换为新的值
                        }
                    }
                }
                public static void Main(string[] args)
                {
                    int N = args.Length;
                    StreamReader[] st = new StreamReader[N];
                    for (int i = 0; i < N; ++i)
                        st[i] = new StreamReader(File.OpenRead(args[i]));
                    Merge(st);
                    for (int i = 0; i < N; ++i)
                        st[i].Dispose();
                    Console.ReadKey();
                }
            }
        }
        //使用堆排序从小到大
        class HeapSort
        {
            //下沉：将索引为k的元素下沉
            private static void sink<T>(T[] a,int k,int N)
                where T:IComparable
            {
                //int minchild = k * 2;//左子节点
                while (k * 2 <= N)
                {
                    int minchild = k * 2;
                    if (minchild < N && Less(a,minchild, minchild + 1))
                        ++minchild;
                    if (!Less(a,k, minchild))
                    {
                        break;
                    }
                    Exchange(a,minchild, k);
                    k = minchild;
                }
            }
            //排序比较
            private static bool Less<T>(T[] pq,int i, int j)
                where T:IComparable
            {
                return pq[i].CompareTo(pq[j]) < 0;
            }
            //交换元素
            private static void Exchange<T>(T[] pq,int i, int j)
            {
                if (i != j)
                {
                    T temp = pq[i];
                    pq[i] = pq[j];
                    pq[j] = temp;
                }
            }
            //排序
            public static void Sort<T>(T[] a)
                where T:IComparable
            {
                int N = a.Length-1;//由于是从1开始的需要减少一个
                for (int k = N / 2; k >= 1; --k)
                    sink(a, k, N);
                while(N>1)
                {
                    Exchange(a, 1, N--);
                    sink(a, 1, N);
                }
            }
            //显示排序结果
            public static void Show<T>(T[] a)
            {
                for (int i = 1; i < a.Length; ++i)
                    Console.Write(a[i] + " ");
            }
            //是否按照顺序
            public static bool IsSorted<T>(T[] a)
                where T:IComparable
            {
                for (int i = 2; i < a.Length; ++i)
                    if (Less(a,i,i-1))
                        return false;
                return true;
            }
        }
        //计算Kendall tau距离 https://en.wikipedia.org/wiki/Kendall_tau_distance
        class Kendalltau
        {
            //N个从0~N-1的不重复元素的逆序数
            //简单的写法
            public static int GetKendalltauDistance1(int[] a,int[] b)
            {
                int N = a.Length;
                int[] ainv = new int[N];
                for (int i = 0; i < N; ++i)
                    ainv[a[i]] = i;//ainv代表a[i]元素的在a[]中的出现的位置
                int[] bnew = new int[N];
                for (int i = 0; i < N; ++i)
                    bnew[i] = ainv[b[i]];//b中的元素在相对于出现在a中的位置
                //return Count_1(bnew);
                counter = 0;
                aux = new int[N];
                Sort(bnew, 0, N - 1);
                return counter;

            }
            //2.使用插入排序计算逆序数 http://blog.csdn.net/zhufenghao/article/details/49594035
            public static int Count_1(int[] bnew)
            {
                //b相对于a的位置
                int dis = 0;
                for(int i=0;i<bnew.Length;++i)
                {
                    for(int j=i;j>0 && bnew[j-1] > bnew[j];--j)//如果是逆序的（也就是说对于公共的元素，在b中的位置（即i）与在a的位置(即bnew[j])不同。）
                    {
                        ++dis;
                        int temp = bnew[j];
                        bnew[j] = bnew[j - 1];
                        bnew[j - 1] = temp;
                    }
                }
                return dis;
            }
            //3.归并排序计算逆序数
            public static void Sort(int[] bnew,int lo,int hi)
            {
                if (lo >= hi)
                    return;
                int mid = lo + (hi - lo) / 2;
                Sort(bnew, lo, mid);
                Sort(bnew, mid + 1, hi);
                if (bnew[mid] <= bnew[mid + 1])
                    return;
                DoMerge(bnew, lo, mid, hi);
            }
            //辅助数组
            private static int[] aux;
            //计算交换次数
            private static int counter = 0;
            private static void DoMerge(int[] bnew,int lo,int mid,int hi)
            {
                for (int k = lo; k <= hi; ++k)
                    aux[k] = bnew[k];
                int i = lo;
                int j = mid+1;
                for(int k=lo;k<=hi;++k)
                {
                    if (i > mid)
                        bnew[k] = aux[j++];
                    else if (j > hi)
                        bnew[k] = aux[i++];
                    else if (bnew[i] <= bnew[j])
                    { bnew[k] = aux[i++]; }
                    else
                    { bnew[k] = aux[j++]; counter += mid - k + 1; }//如果b中j对应的位置已经与a逆序了，那么他之前的元素（也就是前半段的元素从lo~i）也必然是逆序的。也就是（mid - k +1）个
                }
            }
        }
        //测试程序
        class App
        {
            static void Main(string[] args)
            {
                //int[] a = Common.GenerateRadomData(400, 0, 5000);
                //int[] copied_a=new int[400];
                //a.CopyTo(copied_a,0);
                //Shell.Sort(a);
                //Console.WriteLine(Shell.IsSorted(a));
                //Merge.SortBU(a);
                //Merge.Sort(a);//如果a是有序的，排序只消耗线性事件
                //Console.WriteLine(Merge.IsSorted(a));
                //a.Show();
                //QuickSort.Sort(a);
                //QuickSort.Sort_Dijkstra(a, 0, a.Length - 1);
                //Console.WriteLine(QuickSort.IsSorted(a));
                //a.Show();
                //最大二叉堆的测试
                //MaxBinaryHeap<int> maxBinaryHeap = new MaxBinaryHeap<int>(50000);
                //foreach(var item in a)
                //{
                //    maxBinaryHeap.Insert(item);
                //}
                ////读出数据
                //int pre = 5001;
                //bool correct = true;
                //foreach(int i in Enumerable.Range(0, 50000))
                //{
                //    int curr = maxBinaryHeap.DeleteMax();
                //    correct = correct && pre > curr;
                //    Console.Write(curr + " ");
                //}
                //Console.WriteLine(correct);
                //此时索引优先队列
                //IndexMaxPriorityQueue<string> idq = new IndexMaxPriorityQueue<string>(10);
                //idq.Insert(1, "k");
                //idq.Insert(6, "d");
                //idq.Insert(3, "f");
                //idq.Insert(4, "c");
                ////idq.Insert(1, "a");
                //while (idq.IsEmpty() == false)
                //{
                //    string minid = idq.Min();
                //    Console.WriteLine(idq.DeleteMin()+":"+minid);
                //}
                //多路归并
                //IndexMaxPriorityQueue<string>.Multiway.Main(args);
                //堆排序
                //HeapSort.Sort(a);
                //HeapSort.Show(a);
                //Console.WriteLine(HeapSort.IsSorted(a));
                //堆排序的应用：找出N个元素最大的K个。构建最小堆，如果元素>根节点。替换根节点然后下沉处理 参考 http://blog.csdn.net/xiaoding133/article/details/8037086
                //计算Kendalltau 逆序数
                int[] a1 = new int[] { 0, 3, 1, 6, 2, 5, 4 };
                int[] a2 = new int[] { 1, 0, 3, 6, 4, 2, 5 };
                int[] b1 = new int[] { 0, 1, 2, 3, 4 };
                int[] b2 = new int[] { 2,3,0,1,4 };
                Console.WriteLine(Kendalltau.GetKendalltauDistance1(a1, a2));
                Console.WriteLine(Kendalltau.GetKendalltauDistance1(b1, b2));
                Console.ReadKey();
            }
        }
    }
    namespace Quiz
    {
        //习题
        //2.5.32 八字迷问题
        //https://zh.wikipedia.org/wiki/A*%E6%90%9C%E5%AF%BB%E7%AE%97%E6%B3%95
        //A*： f(n)=h(n)+g(n) h(n)表示起点到n的距离。g(n)表示n到终点的距离预估距离。若h(n)=0表示的就是n的dijkstra算法的距离值。g(n)=0,则使用的是BFS
    }
}