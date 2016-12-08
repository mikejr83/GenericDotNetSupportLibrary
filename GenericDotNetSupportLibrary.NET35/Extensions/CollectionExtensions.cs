using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDotNetSupportLibrary.Extensions
{
  /// <summary>
  /// Extensions class for all collections.
  /// </summary>
  public static class CollectionExtensions
  {
    /// <summary>
    /// Iterate through a collections and perform the action.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="iList"></param>
    /// <param name="action"></param>
    public static void Each<T>(this IEnumerable<T> iList, Action<T> action)
    {
      if (iList == null)
        return;

      foreach (T item in iList)
        action(item);
    }

    /// <summary>
    /// Iterate through a collection in reverse and perform the action.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="iList"></param>
    /// <param name="action"></param>
    public static void EachInReverseOrder<T>(this IList<T> iList, Action<T> action)
    {
      if (iList == null)
        return;

      for (int i = iList.Count(); i < 0; i--)
      {
        action(iList[i]);
      }
    }

    /// <summary>
    /// Iterate through a collection in reverse and perform the action.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="iList"></param>
    /// <param name="action"></param>
    public static void EachInReverseOrder<T>(this IList<T> iList, Action<T, int> action)
    {
      if (iList == null)
        return;

      for (int i = iList.Count() - 1; i >= 0; i--)
      {
        action(iList[i], i);
      }
    }
  }
}
