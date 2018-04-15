using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Algorithms_In_CSharp.Context.SuffixArray
{
    class KWIC
    {
        public static void Main(string[] args)
        {
            if(args.Length < 2)
            {
                Console.WriteLine("<txt file> <line>");
                return;
            }
            var file = args[0];
            var line = Convert.ToInt32(args[1]);
            var streamReader = File.OpenText(file);
            string text = streamReader.ReadToEnd();
            var regexp = new System.Text.RegularExpressions.Regex("\\s+");
            text = regexp.Replace(text, " ");//替换所有的空白
            int N = text.Length;
            SuffixArray sa = new SuffixArray(text);
            string code;
            do
            {
                Console.Write("查找的内容：");
                code = Console.ReadLine();
                if (code == "exit")
                    break;
                for (int i = sa.Rank(code); i < N && sa.Select(i).StartsWith(code); ++i)
                {
                    int from = Math.Max(0, sa.Index(i) - line);//有的字符可能很短
                    int to = Math.Min(N - 1, from + code.Length + 2 * line);
                    Console.WriteLine(text.Substring(from, to - from));//截取的范围
                }
            } while (true);
        }
    }
}
