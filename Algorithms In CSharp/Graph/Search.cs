using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph
{
    /// <summary>
    /// 用于处理搜索的类
    /// </summary>
    public class Search
    {
        private bool[] marked;
        private int count;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="g">图</param>
        /// <param name="s">起点</param>
        public Search(Graph g, int s)
        {
            marked = new bool[g.V];//数组的默认值为false
            DFS(g, s);
        }
        /// <summary>
        /// 判断s与v是否是连通的
        /// </summary>
        /// <param name="v">终点</param>
        /// <returns></returns>
        public bool Marked(int v)
        {
            return marked[v];
        }
        /// <summary>
        /// 与s连通的节点的数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.count;
        }
        private void DFS(Graph g, int v)
        {
            marked[v] = true;
            count++;
            foreach (int w in g.Adj(v))
                if (!marked[w])
                    DFS(g, w);
        }
    }
}
