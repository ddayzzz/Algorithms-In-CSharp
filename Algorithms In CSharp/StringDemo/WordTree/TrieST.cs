using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.StringDemo.WordTree
{
    /// <summary>
    /// 定义单词查找树的结构
    /// </summary>
    /// <typeparam name="Value">模板参数，指定键的类型</typeparam>
    class TrieST<Value>
    {
        private Value GetDefaultValue()
        {
            dynamic test_def = default(Value);
            if (test_def != null)
                --test_def;
            return test_def;
        }
        private class Node
        {
            public Value Val { get; set; }
            public Node[] Next { get; set; }
            public Node(Value val, int R):this(R)
            {
                Val = val;
            }
            public Node(int R)
            {
                Next = new Node[R];
                dynamic test_def = default(Value);
                if (test_def != null)
                    --test_def;
                Val = test_def;
            }
        }
        private int R;//保存基数
        private Node root;//保存根节点
        private Alphabet alphabet;
        public TrieST(Alphabet alphabet)
        {
            R = alphabet.R();
            this.alphabet = alphabet;
        }
        private bool CheckValIsNotNull(Value v)
        {
            dynamic t = v;
            if(t != null)
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
        /// <summary>
        /// 返回以x作为根节点的子单词树中与Key 相关联的值
        /// </summary>
        /// <param name="x">字数</param>
        /// <param name="key">查找的字符串</param>
        /// <param name="d">第几位？</param>
        /// <returns></returns>
        private Node Get(Node x, String key, int d)
        {
            if (x == null)
                return null;
            if (d == key.Length)
                return x;//已经找到了完整的字符串
            int c = CharAt(key, d);
            return Get(x.Next[c], key, d + 1);//寻找下一个字符是否有内容
        }
        /// <summary>
        /// 查找键在查找树的编号
        /// </summary>
        /// <param name="key"></param>
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
            return x.Val;
        }
        private Node Put(Node x, String key, Value val, int d)
        {
            if (x == null)
                x = new Node(R);
            if(d == key.Length)
            {
                x.Val = val;return x;
            }
            int c = CharAt(key, d);
            x.Next[c] = Put(x.Next[c], key, val, d + 1);//继续递归到子字符串
            return x;
        }
        public void Put(String key, Value val)
        {
            root = Put(root, key, val, 0);
        }
        private int Size(Node x)
        {
            if (x == null)
                return 0;
            int cnt = 0;
            if (CheckValIsNotNull(x.Val))
                ++cnt;
            for (int c = 0; c < R; ++c)
                cnt += Size(x.Next[c]);
            return cnt;
        }
        public int Size() { return Size(root); }
        private void Collect(Node x, String pre, Queue<String> q)
        {
            if (x == null)
                return;
            if (CheckValIsNotNull(x.Val))
                q.Enqueue(pre);
            for (int c = 0; c < R; ++c)
                Collect(x.Next[c], pre + alphabet.ToChar(c), q);
        }
        public IEnumerable<String> Keys() { return KeysWithPrefix(""); }
        public IEnumerable<String> KeysWithPrefix(String pre)
        {
            Queue<String> q = new Queue<string>();
            Collect(Get(root, pre, 0), pre, q);//前缀必须存在
            return q;
        }
        /// <summary>
        /// 从x开始搜索，返回字符s的在d位的最长的字符串的长度。length 指定了起始的长度。返回的长度>=length
        /// </summary>
        /// <param name="x"></param>
        /// <param name="s"></param>
        /// <param name="d"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private int Search(Node x, String s, int d, int length)
        {
            if (x == null)
                return length;//不存在，返回的是基础的长度
            if (CheckValIsNotNull(x.Val))
                length = d;//如果这个节点是有内容的，需要一次作为起点开始检查
            if (d == s.Length)
                return length;//栈上已经到了d-1位了。
            int c = CharAt(s, d);
            return Search(x.Next[c], s, d + 1, length);
        }
        public String LogestPrefixOf(String s)
        {
            int length = Search(root, s, 0, 0);
            return s.Substring(0, length);
        }
        private Node Delete(Node x, string key, int d)
        {
            if (x == null)
                return null;//不需要删除
            if (d == key.Length)
                x.Val = GetDefaultValue();
            else
            {
                int c = CharAt(key, d);
                x.Next[c] = Delete(x.Next[c], key, d + 1);//删除剩余的子部分
            }
            if (CheckValIsNotNull(x.Val))
                return x;//如果删除的不是正好的key的话，直接返回的，因为这个只是一个路径，并不需要删除
            for(int i=0;i < alphabet.R();++i)
            {
                //如果还有公共部分
                if (x.Next[i] != null)
                    return x;
            }
            return null;
        }
        public void Delete(string key)
        {
            root = Delete(root, key, 0);
        }
    }
    public class Test
    {
        public static void Main()
        {
            Alphabet alphabet = Alphabet.LOWERCASE;
            TrieST<int> trieST = new TrieST<int>(alphabet);
            String[] ss = new string[]
            {
                "she", "sells", "sea", "shells","by","the","shore"
            };
            for (int i = 0; i < ss.Length; ++i)
            {
                trieST.Put(ss[i], i);
            }
            foreach (var v in trieST.KeysWithPrefix(""))
                Console.WriteLine(v);
            Console.WriteLine("KeyWithPrefix s:");
            foreach (var v in trieST.KeysWithPrefix("s"))
                Console.WriteLine(v);
            trieST.Delete("shore");
            Console.WriteLine("After shells deleted:");
            foreach (var v in trieST.KeysWithPrefix(""))
                Console.WriteLine(v);
            Console.WriteLine("longest prefix of s:");
            Console.WriteLine(trieST.LogestPrefixOf("shellsort"));
            Console.WriteLine("KeyWithPrefix:");
            foreach (var ks in trieST.KeysWithPrefix("sel"))
                Console.WriteLine(ks);
            Console.WriteLine("KeyWithPrefix s:");
            foreach (var ks in trieST.KeysWithPrefix("s"))
                Console.WriteLine(ks);
            Console.ReadKey();
        }
    }
}
