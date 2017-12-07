using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph
{
    public sealed class Edge:IComparable<Edge>, IComparable
    {
        public Double Weight { get; }
        private readonly Int32 v;
        private readonly Int32 w;

        public Edge(Int32 v, Int32 w, Double weight)
        {
            this.v = v;
            this.w = w;
            Weight = weight;
        }

        public Int32 CompareTo(Edge other)
        {
            if (this.Weight < other.Weight)
                return -1;
            else if (Weight > other.Weight)
                return 1;
            return 0;
        }
        public Int32 Other(Int32 vertex)
        {
            if (vertex == v)
                return w;
            else if (vertex == w)
                return v;
            else
                throw new ArgumentException("不是有效的边关联顶点");
        }
        public Int32 Either()
        {
            return v;
        }
        public override string ToString()
        {
            return String.Format("{0}-{1} {2}", v, w, Weight);
        }

        public int CompareTo(object obj)
        {
            var d = Weight - ((Edge)obj).Weight;
            if (d < 0.0)
                return -1;
            else if (d > 0.0)
                return 1;
            return 0;
        }
    }
}
