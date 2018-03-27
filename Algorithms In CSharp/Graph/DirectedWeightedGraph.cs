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
    public class DirectedWeightedGraph:Digraph, IWeighted
    {
        public DirectedWeightedGraph(int v) : base(v) { }
        public DirectedWeightedGraph(TextReader reader) : base(reader)
        {

        }

        public void AddEdge(Int32 v, Int32 w, Double weight)
        {
            adj[v].Add(new Tuple<int, double>(w, weight));
            ++E;
        }

        public Double GetWeight(Int32 v, Int32 w)
        {
            Tuple<int,double> res = adj[v].FirstOrDefault((item)=>item.Item1==w);
            if (res != null)
                return res.Item2;
            else
                throw new IndexOutOfRangeException("顶点不存在");
        }

        protected override void AddEdge(String data)
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
    }
}
