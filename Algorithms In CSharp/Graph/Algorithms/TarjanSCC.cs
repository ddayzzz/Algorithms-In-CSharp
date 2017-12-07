using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph.Algorithms
{
    /// <summary>
    /// Targan 强连通分量的算法
    /// </summary>
    class TarjanSCC
    {
        //思路
        //参考了 http://blog.csdn.net/u013376508/article/details/50995675
        //wikipedia https://en.wikipedia.org/wiki/Tarjan%27s_strongly_connected_components_algorithm
        //Wikipedia image https://en.wikipedia.org/wiki/File:Tarjan%27s_Algorithm_Animation.gif
        //CNBLOG https://www.cnblogs.com/shadowland/p/5872257.html
        private List<List<int>> scc;//保存一组强连通分量
        private Stack<int> s;//定义一个临时的栈
        private bool[] onStack;//是否在栈上
        private int[] DFN;//顶点被访问的顺序。-1表示没有访问
        private int[] LOW;//顶点的能够搜索的最小的LOW编号
        private int time = 0;//定义搜索的时间
        private void tarjan(Digraph g, int v)
        {
            LOW[v] = DFN[v] = time++;
            s.Push(v);//v入栈
            onStack[v] = true;//一次访问中
            foreach(int w in g.Adj(v))
            {
                if(LOW[w] == -1)
                {
                    tarjan(g, w);//访问这个点
                    LOW[v] = Math.Min(LOW[w], LOW[v]);//设置这个点的最小能够找到的点为v或w的最小的点
                }
                else if(onStack[w])//w是v中遍历节点中的路径，
                {
                    LOW[v] = Math.Min(LOW[v], DFN[w]);//v顶点是由w派生出来的。
                }
                //其他的非联通的部分
            }
            if(LOW[v] == DFN[v])
            {
                List<int> temp = new List<int>();
                int j = -1;
                while(v !=j)
                {
                    j = s.Pop();
                    onStack[j] = false;
                    temp.Add(j);
                }
                scc.Add(temp);
            }

        }
        public TarjanSCC(Digraph g)
        {
            scc = new List<List<int>>();
            s = new Stack<int>();
            onStack = new bool[g.V];
            DFN = new int[g.V];
            LOW = new int[g.V];
            for (int i = 0; i < DFN.Length; ++i)
            {
                DFN[i] = -1;
                LOW[i] = -1;
            }
            for (int i = 0; i < g.V; ++i)
                if (DFN[i] == -1)
                    tarjan(g, i);
        }
        public int Count() => scc.Count;
        public List<List<int>> SCC() => scc;
        public static void Main()
        {
            Digraph digraph = new Digraph(System.IO.File.OpenText("tinyDG.txt"));
            TarjanSCC tarjanSCC = new TarjanSCC(digraph);
            Console.WriteLine($"含有{tarjanSCC.Count()}个联通分量");
            foreach(var connected in tarjanSCC.SCC())
            {
                foreach(var item in connected)
                {
                    Console.Write("{0} ", item);
                }
                Console.WriteLine();
            }
        }
    }
}
