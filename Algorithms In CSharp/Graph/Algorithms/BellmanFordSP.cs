using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph.Algorithms
{
    class BellmanFordSP:IShortestPath<DirectedEdge>
    {
        private double[] distTo;
        private DirectedEdge[] edgeTo;
        private bool[] onQueue;//节点是否对应于
        private Queue<int> queue;//保存待放松的节点
        private int cost;//放松的次数
        private IEnumerable<DirectedEdge> cycle;//是否有负权？
        public BellmanFordSP(DirectedWeightedGraph g, int s)
        {
            distTo = new double[g.V];
            edgeTo = new DirectedEdge[g.V];
            onQueue = new bool[g.V];
            queue = new Queue<int>();
            for (int i = 0; i < g.V; ++i)
                distTo[i] = double.PositiveInfinity;
            distTo[s] = 0.0;
            queue.Enqueue(s);
            while(queue.Count > 0 && !HasNegativeCycle())
            {
                int v = queue.Dequeue();
                onQueue[v] = false;
                Relax(g, v);
            }
        }
        
        public void Relax(DirectedWeightedGraph g, int v)
        {
            foreach (DirectedEdge edge in g.GetEdge(v))
            {
                int w = edge.End;
                //如果存在放松的条件
                if (distTo[w] > distTo[v] + edge.Weight)
                {
                    distTo[w] = distTo[v] + edge.Weight;
                    edgeTo[w] = edge;
                    //添加为需要继续放松的
                    if(this.onQueue[w] == false)
                    {
                        this.queue.Enqueue(w);
                        this.onQueue[w] = true;
                    }
                }
                if (cost++ % g.V == 0)//根据436 页 定理X.V 个不含有负权环图的节点，放松 V 次可以得到最短路径
                {
                    FindNegtiveCycle();
                    if (HasNegativeCycle())
                        return;
                }
                
            }
        }
        private void FindNegtiveCycle()
        {
            int V = edgeTo.Length;//图的节点数量
            DirectedWeightedGraph spt = new DirectedWeightedGraph(V);
            for(int v=0;v<V;++v)
            {
                if (edgeTo[v] != null)
                    spt.AddEdge(edgeTo[v]);
            }
            DirectedCycle cf = new DirectedCycle(spt);
            //需要转换形式
            Queue<DirectedEdge> edges = new Queue<DirectedEdge>();//没有还
            var list = cf.Cycle().ToList();
            if (list.Count == 0)
                return; 
            for(int i=0;i<list.Count - 1;++i)
            {
                edges.Enqueue(new DirectedEdge(list[i], list[i + 1], spt.GetWeight(list[i], list[i + 1])));
            }
            cycle = edges;
        }
        public bool HasNegativeCycle()
        {
            return cycle != null;
        }
        public IEnumerable<DirectedEdge> NegativeCycle()
        {
            return cycle;
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
        public static void Main()
        {
            var fs = System.IO.File.OpenText("tinyEWDnc.txt");
            DirectedWeightedGraph graph = new DirectedWeightedGraph(fs);
            BellmanFordSP sP = new BellmanFordSP(graph, 0);
            if(sP.HasNegativeCycle())
            {
                Console.WriteLine("存在一个负权");
                foreach(var edge in sP.NegativeCycle())
                {
                    Console.WriteLine(edge);
                }
            }
        }
    }
}
