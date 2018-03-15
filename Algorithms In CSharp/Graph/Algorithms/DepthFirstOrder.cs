
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph.Algorithms
{
    /// <summary>
    /// 有向图基于深度优先搜索的顶点顺序
    /// 参考 http://blog.csdn.net/u011589125/article/details/41593649
    /// </summary>
    public class DepthFirstOrder
    {
        private bool[] marked;
        private Queue<int> pre;//所有顶点的前序排序
        private Queue<int> post;//所有顶点的后序排列
        private Stack<int> reversePost;//所有顶点的逆后序排列
        public DepthFirstOrder(IGraph g)
        {
            pre = new Queue<int>();
            post = new Queue<int>();
            reversePost = new Stack<int>();
            marked = new bool[g.V];
            for (int i = 0; i < g.V; ++i)
                if (!marked[i])  //遍历所有的顶点
                    dfs(g, i);//便利还没有搜索的顶点
        }

        private void dfs(IGraph g, int v)
        {
            //前向排序是顶点的顺序遍历之前的顺序
            pre.Enqueue(v);
            marked[v] = true;
            foreach (int w in g.Adj(v))
                if (!marked[w])
                    dfs(g, w);
            post.Enqueue(v);//这个是逆序的
            reversePost.Push(v);//递归后将顶点压栈
        }
        /// <summary>
        /// 返回前序的顶点顺序
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> Pre()
        {
            return pre;
        }
        /// <summary>
        /// 返回后序的顶点顺序
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> Post()
        {
            return post;
        }
        /// <summary>
        /// 返回一个后序添加的栈中的顶点
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> ReversePost()
        {
            return reversePost;
        }
    }
}
