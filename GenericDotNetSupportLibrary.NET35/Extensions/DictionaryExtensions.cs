using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDotNetSupportLibrary.Extensions
{
  public static class DictionaryExtensions
  {
    public static Dictionary<TKey, TValue> MergeDictionariesNonDistructive<TKey, TValue>(this Dictionary<TKey, TValue> sourceDictionary, Dictionary<TKey, TValue> secondaryDictionary)
    {
      return DictionaryExtensions.MergeDictionariesNonDistructive(sourceDictionary, secondaryDictionary, true);
    }

    public static Dictionary<TKey, TValue> MergeDictionariesNonDistructive<TKey, TValue>(this Dictionary<TKey, TValue> sourceDictionary, Dictionary<TKey, TValue> secondaryDictionary, bool overwrite)
    {
      Dictionary<TKey, TValue> newDictionary = new Dictionary<TKey, TValue>(sourceDictionary);

      foreach (KeyValuePair<TKey, TValue> kvp in secondaryDictionary)
      {
        if (newDictionary.ContainsKey(kvp.Key) && overwrite)
          newDictionary[kvp.Key] = kvp.Value;
        else
          newDictionary.Add(kvp.Key, kvp.Value);
      }

      return newDictionary;
    }

    public static void MergeDictionaries<TKey, TValue>(this Dictionary<TKey, TValue> sourceDictionary, Dictionary<TKey, TValue> secondaryDictionary)
    {
      DictionaryExtensions.MergeDictionaries(sourceDictionary, secondaryDictionary, true);
    }

    public static void MergeDictionaries<TKey, TValue>(this Dictionary<TKey, TValue> sourceDictionary, Dictionary<TKey, TValue> secondaryDictionary, bool overwrite)
    {
      foreach (KeyValuePair<TKey, TValue> kvp in secondaryDictionary)
      {
        if (sourceDictionary.ContainsKey(kvp.Key) && overwrite)
          sourceDictionary[kvp.Key] = kvp.Value;
        else
          sourceDictionary.Add(kvp.Key, kvp.Value);
      }
    }
  }
}
