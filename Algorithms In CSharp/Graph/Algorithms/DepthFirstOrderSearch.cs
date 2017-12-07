using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph.Algorithms
{
    /// <summary>
    /// 深度优先搜索算法（单点）
    /// </summary>
    public class DepthFirstSearch
    {
        private bool[] marked;
        private int[] edgeTo;
        private readonly int s;//起点
        private void DFS(Graph g, int v)
        {
            marked[v] = true;
            foreach (int w in g.Adj(v))
                if (!marked[w])
                {
                    edgeTo[w] = v;
                    DFS(g, w);
                }
        }
        public DepthFirstSearch(Graph g, int s)
        {
            marked = new bool[g.V];
            edgeTo = new int[g.V];
            this.s = s;
            DFS(g, s);
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
    }
}
