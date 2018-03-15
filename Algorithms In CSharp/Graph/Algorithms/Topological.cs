using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph.Algorithms
{
    public class Topological
    {
        private IEnumerable<int> order;//顶点的拓扑排序
        public Topological(IDirectedGraph g)
        {
            DirectedCycle directedCycle = new DirectedCycle(g);
            if (!directedCycle.HasCycle())
            {
                //DAG
                DepthFirstOrder dfs = new DepthFirstOrder(g);
                order = dfs.ReversePost();
            }
        }
        public IEnumerable<int> Order() => order;
        public bool IsDAG() => order != null;
        public static void Main()
        {
            string filename = "tinyDAG.txt";
            Digraph digraph = new Digraph(File.OpenText(filename));
            Topological top = new Topological(digraph);
            if (top.IsDAG())
                foreach (int v in top.Order())
                    Console.WriteLine(v);
            //符号图测试
            //string filename = "jobs.txt";
            //SymbolGraph digraph = new SymbolGraph(filename, '/');
            //Topological top = new Topological(digraph.G);
            //if (top.IsDAG())
            //    foreach (int v in top.Order())
            //        Console.WriteLine(digraph.Name(v));

        }
    }
    /// <summary>
    /// 使用优先队列的拓扑排序
    /// </summary>
    public class Topological_Queue
    {
        private List<Int32> result;
        public Topological_Queue(IDirectedGraph g)
        {
            DirectedCycle directedCycle = new DirectedCycle(g);
            if (!directedCycle.HasCycle())
            {
                //DAG
                solve(g);
            }
        }
        private void solve(IDirectedGraph g)
        {
            result = new List<int>();
            Degrees degrees_info = new Degrees(g);
            int[] degrees = new int[g.V];
            for (int i = 0; i < g.V; ++i)
                degrees[i] = degrees_info.InDegree(i);
            Queue<int> starts = new Queue<int>();
            foreach (Int32 s in degrees_info.Sources())
                starts.Enqueue(s);
            //只要还有顶点
            while(starts.Count > 0)
            {
                Int32 v = starts.Dequeue();
                result.Add(v);
                foreach(var w in g.Adj(v))
                {
                    //删除这个顶点的指出的边，因为这个顶点已经被删除了
                    --degrees[w];
                    if (degrees[w] == 0)
                        //下一个拓扑排序的顺序
                        starts.Enqueue(w);
                }
            }
        }
        /// <summary>
        /// 返回一个拓扑排序的结果
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Int32> Order()
        {
            return result;
        }
        public bool IsDAG()
        {
            return result != null;
        }
        public static void Main()
        {
            //string filename = "tinyDAG.txt";
            //Digraph digraph = new Digraph(File.OpenText(filename));
            //Topological top = new Topological(digraph);
            //if (top.IsDAG())
            //    foreach (int v in top.Order())
            //        Console.WriteLine(v);
            //符号图测试
            string filename = "tinyDAG.txt";
            Digraph digraph = new Digraph(System.IO.File.OpenText(filename));
            Topological_Queue top = new Topological_Queue(digraph);
            if (top.IsDAG())
                foreach (int v in top.Order())
                    Console.WriteLine(v);

        }
    }
}
