using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph
{
    public interface IDirectedGraph:IGraph
    {
        IDirectedGraph Reverse();
        /// <summary>
        /// 添加一个已经存在的有向边
        /// </summary>
        /// <param name="edge">边</param>
        void AddEdge(DirectedEdge edge);
    }
}
