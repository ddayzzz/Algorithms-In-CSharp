using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
namespace Algorithms_In_CSharp.Chapter1
{
    //静态方法实现
    class BasicStaticMethodImpl
    {
        //应用牛顿迭代法
        public static double Sqrt(double c)
        {
            if (c < 0) throw new ArgumentException("参数的值小于0");
            double err = 1e-15;
            double t = c;
            while(Math.Abs(t-c/t) > err *t)
            {
                t = (c / t + t) / 2.0;
            }
            return t;
        }
        public static void Main()
        {
            //sqrt
            Console.WriteLine(Sqrt(0));
            //格式化输出 ref http://www.cnblogs.com/flyingbread/archive/2007/01/18/620287.html
            int i = 123456;
            Console.WriteLine("{0:C}", i); // ￥123,456.00
            Console.WriteLine("{0:D}", i); // 123456
            Console.WriteLine("{0:E}", i); // 1.234560E+005
            Console.WriteLine("{0:F5}", i); // 123456.00
            Console.WriteLine("{0:G}", i); // 123456
            Console.WriteLine("{0:N}", i); // 123,456.00
            Console.WriteLine("{0:P}", i); // 12,345,600.00 %
            Console.WriteLine("{0:X}", i); // 1E240
            Console.ReadKey();
            

        }
    }
    //绘图
    class Drawing
    {
        //画直线
        private static void DrawPaintHandler(object sender,PaintEventArgs paintEventArgs)
        {
            Graphics graph = paintEventArgs.Graphics;
            Pen pen = new Pen(Color.Blue, 2);
            graph.DrawLine(pen, 10, 10, 100, 100);
        }
        //画图形
        private static void DrawShape(object sender,PaintEventArgs e)
        {
            //ref http://blog.csdn.net/u010763324/article/details/40683107
            Graphics graph = e.Graphics;
            Pen pen = new Pen(Color.Blue, 2);
            Point point1 = new Point(0, 0);
            point1.X = 100;
            point1.Y = 100; 
            graph.FillEllipse(Brushes.Black, point1.X, point1.Y, 5, 5);//画一个椭圆（包括圆）
        }
        static void Main()
        {

            //Form form = new Form();
            ////form.Paint += DrawPaintHandler;
            //form.Paint += DrawShape;
            //form.ShowDialog();

        }
    }
    //1.1.10 二分查找
    class BinarySearch
    {
        public static int rank(int key,int[] a)
        {
            int lo = 0;
            int hl = a.Length - 1;
            while(lo <= hl)
            {
                int mid = (hl - lo) / 2;
                if (key < a[mid])
                    hl = mid - 1;
                else if (key > a[mid])
                    lo = mid + 1;
                else return mid;
            }
            return -1;
        }

    }
    
    namespace BagDemo
    {
        //1.3.1.4 背包实现
        class Bag<Item> : IEnumerable<Item>
        {
            //迭代接口
            private List<Item> data = new List<Item>();
            public Bag()
            {
            }
            public void Add(Item item)
            {
                data.Add(item);
            }

            public IEnumerator<Item> GetEnumerator()
            {
                return data.GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return data.GetEnumerator();
            }


        }
        class App
        {
            public static void Main()
            {
                Bag<double> bag = new Bag<double>();
                for (int i = 0; i < 5; ++i)
                    bag.Add(i);
                double result = 0.0;
                foreach (double item in bag)
                {
                    result += item;
                }
                Console.WriteLine(result);
                Console.ReadKey();
            }
        }
    }
    //算法1.1 下压LIFO 栈
    namespace ResizeableLIFOStackDemo
    {
        //迭代器的实现
        class ResizeableLIFOStackIteratorImpl<Item> : IEnumerator<Item>
        {
            private int position;
            private Item[] data;
            public ResizeableLIFOStackIteratorImpl(Item[] data,int currSize)
            {
                this.data = data;
                this.position = currSize;
            }
            public Item Current => data[position];

            object IEnumerator.Current => data[position];

            public void Dispose()
            {
                
            }

            public bool MoveNext()
            {
                --position;
                return position >= 0;
            }

            public void Reset()
            {
                position = -1;
            }
        }
        class ResizeableLIFOStack<Item>:IEnumerable<Item>
        {
            
