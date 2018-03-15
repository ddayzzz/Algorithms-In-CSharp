using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph
{
    /// <summary>
    /// 定义图的度
    /// </summary>
    class Degrees
    {
        private Int32[] indegrees;
        private Int32[] outdegrees;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="g">图</param>
        public Degrees(IGraph g)
        {
            indegrees = new int[g.V];
            outdegrees = new int[g.V];
            //计算入度
            for(int i=0;i<g.V;++i)
            {
                for(int j=0;j<g.V;++j)
                {
                    if (g.Connected(i, j))
                        outdegrees[i]++;
                    if (g.Connected(j, i))
                        indegrees[i]++;
                }
            }
        }
        /// <summary>
        /// 返回顶点的入度
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Int32 InDegree(int v)
        {
            return indegrees[v];
        }
        /// <summary>
        /// 返回顶点的出度
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Int32 OutDegree(Int32 v)
        {
            return outdegrees[v];
        }
        /// <summary>
        /// 返回所有的起点集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Int32> Sources()
        {
            for(int i=0;i<indegrees.Length;++i)
                if (indegrees[i] == 0)
                    yield return i;
        }
        /// <summary>
        /// 返回所有终点的集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Int32> Sinks()
        {
            for (int i = 0; i < indegrees.Length; ++i)
                if (indegrees[i] == 0)
                    yield return i;
        }
        /// <summary>
        /// 图是一个映射吗？允许自环，每个顶点出度为1
        /// </summary>
        /// <returns></returns>
        public Boolean IsMap()
        {
            for (int i = 0; i < indegrees.Length; ++i)
                if (outdegrees[i] != 1)
                    return false;
            return true;
        }
    }
}
