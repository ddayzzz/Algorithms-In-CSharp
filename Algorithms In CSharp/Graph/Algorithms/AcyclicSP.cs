using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph.Algorithms
{
    /// <summary>
    /// 效率高的处理。使用了拓扑排序。因为不可能再遇到需要放松的节点了。按照一定的顺序放松
    /// </summary>
    class AcyclicSP : IShortestPath<DirectedEdge>
    {
        private DirectedEdge[] edgeTo;//edgeTo[i] 保存的是 到 i 的边
        private Double[] distTo;// disTo[i] 保存的是从 v 到 i 的距离
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="g">有向加权非负权图</param>
        /// <param name="s">起点</param>
        public AcyclicSP(DirectedWeightedGraph g, int s)
        {
            edgeTo = new DirectedEdge[g.V];
            distTo = new Double[g.V];
            for (Int32 v = 0; v < g.V; ++v)
            {
                distTo[v] = Double.PositiveInfinity;
            }
            Topological top = new Topological(g);

        }

        public Double DistTo(Int32 v)
        {
            return this.distTo[v];
        }

        public Boolean HasPathTo(Int32 v)
        {
            return this.distTo[v] != Double.PositiveInfinity;
        }

        public IEnumerable<DirectedEdge> PathTo(Int32 v)
        {
            if (!HasPathTo(v))
                return null;
            Stack<DirectedEdge> edges = new Stack<DirectedEdge>();
            //向前退。自己到自己是没有路径的
            for (DirectedEdge edge = edgeTo[v]; edge != null; edge = edgeTo[edge.Src])
            {
                edges.Push(edge);
            }
            return edges;
        }

        private void Relax(DirectedWeightedGraph g, int v)
        {
            foreach (DirectedEdge edge in g.GetEdge(v))
            {
                int w = edge.End;
                //如果存在放松的条件
                if (distTo[w] > distTo[v] + edge.Weight)
                {
                    distTo[w] = distTo[v] + edge.Weight;
                    edgeTo[w] = edge;
                }
            }
        }

        public static void Main()
        {
            System.IO.TextReader s = System.IO.File.OpenText("mediumEWD.txt");
            DirectedWeightedGraph graph = new DirectedWeightedGraph(s);
            Console.Write("请输入起点：");
            int st = int.Parse(Console.ReadLine());
            DijkstraSP dijkstraSP = new DijkstraSP(graph, st);
            for (int i = 0; i < graph.V; ++i)
            {
                if (i == st)
                {
                    Console.WriteLine("{0}->{0} : 0.0", st);
                    continue;
                }
                if (!dijkstraSP.HasPathTo(i))
                {
                    Console.WriteLine("{0}->{1} : No paths", st, i);
                    continue;
                }
                Double len = 0.0;
                foreach (var edge in dijkstraSP.PathTo(i))
                {
                    Console.Write(edge);
                    Console.Write(",");
                    len += edge.Weight;
                }
                Console.WriteLine(" : {0}", len);
            }
        }
    }
}
