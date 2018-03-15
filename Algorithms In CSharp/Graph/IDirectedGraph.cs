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
        
    }
}
