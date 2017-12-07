using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Algorithms_In_CSharp.Graph
{
    /// <summary>
    /// 符号图
    /// </summary>
    public class SymbolGraph
    {
        internal Digraph G;//保存的图的数据成员
        internal Dictionary<string, int> st;//保存顶点名称->图中索引的成员
        internal string[] keys;//保存对应索引的名称
        public SymbolGraph(string filename, char delimiter)
        {
            st = new Dictionary<string, int>();//符号和符号的编号的映射
            try
            {
                var reader = new System.IO.StreamReader(filename);
                string oneline = reader.ReadLine();
                while (oneline != null)
                {
                    string[] vw = oneline.Split(delimiter);
                    for (int i = 0; i < vw.Length; ++i)
                    {
                        if (!Contains(vw[i]))
                        {
                            st.Add(vw[i], st.Count);
                        }
                    }
                    oneline = reader.ReadLine();
                }
                keys = new string[st.Count];
                G = new Digraph(st.Count);
                foreach (var item in st)
                    keys[item.Value] = item.Key;
                //重新定位
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                //添加边
                oneline = reader.ReadLine();
                while (oneline != null)
                {
                    string[] vetx = oneline.Split(delimiter);
                    int v = st[vetx[0]];
                    for (int i = 1; i < vetx.Length; ++i)
                        G.AddEdge(v, st[vetx[i]]);
                    oneline = reader.ReadLine();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
        /// <summary>
        /// 获取顶点key对应的索引
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int Index(string key)
        {
            if (st.ContainsKey(key))
                return st[key];
            return -1;
        }
        /// <summary>
        /// 是否包含名为key顶点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return st.ContainsKey(key);
        }
        /// <summary>
        /// 索引v的顶点名称
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public string Name(int v)
        {
            return keys[v];
        }
        public override string ToString()
        {
            StringWriter sw = new StringWriter();
            for (int i = 0; i < G.V; ++i)
            {
                sw.WriteLine(Name(i) + ":");
                foreach (var item in G.Adj(i))
                {
                    sw.WriteLine("\t" + Name(item));
                }
            }
            return sw.GetStringBuilder().ToString();
        }
        /// <summary>
        /// 测试
        /// </summary>
        public static void Main()
        {
            SymbolGraph sym = new SymbolGraph("movies.txt", '/');
            Console.WriteLine(sym.ToString());
            Console.ReadKey();
        }
    }
}
