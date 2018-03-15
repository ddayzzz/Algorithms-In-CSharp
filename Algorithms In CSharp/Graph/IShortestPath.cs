using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph
{
    /// <summary>
    /// 最短路径都有的
    /// </summary>
    interface IShortestPath<EdgeType> where EdgeType:Edge
    {
        /// <summary>
        /// 到达顶点 v 的最短路径长度
        /// </summary>
        /// <param name="v">顶点</param>
        /// <returns></returns>
        Double DistTo(Int32 v);
        /// <summary>
        /// 是否有通路
        /// </summary>
        /// <param name="v">顶点</param>
        /// <returns></returns>
        Boolean HasPathTo(int v);
        /// <summary>
        /// 返回路径
        /// </summary>
        /// <param name="v">顶点</param>
        /// <returns></returns>
        IEnumerable<EdgeType> PathTo(int v);
    }
}