            //默认放2个元素
            private Item[] data = new Item[2];
            private int N=0;//保存当前的大小（可以表示尾后位置）
            //私有方法，调整栈大小
            private void resize(int newSize)
            {
                Item[] newData = new Item[newSize];
                for (int i = 0; i < N; ++i)
                    newData[i] = data[i];
                data = newData;
            }
            //是否已满
            public bool IsFull()
            {
                return data.Length == N;
            }
            //是否已空
            public bool IsEmpty()
            {
                return N == 0;
            }
            //压栈
            public void Push(Item item)
            {
                if (IsFull())
                    resize(N * 2);
                data[N++] = item;
            }
            //弹出元素
            public Item Pop()
            {
                Item item;
                if (!IsEmpty())
                {
                    item = data[--N];
                    data[N] = default(Item);
                    return item;
                }
                
                else
                    throw new Exception("不能在空栈上弹出元素");
                    
            }

            public IEnumerator<Item> GetEnumerator()
            {
                return new ResizeableLIFOStackIteratorImpl<Item>(data,N);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new ResizeableLIFOStackIteratorImpl<Item>(data,N);
            }
        }
        class App
        {
            static void Main()
            {
                ResizeableLIFOStack<int> data = new ResizeableLIFOStack<int>();
                foreach (int i in Enumerable.Range(0, 11))
                {
                    data.Push(i);
                }
                foreach(var item in data)
                {
                    Console.WriteLine(item);
                }
                Console.ReadKey();
            }
        }
    }
    //算法1.3 FIFO队列实现（链式）
    namespace FIFOQueueDemo
    {
        
        public class ListQueue<Item>:IEnumerable<Item>
        {
            //节点定义
            public class Node
            {
                public Node Next;
                public Item Data { get; set; }
                public Node(Item data)
                {
                    this.Data = data;
                }
                public Node()
                {
                    this.Data = default(Item);
                }
            }
            //保存大小
            private int currentSize = 0;
            //是否为空
            public bool IsEmpty()
            {
                return currentSize == 0;
            }
            //对头
            private Node head=null;
            //队尾
            private Node tail=null;
            //入队
            public void EnQueue(Item item)
            {
                Node node = new Node(item);
                if (IsEmpty())
                {
                    head = node;
                    tail = node;
                }
                else
                {
                    node.Next = null;
                    tail.Next = node;
                    tail = node;
                }
                ++currentSize;
            }
            //出队
            public Item DeQueue()
            {
                if(!IsEmpty())
                {
                    Item item = head.Data;
                    head = head.Next;
                    --currentSize;
                    return item;
                }
                throw new Exception("没有元素可以出队");
            }

            public IEnumerator<Item> GetEnumerator()
            {
                return new ListQueueIterator(head);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new ListQueueIterator(head);
            }

            public class ListQueueIterator : IEnumerator<Item>
            {
                //迭代的属性
                //遍历的开始的节点
                public Node current;
                //构造函数
                public ListQueueIterator(Node head)
                {
                    this.current = new Node();
                    //由于缺少表头。但是MoveNext在迭代之前会调用一次确定是否能够进行遍历
                    current.Next = head;//head是直接包含数据的
                }
                public Item Current => current.Data;

                object IEnumerator.Current => current.Data;

                public void Dispose()
                {
                    //throw new NotImplementedException();
                }

                public bool MoveNext()
                {
                    current = current.Next;
                    return current != null;
                }

                public void Reset()
                {
                    current = null;
                }
            }
        }
        class App
        {
            static void Main()
            {
                ListQueue<int> queue = new ListQueue<int>();
                foreach (int i in Enumerable.Range(1, 8))
                    queue.EnQueue(i);
                foreach (var item in queue)
                    Console.WriteLine(item);
                foreach (int i in Enumerable.Range(1, 9))
                    queue.DeQueue();
                foreach (var item in queue)
                    Console.WriteLine(item);
                Console.ReadKey();
            }
        }

    }
    //提高题1.3.33 双向链表实现双向队列
    namespace DequeDemo
    {
        class Deque<Item>:IEnumerable<Item>
        {
            class Node
            {
                public Node prior =null;//前驱结点
                public Node next =null;//后继节点
                public Item Data { get; set; }
            }
            //迭代器:从左往右
            class Iterator : IEnumerator<Item>
            {
                //定义表头表示结束
                public Node head;
                public Node current;
                //构造函数
                public Iterator(Node head)
                {
                    this.head = head;
                    this.current = head;
                }
                public Item Current => current.Data;
                object IEnumerator.Current => current.Data;

