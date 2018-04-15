using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Context.FlowNetwork
{
    /// <summary>
    /// 求解最大流的 Ford Fulkerson 算法
    /// 帮助理解：http://www.cnblogs.com/Booble/archive/2011/03/04/1970453.html
    /// </summary>
    class FordFulkerson
    {
        private static readonly double FLOATING_POINT_EPSILON = 1E-11;
        private bool[] marked;
        private FlowEdge[] edgeTo;
        public FordFulkerson(FlowNetwork G, int s, int t)
        {
            if(!IsFeasible(G, s,t))
            {
                Console.WriteLine("网络配置不可行！");
                return;
            }
            Value = 0.0;
            while(HasAugmentingPath(G, s,t))
            {
                //利用所有存在的增广路径，计算瓶颈容量
                double bottle = double.PositiveInfinity;
                for(int v=t;v !=s;v=edgeTo[v].Other(v))
                {
                    bottle = Math.Min(bottle, edgeTo[v].ResidualCapacityTo(v));
                }
                //增大流量
                for (int v = t; v != s; v = edgeTo[v].Other(v))
                    edgeTo[v].AddResidualFlowTo(v, bottle);//这个需要判断是否是逆路径
                Value += bottle;
            }
        }

        public double Value { get; protected set; }
        public bool InCut(int v) { return marked[v]; }
        public static void Main(String[] args)
        {
            if(args.Length < 1)
            {
                Console.WriteLine("<digraph file>");
                return;
            }
            FlowNetwork flowNetwork = new FlowNetwork(System.IO.File.OpenText(args[0]));
            int s = 0, t = flowNetwork.V - 1;
            FordFulkerson maxFlow = new FordFulkerson(flowNetwork, s, t);
            Console.WriteLine("最大流 从{0} 到 {1}", s, t);
            for(int v=0;v<flowNetwork.V;++v)
            {
                foreach(FlowEdge edge in flowNetwork.GetEdge(v))
                {
                    if (v == edge.Src && edge.Flow() > 0)
                        Console.WriteLine("\t" + edge);
                }
            }
            Console.WriteLine("最大的流：{0}", maxFlow.Value);
        }
        /// <summary>
        /// 是否存在一个增广路径
        /// </summary>
        /// <param name="G"></param>
        /// <param name="s">st图的起点</param>
        /// <param name="t">汇点</param>
        /// <returns></returns>
        private bool HasAugmentingPath(FlowNetwork G, int s,int t)
        {
            marked = new bool[G.V];
            edgeTo = new FlowEdge[G.V];
            Queue<int> q = new Queue<int>();
            marked[s] = true;
            q.Enqueue(s);
            while(q.Count > 0 && !marked[t])
            {
                int v = q.Dequeue();
                foreach(FlowEdge e in G.GetEdge(v))//类似于 BFS 探测。
                {
                    int w = e.Other(v);
                    if(!marked[w] && e.ResidualCapacityTo(w) > 0) // w点没有访问，同时返回从 v-> w 的剩余流量（容量-当前的流量）
                    {
                        //在剩余网络中，对于任意一条连接到一个未被标记的顶点的边
                        edgeTo[w] = e;
                        marked[w] = true;
                        q.Enqueue(w);
                    }
                }
            }
            return marked[t];
        }
        /// <summary>
        /// 检查顶点 v 的局部平衡
        /// </summary>
        /// <param name="G"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private bool LocalEq(FlowNetwork G, int v)
        {
            double netflow = 0.0;//净流量
            foreach(FlowEdge e in G.GetEdge(v))
            {
                if (v == e.Src)
                    netflow -= e.Flow();//流出的流量
                else
                    netflow += e.Flow();//流入的流量
            }
            return Math.Abs(netflow) < FLOATING_POINT_EPSILON;
        }
        private bool IsFeasible(FlowNetwork G, int s, int t)
        {
            for (int v = 0; v < G.V; v++)
            {
                foreach (FlowEdge e in G.GetEdge(v))
                {
                    if (e.Flow() < 0.0 || e.Flow() > e.Capacity) // 必须满足限制
                    {
                        //JAVA : System.err.println("Edge does not satisfy capacity constraints: " + e);
                        return false;
                    }
                }
            }
            //检查每个顶点的局部平衡
            for (int v = 0; v < G.V; v++)
            {
                if (v != s && v != t && !LocalEq(G, v))
                    return false;
            }
            return true;
        }
        private double Excess(FlowNetwork G, int v)
        {
            double excess = 0.0;
            foreach(FlowEdge e in G.GetEdge(v))
            {
                if (v == e.Src) excess -= e.Flow();
                else excess += e.Flow();
            }
            return excess;
        }
    }
}
