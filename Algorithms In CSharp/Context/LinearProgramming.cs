using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms_In_CSharp.Context
{
    /// <summary>
    /// 线性规划
    /// 来自 算法4 的实现
    /// </summary>
    class LinearProgramming
    {
        private static readonly double EPSILON = 1.0E-10;
        private double[,] a;//tableaux
        private int m;//限制的数量
        private int n;//变量的个数
        private int[] basis;// basis[i] = 变量的关联的行 i

        public LinearProgramming(double[,] A, double[] b, double[] c)
        {
            m = b.Length; n = c.Length;
            for (int i = 0; i < m; ++i)
                if (!(b[i] >= 0))
                    throw new ArgumentException("RHS 必须非负");
            a = new double[m + 1, n + m + 1];
            for (int i = 0; i < m; ++i)
                for (int j = 0; j < n; ++j)
                    a[i, j] = A[i, j];
            for (int i = 0; i < m; ++i)
                a[i, n + 1] = 1.0;
            for (int j = 0; j < n; ++j)
                a[m, j] = c[j];
            for (int i = 0; i < m; ++i)
                a[i, m+n] = b[i];
            basis = new int[m];
            for (int i = 0; i < m; ++i)
                basis[i] = n + i;

        }

        private void Solve()
        {
            
        }
        /// <summary>
        /// 有正数的cost的非基本列的最低的index
        /// </summary>
        /// <returns></returns>
        private int Bland()
        {
            for(int j=0;j<m+n;++j)
            {
                if (a[m, j] > 0)
                    return j;
            }
            return -1;//最优的
        }

        private int Dantzig()
        {
            int q = 0;
            for(int j=1;j<m+n;++j)
            {
                if (a[m, j] > a[m, q])
                    q = j;
            }
            if (a[m, q] <= 0)
                return -1;//最优
            else
                return q;
        }
        /// <summary>
        /// 找到最小的行p，使用斜率（ratio）最小的原则 
        /// </summary>
        /// <param name="q"></param>
        /// <returns>返回-1表示不存在这个行</returns>
        private int MinRatioRule(int q)
        {
            int p = -1;
            for(int i=0;i<m;++i)
            {
                if (a[i, q] <= EPSILON)
                    continue;
                else if (p == -1)
                    p = i;
                else if ((a[i, m + n] / a[i, q]) < (a[p, n + m] / a[p, q]))
                    p = i;
            }
            return p;
        }
        /// <summary>
        /// pivot on entry (p, q) using Gauss-Jordan elimination
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        private void Pivot(int p, int q)
        {
            // 处理行p以及列q
            for(int i=0;i<=m;++i)
            {
                for(int j=0;j<=m+n;++j)
                {
                    if (i != p && j != q)
                        a[i, j] -= a[p, j] * a[i, q] / a[p, q];
                }
            }
            //zero out column q
            for(int i=0;i<=m;++i)
            {
                if (i != p)
                    a[i, q] = 0.0;
            }
            // scale row p
            for(int j=0;j<=m+n;++j)
            {
                if (j != q)
                    a[p, j] /= a[p, q];
            }
            a[p, q] = 1.0;
        }
        /// <summary>
        /// Returns the optimal value of this linear program.
        /// </summary>
        /// <returns></returns>
        public double Value()
        {
            return -a[m, m + n];
        }
        /// <summary>
        /// Returns the optimal primal(原始的) solution to this linear program.
        /// </summary>
        /// <returns></returns>
        public double[] Primal()
        {
            double[] x = new double[n];
            for (int i = 0; i < m; i++)
                if (basis[i] < n) x[basis[i]] = a[i,m + n];
            return x;
        }
        /// <summary>
        /// Returns the optimal dual solution to this linear program
        /// </summary>
        /// <returns></returns>
        public double[] Dual()
        {
            double[] y = new double[m];
            for (int i = 0; i < m; i++)
                y[i] = -a[m][n + i];
            return y;
        }
    }
}
