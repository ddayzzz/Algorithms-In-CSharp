using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph
{
    /// <summary>
    /// 有向欧拉图
    /// http://blog.csdn.net/shaya118/article/details/41833035
    /// </summary>
    public class Euler
    {
        private Boolean[] marked;
        private Stack<Int32> results;
        /// <summary>
        /// 定义构造函数
        /// </summary>
        /// <param name="g">有向图</param>
        public Euler(Digraph g)
        {
            Degrees degrees = new Degrees(g);
            //有向图的欧拉图判定定理，出度=入度
            for (int i = 0; i < g.V; ++i)
            {
                if (degrees.InDegree(i) != degrees.OutDegree(i))
                {
                    return;
                }
            }
            //是欧拉图
            //寻找欧拉环
            marked = new Boolean[g.V];
            results = new Stack<Int32>();
            dfs(g, 0);
        }
        private void dfs(Digraph g, int v)
        {
            marked[v] = true;
            foreach(var w in g.Adj(v))
            {
                if(!marked[w])
                {
                    //存在没有访问的顶点
                    dfs(g, w);
                }
            }
            //所有的顶点已经访问了
            results.Push(v);
        }
        public Boolean HasEulerCircle()
        {
            return results != null;
        }
        public Stack<Int32> EulerCircle()
        {
            return results;
        }
        public static void Main()
        {
            Digraph digraph = new Digraph(File.OpenText("none_euler.txt"));
            Euler euler = new Euler(digraph);
            if(euler.HasEulerCircle())
            {
                foreach (var v in euler.EulerCircle())
                    Console.Write("{0} ", v);
            }
        }
    }
}
