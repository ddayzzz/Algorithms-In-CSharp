using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Algorithms_In_CSharp.Graph
{
    public class Digraph:Graph
    {
        public Digraph(int V):base(V)
        {
        }
        public Digraph(TextReader reader):base(reader)
        {
        }
        public override void AddEdge(int v, int w)
        {
            if (v < V && w < V)
            {
                adj[v].Add(w);
            }
            ++this.E;
        }
        public Digraph Reverse()
        {
            Digraph digraph = new Digraph(V);
            for(int i=0;i<V;++i)
            {
                foreach(int j in Adj(i))
                {
                    digraph.AddEdge(j, i);
                }
            }
            return digraph;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Digraph:\n");
            for(int i=0;i<V;++i)
            {
                foreach(var j in Adj(i))
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
            foreach(var i in directedCycle.Cycle())
            {
                Console.Write(i + "->");
            }
        }
    }
    
    
}
