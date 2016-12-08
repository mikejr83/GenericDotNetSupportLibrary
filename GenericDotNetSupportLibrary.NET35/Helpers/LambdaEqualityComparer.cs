using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDotNetSupportLibrary.Helpers
{
  /// <summary>
  /// Allows for the creation of an IEqualityComparer with a Lambda statement.
  /// </summary>
  /// <typeparam name="T">Comparable object.</typeparam>
  public class LambdaEqualityComparer<T> : IEqualityComparer<T>
  {
    private readonly Func<T, T, bool> _LambdaEquals;
    private readonly Func<T, int> _LambdaHash;

    /// <summary>
    /// Creates a comparer taking in a single function representing the two objects and a boolean expression to test equality.
    /// </summary>
    /// <param name="lambdaEquals"></param>
    public LambdaEqualityComparer(Func<T, T, bool> lambdaEquals) :
      this(lambdaEquals, o => 0)
    {
    }

    /// <summary>
    /// Creates a comparer taking in a function representing the two objects to compare and a boolean expression to test equality along with a hashing lambda function.
    /// </summary>
    /// <param name="lambdaEquals"></param>
    /// <param name="lambdaHash"></param>
    public LambdaEqualityComparer(Func<T, T, bool> lambdaEquals, Func<T, int> lambdaHash)
    {
      if (lambdaEquals == null)
        throw new ArgumentNullException("lambdaEquals");
      if (lambdaHash == null)
        throw new ArgumentNullException("lambdaHash");

      _LambdaEquals = lambdaEquals;
      _LambdaHash = lambdaHash;
    }

    /// <summary>
    /// Test eqality between two objects using the lambda defined equality function.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool Equals(T x, T y)
    {
      return _LambdaEquals(x, y);
    }

    /// <summary>
    /// Gets the hash code of an object using the lambda defined hash code function.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public int GetHashCode(T obj)
    {
      return _LambdaHash(obj);
    }
  }
}
