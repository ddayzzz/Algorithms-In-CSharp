using Algorithms_In_CSharp.Chapter1.BagDemo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Graph
{
    public class Graph:IEnumerable<Edge>, IGraph
    {
        /// <summary>
        /// 虚函数：顶点数目
        /// </summary>
        public int V { get; protected set; }
        /// <summary>
        /// 虚函数：边数量
        /// </summary>
        public virtual int E { get; protected set; }
        /// <summary>
        /// 保存邻接表
        /// </summary>
        protected Bag<Tuple<int, double>>[] adj;//邻接表
        /// <summary>
        /// 构造函数，指定顶点数量
        /// </summary>
        /// <param name="V">顶点数量</param>
        public Graph(int V)
        {
            this.V = V;
            this.E = 0;//零图
            this.adj = new Bag<Tuple<int, double>>[V];
            for (int v = 0; v < V; ++v)
                adj[v] = new Bag<Tuple<int, double>>();//类型实例化
        }
        /// <summary>
        /// 从文件流中读取
        /// </summary>
        /// <param name="reader">已经打开的流对象</param>
        public Graph(System.IO.TextReader reader)
        {
            //标准的图
            string V = reader.ReadLine();//点数
            adj = new Bag<Tuple<int, double>>[int.Parse(V)];
            for (int i = 0; i < adj.Length; ++i)
                adj[i] = new Bag<Tuple<int, double>>();
            string E = reader.ReadLine();//边数
            int ie = int.Parse(E);
            this.V = adj.Length;
            this.E = ie;
            for (int i = 0; i < ie; ++i)
            {
                AddEdge(reader.ReadLine());
            }
            reader.Close();
        }
        /// <summary>
        /// 添加边
        /// </summary>
        /// <param name="data">顶点、顶点、权重的数据组字符串</param>
        public virtual void AddEdge(string data)
        {
            string[] vw = data.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if(vw.Length == 2)
            {
                int v = int.Parse(vw[0]);
                int w = int.Parse(vw[1]);
                adj[v].Add(new Tuple<int, double>(w,1.0));
                adj[w].Add(new Tuple<int, double>(v, 1.0));
                ++E;//加上无向图的数量需要+1
            }
            else
                throw new IndexOutOfRangeException("没有提供足够的数据连接");
        }
        public virtual void AddEdge(int v,int w)
        {
            adj[v].Add(new Tuple<int, double>(w, 1.0));
            adj[w].Add(new Tuple<int, double>(v, 1.0));
            ++E;
        }
        /// <summary>
        /// v和w之间是否有之间相连的边
        /// </summary>
        /// <param name="v">顶点v</param>
        /// <param name="w">顶点w</param>
        /// <returns></returns>
        public bool Connected(int v, int w)
        {
            var t = adj[v];
            foreach (var item in t)
                if (item.Item1 == w)
                    return true;
            return false;
        }
        /// <summary>
        /// 邻接顶点的序列
        /// </summary>
        /// <param name="v">顶点</param>
        /// <returns></returns>
        public virtual IEnumerable<int> Adj(int v)
        {
            foreach (var item in adj[v])
            {
                yield return item.Item1;
            }
        }
        /// <summary>
        /// 转换为字符显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("V={0}\nE={1}", this.V, this.E);
            for (int i = 0; i < V; ++i)
            {
                foreach (var item in adj[i])
                {
                    sw.WriteLine(string.Format("Edge:{0}-->{1}", i, item));
                }
            }
            return sw.GetStringBuilder().ToString();
        }

        public IEnumerator<Edge> GetEnumerator()
        {
            for(int i=0;i<V;++i)
            {
                foreach(var item in adj[i])
                {
                    //产生新的边对象
                    yield return new Edge(i, item.Item1, item.Item2);//这是无向不加权图
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public virtual IEnumerable<Edge> GetEdge(Int32 v)
        {
            foreach (int w in Adj(v))
                yield return new Edge(v, w, 1.0);//默认的权值为1.0
        }
    }
}
