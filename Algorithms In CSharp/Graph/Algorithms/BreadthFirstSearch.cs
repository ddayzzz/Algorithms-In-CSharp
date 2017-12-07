using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph.Algorithms
{
    public class BreadthFirstSearch
    {
        private bool[] marked;
        private int[] edgeTo;
        private readonly int s;//起点
        public BreadthFirstSearch(Graph g, int s)
        {
            marked = new bool[g.V];
            edgeTo = new int[g.V];
            this.s = s;
            BFS(g, s);
        }
        public bool HasPathTo(int v)
        {
            return marked[v];
        }
        public IEnumerable<int> PathTo(int v)
        {
            if (!HasPathTo(v))
                return null;
            Stack<int> stack = new Stack<int>();
            for (int x = v; x != s; x = edgeTo[x])
            {
                stack.Push(x);//放入一条路径
            }
            stack.Push(s);
            return stack;
        }

        /// <summary>
        /// BFS广度优先求出以s到任何一个点的最短路径
        /// </summary>
        /// <param name="g">图</param>
        /// <param name="s">起点</param>
        private void BFS(Graph g, int s)
        {
            Queue<int> queue = new Queue<int>();//保存各个顶点
            marked[s] = true;//起点首先被标记
            queue.Enqueue(s);
            while (queue.Count > 0)
            {
                int v = queue.Dequeue();
                foreach (int w in g.Adj(v))
                {
                    if (!marked[w])
                    {
                        edgeTo[w] = v;//保存路径的最后一条边
                        marked[w] = true;//被访问
                        queue.Enqueue(w);
                    }
                }
            }
        }
    }
}
