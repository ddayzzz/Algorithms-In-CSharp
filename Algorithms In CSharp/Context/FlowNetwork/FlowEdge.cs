using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithms_In_CSharp.Graph;
namespace Algorithms_In_CSharp.Context.FlowNetwork
{
    class FlowEdge:DirectedEdge
    {
        private double m_flow;//流量
        public FlowEdge(int v, int w, double cap):base(v, w, cap)
        {
            this.m_flow = 0.0;
        }
        /// <summary>
        /// 边的容量
        /// </summary>
        public double Capacity { get { return this.Weight; } }
        /// <summary>
        /// 边中的流量
        /// </summary>
        /// <returns></returns>
        public double Flow()
        {
            return m_flow;
        }
        /// <summary>
        /// v 中剩余的流量
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double ResidualCapacityTo(int v)
        {
            if (v == this.Src)
                return m_flow;//如果是是同一个起点
            else if (v == this.End)
                return Capacity - m_flow;
            else
                throw new ArgumentException("非法的顶点");

        }
        /// <summary>
        /// 将v的流量增加 delta
        /// </summary>
        /// <param name="v"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public void AddResidualFlowTo(int v, double delta)
        {
            if (v == this.Src)
                m_flow -= delta;
            else if (v == this.End)
                m_flow += delta;
            else
                throw new ArgumentException("非法的顶点");

        }
        public override string ToString()
        {
            return String.Format("{0}->{1} {2:F2} {3:F2}", Src, End, Capacity, m_flow);
        }
    }
}