                public void Dispose()
                {
                    //throw new NotImplementedException();
                }

                public bool MoveNext()
                {
                    current = current.next;
                    return current != head;
                }

                public void Reset()
                {
                    head = null;
                    current = null;
                }
            }
            //表头
            Node head;
            //是否为空
            public bool IsEmpty()
            {
                return currentSize == 0;
            }
            //构造函数
            public Deque()
            {
                this.head = new Node();
                //设置初始状态
                this.head.next = this.head;
                this.head.prior = this.head;
            }
            //当前的大小
            int currentSize = 0;
            //左入队
            public void PushLeft(Item item)
            {
                //注意替换表头
                Node newNode = new Node();
                newNode.Data = item;
                if(IsEmpty())
                {
                    newNode.next = this.head;
                    newNode.prior = this.head;
                    this.head.prior = newNode;
                    this.head.next = newNode;
                }
                else
                {
                    newNode.next = this.head.next;
                    newNode.prior = this.head;
                    this.head.next.prior = newNode;
                    this.head.next = newNode;
                }
                
                ++currentSize;
            }
            //右入队
            public void PushRight(Item item)
            {
                Node newNode = new Node();
                newNode.Data = item;
                newNode.prior = this.head.prior;
                newNode.next = this.head;
                this.head.prior.next = newNode;
                this.head.prior = newNode;
                ++currentSize;
            }
            //左出队
            public Item PopLeft()
            {
                if (!IsEmpty())
                {
                    Item left = this.head.next.Data;
                    this.head.next.next.prior = this.head;
                    this.head.next = this.head.next.next;
                    --currentSize;
                    return left;
                }
                throw new Exception("没有元素可以左出队");
            }
            //右出队
            public Item PopRight()
            {
                if (!IsEmpty())
                {
                    Item right = this.head.prior.Data;
                    this.head.prior.prior.next = this.head;
                    this.head.prior = this.head.prior.prior;
                    
                    --currentSize;
                    return right;
                }
                throw new Exception("没有元素可以右出队");
            }

            public IEnumerator<Item> GetEnumerator()
            {
                return new Iterator(head);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Iterator(head);
            }
        }
        class App
        {
            static void Main()
            {
                Deque<int> deque = new Deque<int>();
                Deque<int> empty = new Deque<int>();
                foreach (int i in Enumerable.Range(1, 10))
                    deque.PushRight(i);
                //foreach (int i in Enumerable.Range(1, 10))
                //    Console.WriteLine(deque.PopRight());
                foreach(var item in deque)
                {
                    Console.WriteLine(item);
                }
                Console.ReadKey();
            }
        }
    }
    //提高题1.3.39 环形缓冲区 https://zh.wikipedia.org/wiki/%E7%92%B0%E5%BD%A2%E7%B7%A9%E8%A1%9D%E5%8D%80
    //full的判断条件 R-H+1==0(R H是队尾和对头)
    //1.4算法分析
    namespace AlgorithmAnalysisDemo
    {
        //ref http://lxy.me/c-code-run-time.html
        using System.Threading;
        using System.Diagnostics;//引用相关的命名空间
        using System.Runtime.InteropServices;

