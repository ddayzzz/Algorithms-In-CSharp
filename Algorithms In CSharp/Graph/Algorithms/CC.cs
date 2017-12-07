using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithms_In_CSharp.Chapter1.BagDemo;
using Algorithms_In_CSharp.Chapter3;
namespace Algorithms_In_CSharp.Graph.Algorithms
{
    /// <summary>
    /// 连通分量 定义：https://zh.wikipedia.org/wiki/连通图
    /// </summary>
    public class CC
    {
        private bool[] marked;
        private int[] id;
        private int count;
        /// <summary>
        /// 预处理构造函数
        /// </summary>
        /// <param name="g"></param>
        public CC(Graph g)
        {
            marked = new bool[g.V];
            id = new int[g.V];//id[v]保存的是v所属的分量ID（0~count -1)
            for (int i = 0; i < g.V; ++i)//极端情况是所有的点都是一个分量
            {
                if (!marked[i])
                {
                    DFS(g, i);
                    ++count;
                }
            }
        }
        /// <summary>
        /// v和w是否连通
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public bool Connected(int v, int w)
        {
            return id[v] == id[w];
        }
        /// <summary>
        /// 连通分量数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.count;
        }
        /// <summary>
        /// v所在的的连通分量的标识符
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int ID(int v)
        {
            return id[v];
        }
        private void DFS(Graph g, int v)
        {
            marked[v] = true;
            id[v] = count;
            foreach (int w in g.Adj(v))
                if (!marked[w])
                {

                    DFS(g, w);
                }
        }
        public Bag<int>[] Component()
        {
            Bag<int>[] result = new Bag<int>[count];
            for (int i = 0; i < count; ++i)
                result[i] = new Bag<int>();
            for (int i = 0; i < marked.Length; ++i)
            {
                result[ID(i)].Add(i);
            }
            return result;
        }
    }
}
