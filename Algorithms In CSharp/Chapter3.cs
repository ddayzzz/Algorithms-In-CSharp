using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingFunctionGraph;
using System.Drawing;
using System.Windows.Forms;

namespace Algorithms_In_CSharp
{
    namespace Chapter3
    {
        public interface IAdvancedSymbolTabletOpt<Key,Value>
            where Key : IComparable
        {
            //获取[lo...hi]之间的键的数量
            int Size(Key lo, Key hi);
            IEnumerable<KeyValuePair<Key, Value>> Keys(Key lo, Key hi);
            //表中的所有键的集合，已排序
            IEnumerable<KeyValuePair<Key, Value>> Keys();
            Key Floor(Key key);
            Key Ceiling(Key key);
        }
        // 符号表的基类
        public abstract class SymbolTablet<Key, Value>
            where Key : IComparable
        {
            public abstract void Put(Key key, Value value);
            public abstract Value Get(Key key);
            public abstract void Delete(Key key);
            public abstract bool Contains(Key key);
            public abstract bool IsEmpty();
            public abstract int Size();
            public abstract Key Min();
            public abstract Key Max();
            
            public abstract int Rank(Key key);
            //排名为k的键
            public abstract Key Select(int k);
            //删除最小的键
            public abstract void DeleteMin();
            public abstract void DeleteMax();
            
        }
        // 使用二分查找的符号表（基于数组）
        public class BinarySearchST<Key, Value>:SymbolTablet<Key, Value>, IEnumerable,IAdvancedSymbolTabletOpt<Key, Value>
            where Key:IComparable
        {
            //字段
            //保存键
            private Key[] keys;
            //保存值
            private Value[] values;
            //当前的存入的元素的大小
            private int N;

            public BinarySearchST(int capacity)
            {
                keys = new Key[capacity];
                values = new Value[capacity];
                N = 0;
            }
            public override int Size() { return N; }
            public override Value Get(Key key)
            {
                if (IsEmpty() == false)
                    return default(Value);
                int i = Rank(key);
                if (i < N && keys[i].CompareTo(key) == 0)
                    return values[i];
                else
                    return default(Value);
            }
            // 获取key的位置
            public override int Rank(Key key)
            {
                //keys是顺序的
                int lo = 0;int hi = N - 1;
                while(lo <= hi)
                {
                    int mid = (hi - lo) / 2+ lo;
                    int cmp = key.CompareTo(keys[mid]);
                    if (cmp < 0)
                        hi = mid - 1;
                    else if (cmp > 0)
                        lo = mid + 1;
                    else
                        return mid;
                }
                return lo;
            }
            //放入元素
            public override void Put(Key key, Value value)
            {
                Stopwatch Stopwatch1 = new Stopwatch();
                Stopwatch1.Start();
                //查找是否存在现有的值
                int fnd = Rank(key);
                if (fnd < N && key.CompareTo(keys[fnd]) == 0)
                { values[fnd] = value; return; }
                // 不存在现有的值 fnd是第一个大于key的最小的元素。因为元素不存在所以key会介于两个区间内。而由于mid是取下界的（hi-lo）/2=0。所以fnd会大于第一个小于keyd的最大元素
                for(int j=N;j>fnd;--j)
                {
                    keys[j] = keys[j - 1];
                    values[j] = values[j - 1];
                }
                values[fnd] = value;
                keys[fnd] = key;
                ++N;
            }

            public override void Delete(Key key)
            {
                int fnd = Rank(key);
                if (fnd < N && key.CompareTo(keys[fnd]) == 0)
                {
                    for(int j=fnd +1;j<N;++j)
                    {
                        keys[j - 1] = keys[j];
                        values[j - 1] = values[j];
                    }
                    --N;
                }
            }

            public override bool Contains(Key key)
            {
                int rank = Rank(key);
                return rank < N && keys[rank].CompareTo(key) == 0;
            }

            public override bool IsEmpty()
            {
                return N == 0;
            }

            public override Key Min()
            {
                return keys[0];
            }

            public override Key Max()
            {
                return keys[N - 1];
            }

            public override void DeleteMin()
            {
                Delete(Min());
            }

            public override void DeleteMax()
            {
                Delete(Max());
            }

            public override Key Select(int k)
            {
                if(k<N && k>=0)
                {
                    return keys[k];
                }
                return default(Key);
            }

            public IEnumerator GetEnumerator()
            {
                //使用yield进行遍历
                for(int i=0;i<N;++i)
                {
                    yield return new KeyValuePair<Key, Value>(keys[i], values[i]);
                }
            }
            //实现高级特性
            //返回介于[lo...hi]的大小
            public int Size(Key lo, Key hi)
            {
                //如果lo和hi不想等。返回的是0
                //lo_r保证keys[lo_r]>=keys[lo]
                //hi_r保证keys[hi_r]>=keys[hi]
                //int lo_r = Rank(lo);
                //int hi_r = Rank(hi);
                //if (hi_r < lo_r)
                //    return 0;
                //else if (Contains(keys[hi_r]))
                //{ return hi_r - lo_r + 1; }
                //else return hi_r - lo_r;
                //上面的代码 性能不行
                if (hi.CompareTo(lo) < 0)
                {
                    return 0;
                }
                else if (Contains(hi))
                    return Rank(hi) - Rank(lo) + 1;
                else
                    return Rank(hi) - Rank(lo);//已经包含了N

            }

            public IEnumerable<KeyValuePair<Key, Value>> Keys(Key lo, Key hi)
            {
                int l = Rank(lo);
                int sz = Size(lo, hi);
                if(sz >=0)
                {
                    for(int i=0;i<sz;++i)
                        yield return new KeyValuePair<Key,Value>(keys[l + i], values[l + i]);
                }
                else
                {
                    yield break;
                }
            }

            public IEnumerable<KeyValuePair<Key,Value>> Keys()
            {
                for(int i=0;i<N;++i)
                {
                    yield return new KeyValuePair<Key, Value>(keys[i], values[i]);
                }
            }
            //返回<=key的最大的键
            public Key Floor(Key key)
            {
                int rank = Rank(key);
                //如果key不存在那么rank是他后面的位置。
                if(Contains(key))
                {
                    return keys[rank];  //存在的情况返回本身
                }
                else
                {
                    //本身不存在
                    if (rank == 0)
                        return key;
                    else if (rank == N)
                        return Max();
                    else
                        return keys[rank - 1];
                }
            }
            //返回>=key的最小的键
            public Key Ceiling(Key key)
            {
                int rank = Rank(key);
                if (rank == N)
                    return key;  // 按道理应该出错
                else
                    return keys[rank];  //如果小于min。rank=0
            }
            //显示元素
            public void Show()
            {
                Console.WriteLine("-----");
                for(int i=0;i<N;++i)
                {
                    Console.WriteLine(keys[i] + ":" + values[i]);
                }
                Console.WriteLine("-----");
            }
        }
        //二叉查找树的接口
        public class BinarySearchTree<Key, Value> : SymbolTablet<Key, Value>, IAdvancedSymbolTabletOpt<Key, Value>
            where Key : IComparable
        {
            //保存根节点
            private Node root;
            //节点的类型
            internal class Node
            {
                internal Key key;
                internal Value val;
                internal Node left, right;//左右两个孩子
                internal int N;//以该节点为根的子节点数量+ 1
                public Node(Key key, Value val,int N) { this.key = key;this.val = val;this.N = N; }
            }
            public override bool Contains(Key key)
            {
                throw new NotImplementedException();
            }

            public override void Delete(Key key)
            {
                throw new NotImplementedException();
            }

            public override void DeleteMax()
            {
                root = DeleteMax(root);
            }

            public override void DeleteMin()
            {
                root = DeleteMin(root);
            }

            public override Value Get(Key key)
            {
                return Get(root, key);
            }

            public override bool IsEmpty()
            {
                throw new NotImplementedException();
            }

            public override Key Max()
            {
                if (root != null)
                    return Max(root).key;
                throw new InvalidOperationException("不存在根节点");
            }

            public override Key Min()
            {
                if (root != null)
                    return Min(root).key;
                throw new InvalidOperationException("不存在根节点");
            }

            public override void Put(Key key, Value value)
            {
                root = Put(root, key, value);
            }

            public override int Rank(Key key)
            {
                return Rank(root, key);
            }

            public override Key Select(int k)
            {
                Node n = Select(root,k);
                if (n == null)
                    return default(Key);
                else
                    return n.key;
            }

            public override int Size()
            {
                if (root == null)
                    return 0;
                else
                    return Size(root);
                    
            }
            //高级的接口
            public int Size(Key lo, Key hi)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<KeyValuePair<Key, Value>> Keys(Key lo, Key hi)
            {
                Queue<KeyValuePair<Key, Value>> queue = new Queue<KeyValuePair<Key, Value>>();
                Keys(root, queue, lo, hi);
                return queue;
            }

            public IEnumerable<KeyValuePair<Key, Value>> Keys()
            {
                return Keys(Min(), Max());
            }

            public Key Floor(Key key)
            {
                Node t = Floor(root, key);
                if (t == null)
                    return default(Key);
                else
                    return t.key;
            }

            public Key Ceiling(Key key)
            {
                Node t = Ceiling(root, key);
                if (t == null)
                    return default(Key);
                else
                    return t.key;
            }
            //特化的一些私有的方法
            //大小
            private int Size(Node node)
            {
                if (node != null)
                    return node.N;
                else
                    return 0;

            }
            //获取元素
            private Value Get(Node x, Key key)
            {
                if (x == null)
                    return default(Value);
                int cmp = key.CompareTo(x.key);
                if (cmp < 0)
                    return Get(x.left, key);
                else if (cmp > 0)
                    return Get(x.right, key);
                else
                    return x.val;
            }
            //添加元素
            private Node Put(Node x,Key key, Value val)
            {
                if (x == null)
                    return new Node(key, val, 1);
                int cmp = x.key.CompareTo(key);
                if (cmp > 0)
                    x.left = Put(x.left, key, val);
                else if (cmp< 0)
                    x.right = Put(x.right, key, val);
                else
                    x.val = val;//更新值
                //更新节点的值
                x.N = Size(x.left) + Size(x.right) + 1;
                return x;
            }
            //查找最大和最小键
            private Node Min(Node x)
            {
                if (x.left == null)
                    return x;
                else
                    return Min(x.left); // 一直查找左边的子树

            }
            private Node Max(Node x)
            {
                if (x.right == null)
                    return x;
                else
                    return Max(x.right); // 一直查找左边的子树

            }
            //取上下界
            private Node Floor(Node x, Key key)
            {
                //如果key < x.key ， 则key的下界一定存在于x的左子树（如果有的话）；没有的话返回key
                //如果key > x.key ，则key的下界可能存在与右子树。如果不存在返回key
                if (x == null)
                    return null;
                int cmp = key.CompareTo(x.key);
                if (cmp < 0)
                    return Floor(x.left, key);
                else if (cmp == 0)
                    return x;
                Node t = Floor(x.right, key);
                if (t == null)
                    return x; //返回根节点
                else
                    return t;
            }
            private Node Ceiling(Node x, Key key)
            {
                //key < x.key : 上界存在于x.left;或者是x节点
                // key > x.key : 上界一定存在x的右子树
                if (x == null)
                    return null;
                int cmp = key.CompareTo(x.key);
                if (cmp > 0)
                    return Ceiling(x.right, key);
                else if (cmp == 0)
                    return x;
                Node t = Ceiling(x.left, key);
                if (t == null)
                    return x;
                else
                    return t;
            }
            //选择键
            private Node Select(Node x, int k)
            {
                if (x == null)
                    return null;
                int t = Size(x.left);
                if (t > k)
                    return Select(x.left, k);
                else if (k > t)
                    return Select(x.right, k - t - 1);//去掉做子树的所有的数量和根节点
                else
                    return x;
            }
            //获取Key的排名
            private int Rank(Node x, Key key)
            {
                if (x == null)
                    return 0;
                int cmp = key.CompareTo(x.key);
                if (cmp < 0)
                    return Rank(x.left, key);
                else if (cmp > 0)
                    return Rank(x.right, key) + Size(x.left) + 1;
                else
                    return Size(x.left);
            }
            //删除节点
            private Node Delete(Node x, Key key)
            {
                if (x == null) return x;
                int cmp = key.CompareTo(x.key);
                if (cmp < 0)
                    x.left = Delete(x.left, key);
                else if (cmp > 0)
                    x.right = Delete(x.right, key);
                else
                {
                    //Hibbard的方法。
                    if (x.right == null) return x.left;//没有右孩子。只要删除本节点。返回孩子给上层的递归的路径
                    if (x.left == null) return x.right;
                    //有孩子的根节点
                    Node rightMin = Min(x.right);
                    rightMin.right = DeleteMin(x.right);
                    rightMin.left = x.left;
                    x = rightMin;
                }
                //更新x的大小
                x.N = Size(x.left) + Size(x.right) + 1;
                return x;
            }
            private Node DeleteMin(Node x)
            {
                //删除最小的节点
                if (x.left == null)
                    return x.right;//右子节点同样是很小的
                x.left = DeleteMin(x.left);
                x.N = Size(x.left) + Size(x.right) + 1;
                return x;
            }
            private Node DeleteMax(Node x)
            {
                //删除最小的节点
                if (x.right == null)
                    return x.left;//右子节点同样是很小的
                x.right = DeleteMax(x.right);
                x.N = Size(x.left) + Size(x.right) + 1;
                return x;
            }
            //支持迭代的中序遍历器(queue保存遍历的队列)
            private void Keys(Node x, Queue<KeyValuePair<Key, Value>> queue, Key lo, Key hi)
            {
                if (x == null)
                    return;
                int cmplo = lo.CompareTo(x.key);
                int cmphi = hi.CompareTo(x.key);
                if (cmplo < 0)
                    Keys(x.left, queue, lo, hi);//首先是将左子树加入队列
                if (cmplo <= 0 && cmphi >= 0)
                    queue.Enqueue(new KeyValuePair<Key, Value>(x.key, x.val));//加入自己
                if (cmphi > 0)
                    Keys(x.right, queue, lo, hi);//如果还在范围内（排除了相等）。继续添加队列元素
            }
            //递归版本的前序遍历
            private void Keys_Preorder_Recurring(Node root, Queue<Node> queue)
            {
                if (root != null)
                {
                    queue.Enqueue(root);
                    Keys_Preorder_Recurring(root.left, queue);
                    Keys_Preorder_Recurring(root.right, queue);
                }
            }
            public IEnumerable<KeyValuePair<Key, Value>> Keys_Preorder_Recurring()
            {
                Queue<Node> nodes = new Queue<Node>();
                Keys_Preorder_Recurring(root, nodes);
                foreach (var n in nodes)
                {
                    yield return new KeyValuePair<Key, Value>(n.key, n.val);
                }
            }
            //非递归版本的前序遍历
            //非递归版本的前序遍历
            public IEnumerable<KeyValuePair<Key, Value>> Keys_Preorder_NoRecurring()
            {
                Stack<Node> stack = new Stack<Node>();
                Queue<Node> queue = new Queue<Node>();
                Node ptree = root;
                while (ptree != null || stack.Count > 0)
                {
                    while(ptree !=null)
                    {
                        queue.Enqueue(ptree);
                        stack.Push(ptree);
                        ptree = ptree.left;
                    }
                    ptree = stack.Pop();
                    ptree = ptree.right;
                }
                foreach(var n in queue)
                {
                    yield return new KeyValuePair<Key, Value>(n.key, n.val);
                }
            }
            //非递归的中序遍历
            public IEnumerable<KeyValuePair<Key, Value>> Keys_Inorder_NoRecurring()
            {
                Stack<Node> stack = new Stack<Node>();
                Queue<Node> queue = new Queue<Node>();
                Node ptree = root;
                while(stack.Count > 0 || ptree !=null)
                {
                    while (ptree != null)
                    {
                        stack.Push(ptree);
                        ptree = ptree.left;
                    }
                    Node top = stack.Pop();
                    queue.Enqueue(top);
                    ptree = top.right;
                }
                foreach (var n in queue)
                {
                    yield return new KeyValuePair<Key, Value>(n.key, n.val);
                }
            }
            //递归版本的后序遍历
            private void Keys_Postorder_Recurring(Node root, Queue<Node> queue)
            {
                if (root != null)
                {

                    Keys_Postorder_Recurring(root.left, queue);
                    Keys_Postorder_Recurring(root.right, queue);
                    queue.Enqueue(root);
                }
            }
            public IEnumerable<KeyValuePair<Key, Value>> Keys_Postorder_Recurring()
            {
                Queue<Node> nodes = new Queue<Node>();
                Keys_Postorder_Recurring(root, nodes);
                foreach (var n in nodes)
                {
                    yield return new KeyValuePair<Key, Value>(n.key, n.val);
                }
            }
            //非递归的后序遍历
            //http://www.jianshu.com/p/456af5480cee
            public IEnumerable<KeyValuePair<Key, Value>> Keys_Postorder_NoRecurring()
            {
                Stack<Node> stack = new Stack<Node>();
                Queue<Node> queue = new Queue<Node>();
                Node ptree = root;
                Node lastVisit = root;
                while (ptree != null || stack.Count > 0)
                {
                    while (ptree != null)
                    {
                        stack.Push(ptree);
                        ptree = ptree.left;
                    }
                    ptree = stack.Peek();
                    if(ptree.right ==null || ptree.right == lastVisit)
                    {
                        //已经访问过或者没有右子树
                        queue.Enqueue(ptree);
                        stack.Pop();
                        lastVisit = ptree;//保证能够访问栈中的元素（ptree的父节点，他在考察右子树是否遍历完成）
                        ptree = null;//确保使用的是栈
                    }
                    else
                    {
                        //必须要再次遍历右子树
                        ptree = ptree.right;
                    }
                }
                foreach (var n in queue)
                {
                    yield return new KeyValuePair<Key, Value>(n.key, n.val);
                }
            }
            //层次遍历
            public IEnumerable<KeyValuePair<Key, Value>> Keys_LevelTreaverse()
            {
                Queue<Node> nodes=new Queue<Node>();
                Queue<Node> res = new Queue<Node>();
                nodes.Enqueue(root);
                while(nodes.Count>0)
                {
                    Node n = nodes.Dequeue();
                    res.Enqueue(n);
                    if (n.left != null)
                        nodes.Enqueue(n.left);
                    if (n.right != null)
                        nodes.Enqueue(n.right);
                    
                }
                foreach (var n in res)
                {
                    yield return new KeyValuePair<Key, Value>(n.key, n.val);
                }
            }
        }
        //测试
        class App
        {
            static void Main()
            {
                //string[] alpha = "S E B R C H E X B M P L E".Split(' ');
                //BinarySearchST<string, int> bs = new BinarySearchST<string, int>(alpha.Length);
                //Dictionary<int, double> dd = new Dictionary<int, double>();
                //foreach(int i in Enumerable.Range(0,alpha.Length))
                //{
                //    bs.Put(alpha[i], i);
                //}
                //bs.Show();
                ////删除某些元素
                //bs.Delete("X");
                ////扩展功能
                //Console.WriteLine(bs.Ceiling("A"));
                //Console.WriteLine(bs.Floor("Z"));
                //Console.WriteLine("*******");
                //foreach(var i in bs.Keys("A","Z"))
                //{
                //    Console.WriteLine(i.Key + ":" + i.Value);
                //}
                //绘制图形

                //DrawingFunctionGraph.Form1 form1 = new Form1(30);
                //List<MyPoint> points=new List<MyPoint>();
                //form1.DrawingCoordinary(OriginPointType.Center);
                //for (float d = 0.0f; d <= 1.0f; d = d + 0.01f)
                //    points.Add(new MyPoint(d, d));
                //form1.DrawingPoints(points);
                //Application.Run(form1);
                //二叉搜索树
                string[] alpha = "I W O E O R P T U D G S B G A K A M C L Z L".Split(' ');
                BinarySearchTree<string, int> bst = new BinarySearchTree<string,int>();
                int i = 1;
                foreach(var s in alpha)
                {
                    bst.Put(s,i++);
                }
                //中序遍历一下
                Console.WriteLine("递归版本的中序遍历：");
                foreach (var kv in bst.Keys())
                {
                    Console.WriteLine("{0} : {1}", kv.Key, kv.Value);
                }
                Console.WriteLine("非递归版本的中序遍历：");
                foreach (var kv in bst.Keys_Inorder_NoRecurring())
                {
                    Console.WriteLine("{0} : {1}", kv.Key, kv.Value);
                }
                Console.WriteLine("递归版本的前序遍历：");
                foreach(var kv in bst.Keys_Preorder_Recurring())
                {
                    Console.WriteLine("{0} : {1}", kv.Key, kv.Value);
                }
                Console.WriteLine("非递归版本的前序遍历：");
                foreach (var kv in bst.Keys_Preorder_NoRecurring())
                {
                    Console.WriteLine("{0} : {1}", kv.Key, kv.Value);
                }
                //post
                Console.WriteLine("递归版本的后序遍历：");
                foreach (var kv in bst.Keys_Postorder_Recurring())
                {
                    Console.WriteLine("{0} : {1}", kv.Key, kv.Value);
                }
                Console.WriteLine("非递归版本的后序遍历：");
                foreach (var kv in bst.Keys_Postorder_NoRecurring())
                {
                    Console.WriteLine("{0} : {1}", kv.Key, kv.Value);
                }
                Console.WriteLine("层次遍历：");
                foreach (var kv in bst.Keys_LevelTreaverse())
                {
                    Console.WriteLine("{0} : {1}", kv.Key, kv.Value);
                }
                Console.ReadKey();
            }
        }
    }
}
