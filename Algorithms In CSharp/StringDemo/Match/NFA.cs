using Algorithms_In_CSharp.Chapter1.BagDemo;
using Algorithms_In_CSharp.Graph.Algorithms;
using Algorithms_In_CSharp.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.StringDemo.Match
{
    /// <summary>
    /// 非确定型有限状态自动机的定义
    /// </summary>
    class NFA
    {
        private char[] re;//匹配状态
        private IDirectedGraph G;//状态转换(epsilon)转换
        private int M;//状态的数量
        public enum NFAType
        {
            Normal,
            MultiOrOperator,
            AppearAtOnce,
            RepeatNumber,
            Range,
            Diff
        }
        /// <summary>
        /// 判断NFA能否识别的文本txt
        /// </summary>
        /// <param name="txt">文本</param>
        /// <returns></returns>
        public bool Recognizes(string txt)
        {
            Bag<int> pc = new Bag<int>();//这个保存某个状态通过E-转移或者某种输入的移动。初始化时，使用状态0的可达的状态
            DepthFirstSearch dfs = new DepthFirstSearch(G, 0);
            for (int v = 0; v < G.V; ++v)
                if (dfs.HasPathTo(v))
                    pc.Add(v);//如果从0状态到某种状态是可行的
            for(int i=0;i<txt.Length;++i)
            {
                Bag<int> match = new Bag<int>();
                foreach(var v in pc)
                {
                    if(v < M)//是一个状态
                    {
                        if (re[v] == txt[i] || re[v] == '.')
                            match.Add(v + 1);//如果是任何一种类型的转换。1
                    }
                }
                pc = new Bag<int>();
                dfs = new DepthFirstSearch(G, match);//将所有可以到的点加入（当前匹配或者'.'）
                for (int v = 0; v < G.V; ++v)
                {
                    if (dfs.HasPathTo(v))
                        pc.Add(v);
                }//新的可达
            }
            foreach (var v in pc)
                if (v == M)//虚拟的点，表示结束 1 代码添加后面的状态
                    return true;
            return false;
           
        }
        public NFA(string regexp, NFAType type)
        {
            switch(type)
            {
                case NFAType.Normal:
                    doDFA_normal(regexp);break;
                case NFAType.MultiOrOperator:
                    doDFA_multiOr(regexp);break;
                case NFAType.AppearAtOnce:
                    doDFA_appearAtLeastOnce(regexp);break;
            }
        }
        private void doDFA_normal(string regexp)
        {
            Stack<int> ops = new Stack<int>();
            re = regexp.ToArray();
            M = re.Length;
            G = new DirectedWeightedGraph(M + 1);
            for(int i=0;i<M;++i)
            {
                int lp = i;
                if(re[i] == '(' || re[i] == '|')
                {
                    //左括号或者或运算符
                    ops.Push(i);
                }
                else if(re[i] == ')')
                {
                    int or = ops.Pop();
                    if (re[or] == '|')
                    {
                        //如果右括号，而且之前是或运算符的右边部分
                        lp = ops.Pop();//需要建立某个双向的连接，
                        G.AddEdge(lp, or + 1);//从左括号到或运算符后（不满足的一个）
                        G.AddEdge(or, i);//从左括号到右括号
                    }
                    else
                        lp = or;//如果仅仅是一组字符串而已用()括住的话。lp 设置为左括号的位置。因为括号的优先级是很高的。
                    //所以在栈上括号在 * 顶部
                }
                if(i < M-1 && re[i+1] == '*')//查看下一个字符
                {
                    G.AddEdge(lp, i + 1);//这个字符是可以>=0的，双向状态转换
                    G.AddEdge(i + 1, lp);
                }

                if (re[i] == '(' || re[i] == '*' || re[i] == ')')
                    G.AddEdge(i, i + 1);//后面的状态
            }
        }
        private void doDFA_multiOr(string regexp)
        {
            Stack<int> ops = new Stack<int>();
            re = regexp.ToArray();
            M = re.Length;
            G = new DirectedWeightedGraph(M + 1);
            for (int i = 0; i < M; ++i)
            {
                int lp = i;
                if (re[i] == '(' || re[i] == '|')
                {
                    //左括号或者或运算符
                    ops.Push(i);
                }
                else if (re[i] == ')')
                {
                    //可能顶部会有 or 运算符
                    int tc = ops.Pop();
                    List<int> listOfOrLast = new List<int>();
                    while (re[tc] == '|')
                    {
                        G.AddEdge(tc, i);
                        listOfOrLast.Add(tc + 1);
                        tc = ops.Pop();
                    }
                    // 最后一次的 tc 对应的位置是 左括号。
                    for (int k = 0; k < listOfOrLast.Count; ++k)
                        G.AddEdge(tc, listOfOrLast[k]);
                    // lp 需要更新为下一次处理的
                    lp = tc;
                }
                if (i < M - 1 && re[i + 1] == '*')//查看下一个字符
                {
                    G.AddEdge(lp, i + 1);//这个字符是可以>=0的，双向状态转换
                    G.AddEdge(i + 1, lp);
                }
                if (re[i] == '(' || re[i] == '*' || re[i] == ')')
                    G.AddEdge(i, i + 1);//后面的状态
            }
        }
        /// <summary>
        /// 支持 . 这种通配符。receive 已经实现了？？？
        /// </summary>
        /// <param name="regexp"></param>
        [Obsolete("已经实现了吧")]
        private void doDFA_wildcard(string regexp)
        {
            Stack<int> ops = new Stack<int>();
            re = regexp.ToArray();
            M = re.Length;
            G = new DirectedWeightedGraph(M + 1);
            for (int i = 0; i < M; ++i)
            {
                int lp = i;
                if (re[i] == '(' || re[i] == '|')
                {
                    //左括号或者或运算符
                    ops.Push(i);
                }
                else if (re[i] == ')')
                {
                    int or = ops.Pop();
                    if (re[or] == '|')
                    {
                        //如果右括号，而且之前是或运算符的右边部分
                        lp = ops.Pop();//需要建立某个双向的连接，
                        G.AddEdge(lp, or + 1);//从左括号到或运算符后（不满足的一个）
                        G.AddEdge(or, i);//从左括号到右括号
                    }
                    else
                        lp = or;//如果仅仅是一组字符串而已用()括住的话。lp 设置为左括号的位置。因为括号的优先级是很高的。
                    //所以在栈上括号在 * 顶部
                }
                if (i < M - 1 && re[i + 1] == '*')//查看下一个字符
                {
                    G.AddEdge(lp, i + 1);//这个字符是可以>=0的，双向状态转换
                    G.AddEdge(i + 1, lp);
                }

                if (re[i] == '(' || re[i] == '*' || re[i] == ')')
                    G.AddEdge(i, i + 1);//后面的状态
            }
        }
        /// <summary>
        /// 匹配至少出现一次的运算符 + 
        /// 来自于：https://www.cnblogs.com/catch/p/3722082.html
        /// </summary>
        /// <param name="regexp">正则表达式</param>
        private void doDFA_appearAtLeastOnce(string regexp)
        {
            Stack<int> ops = new Stack<int>();
            re = regexp.ToArray();
            M = re.Length;
            G = new DirectedWeightedGraph(M + 1);
            for (int i = 0; i < M; ++i)
            {
                int lp = i;
                if (re[i] == '(' || re[i] == '|')
                {
                    //左括号或者或运算符
                    ops.Push(i);
                }
                else if (re[i] == ')')
                {
                    int or = ops.Pop();
                    if (re[or] == '|')
                    {
                        //如果右括号，而且之前是或运算符的右边部分
                        lp = ops.Pop();//需要建立某个双向的连接，
                        G.AddEdge(lp, or + 1);//从左括号到或运算符后（不满足的一个）
                        G.AddEdge(or, i);//从左括号到右括号
                    }
                    else
                        lp = or;//如果仅仅是一组字符串而已用()括住的话。lp 设置为左括号的位置。因为括号的优先级是很高的。
                    //所以在栈上括号在 * 顶部
                }
                if (i < M - 1 && re[i + 1] == '*')//查看下一个字符
                {
                    G.AddEdge(lp, i + 1);//这个字符是可以>=0的，双向状态转换
                    G.AddEdge(i + 1, lp);
                } else if (i < M - 1 && re[i + 1] == '+')//查看下一个字符是不是+
                {
                    //G.AddEdge(lp, i + 1);//这个字符是可以>=0的，单向状态转换
                    G.AddEdge(i + 1, lp);
                }

                if (re[i] == '(' || re[i] == '*' || re[i] == ')' || re[i] == '+')
                    G.AddEdge(i, i + 1);//后面的状态
            }
        }
        /// <summary>
        /// 允许出现指定重复次数 即 {min, max} 
        /// </summary>
        /// <param name="regexp"></param>
        /// 
        [Obsolete("没有实现出来")]
        private void doDFA_repeatNumber(string regexp)
        {
            Stack<int> ops = new Stack<int>();
            re = regexp.ToArray();
            M = re.Length;
            G = new DirectedWeightedGraph(M + 1);
            for (int i = 0; i < M; ++i)
            {
                int lp = i;
                if (re[i] == '(' || re[i] == '|')
                {
                    //左括号或者或运算符
                    ops.Push(i);
                }
                else if (re[i] == ')')
                {
                    int or = ops.Pop();
                    if (re[or] == '|')
                    {
                        //如果右括号，而且之前是或运算符的右边部分
                        lp = ops.Pop();//需要建立某个双向的连接，
                        G.AddEdge(lp, or + 1);//从左括号到或运算符后（不满足的一个）
                        G.AddEdge(or, i);//从左括号到右括号
                    }
                    else
                        lp = or;//如果仅仅是一组字符串而已用()括住的话。lp 设置为左括号的位置。因为括号的优先级是很高的。
                    //所以在栈上括号在 * 顶部
                }
                if (i < M - 1 && re[i + 1] == '*')//查看下一个字符
                {
                    G.AddEdge(lp, i + 1);//这个字符是可以>=0的，双向状态转换
                    G.AddEdge(i + 1, lp);
                }
                else if (i < M - 1 && re[i + 1] == '+')//查看下一个字符是不是+
                {
                    //G.AddEdge(lp, i + 1);//这个字符是可以>=0的，单向状态转换
                    G.AddEdge(i + 1, lp);
                }

                if (re[i] == '(' || re[i] == '*' || re[i] == ')' || re[i] == '+')
                    G.AddEdge(i, i + 1);//后面的状态
            }
        }
        public static void Main(string[] args)
        {
            //string regexp = "(.*" + args[0] + ".*)";
            string regexp = args[0];
            NFA nfa = new NFA(regexp, NFAType.AppearAtOnce);
            var rstream = Console.OpenStandardInput();
            var reader = new System.IO.StreamReader(rstream);
            while(!reader.EndOfStream)
            {
                string txt = reader.ReadLine();
                if (nfa.Recognizes(txt))
                    Console.WriteLine(txt);
            }
            Console.Read();
        }
    }
}
