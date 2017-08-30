using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp
{
    using System.Diagnostics;
    public static class Common
    {
        //返回N个大小的随机数据
        public static int[] GenerateRadomData(int N,int min=0,int max=100)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            int[] t = new int[N];
            int distance = max - min;
            for (int i = 0; i < N; ++i)
                t[i] = random.Next() % distance + min;
            return t;
        }
        /// <summary>
        /// 统计时间支持action类型（返回调用时间。单位ms）
        /// </summary>
        /// <param name="action">Action类型</param>
        /// <returns></returns>
        public static long CountTime(Action action)
        {
            Stopwatch Stopwatch = new Stopwatch();
            Stopwatch.Start();
            action();
            Stopwatch.Stop();
            return Stopwatch.ElapsedMilliseconds;
        }
    }
}
