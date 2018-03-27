using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.StringDemo
{
    /// <summary>
    /// 产生随机的字符串
    /// </summary>
    class RandomStrings
    {
        /// <summary>
        /// 产生随机定长字符串的数组
        /// </summary>
        /// <param name="alphabet">字母表</param>
        /// <param name="length">每个字符串长度</param>
        /// <param name="count">字符串的长度</param>
        /// <returns></returns>
        public static String[] GenerateRandomStrings(Alphabet alphabet, int length, int count)
        {
            Random random = new Random();
            String[] res = new string[count];
            for(int i=0;i<count;++i)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for(int j=0;j<length;++j)
                {
                    int rann = random.Next(0, alphabet.R());
                    stringBuilder.Append(alphabet.ToChar(rann));
                }
                res[i] = stringBuilder.ToString();
            }
            return res;
        }
        /// <summary>
        /// 产生不规则长度的字符串，保存为数组
        /// </summary>
        /// <param name="alphabet"></param>
        /// <param name="count"></param>
        /// <param name="maxSize">上界(开区间)</param>
        /// <returns></returns>
        public static String[] GenerateRandomStrings_DifferentLength(Alphabet alphabet, int count, int maxSize)
        {
            Random random = new Random();
            String[] res = new string[count];
            for (int i = 0; i < count; ++i)
            {
                StringBuilder stringBuilder = new StringBuilder();
                int length = random.Next(1, maxSize);
                for (int j = 0; j < length; ++j)
                {
                    int rann = random.Next(0, alphabet.R());
                    stringBuilder.Append(alphabet.ToChar(rann));
                }
                res[i] = stringBuilder.ToString();
            }
            return res;
        }
    }
}
