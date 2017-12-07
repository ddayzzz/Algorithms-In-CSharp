using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph.Algorithms
{
    /// <summary>
    /// 实现最小生成树的普里姆算法
    /// </summary>
    class PrimMST
    {
        private Edge[] edgeTo;
        private Double[] disTo;//disTo[w] = edgeTo[w].Weight
        private Boolean[] marked;//是否已经被标记了
        private Chapter2.SortDemos.IndexPriorityQueue<Double> pq;//保存最小边的最小堆
        public PrimMST(EdgeWightedGraph g)
        {
            pq = new Chapter2.SortDemos.IndexPriorityQueue<double>(g.V, (Double s, Double b) =>
            {
                return s < b;
            });//允许的标号[0,g.V]
            edgeTo = new Edge[g.V];
            marked = new Boolean[g.V];
            disTo = new Double[g.V];
            for (Int32 v = 0; v < g.V; ++v)
                disTo[v] = Double.PositiveInfinity; //表示为正无穷
            pq.Insert(0, 0.0);
            disTo[0] = 0.0;
            while (!pq.IsEmpty())
                visit(g, pq.DeleteMin());//获取二叉堆中总是最小的索引

        }
        private void visit(EdgeWightedGraph g, Int32 v)
        {
            marked[v] = true;
            foreach(Edge e in g.Adj(v))
            {
                int w = e.Other(v);
                if (marked[w])
                    continue;//访问过了
                if(e.Weight < disTo[w])
                {
                    //从MST到w的距离变短了。最佳边变成e
                    edgeTo[w] = e;
                    disTo[w] = e.Weight;
                    if (pq.Contains(w))
                        pq.Change(w, disTo[w]);
                    else
                        pq.Insert(w, disTo[w]);
                }
            }
        }
        public IEnumerable<Edge> Edges()
        {
            for(int i=1;i<edgeTo.Length;++i)//由树的定义，最小生成树只有v-1条边
            {
                yield return edgeTo[i];
            }
        }
        public Double Weight()
        {
            Double res = 0.0;
            foreach (var edge in Edges())
                res += edge.Weight;
            return res;
        }
        public static void Main()
        {

        }
    }
}
