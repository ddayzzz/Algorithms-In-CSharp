using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Algorithms_In_CSharp.Context.BTree
{
    /// <summary>
    /// B 树的集合实现
    /// </summary>
    class BTree<TKey, TValue>
        where TKey : IComparable<TKey>
        where TValue : class
    {
        public readonly static int M = 16;//这个是 B树 的每个页的中键的数量的最大值
        private int m_height = 0;//B树的高度
        private Node m_root;//根键
        private int n;// B树的key-value 对数量
        public BTree()
        {
            m_root = new Node(0);
        }

        public bool Contains(TKey key)
        {
            return Search(m_root, key, m_height) != null;
        }


        public int Size()
        {
            return n;
        }
        public int Height() { return m_height; }
        public TValue Get(TKey key)
        {
            if (key == null) throw new ArgumentNullException("argument to Get() is null");
            return Search(m_root, key, m_height);//自顶向下
        }
        private TValue Search(Node x, TKey key, int ht)
        {
            Entry[] children = x.children;
            if(ht == 0)
            {
                for (int j = 0; j < x.m; ++j)
                    if (key.CompareTo(children[j].key) == 0)
                        return children[j].value;
            }
            else
            {
                //内部节点
                for (int j = 0; j < x.m; ++j)
                    if (j + 1 == x.m || key.CompareTo(children[j + 1].key) < 0)
                        return Search(children[j].next, key, ht - 1);// ht - 1 向下查找。注意 如果找到了小于的键值，注意这个是后一个，可能还有相等的
            }
            return null;//没有找到
        }
        /// <summary>
        /// 插入 key-value 到符号表 如果 value 是 null，那么就删除 key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("不能插入空键");
            Node u = Insert(m_root, key, value, m_height);
            ++n;
            if (u == null)
                return;//插入操作没有分解
            // 可能需要向上分解
            Node t = new Node(2); // 上层分出两个节点
            t.children[0] = new Entry(m_root.children[0].key, null, m_root);// u 是分出来的后 M /2的部分。需要把他的最小的键给分出来的
            t.children[1] = new Entry(u.children[0].key, null, u);//
            m_root = t;
            ++m_height;
        }

        private Node Split(Node h)
        {
            Node t = new Node(M / 2);
            h.m = M / 2;
            for (int j = 0; j < M / 2; ++j)
                t.children[j] = h.children[M / 2 + j];//拷贝后面的部分
            return t;//返回后面的 M/2 部分
        }
        private Node Insert(Node h, TKey key, TValue value, int ht)
        {
            int j;
            Entry t = new Entry(key, value, null);
            //外部节点。也就是叶子
            if (ht == 0)
            {
                for (j = 0; j < h.m; ++j)
                    if (key.CompareTo(h.children[j].key) < 0)
                        break;//j 更新为比key小的位置
            }
            else
            {
                // 内部节点
                for(j=0;j<h.m;++j)
                {
                    if((j+1 == h.m)/*是否是最后一个元素到了*/ || key.CompareTo(h.children[j+1].key) < 0)// key < j+1位置的键值
                    {
                        Node u = Insert(h.children[j++].next, key, value, ht - 1); // 在他的上界范围内插入，注意还要向下插入到关键字的指针
                        if (u == null)
                            return null;//不需要分解
                        t.key = u.children[0].key; // 经过了分解。此时的 u 是后面的一部分。按照 P569 所示。从后半部分选择最小的作为分解出来的一个节点的最小。
                        t.next = u;//将分出的节点设置新的连接
                        break;
                    }
                }
            }
            // 这个是数组的插入操作，需要保持顺序所以向后移动一位
            for (int i = h.m; i > j; --i)
                h.children[i] = h.children[i - 1];
            h.children[j] = t;
            h.m++;
            if (h.m < M)
                return null;
            else
                return Split(h);//不满足条件了需要向上分解
        }

        public override String ToString()
        {
            return ToString(m_root, m_height, "") + "\n";
        }

        private String ToString(Node h, int ht, String indent)
        {
            StringBuilder s = new StringBuilder();
            Entry[] children = h.children;

            if (ht == 0)
            {
                for (int j = 0; j < h.m; j++)
                {
                    s.Append(indent + children[j].key + " " + children[j].value + "\n");//根节点
                }
            }
            else
            {
                for (int j = 0; j < h.m; j++)
                {
                    if (j > 0) s.Append(indent + "(" + children[j].key + ")\n");
                    s.Append(ToString(children[j].next, ht - 1, indent + "     "));
                }
            }
            return s.ToString();
        }
        /// <summary>
        /// B树中的节点：内部和外部
        /// </summary>
        class Node
        {
            public int m;//存在的节点词条的数量
            public Entry[] children = new Entry[M];//最多 M 个键，M + 1 指向
            public Node(int k)
            {
                m = k;
            }
        }

        /// <summary>
        /// 类似于从 Node 伸出去的用于连接的桥梁，类似于列表中的 Node，在 B树中，假设有 M 个键，那么就会有 M + 1个链接指向子树
        /// 对于内部节点：使用 key 和 next
        /// 对于外部节点：使用 key 和 value
        /// </summary>
        class Entry
        {
            public TKey key;
            public readonly TValue value;
            public Node next;
            public Entry(TKey key, TValue value, Node next)
            {
                this.key = key;
                this.value = value;
                this.next = next;
            }
        }
    }
    class BTreeTest
    {
        public static void Main()
        {
            BTree<String, String> st = new BTree<String, String>();

            st.Put("www.cs.princeton.edu", "128.112.136.12");
            st.Put("www.cs.princeton.edu", "128.112.136.11");
            st.Put("www.princeton.edu", "128.112.128.15");
            st.Put("www.yale.edu", "130.132.143.21");
            st.Put("www.simpsons.com", "209.052.165.60");
            st.Put("www.apple.com", "17.112.152.32");
            st.Put("www.amazon.com", "207.171.182.16");
            st.Put("www.ebay.com", "66.135.192.87");
            st.Put("www.cnn.com", "64.236.16.20");
            st.Put("www.google.com", "216.239.41.99");
            st.Put("www.nytimes.com", "199.239.136.200");
            st.Put("www.microsoft.com", "207.126.99.140");
            st.Put("www.dell.com", "143.166.224.230");
            st.Put("www.slashdot.org", "66.35.250.151");
            st.Put("www.espn.com", "199.181.135.201");
            st.Put("www.weather.com", "63.111.66.11");
            st.Put("www.yahoo.com", "216.109.118.65");


            Console.WriteLine("cs.princeton.edu:  " + st.Get("www.cs.princeton.edu"));
            Console.WriteLine("hardvardsucks.com: " + st.Get("www.harvardsucks.com"));
            Console.WriteLine("simpsons.com:      " + st.Get("www.simpsons.com"));
            Console.WriteLine("apple.com:         " + st.Get("www.apple.com"));
            Console.WriteLine("ebay.com:          " + st.Get("www.ebay.com"));
            Console.WriteLine("dell.com:          " + st.Get("www.dell.com"));
            Console.WriteLine();

            Console.WriteLine("size:    " + st.Size());
            Console.WriteLine("height:  " + st.Height());
            Console.WriteLine(st);
            Console.WriteLine();
        }
    }
    class BTreeIP
    {
        public static void Main(String[] args)
        {
            //读取 CSV IP 地址列表
            BTree<string, string> ips = new BTree<string, string>();
            string file = "IP.csv";
            BufferedStream buffered = new BufferedStream(File.OpenRead(file));
            StreamReader reader = new StreamReader(buffered);
            while(!reader.EndOfStream)
            {
                String[] data = reader.ReadLine().Split(',');
                ips.Put(data[0], data[1]);
            }
            string input;
            do
            {
                Console.Write("查找的内容(exit 表示退出;print 打印树)：");
                input = Console.ReadLine();
                if (input == "exit")
                    break;
                else if (input == "print")
                {
                    Console.WriteLine(ips.ToString());
                    Console.WriteLine("Height:{0}, Pairs:{1}", ips.Height(), ips.Size());
                    continue;
                }
                string res = ips.Get(input);
                if (res == null)
                    Console.WriteLine("没有结果");
                else
                    Console.WriteLine(res);
            } while (true);
        }
    }
}
