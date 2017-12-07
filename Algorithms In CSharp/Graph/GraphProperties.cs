using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph
{
    public class GraphProperties
    //reference https://www.tutorialspoint.com/graph_theory/graph_theory_basic_properties.htm
    {
        private Graph graph;
        private int[] distance;//保存从某一点开始到w的距离
        private int[] edgeTo;//前驱数组
        private int INF = 65535;//INF
        public GraphProperties(Graph g)
        {
            this.graph = g;
        }
        /// <summary>
        /// v点的离心率
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int Eccentricity(int v)
        {
            //需要求出v的最远距离的最短路径的长度
            SortedSet<int> set = new SortedSet<int>();
            Algorithms.DepthFirstSearch ds = new Algorithms.DepthFirstSearch(graph, v);
            for (int i = 0; i < graph.V; ++i)
            {
                if (ds.HasPathTo(i))
                {
                    int count = 0;
                    foreach (var w in ds.PathTo(i))
                        ++count;
                    set.Add(count);//由于是无向无权图，所以DFS探测的是最长的路径。由于是简单图，路径的长度就是简单路径的长度
                }
            }
            return set.Max;
        }
        /// <summary>
        /// 图的直径
        /// </summary>
        public int Diameter()
        {
            SortedSet<int> set = new SortedSet<int>();
            for (int v = 0; v < graph.V; ++v)
            {
                set.Add(Eccentricity(v));
            }
            return set.Max;
        }
        public int Radius()
        {
            SortedSet<int> set = new SortedSet<int>();
            for (int v = 0; v < graph.V; ++v)
            {
                set.Add(Eccentricity(v));
            }
            return set.Min;
        }
        /// <summary>
        /// 图的某个中点
        /// </summary>
        public int Center()
        {
            int radius = Radius();
            for (int v = 0; v < graph.V; ++v)
            {
                int e = Eccentricity(v);
                if (e == radius)
                    return v;
            }
            return -1;
        }
        /// <summary>
        /// 返回图的周长(不包含n小于等于3的圈的长度)
        /// </summary>
        /// <returns>如果图是有环图，返回最短环的长度。如果是无环图，返回65535表示INF</returns>
        public int Girth()
        {
            //ref http://morris821028.github.io/2014/05/05/oj/uva/uva-12544/
            //终止的情况就是所有的节点都被访问了
            int min = INF;
            for (int i = 0; i < graph.V; ++i)
            {
                min = Math.Min(min, BFS(i));
            }
            return min;
        }
        private int BFS(int s)
        {
            int min = INF;
            this.distance = new int[graph.V];
            this.edgeTo = new int[graph.V];
            this.distance[s] = 1;//起始点到起始点的路径为1
            this.edgeTo[s] = -1;//前驱节点为-1
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(s);//搜索这个点
            while (queue.Count > 0)
            {
                int v = queue.Dequeue();//搜这个顶点的边
                foreach (int w in graph.Adj(v))
                {
                    if (edgeTo[v] == w)
                        continue;//v的前驱和w相同避免往回走
                    if (this.distance[w] == 0)
                    {
                        this.distance[w] = this.distance[v] + 1;
                        this.edgeTo[w] = v;
                        queue.Enqueue(w);
                    }
                    else
                    {
                        //一条简单路径上存在已经访问的点
                        min = Math.Min(min, this.distance[v] + this.distance[w] - 1);//从w出发的，构成一个最小的圈。这个问题是解决了w来自于v，而w不是s开始的问题，可以找到最小的顶点
                    }
                }
            }
            return min;
        }
    }
}
