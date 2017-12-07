using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph
{
    /// <summary>
    /// 顶一个有向图的环
    /// </summary>
    class DirectedCycle
    {
        private bool[] marked;
        private int[] edgeTo;
        private Stack<int> cycle;//保存一个环
        private bool[] onStack;//递归调用栈上的所有顶点。用来确定一条路径
        public DirectedCycle(Digraph digraph)
        {
            onStack = new bool[digraph.V];
            edgeTo = new int[digraph.V];
            marked = new bool[digraph.V];
            for(int v=0;v < digraph.V;++v)
            {
                if (!marked[v])
                    dfs(digraph, v);
            }
        }
        private void dfs(Digraph digraph, int v)
        {
            onStack[v] = true;
            marked[v] = true;
            foreach(int w in digraph.Adj(v))
            {
                if (HasCycle())
                    return;
                else if(!marked[w])
                {
                    edgeTo[w] = v;
                    dfs(digraph, w);
                }
                else if(onStack[w])//如果访问过且在一个路径中
                {
                    cycle = new Stack<int>();
                    for (int x = v; x != w; x = edgeTo[x])
                        cycle.Push(x);//把前驱放入栈中直到环的起点的后继
                    cycle.Push(w);
                    cycle.Push(v);//为何从v开始呢？因为v->w是有连接的。此时栈顶和栈底都是一个
                }
                
            }
            onStack[v] = false;//循环结束，不是一条栈上链的
        }
        public bool HasCycle()
        {
            return cycle != null;
        }
        public IEnumerable<int> Cycle()
        {
            if(cycle != null)
            {
                foreach (var item in cycle)
                    yield return item;
            }
            else
            {
                yield break;
            }
        }
    }
}
