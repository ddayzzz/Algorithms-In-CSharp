using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Algorithms_In_CSharp.Chapter1.BagDemo;

namespace Algorithms_In_CSharp.Context.FlowNetwork
{
    class FlowNetwork:Graph.WeightedGraph
    {
        //保存获取边的迭代的 Edges！
        private Bag<FlowEdge>[] temp_adjs;
        public FlowNetwork(int V):base(V)
        {
            temp_adjs = new Bag<FlowEdge>[V];
        }
        public FlowNetwork(TextReader reader) : base(reader) {
            temp_adjs = new Bag<FlowEdge>[V];
        }
        public new IEnumerator<FlowEdge> GetEnumerator()
        {
            for (int i = 0; i < V; ++i)
            {
                if (temp_adjs[i] == null)
                {
                    temp_adjs[i] = new Bag<FlowEdge>();
                    foreach (var item in adj[i])
                    {
                        temp_adjs[i].Add(new FlowEdge(i, item.Item1, item.Item2));
                    }
                }
                foreach (var e in temp_adjs[i])
                    yield return e;
            }
        }
        public new IEnumerable<FlowEdge> GetEdge(Int32 v)
        {
            if (temp_adjs[v] == null)
            {
                temp_adjs[v] = new Bag<FlowEdge>();
                foreach (var item in adj[v])
                {
                    temp_adjs[v].Add(new FlowEdge(v, item.Item1, item.Item2));
                }
            }
            foreach (var e in temp_adjs[v])
                yield return e;
        }

    }
}
