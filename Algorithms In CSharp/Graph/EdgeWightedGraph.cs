using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Algorithms_In_CSharp.Graph
{
    class EdgeWightedGraph
    {
        public virtual Int32 V { get; protected set; }
        public virtual Int32 E { get; protected set; }
        private Chapter1.BagDemo.Bag<Edge>[] adj;//边的邻接表
        public EdgeWightedGraph(Int32 V)
        {
            this.V = V;
            this.E = 0;
            adj = new Chapter1.BagDemo.Bag<Edge>[V];
            for (var i = 0; i < V; ++i)
                adj[i] = new Chapter1.BagDemo.Bag<Edge>();
        }
        public EdgeWightedGraph(TextReader reader)
        {
            //添加边和权
            V = Int32.Parse(reader.ReadLine());//点数
            int E = Int32.Parse(reader.ReadLine());//边数(无向图边长*2.AddEdge会加上this.E。这是一个临时的变量)
            //标准的图
            adj = new Chapter1.BagDemo.Bag<Edge>[V];
            for (var i = 0; i < V; ++i)
                adj[i] = new Chapter1.BagDemo.Bag<Edge>();
            for (int i = 0; i < E; ++i)
            {
                string[] vw = reader.ReadLine().Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (vw.Length == 3)
                {
                    int v = int.Parse(vw[0]);
                    int w = int.Parse(vw[1]);
                    Double wei = Double.Parse(vw[2]);
                    AddEdge(new Edge(v, w, wei));
                }
                else
                {
                    throw new IndexOutOfRangeException("没有提供足够的数据连接");
                }
            }
            reader.Close();
        }
        public virtual void AddEdge(Edge e)
        {
            int v = e.Either();
            int w = e.Other(v);
            adj[v].Add(e);
            adj[w].Add(e);
            ++E;
        }
        public IEnumerable<Edge> Adj(Int32 v)
        {
            return adj[v];
        }
        public IEnumerable<Edge> Edges()
        {
            for(int v=0;v <V;++v)
            {
                foreach(Edge e in adj[v])
                {
                    //每一条边
                    if (e.Other(v) > v)//由于是无向图所以会有重复。
                        yield return e;
                }
            }
        }
        public static void Main(String[] args)
        {
            if(args.Length == 1)
            {
                String file = args[0];
                var g = new EdgeWightedGraph(File.OpenText(file));
                foreach (var edge in g.Edges())
                    Console.WriteLine(edge);
                Console.WriteLine("Prim MST:");
                Algorithms.PrimMST mST = new Algorithms.PrimMST(g);
                foreach (var edge in mST.Edges())
                    Console.WriteLine(edge);
                Console.WriteLine("权：{0}", mST.Weight());
                Console.WriteLine("Kruskal MST:");
                Algorithms.KruskalMST kmST = new Algorithms.KruskalMST(g);
                foreach (var edge in kmST.Edges())
                    Console.WriteLine(edge);
                Console.WriteLine("权：{0}", kmST.Weight());
            }
        }
    }
}
