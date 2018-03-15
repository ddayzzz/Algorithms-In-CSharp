using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Algorithms_In_CSharp.Graph
{
    /// <summary>
    /// 有向加权图定义
    /// </summary>
    class DirectedWeightedGraph:WeightedGraph, IDirectedGraph
    {
        public DirectedWeightedGraph(int v) : base(v) { }
        public DirectedWeightedGraph(TextReader reader) : base(reader)
        {

        }
        public override void AddEdge(String data)
        {
            string[] vw = data.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (vw.Length == 3)
            {
                int v = int.Parse(vw[0]);
                int w = int.Parse(vw[1]);
                double weight = double.Parse(vw[2]);
                adj[v].Add(new Tuple<int, double>(w, weight));
            }
            else
                throw new IndexOutOfRangeException("没有提供足够的数据连接");
        }
        public new IEnumerator<DirectedEdge> GetEnumerator()
        {
            for (int i = 0; i < V; ++i)
            {
                foreach (var item in adj[i])
                {
                    //产生新的边对象
                    yield return new DirectedEdge(i, item.Item1, item.Item2);//这是无向不加权图
                }
            }
        }

        public IDirectedGraph Reverse()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Edge> GetEdge(Int32 v)
        {
            foreach (var tup in adj[v])
                yield return new DirectedEdge(v, tup.Item1, tup.Item2);
        }
    }
}
