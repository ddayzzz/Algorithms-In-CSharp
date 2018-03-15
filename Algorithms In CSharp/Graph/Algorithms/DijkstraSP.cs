using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph.Algorithms
{
    /// <summary>
    /// Dijkstra 算法求有向图单源最短路径
    /// </summary>
    class DijkstraSP:IShortestPath<DirectedEdge>
    {
        private DirectedEdge[] edgeTo;//edgeTo[i] 保存的是 到 i 的边
        private Double[] disTo;// disTo[i] 保存的是从 v 到 i 的距离
        private Chapter2.SortDemos.IndexPriorityQueue<Double> pq;//始终有最短的边
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="graph">有向加权非负权图</param>
        /// <param name="s">起点</param>
        public DijkstraSP(DirectedWeightedGraph graph, int s)
        {
            edgeTo = new DirectedEdge[graph.V];
            disTo = new Double[graph.V];
            pq = new Chapter2.SortDemos.IndexPriorityQueue<double>(graph.V);
            for(Int32 v=0;v<graph.V;++v)
            {
                disTo[v] = Double.PositiveInfinity;
                
            }
            disTo[s] = 0.0;//自己到自己为0
            pq.Insert(s, 0.0);
            while (!pq.IsEmpty())
                Relax(graph, pq.DeleteMin());//放松节点，找到到某个点的最短的距离
        }

        public Double DistTo(Int32 v)
        {
            return this.disTo[v];
        }

        public Boolean HasPathTo(Int32 v)
        {
            return this.disTo[v] != Double.PositiveInfinity;
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
            foreach(DirectedEdge edge in g.GetEdge(v))
            {
                int w = edge.End;
                //如果存在放松的条件
                if (disTo[w] > disTo[v] + edge.Weight)
                {
                    //存在，需要更新
                    disTo[w] = disTo[v] + edge.Weight;
                    edgeTo[w] = edge;
                    //重新累计从 s 到某个点的最短距离。这个距离只有可能减小
                    if (pq.Contains(w))
                        pq.Change(w, disTo[w]);
                    else
                        pq.Insert(w, disTo[w]);
                }
            }
        }
        public static void Main()
        {
            System.IO.TextReader s = System.IO.File.OpenText("tinyEWDAG.txt");
            DirectedWeightedGraph graph = new DirectedWeightedGraph(s);
            Console.Write("请输入起点：");
            int st = int.Parse(Console.ReadLine());
            DijkstraSP dijkstraSP = new DijkstraSP(graph, st);
            for(int i=0;i<graph.V;++i)
            {
                if (i == st)
                {
                    Console.WriteLine("{0}->{0} : 0.0", st);
                    continue;
                }
                if(!dijkstraSP.HasPathTo(i))
                {
                    Console.WriteLine("{0}->{1} : No paths", st, i);
                    continue;
                }
                Double len = 0.0;
                foreach(var edge in dijkstraSP.PathTo(i))
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
