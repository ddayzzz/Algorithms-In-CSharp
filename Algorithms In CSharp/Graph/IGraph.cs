using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph
{
    /// <summary>
    /// 定义图的特性
    /// </summary>
    public interface IGraph
    {
        /// <summary>
        /// 图的属性，顶点数量
        /// </summary>
        int V { get; }
        /// <summary>
        /// 边的数量
        /// </summary>
        int E { get; }
        /// <summary>
        /// 添加的边
        /// </summary>
        /// <param name="v">点1</param>
        /// <param name="w">点2</param>
        void AddEdge(int v, int w);
        /// <summary>
        /// 判断是否已经连接
        /// </summary>
        /// <param name="v">点1</param>
        /// <param name="w">点2</param>
        /// <returns></returns>
        Boolean Connected(int v, int w);
        /// <summary>
        /// 返回点v的邻接顶点
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        IEnumerable<int> Adj(int v);
        /// <summary>
        /// 返回一个顶点的邻接边
        /// </summary>
        /// <param name="v">顶点</param>
        /// <returns></returns>
        IEnumerable<Edge> GetEdge(int v);
    }
}
