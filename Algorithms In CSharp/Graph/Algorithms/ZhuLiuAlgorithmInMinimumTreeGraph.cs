using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph.Algorithms
{
    /// <summary>
    /// 用于求最小树形图（查找有向图中的最小生成树）
    /// 介绍：http://www.cnblogs.com/hdu-zsk/p/8167687.html
    /// </summary>
    [Obsolete("这个算法还没有彻底弄明白")]
    class ZhuLiuAlgorithmInMinimumTreeGraph
    {
        private int[] pre;//存储父节点
        private int[] vis;//标记节点
        private int[] id;//id[i]记录节点i所在环的编号
        private double[] inw;//inw[i] 记录i入边的权值
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="digraph">有向图</param>
        /// <param name="root">起点</param>
        public ZhuLiuAlgorithmInMinimumTreeGraph(DirectedWeightedGraph digraph, int root)
        {
            pre = new int[digraph.V];
            vis = new int[digraph.V];
            id = new int[digraph.V];
            inw = new double[digraph.V];
            MinWeight = Solve(digraph, root);
        }
        private double Solve(DirectedWeightedGraph digraph, int root)
        {
            int vertex_size = digraph.V;
            int edge_size = digraph.E;
            double res = 0;//记录最终的最小树形图的
            int v;
            List<DirectedEdge> edge = ((IEnumerable<DirectedEdge>)digraph).ToList(); //digraph.ToList();
            while (true)
            {
                for (int i = 0; i < vertex_size; ++i)
                {
                    inw[i] = double.PositiveInfinity;//初始化权重很大
                }
                for (int i = 0; i < edge_size; ++i)
                {
                    DirectedEdge dedge = (DirectedEdge)edge[i];
                    if (dedge.Src != dedge.End && dedge.Weight < inw[dedge.End])
                    {
                        //如果不是一个环、边的权值小于指向节点的权值。记录前驱
                        pre[dedge.End] = dedge.Src;
                        inw[dedge.End] = dedge.Weight;
                    }
                }
                //不能存在孤立点
                for (int i = 0; i < vertex_size; ++i)
                    if (i != root && double.IsInfinity(inw[i]))
                        return -1;
                //找有向环
                int tn = 0;//记录当前查找中的环总数
                for (int i = 0; i < vertex_size; ++i)
                {
                    id[i] = -1;
                    vis[i] = -1;
                }
                inw[root] = 0;//这是根
                for (int i = 0; i < vertex_size; ++i)
                {
                    res += inw[i];//累加
                    v = i;//v 是开始遍历的节点。
                    //找图中的有向环 三种情况会终止while循环    
                    //1,直到出现带有同样标记的点说明成环    
                    //2,节点已经属于其他环    
                    //3,遍历到根    
                    while (vis[v] != i && id[v] == -1 && v != root)
                    {
                        vis[v] = i;//已经访问了
                        v = pre[v];//向上查找
                    }
                    //因为找到某节点属于其他环  或者 遍历到根  说明当前没有找到有向环    
                    if (v != root && id[v] == -1)//必须上述查找已经找到有向环    
                    {
                        for (int u = pre[v]; u != v; u = pre[u])
                            id[u] = tn;//记录节点所属的 环编号    
                        id[v] = tn++;//记录节点所属的 环编号  环编号累加    
                    }
                }
                if (tn == 0)
                    break;//不存在有向环    
                                   //可能存在独立点    
                for (int i = 0; i < vertex_size; i++)
                    if (id[i] == -1)
                        id[i] = tn++;//环数累加    
                                     //对有向环缩点  和SCC缩点很像吧      
                for(int i=0;i<edge_size;++i)
                {
                    DirectedEdge dedge = (DirectedEdge)edge[i];
                    v = dedge.Src;
                    dedge.Src = id[dedge.Src];
                    dedge.End = id[dedge.End];
                    //<u, v>有向边     
                    //两点不在同一个环 u到v的距离为 边权cost - in[v]    
                    if (dedge.Src != dedge.End)
                        dedge.Weight -= inw[v];//更新边权值 继续下一条边的判定    
                }
                vertex_size = tn;//以环总数为下次操作的点数 继续执行上述操作 直到没有环    
                root = id[root];
            }
            return res;
        }
        public double MinWeight { get; private set; }
        public static void Main()
        {
            StreamReader reader = File.OpenText("zhuliudirectedgraph.txt");
            DirectedWeightedGraph graph = new DirectedWeightedGraph(reader);
            foreach(DirectedEdge edge in graph)
            {
                Console.WriteLine(edge.Src + "->" + edge.End);
            }
            ZhuLiuAlgorithmInMinimumTreeGraph minimumTreeGraph = new ZhuLiuAlgorithmInMinimumTreeGraph(graph, 1);
            double res = minimumTreeGraph.MinWeight;
            int j = 25;
        }
    }
}
