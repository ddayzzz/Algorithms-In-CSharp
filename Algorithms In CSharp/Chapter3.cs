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
        //封装为集合类型
        public abstract class IMathSet<Key>
            where Key:IComparable
        {
            /// <summary>
            /// 添加元素
            /// </summary>
            /// <param name="key">添加的元素</param>
            public abstract void Add(Key key);
            /// <summary>
            /// 返回不在universe集合的元素集合
            /// </summary>
            /// <param name="universe">大集合</param>
            /// <returns></returns>
            public abstract IMathSet<Key> Complement(IMathSet<Key> universe);
            /// <summary>
            /// 并集
            /// </summary>
            /// <param name="a"></param>
            public abstract void Union(IMathSet<Key> a);
            public abstract void Intersection(IMathSet<Key> a);
            public abstract void Delete(Key key);
            public abstract bool Contains(Key key);
            public abstract bool IsEmpty();
            public abstract int Size();
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
        //平衡二叉查找树
        namespace BalancedBinaryTree
        {
            //红黑树的定义
            class RBTree<Key, Value>: SymbolTablet<Key, Value>
                where Key:IComparable
            {
                //定义颜色 readonly == final in Java
                private static readonly bool RED = true;
                private static readonly bool BLACK = false;
                //构造函数
                public RBTree()
                {
                    root = null;
                }
                //根节点
                private Node root;
                //定义节点
                internal class Node
                {
                    internal Key key;
                    internal Value val;
                    internal Node left, right;//左右的儿子
                    internal bool color;
                    internal int N;//这棵树的总子节点数量
                    //构造函数
                    internal Node(Key key, Value val, int N, bool color)
                    {
                        this.key = key;
                        this.val = val;
                        this.N = N;
                        this.color = color;
                    }
                    //打印数据
                    public override string ToString()
                    {
                        return key.ToString() + ":" + val.ToString();
                    }
                }
                //判断节点是否是红节点
                private bool IsRed(Node node)
                {
                    if(node !=null)
                    {
                        return node.color == RED;
                    }
                    else
                    {
                        return false;
                    }
                }
                //删除的自底向上的修复
                private Node Balance(Node h)
                {
                    //调整
                    if (IsRed(h.right))
                        h = rotateLeft(h);//按照LLRB红黑树的定义，红色的节点必须在左儿子处
                    //if (IsRed(h.right) && !IsRed(h.left))
                       // h = rotateLeft(h);//按照红黑树的定义，红色的节点必须在左儿子处
                    //是否左边连续两个红色
                    if (IsRed(h.left) && IsRed(h.left.left))
                        h = rotateRight(h);
                    //反转颜色
                    if (IsRed(h.left) && IsRed(h.right))
                        flipColors(h);
                    h.N = Size(h.left) + Size(h.right) + 1;
                    return h;
                }
                //左旋转h节点
                private Node rotateLeft(Node h)
                {
                    Node x = h.right;
                    h.right = x.left;
                    x.left = h;
                    x.color = h.color;
                    h.color = RED;
                    x.N = h.N;
                    h.N = 1 + Size(h.left) + Size(h.right);
                    return x;
                }
                //右旋转
                private Node rotateRight(Node h)
                {
                    Node x = h.left;
                    h.left = x.right;
                    x.right = h;
                    x.color = h.color;
                    h.color = RED;
                    x.N = h.N;
                    h.N = Size(h.left) + Size(h.right) + 1;
                    return x;
                }
                //反转颜色
                private void flipColors(Node h)
                {
                    //转换颜色
                    h.color = !h.color;
                    h.left.color = !h.left.color;
                    h.right.color = !h.right.color;

                }
                //删除最小的节点
                private Node moveRedLeft(Node h)
                {
                    //假设节点h为红色，h.left 和h.left.left 是黑色的
                    //将h.left 或者是h.left 的子节点之一变红。
                    flipColors(h);//我们需要得到最小的节点是红孩子。按照性质红节点的两个孩子是黑色的。需要反色处理
                    if(IsRed(h.right.left))//处理违反性质的情况：红孩子的节点不是黑节点
                    {
                        h.right = rotateRight(h.right);//都变成连续的右红节点。
                        h = rotateLeft(h);//右子树右旋了。红节点到了右边不符合规定。然后h左旋，变成了一个四节点的树
                        //可以给左子树形成一条红-黑-红的路线
                        //flipColors(h);//上一条语句执行后，违反了性质（旋转后红色的根节点的右孩子还是红色），需要重新设置颜色。
                        //再次反色形成黑-红-红。成为3-节点
                        flipColors(h);
                    }
                    return h;
                }
                private Node DeleteMin(Node h)
                {
                    if (h.left == null)
                    {
                        Console.WriteLine("删除了节点：{0}", h.ToString());
                        return null;
                    }

                    if (!IsRed(h.left) && !IsRed(h.left.left))//处理的情况：不是左红孩子。要从右边借一个红孩子过来
                        h = moveRedLeft(h);//给左子树添加孩子
                    h.left = DeleteMin(h.left);
                    return Balance(h);

                }
                public override void DeleteMin()
                {
                    if (!IsRed(root.left) && !IsRed(root.right))
                        root.color = RED;//根节点设置为红色
                    root = DeleteMin(root);
                    if (!IsEmpty())
                        root.color = BLACK;
                }
                //---删除最大的节点
                private Node moveRedRight(Node h)
                {
                    //假设节点h为红色，h.right 和h.right.left 是黑色
                    //将h.right 或者h.right的子节点之一变红
                    flipColors(h);//红节点向下
                    if (IsRed(h.left.left))//左孙子是一个红节点，我需要借一个过来到右边来。
                    {
                        h = rotateRight(h);//右旋转添加两个连续个红节点
                        flipColors(h);//右子树为红-原左子树的节点的颜色-红。再次反色就可以向下传递
                    }
                        
                    else
                    {
                        //h = rotateRight(h);//右旋转添加两个连续个红节点
                        //flipColors(h);
                        //左边出现的红-红可以被修复
                    }

                    return h;
                }
                private Node DeleteMax(Node h)
                {
                    if (IsRed(h.left))//左孩子是红节点，说明这个节点是3-节点。由于红节点在左侧，所以右旋即可/
                        h = rotateRight(h);
                    if (h.right == null)
                    {
                        Console.WriteLine("删除了节点：{0}", h.ToString());
                        return null;
                    }
                    if (!IsRed(h.right) && !IsRed(h.right.left))
                        h = moveRedRight(h);//给右子树添加一个红节点
                    h.right = DeleteMax(h.right);
                    return Balance(h);
                }
                //删除最大节点的接口
                public override void DeleteMax()
                {
                    if (!IsRed(root.left) && !IsRed(root.right))
                        root.color = RED;
                    root = DeleteMax(root);
                    if (!IsEmpty())
                        root.color = BLACK;//红黑树的定义
                }
                //插入节点
                private Node Put(Node h, Key key, Value val)
                {
                    if (h == null)
                        return new Node(key, val, 1, RED);
                    int cmp = key.CompareTo(h.key);
                    if (cmp < 0)
                        h.left = Put(h.left, key, val);
                    else if (cmp > 0)
                        h.right = Put(h.right, key, val);
                    else
                        h.val = val;
                    //调整
                    if (IsRed(h.right) && !IsRed(h.left))
                        h = rotateLeft(h);//按照红黑树的定义，红色的节点必须在左儿子处
                    //是否左边连续两个红色
                    if (IsRed(h.left) && IsRed(h.left.left))
                        h = rotateRight(h);
                    //反转颜色
                    if (IsRed(h.left) && IsRed(h.right))
                        flipColors(h);
                    h.N = Size(h.left) + Size(h.right) + 1;
                    return h;
                }
                
                public override void Put(Key key, Value value)
                {
                    root = Put(root, key, value);
                    root.color = BLACK;
                }

                public override Value Get(Key key)
                {
                    throw new NotImplementedException();
                }
                //直接删除
                private Node Delete(Node h, Key key)
                {
                    if(key.CompareTo(h.key) < 0)
                    {
                        if (!IsRed(h.left) && !IsRed(h.left.left))
                            h = moveRedLeft(h);//左边还没有红节点，需要借一个
                        h.left = Delete(h.left, key);
                    }
                    else
                    {
                        //待删除的key>=h.key
                        //确保在右子树中能出现红色右孩子
                        if (IsRed(h.left))
                            h = rotateRight(h);
                        if (key.CompareTo(h.key) == 0 && (h.right == null))
                            return null; // 在树底找到要删除的键
                        if (!IsRed(h.right) && !IsRed(h.right.left))//确保删除的节点不为2节点。
                            h = moveRedRight(h);
                        if (key.CompareTo(h.key) == 0)
                        {
                            //h.key == key
                            //属于待删除的节点，那么使用代替删除的方式，从右子树中选择最小的节点，修改他的值。然后在右子树中删除这个最小的键
                            Node temp = Min(h.right);
                            h.key = temp.key;
                            h.val = temp.val;
                            h.right = DeleteMin(h.right);
                        }
                        else
                        {
                            //如果节点>h.key
                            h.right = Delete(h.right, key);
                        }
                    }
                    return Balance(h);
                }
                public override void Delete(Key key)
                {
                    if(Contains(key))
                    {
                        if (!IsRed(root.left) && !IsRed(root.right))
                            root.color = RED;
                        root = Delete(root, key);
                        if (!IsEmpty())
                            root.color = BLACK;//头节点是红色的。因为删除一个节点可能会把头结点变红
                    }
                }
                private bool Contains(Node head, Key key)
                {
                    if(head !=null)
                    {
                        int cmp = key.CompareTo(head.key);
                        if (cmp > 0)
                            return Contains(head.right, key);
                        else if (cmp < 0)
                            return Contains(head.left, key);
                        else
                            return true;
                    }
                    return false;
                }
                public override bool Contains(Key key)
                {
                    return Contains(root, key);
                }

                public override bool IsEmpty()
                {
                    if (root == null || root.N == 0)
                        return true;
                    else
                        return false;

                }

                public override int Size()
                {
                    throw new NotImplementedException();
                }
                public int Size(Node node)
                {
                    if(node !=null)
                    {
                        return node.N;
                    }
                    else
                    {
                        return 0;
                    }
                }
                private Node Min(Node head)
                {
                    if(head.left !=null)
                    {
                        return Min(head.left);
                    }
                    else
                    {
                        return head;
                    }
                }
                public override Key Min()
                {
                    return Min(root).key;
                }
                private Node Max(Node head)
                {
                    if (head.right != null)
                    {
                        return Min(head.right);
                    }
                    else
                    {
                        return head;
                    }
                }
                public override Key Max()
                {
                    return Max(root).key;
                }

                public override int Rank(Key key)
                {
                    throw new NotImplementedException();
                }

                public override Key Select(int k)
                {
                    throw new NotImplementedException();
                }
                
                private void Print(int depth, Node head)
                {
                    if(head !=null)
                    {
                        //输出根节点
                        Console.WriteLine(string.Format("{0}{1}:{2}", new string(' ', depth), head.ToString(), head.color == RED ? "RED":"BLACK"));
                        //孩子
                        Print(depth + 1, head.left);
                        Print(depth + 1, head.right);
                    }
                    else
                    {
                        Console.WriteLine(string.Format("{0}null", new string(' ', depth)));
                    }
                }
                public void Print()
                {
                    Print(0, root);
                }
            }
            class Test
            {
                public static void Main()
                {
                    Console.WriteLine("polygenelubricants".GetHashCode());
                    RBTree<string, int> rbtree1 = new RBTree<string, int>();
                    var test_sets = "A B C D E F G H I J K L M N O P q r s t u v w x y z j 1 6 9 7 6 4 1 3 0 @ # $ $ % ^ & ^ * ( ) ) _".Split(' ');
                    foreach(var i in test_sets) 
                    {
                        rbtree1.Put(i, 0);
                    }
                    int deleteCount = 0;
                    while (rbtree1.IsEmpty() == false)
                    {
                        rbtree1.Delete(test_sets[test_sets.Length - (deleteCount++) -1]);
                        rbtree1.Print();
                        Console.WriteLine("----");
                    }
                    Console.WriteLine("{0}", deleteCount == test_sets.Length ? "成功" : "失败");
                    //System.Random random = new Random();
                    //int count = 0;
                    //while (rbtree1.IsEmpty() == false)
                    //{
                    //    //随机删除某种类型
                    //    int code = random.Next() % 2;
                    //    if(code == 0)
                    //    {
                    //        Console.Write("删除最小：");
                    //        rbtree1.DeleteMin();
                    //    }
                    //    else
                    //    {
                    //        Console.Write("删除最大：");
                    //        rbtree1.DeleteMax();
                    //    }
                    //    rbtree1.Print();
                    //    ++count;
                    //}
                    //Console.WriteLine(count == test_sets.Length ? "成功" : "失败");
                    //rbtree1.Print();
                    //Console.WriteLine("---------------------");
                    //rbtree1.DeleteMin();
                    //rbtree1.Print();
                    //Console.WriteLine("---------------------");
                    //rbtree1.DeleteMin();
                    //rbtree1.Print();
                    //Console.WriteLine("---------------------");
                    //rbtree1.DeleteMin();
                    //rbtree1.Print();
                    //删除最大节点的测试
                    //rbtree1.Print();
                    //Console.WriteLine("---------------------");
                    //rbtree1.DeleteMax();
                    //rbtree1.Print();
                    //Console.WriteLine("---------------------");
                    //rbtree1.DeleteMax();
                    //rbtree1.Print();
                    //Console.WriteLine("---------------------");
                    //rbtree1.DeleteMax();
                    //rbtree1.Print();
                    Console.ReadKey();
                }
            }
        }
        //散列表
        namespace HashTable
        {
            /// <summary>
            /// 基于线性探测法的哈希表
            /// </summary>
            /// <typeparam name="Key">存储的键类型。必须实现Equals方法</typeparam>
            /// <typeparam name="Value">存储的值类型</typeparam>
            public class LinearProbingHashST<Key, Value>:IEnumerable<KeyValuePair<Key,Value>>
                where Key:class
            {
                private int N=0;//符号表的键值对总数
                private int M = 16;//线性表的总大小
                private Key[] keys;
                private Value[] values;
                public LinearProbingHashST(int size=16)
                {
                    this.M = size;
                    keys = new Key[M];
                    values = new Value[M];

                }
                private int GetHashCode(Key key)
                {
                    return (key.GetHashCode() & 0x7fffffff) % M;
                }
                public void Put(Key key, Value value)
                {
                    if (N * 2 >= M)
                        Resize(2 * M);
                    int pos;
                    for(pos=this.GetHashCode(key);keys[pos] !=null;pos = (pos + 1) % M)
                        if(keys[pos].Equals(key))
                        {
                            values[pos] = value;
                            return;
                        }
                    //探测到了插入位置
                    keys[pos] = key;
                    values[pos] = value;
                    ++N;
                }
                public Value Get(Key key)
                {
                    int pos;
                    for (pos = this.GetHashCode(key); keys[pos] != null; pos = (pos + 1) % M)
                        if (keys[pos].Equals(key))
                        {
                            return values[pos];
                        }
                    return default(Value);
                }
                private void Resize(int capacity)
                {
                    var newObj = new LinearProbingHashST<Key, Value>(capacity);
                    for(int i=0;i<this.M;++i)
                    {
                        if(keys[i] !=null)
                        {
                            newObj.Put(keys[i], values[i]);
                        }
                    }
                    //更新
                    this.M = capacity;
                    this.keys = newObj.keys;
                    this.values = newObj.values;
                }

                public IEnumerator<KeyValuePair<Key, Value>> GetEnumerator()
                {
                    for(int i =0;i<M;++i)
                    {
                        if(keys[i] !=null)
                        {
                            yield return new KeyValuePair<Key, Value>(keys[i], values[i]);
                        }
                    }
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }
                //是否包含？
                public bool Contains(Key key)
                {
                    int pos;
                    for (pos = this.GetHashCode(key); keys[pos] != null; pos = (pos + 1) % M)
                        if (keys[pos].Equals(key))
                        {
                            return true;
                        }
                    return false;
                }
                //删除元素
                public void Delete(Key key)
                {
                    if (!Contains(key))
                        return;
                    int pos = this.GetHashCode(key);
                    while (!keys[pos].Equals(key))
                        pos = (pos + 1) % M;
                    keys[pos] = null;
                    values[pos] = default(Value);//我觉得value是什么不重要。但是如果他的复制的代价大的话。可以不取消这句
                    //需要将投后方的键簇向前移动一个单位
                    pos = (pos + 1) % M;
                    while(keys[pos] !=null)
                    {
                        Key tempKey = keys[pos];
                        Value tempValue = values[pos];
                        keys[pos] = null;
                        values[pos] = default(Value);
                        --N;//总的来说我少了一个Put操作可能会加回去
                        Put(tempKey, tempValue);
                        pos = (pos + 1) % M;
                    }
                    --N;
                    if (N > 0 && N == M / 8)
                        Resize(M / 2);
                }
                //是否为空
                public bool IsEmpty()
                {
                    return N == 0;
                }
            }
            //哈希表测试
            public class Test
            {
                public static void Main()
                {
                    LinearProbingHashST<string, int> lbhst1 = new LinearProbingHashST<string, int>();
                    System.Random radom = new Random();
                    for (int i = 0; i < 100; ++i)
                    {
                        var t = radom.Next(0, 10);
                        lbhst1.Put(t.ToString(), t);
                    }
                    foreach(var kv in lbhst1)
                    {
                        Console.WriteLine("{0}->{1}", kv.Key, kv.Value);
                    }
                    
                    int fnd;
                    fnd = lbhst1.Get("10");
                    Console.WriteLine("Find {0}:{1}", "10", fnd == default(int) ? "NULL" : fnd.ToString());
                    lbhst1.Delete("5");
                    lbhst1.Delete("3");
                    lbhst1.Delete("2");
                    Console.WriteLine("After deletion--------------------");
                    foreach (var kv in lbhst1)
                    {
                        Console.WriteLine("{0}->{1}", kv.Key, kv.Value);
                    }
                    Console.ReadKey();
                }
            }
            
        }
        //对应书的3.5节：应用
        namespace Application
        {
            //集合，使用了位运算
            public class Set
            {
                private int maxValue=0;//记录最大的数据
                private int[] data;//没有置位
                private int basicSize=sizeof(int) * 8;//基础大小
                public Set()
                {
                    data = null;
                }
                public void Add(int key)
                {
                    if (key > maxValue)
                        maxValue = key;
                    if (data == null || maxValue / basicSize + 1 > data.Length)
                        ReSize(maxValue / basicSize + 1);
                    //调整了个大小
                    if (!Contains(key))
                        data[key / basicSize] = (int)(data[key / basicSize] | (1 << (key % basicSize)));//置位

                }

                public Set Complement(Set universe)
                {
                    Set result = new Set();
                    if (Size() <=universe.Size())//补集定义
                    {
                        
                        result.ReSize(universe.Size());
                        for (int i = 0; i < Size(); ++i)
                            //使用异或
                            result.data[i] = universe.data[i] ^ data[i];
                    }
                    return result;

                }

                public bool Contains(int key)
                {
                    if(!IsEmpty())
                    {
                        return (data[key / basicSize] & (1 << (key % basicSize))) != 0;
                    }
                    return false;
                }

                public void Delete(int key)
                {
                    if (!IsEmpty())
                    {
                        data[key / basicSize] = data[key / basicSize] & ~(1 << (key % basicSize));
                    }
                }

                public Set Intersection(Set a)
                {
                    //求交集
                    Set result = new Set();
                    //修改大小
                    result.ReSize(Math.Min(a.Size(), Size()));
                    int i=0, j=0;
                    for (; i < Size() && j < a.Size(); ++i, ++j)
                        result.data[i] = data[i] & a.data[j];
                    return result;
                }

                public bool IsEmpty()
                {
                    return Size() == 0;
                }

                public int Size()
                {
                    return data == null ? 0 : data.Length;
                }

                public Set Union(Set a)
                {
                    //求并集
                    Set result = new Set();
                    //修改大小
                    result.ReSize(Math.Max(a.Size(), Size()));
                    int i = 0, j = 0;
                    for (; i < Size() && j < a.Size(); ++i, ++j)
                        result.data[i] = data[i] | a.data[j];
                    //并集还要补全
                    for (; i < Size(); ++i)
                        result.data[i] = data[i];
                    for (; j < a.Size(); ++j)
                        result.data[j] = a.data[j];
                    return result;
                }

                //大小修改
                private void ReSize(int newSize)
                {
                    //newSize是维度大小
                    int[] newArray = new int[newSize];
                    if(data !=null)
                    {
                        //分配数组
                        var currSize = data.Length;
                        for (int i = 0; i < currSize; ++i)
                            newArray[i] = data[i];
                    }
                    //复制原来的值到新的数组
                    this.data = newArray;
                }
                //打印数据
                public override string ToString()
                {
                    if(!IsEmpty())
                    {
                        StringBuilder retStr = new StringBuilder("{");
                        int i;
                        for(i=0;i<Size();++i)
                        {
                            for(int j=0;j<basicSize;++j)
                            {

                                int wei = 1 << j;
                                if ((wei & data[i]) != 0)
                                    retStr.Append((i * basicSize + j).ToString()).Append(",");
                            }
                        }
                        return retStr.Append("}").ToString();
                    }
                    else
                    {
                        return "{空集}";
                    }
                }
            }
            //稀疏向量
            public class SparseVector
            {
                //维护一个元素为哈希表对象的数据
                //哈希表是列号->值
                private Dictionary<int, double> vector;
                //构造函数
                public SparseVector() { this.vector = new Dictionary<int, double>(); }
                public int Size() { return vector.Count; }
                public double Get(int i)
                {
                    if (vector.ContainsKey(i))
                        return vector[i];
                    else
                        return 0.0;
                }
                public void Put(int column, double value)
                {
                    vector.Add(column, value);
                }
                //点乘
                public double Dot(double[] that)
                {
                    double sum = 0.0;
                    foreach(int i in vector.Keys)
                    {
                        sum += that[i] * this.Get(i);
                    }
                    return sum;
                }
                public double Dot(SparseVector that)
                {
                    double sum = 0.0;
                    foreach (int i in vector.Keys)
                    {
                        sum += that.Get(i) * this.Get(i);
                    }
                    return sum;
                }
            }
            //由稀疏相量组成的系数矩阵
            public class SparseMatrix
            {
                //保存数组
                private SparseVector[] matrix;
                public int Row { get; }
                public int Column { get; }
                public SparseMatrix(int row, int column)
                {
                    this.Row = row;
                    this.Column = column;
                    matrix = new SparseVector[row];
                    //初始化对象
                    for (int i = 0; i < row; ++i)
                        matrix[i] = new SparseVector();
                }
                //插入元素
                public void Put(int row, int column, double value)
                {
                    matrix[row].Put(column, value);
                }
                //获取元素
                public double Get(int row,int column)
                {
                    return matrix[row].Get(column);
                }
                //矩阵和矩阵的乘法
                public SparseMatrix Dot(SparseMatrix matrix)
                {
                    if (this.Column == matrix.Row)
                    {
                        SparseMatrix result = new SparseMatrix(this.Row, matrix.Column);
                        for (int i = 0; i < this.Row; ++i)
                        {
                            for (int p = 0; p < matrix.Column; ++p)
                            {
                                double sum = 0.0;
                                for (int j = 0; j < this.Column; ++j)
                                {
                                    sum += this.Get(i, j) * matrix.Get(j,p);
                                }
                                result.Put(i, p, sum);
                            }
                        }
                        return result;
                    }
                    throw new ArgumentException("矩阵的维度不满足乘法定义");
                }
                //打印
                public override string ToString()
                {
                    StringBuilder sb = new StringBuilder();
                    for(int i =0;i<this.Row;++i)
                    {
                        sb.Append("{");
                        for (int j=0;j<this.Column;++j)
                        {
                            sb.Append(matrix[i].Get(j).ToString()).Append(" ");
                        }
                        sb.Append("\n");
                    }
                    return sb.ToString();
                }
            }
            public class Test
            {
                public static void SetTest()
                {
                    Set set = new Set();
                    Set set2 = new Set();
                    System.Random radom = new Random();
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sb2 = new StringBuilder();
                    for (int i = 0; i < 10; ++i)
                    {
                        int u = radom.Next(1, 5);
                        set.Add(u);
                        sb.Append(u.ToString()).Append(",");
                    }
                    for (int i = 0; i < 10; ++i)
                    {
                        int u = radom.Next(1, 6);
                        set2.Add(u);
                        sb2.Append(u.ToString()).Append(",");
                    }
                    Console.WriteLine("产生的随机数序列1：{0}", sb.ToString());
                    Console.WriteLine("产生的随机数序列2：{0}", sb2.ToString());
                    Console.WriteLine(set.ToString());
                    Console.WriteLine(set2.ToString());
                    Console.WriteLine("两个序列的交集：{0}", set.Intersection(set2).ToString());
                    Console.WriteLine("两个序列的并集：{0}", set.Union(set2).ToString());
                    Console.WriteLine("两个序列的补集：{0}", set.Complement(set2).ToString());
                    set.Delete(4);
                    Console.WriteLine("set删除4:{0}", set.ToString());
                    Console.ReadKey();
                }
                public static void SparseVectorTest()
                {
                    double[] result = new double[5];
                    SparseVector[] matrix = new SparseVector[5];
                    for (int i = 0; i < matrix.Length; ++i)
                        matrix[i] = new SparseVector();//注意！new数组只是分配了可以存放5个SparseVector（类型）的空间，但是并没有实例化类
                    matrix[0] = new SparseVector();
                    matrix[0].Put(1, 0.9);
                    matrix[1].Put(2, 0.36);
                    matrix[1].Put(3, 0.36);
                    matrix[1].Put(4, 0.18);
                    matrix[2].Put(3, 0.9);
                    matrix[3].Put(0, 0.9);
                    matrix[4].Put(0, 0.45);
                    matrix[4].Put(2, 0.45);
                    //定义一个向量
                    double[] b = { 0.05, 0.04, 0.36, 0.37, 0.19 };
                    for(int i=0;i<matrix.Length;++i)
                    {
                        var sum = matrix[i].Dot(b);
                        result[i] = sum;
                    }
                    Console.ReadKey();
                }
                public static void SparseMatrixTest()
                {
                    SparseMatrix m1 = new SparseMatrix(5, 5);
                    m1.Put(0, 1, 0.9);
                    m1.Put(1, 2, 0.36);
                    m1.Put(1, 3, 0.36);
                    m1.Put(1, 4, 0.18);
                    m1.Put(2, 3, 0.9);
                    m1.Put(3, 0, 0.9);
                    m1.Put(4, 0, 0.47);
                    m1.Put(4, 2, 0.47);
                    SparseMatrix m2 = new SparseMatrix(5, 2);
                    m2.Put(0, 0, 0.05);
                    m2.Put(1, 0, 0.04);
                    m2.Put(2, 0, 0.36);
                    m2.Put(3, 0, 0.37);
                    m2.Put(4, 0, 0.19);
                    m2.Put(0, 1, 0.44);
                    m2.Put(1, 1, 0.47);
                    m2.Put(2, 1, 0.8);
                    m2.Put(3, 1, 1.2);
                    m2.Put(4, 1, 6.3);
                    var plus = m1.Dot(m2);
                    Console.WriteLine(plus.ToString());
                    Console.ReadKey();
                }
                public static void Main()
                {
                    //SparseVectorTest();
                    SparseMatrixTest();
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

