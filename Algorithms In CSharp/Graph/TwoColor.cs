using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph
{
    /// <summary>
    /// 图是否是二分图（双色）
    /// </summary>
    public class TwoColor
    {
        private bool[] marked;
        private bool[] color;
        private bool isTwoColorable = true;
        public TwoColor(Graph g)
        {
            marked = new bool[g.V];
            color = new bool[g.V];
            for (int i = 0; i < g.V; ++i)
                if (!marked[i])
                    DFS(g, i);//可能存在子图
        }
        private void DFS(Graph g, int v)
        {
            marked[v] = true;//这个节点被访问了
            foreach (int w in g.Adj(v))
            {
                if (!marked[w])
                {
                    color[w] = !color[v];//周围绘制不相同的颜色
                    DFS(g, w);
                }
                else
                {
                    if (color[v] == color[w])
                        isTwoColorable = false;
                }
            }
        }
        public bool IsTwoColor()
        {
            return isTwoColorable;
        }
    }
}
