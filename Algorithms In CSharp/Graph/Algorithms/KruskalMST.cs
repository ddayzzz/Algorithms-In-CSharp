using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph.Algorithms
{
    /// <summary>
    /// 无向图最小生成树的避圈法：克鲁斯卡尔算法
    /// </summary>
    class KruskalMST
    {
        private Edge[] mst;
        private Double weight = 0.0;
        public KruskalMST(EdgeWightedGraph g)
        {
            mst = new Edge[g.V - 1];//保存边序列
            var pq = new Chapter2.SortDemos.BinaryHeap<Edge>(g.E, (Edge e1, Edge e2) => e1.Weight < e2.Weight);//最小堆
            foreach (var edge in g.Edges())
                pq.Insert(edge);
            var uf = new Chapter1.UnionFindDemo.UnionFind(g.V);//判断v是否已经在一个子树中
            int index = 0;
            while(!pq.IsEmpty())
            {
                var edge = pq.Delete();
                int v = edge.Either();int w = edge.Other(v);
                if (uf.IsConnected(v, w))//不在一个子树中（一个连通分量）
                    continue;
                uf.Union(v, w);
                mst[index++] = edge;
                weight += edge.Weight;
            }
        }
        public IEnumerable<Edge> Edges()
        {
            for (int i = 0; i < mst.Length; ++i)//由树的定义，最小生成树只有v-1条边
            {
                yield return mst[i];
            }
        }
        public Double Weight()
        {
            return weight;
        }
    }
}
