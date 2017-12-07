using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph.Algorithms
{
    /// <summary>
    /// 定义强连通分量的API。使用Kosaraju算法
    /// </summary>
    class KosarajuSCC
    {
        private bool[] marked;
        private int[] id;
        private int count;//强连通分量的数量
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="g">有向图</param>
        public KosarajuSCC(Digraph g)
        {
            marked = new bool[g.V];
            id = new int[g.V];
            DepthFirstOrder order = new DepthFirstOrder(g.Reverse());
            foreach (var w in order.ReversePost())//获取逆图的拓扑排序
                if (!marked[w])//在逆图的拓扑序上
                {
                    dfs(g, w);//
                    ++count;
                }
        }
        /// <summary>
        /// 是否是强连通的（v和w在一个等价类中）
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public bool StronglyConnected(int v, int w)
        {
            return id[v] == id[w];
        }
        /// <summary>
        /// 图中强连通分量的总数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return count;
        }
        /// <summary>
        /// v所在的强连通分量的编号（介于0~Count()-1之间）
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int ID(int v)
        {
            return id[v];
        }
        private void dfs(Digraph g, int v)
        {
            marked[v] = true;
            id[v] = count;
            foreach (var w in g.Adj(v))
                if (!marked[w])
                    dfs(g, w);
        }
        public static void Main()
        {
            Digraph digraph = new Digraph(System.IO.File.OpenText("tinyDAG.txt"));
            KosarajuSCC scc = new KosarajuSCC(digraph);
            Degrees degrees = new Degrees(digraph);
            Console.WriteLine($"包含{scc.Count()}个强连通分量");
            List<int>[] compolents= new List<int>[scc.count];
            for (int i = 0; i < scc.count; ++i)
                compolents[i] = new List<int>();
            for (int i = 0; i < digraph.V; ++i)
            {
                compolents[scc.id[i]].Add(i);
            }
            foreach(var connected in compolents)
            {
                foreach (var item in connected)
                    Console.Write("{0} ", item);
                Console.WriteLine();
            }
        }
    }
}
