using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph
{
    /// <summary>
    /// 表示某个图是加权过的
    /// </summary>
    public interface IWeighted
    {
        void AddEdge(int v, int w, double weight);
        double GetWeight(int v, int w);
    }
}