        //计数器类
        static class Timer
        {
            /// <summary>
            /// 支持action类型（返回调用时间。单位ms）
            /// </summary>
            /// <param name="action">Action类型</param>
            /// <returns></returns>
            public static long Count(Action action)
            {
                Stopwatch Stopwatch = new Stopwatch();
                Stopwatch.Start();
                action();
                Stopwatch.Stop();
                return Stopwatch.ElapsedMilliseconds;
            }

        }
        class App
        {
            [StructLayout(LayoutKind.Sequential)]
            class I
            {
                int i;
            }
            //方法例子：
            public static void Sleep()
            {
                Thread.Sleep(3000);
            }
            public static void Main()
            {
                Console.WriteLine(Timer.Count(Sleep));
                Console.ReadKey();
            }
        }
    }
    //算法1.5 并查集
    namespace UnionFindDemo
    {
        class UnionFind
        {
            //保存所有触点的一维数组
            private int[] id;
            //各个触点所对应的连通分量的大小
            private int[] sz;
            //保存触点的大小
            private int count;
            public UnionFind(int size)
            {
                id = new int[size];
                sz = new int[size];
                for (int i = 0; i < size; ++i)
                {
                    id[i] = i;
                    sz[i] = 1;//自身连接的分量大小
                }
                count = size;
            }
            //将p、q连接
            public void Union(int p,int q)
            {
                int pID = Find(p);
                int qID = Find(q);
                if (pID == qID) return;
                for (int i = 0; i < id.Length; ++i)
                    if (id[i] == pID)
                        id[i] = qID;
                count--;
            }
            //获取p所在的分量
            public int Find(int p)
            {
                return id[p];
            }
            //判断p、q是否连接
            public bool IsConnected(int p,int q)
            {
                return Find(p) == Find(q);
            }
            public int Count()
            {
                return count;
            }
            //快速
            public int Find_QuickUnion(int p)
            {
                while (p != id[p])
                    p = id[p];
                return p;
            }
            public void Union_QuickUnion(int p,int q)
            {
                int pRoot = Find_QuickUnion(p);
                int qRoot = Find_QuickUnion(q);
                if(pRoot !=qRoot)
                {
                    id[pRoot] = qRoot;//将q的子树转移到右子树
                    --count;
                }
            }
            //归并树(加权)
            public void Union_UnionSubTree(int p,int q)
            {
                int pRoot = Find_CompressPath(p);
                int qRoot = Find_CompressPath(q);//使用路径压缩算法
                if(pRoot !=qRoot)
                {
                    if(sz[pRoot] > sz[qRoot])
                    {
                        id[qRoot] = pRoot;
                        sz[pRoot] += sz[qRoot];
                        
                    }
                    else
                    {
                        id[pRoot] = qRoot;
                        sz[qRoot] += sz[pRoot];
                    }
                    --count;
                }
            }
            //Find路径压缩
            public int Find_CompressPath(int p)
            {
                if(id[p] ==p)
                {
                    //基本情况 如果已经找到了根节点，就设置然后返回根节点，赋值给路径上的点
                    return p;
                }
                return id[p]=Find_CompressPath(id[p]);
            }
        }
        class App
        {
            //实现1.5.2.1
            static void Main()
            {
                int totalIDs = 50000000;
                int unions = 200000;
                int offset = unions / 2;
                UnionFind uf = new UnionFind(totalIDs);
                UnionFind uf_quick = new UnionFind(totalIDs);//使用快速连接
                UnionFind uf_quick_compress = new UnionFind(totalIDs);//使用路径压缩和快速连接
                //uf.Union(0, 1);
                //uf.Union(0, 5);
                //uf.Union(2, 6);
                //uf.Union(6, 7);
                //uf.Union(3, 8);
                //uf.Union(3,9);
                //uf.Union(4, 3);
                //uf.Union_UnionSubTree(0, 1);
                //uf.Union_UnionSubTree(0, 5);
                //uf.Union_UnionSubTree(2, 6);
                //uf.Union_UnionSubTree(6, 7);
                //uf.Union_UnionSubTree(3, 8);
                //uf.Union_UnionSubTree(3, 9);
                //uf.Union_UnionSubTree(4, 3);
                int[] links = Common.GenerateRadomData(unions,0, totalIDs);
                Stopwatch Stopwatch1 = new Stopwatch();
                Stopwatch1.Start();
                for (int i = 0; i < offset; ++i)
                    ;//uf.Union(links[i], links[i + offset]);
                Stopwatch1.Stop();
                Console.WriteLine("没有优化Use :" + Stopwatch1.ElapsedMilliseconds.ToString());
                Console.WriteLine(uf.Count());
                //quick-union
                Stopwatch Stopwatch2 = new Stopwatch();
                Stopwatch2.Start();
                for (int i = 0; i < offset; ++i)
                    uf_quick.Union_QuickUnion(links[i], links[i + offset]);
                Stopwatch2.Stop();
                Console.WriteLine("快速union Use :" + Stopwatch2.ElapsedMilliseconds.ToString());
                Console.WriteLine(uf_quick.Count());
                //quick-union和压缩路径
                Stopwatch Stopwatch3 = new Stopwatch();
                Stopwatch3.Start();
                for (int i = 0; i < offset; ++i)
                    uf_quick_compress.Union_UnionSubTree(links[i], links[i + offset]);
                Stopwatch3.Stop();
                Console.WriteLine("快速union+路径压缩 use :" + Stopwatch3.ElapsedMilliseconds.ToString());
                Console.WriteLine(uf_quick_compress.Count());
                Console.ReadKey();
            }
            
        }
    }
}
