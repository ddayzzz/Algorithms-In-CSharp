using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.StringDemo.WordTree
{
    /// <summary>
    /// 三向查找树
    /// </summary>
    /// <typeparam name="Value">模板参数，关联键对应的值：值类型默认为-1，引用类型null</typeparam>
    class TST<Value>
    {
        private Value GetDefaultValue()
        {
            dynamic test_def = default(Value);
            if (test_def != null)
                --test_def;
            return test_def;
        }
        private Node root;
        private Alphabet alphabet;
        public TST(Alphabet alphabet)
        {
            this.alphabet = alphabet;
        }
        private bool CheckValIsNotNull(Value v)
        {
            dynamic t = v;
            if (t != null)
            {
                if (t == -1)
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 返回字符在字符中相对于字母表的索引
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="d">最高位开始的位数</param>
        /// <returns>返回字符的位置或者不足返回-1</returns>
        private int CharAt(String s, int d)
        {
            if (d < s.Length)
                return alphabet.ToIndex(s[d]);
            else
                return -1;
        }
        private class Node
        {
            public char c;//代表的字符
            public Node left, mid, right;// 小于字符，等于字符，大于字符的子树
            public Value val;//和字符出串关联的值
            public Node()
            {
                dynamic test_def = default(Value);
                if (test_def != null)
                    --test_def;
                val = test_def;
            }
            public Node(Value val) : this()
            {
                this.val = val;
            }
        }
        //接口
        /// <summary>
        /// 获取键关联的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public Value Get(String key)
        {
            Node x = Get(root, key, 0);
            if (x == null)
            {
                dynamic dval = default(Value);
                if (dval != null)
                    return --dval;//值类型的默认值一般是0，false(0), 0.0等。
                return dval;
            }
            return x.val;
        }
        private Node Get(Node x, string key, int d)
        {
            if (x == null)
                return null;
            char c = alphabet.ToChar(CharAt(key, d));
            if (c < x.c)
                return Get(x.left, key, d);
            else if (c > x.c)
                return Get(x.right, key, d);
            else if (d < key.Length - 1)
                return Get(x.mid, key, d + 1);// 目前来是是一个子字符串。而且如果还没有到达需要的数位。检查后续
            else return x;//这个满足
        }
        /// <summary>
        /// 插入元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Put(string key, Value value)
        {
            root = Put(root, key, value, 0);
        }
        private Node Put(Node x, string key, Value val, int d)
        {
            char c = alphabet.ToChar(CharAt(key, d));
            if(x == null)
            {
                x = new Node(); x.c = c;//设置为字符。但是因为不确定是否已经把字符串的所有的字符都存入了树状图。所以需要进一步判断是否已经到达最后一个字符了
            }
            if (c < x.c)
                x.left = Put(x.left, key, val, d);
            else if (c > x.c)
                x.right = Put(x.right, key, val, d);
            else if (d < key.Length - 1)
                x.mid = Put(x.mid, key, val, d + 1);
            else
                x.val = val;//到达末尾
            return x;
        }
        /// <summary>
        /// 从x开始搜索，返回字符s的在d位的最长的字符串的长度。length 指定了起始的长度。返回的长度>=length
        /// </summary>
        /// <param name="x">键值</param>
        /// <param name="s">字符串</param>
        /// <param name="d">指定的数位</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        private int Search(Node x, String s, int d, int length)
        {
            if (x == null)
                return length;//指定的长度
            if (CheckValIsNotNull(x.val))
                length = d;//设置新的长度，传过来的是上一次的长度
            if (d == s.Length)
                return length;
            char c = alphabet.ToChar(CharAt(s, d));
            if (c < x.c)
                return Search(x.left, s, d + 1, length);
            else if (c > x.c)
                return Search(x.right, s, d + 1, length);
            else
                return Search(x.mid, s, d + 1, length);

        }
        /// <summary>
        /// 查找s的最长的前缀
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string LongestPrefixOf(string s)
        {
            int length = Search(root, s, 0, 0);
            return s.Substring(0, length);
        }
        private void Collect(Node x, StringBuilder pre,Queue<string> q)
        {
            if (x == null)
                return;
            Collect(x.left, pre, q);//手机收集小于的部分。一定要注意 x.left 与 x.c 不存在任何的关联，仅仅表示这个x.left 的字符小于x.c
            if (CheckValIsNotNull(x.val))
                q.Enqueue(pre.ToString() + x.c);//这个可能也是一个前缀
            Collect(x.mid, pre.Append(x.c), q);//mid 是包含x.c的
            pre.Remove(pre.Length - 1, 1);
            Collect(x.right, pre, q);//同 left
            
        }
        /// <summary>
        /// 查询前缀匹配的键
        /// </summary>
        /// <param name="pre">注意前缀不为空</param>
        /// <returns></returns>
        public IEnumerable<String> KeysWithPrefix(string pre)
        {
            Queue<string> n = new Queue<string>();
            var r = Get(root, pre, 0);
            if (r == null)
                return n;
            if (CheckValIsNotNull(r.val))
                n.Enqueue(pre);
            Collect(r.mid, new StringBuilder(pre), n);
            return n;
        }
        private Node Delete(Node x, string key, int d)
        {
            if (x == null)
                return null;//没有内容，或者 上一次递归的left min right 为空
            char c = alphabet.ToChar(CharAt(key, d));
            if (d == key.Length - 1 && c == x.c)
                x.val = GetDefaultValue();//删除的是key,并且完全相等
            else
            {
                //还处于前缀的部分
                if(c<x.c)
                {
                    x.left = Delete(x.left, key, d);
                }
                else if(c > x.c)
                {
                    x.right = Delete(x.right, key, d);
                }
                else
                {
                    x.mid = Delete(x.mid, key, d + 1);
                }
            }
            if (CheckValIsNotNull(x.val))
                return x;//并不是完整的字符串。主要是，仅仅处理他的mid left right 部分
            if (x.mid == null && x.right == null && x.left == null)
                return null;
            else
                return x;
        }
        public void Delete(string key)
        {
            root = Delete(root, key,0);
        }
        public IEnumerable<string> Keys()
        {
            Queue<string> n = new Queue<string>();
            Collect(root, new StringBuilder(), n);
            return n;
        }
    }
    class TSTTest
    {
        public static void Main()
        {
            Alphabet alphabet = Alphabet.LOWERCASE;
            TST<int> tst = new TST<int>(alphabet);
            String[] ss = new string[]
            {
                "she", "sells", "sea", "shells","by","the","shore"
            };
            for (int i = 0; i < ss.Length; ++i)
            {
                tst.Put(ss[i], i);
            }
            Console.WriteLine("KeyWithPrefix(s):");
            foreach (var key in tst.KeysWithPrefix("s"))
                Console.WriteLine(key);
            Console.WriteLine("KeyWithPrefix(sh):");
            foreach (var key in tst.KeysWithPrefix("sh"))
                Console.WriteLine(key);
            Console.WriteLine("LongestPrefixOf(shells):");
            
            Console.WriteLine(tst.LongestPrefixOf("shells"));
            Console.WriteLine("Delete all keys:");
            foreach (var k in tst.Keys())
                Console.WriteLine(k);
            Console.WriteLine("Delete any key:");
            tst.Delete("dfefef");
            tst.Delete("shell");
            tst.Delete("sho");
            foreach (var k in tst.Keys())
                Console.WriteLine(k);
            for (int i=0;i<ss.Length;++i)
            {
                Console.WriteLine($"Delete {ss[i]}");
                tst.Delete(ss[i]);
                foreach (var k in tst.Keys())
                    Console.WriteLine(k);
            }
            Console.ReadKey();
        }
    }
}
