using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Algorithms_In_CSharp.Graph
{
    public class Digraph : Graph, IEnumerable<DirectedEdge>, IDirectedGraph
    {
        public Digraph(int V):base(V)
        {
        }
        public Digraph(TextReader reader):base(reader)
        {
        }
        
        public IDirectedGraph Reverse()
        {
            Digraph digraph = new Digraph(V);
            for (int i = 0; i < V; ++i)
            {
                foreach (int j in Adj(i))
                {
                    digraph.AddEdge(j, i);
                }
            }
            return digraph;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Digraph:\n");
            for (int i = 0; i < V; ++i)
            {
                foreach (var j in Adj(i))
                {
                    sb.Append(i + "->" + j + "\n");
                }
            }
            return sb.ToString();
        }
        public static void Main()
        {
            StreamReader fileStream = File.OpenText("mediumDG.txt");
            Digraph di = new Digraph(fileStream);
            Console.WriteLine(di);
            Console.Write("环：");
            DirectedCycle directedCycle = new DirectedCycle(di);
            foreach (var i in directedCycle.Cycle())
            {
                Console.Write(i + "->");
            }
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
        public new IEnumerable<DirectedEdge> GetEdge(Int32 v)
        {
            foreach (var tup in adj[v])
                yield return new DirectedEdge(v, tup.Item1, tup.Item2);
        }
        protected override void AddEdge(string data)
        {
            string[] vw = data.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (vw.Length == 2)
            {
                int v = int.Parse(vw[0]);
                int w = int.Parse(vw[1]);
                adj[v].Add(new Tuple<int, double>(w, 1.0));
            }
            else
                throw new IndexOutOfRangeException("没有提供足够的数据连接");
        }
        public override void AddEdge(int v, int w)
        {
            adj[v].Add(new Tuple<int, double>(w, 1.0));
            ++E;
        }
        public override void AddEdge(Edge edge)
        {
            int v, w;
            v = edge.Either();
            w = edge.Other(v);
            adj[v].Add(new Tuple<int, double>(w, edge.Weight));
            ++E;
        }
        public void AddEdge(DirectedEdge edge)
        {
            int v, w;
            v = edge.Src;
            w = edge.End;
            adj[v].Add(new Tuple<int, double>(w, edge.Weight));
            ++E;
        }
    }


}
