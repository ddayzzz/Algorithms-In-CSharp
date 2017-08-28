using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

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
}
