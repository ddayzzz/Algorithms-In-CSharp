using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph
{
    public class DirectedEdge:Edge
    {
        public int Src { get; set; }
        public int End { get; set; }
        public DirectedEdge(int src, int end, double weight):base(src,end,weight)
        {
            Src = src;End = end;
        }
        public override String ToString()
        {
            return String.Format("{0}--{2}-->{1}", Src, End, Weight);
        }
    }
}
