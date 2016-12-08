using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDotNetSupportLibrary.Helpers
{
  public class LambdaComparer<T> : IComparer<T>
  {
    private readonly Func<T, T, int> _LambdaComparer;

    public LambdaComparer(Func<T, T, int> lambdaComparer)
    {
      this._LambdaComparer = lambdaComparer;
    }

    #region IComparer<T> Members

    public int Compare(T x, T y)
    {
      return this._LambdaComparer(x, y);
    }

    #endregion
  }
}
