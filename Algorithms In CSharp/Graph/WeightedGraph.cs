using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Algorithms_In_CSharp.Chapter1.BagDemo;
namespace Algorithms_In_CSharp.Graph
{
    /// <summary>
    /// 实现了加权图的数据结构
    /// </summary>
    class WeightedGraph:Graph, IWeighted
    {
        public WeightedGraph(int v) : base(v) {
            
        }
        public WeightedGraph(TextReader reader):base(reader)
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
                adj[w].Add(new Tuple<int, double>(v, weight));
                ++E;
            }
            else
                throw new IndexOutOfRangeException("没有提供足够的数据连接");
        }

        public void AddEdge(Int32 v, Int32 w, Double weight)
        {
            adj[v].Add(new Tuple<int, double>(w, weight));
            adj[w].Add(new Tuple<int, double>(v, weight));
            ++E;
        }
    }
}
