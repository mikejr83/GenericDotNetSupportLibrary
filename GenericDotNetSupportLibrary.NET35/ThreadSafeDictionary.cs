using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GenericDotNetSupportLibrary
{
  /// <summary>
  /// A wrapper on Dictionary object with thread safety.
  /// </summary>
  /// <typeparam name="TKey"></typeparam>
  /// <typeparam name="TValue"></typeparam>
  public class ThreadSafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
  {
    private ReaderWriterLockSlim _RWLock = new ReaderWriterLockSlim();
    private Dictionary<TKey, TValue> _Dictionary = new Dictionary<TKey, TValue>();

    #region Custom members

    /// <summary>
    /// Gets a value from the dictionary by key.
    /// If the key does not exist, calls the supplied factory to insert the default value
    /// Thread-safe.
    /// </summary>
    /// <param name="key">The key to get or add.</param>
    /// <param name="valueFactory">The factory to return the value of the key does not exist.</param>
    /// <returns></returns>
    public TValue GetOrAdd(TKey key, TValue value)
    {
      return GetOrAdd(key, () => value);
    }

    /// <summary>
    /// Gets a value from the dictionary by key.
    /// If the key does not exist, calls the supplied factory to insert the default value
    /// Thread-safe.
    /// </summary>
    /// <param name="key">The key to get or add.</param>
    /// <param name="valueFactory">The factory to return the value of the key does not exist.</param>
    /// <returns></returns>
    public TValue GetOrAdd(TKey key, Func<TValue> valueFactory)
    {
      //if the values already exists, return it
      _RWLock.EnterReadLock();
      try
      {
        TValue result;
        if (_Dictionary.TryGetValue(key, out result))
          return result;
      }
      finally
      {
        _RWLock.ExitReadLock();
      }

      //prepare to write, but allow other threads to continue to read
      _RWLock.EnterUpgradeableReadLock();

      try
      {
        TValue result;
        //double-check to make sure another thread hasn't inserted the value
        if (!_Dictionary.TryGetValue(key, out result))
        {
          // allow reading while calculating a new value
          TValue value = valueFactory();

          //now, upgrade to a writer lock and add to the dictionary
          _RWLock.EnterWriteLock();
          try
          {
            _Dictionary.Add(key, value);
          }
          finally
          {
            _RWLock.ExitWriteLock();
          }
          return value;
        }
        else
        {
          return result;
        }
      }
      finally
      {
        _RWLock.ExitUpgradeableReadLock();
      }
    }

    #endregion

    #region IDictionary<TKey,TValue> Members

    /// <summary>
    /// Add the specified key value pair
    /// </summary>
    /// <param key="key"></param>
    /// <param key="value"></param>
    public void Add(TKey key, TValue value)
    {
      _RWLock.EnterWriteLock();
      try
      {
        _Dictionary[key] = value;
      }
      finally
      {
        _RWLock.ExitWriteLock();
      }
    }

    /// <summary>
    /// Returns true if the key is present in the dictionary
    /// </summary>
    /// <param key="key"></param>
    /// <returns></returns>
    public bool ContainsKey(TKey key)
    {
      _RWLock.EnterReadLock();
      try
      {
        return _Dictionary.ContainsKey(key);
      }
      finally
      {
        _RWLock.ExitReadLock();
      }
    }

    /// <summary>
    /// Returns a collection of keys
    /// </summary>
    public ICollection<TKey> Keys
    {
      get
      {
        _RWLock.EnterReadLock();
        try
        {
          return _Dictionary.Keys;
        }
        finally
        {
          _RWLock.ExitReadLock();
        }
      }
    }

    /// <summary>
    /// Removes the specified key from the dictionary.
    /// </summary>
    /// <param key="key"></param>
    /// <returns></returns>
    public bool Remove(TKey key)
    {
      _RWLock.EnterWriteLock();
      try
      {
        return _Dictionary.Remove(key);
      }
      finally
      {
        _RWLock.ExitWriteLock();
      }
    }

    /// <summary>
    /// Tries to get the value from the dictionary if present. The output parameter would be filled with the value 
    /// if the value is present.
    /// </summary>
    /// <param key="key"></param>
    /// <param key="value"></param>
    /// <returns></returns>
    public bool TryGetValue(TKey key, out TValue value)
    {
      _RWLock.EnterReadLock();
      try
      {
        return _Dictionary.TryGetValue(key, out value);
      }
      finally
      {
        _RWLock.ExitReadLock();
      }
    }

    /// <summary>
    /// Gets a collection of _Dictionary.
    /// </summary>
    public ICollection<TValue> Values
    {
      get
      {
        _RWLock.EnterReadLock();
        try
        {
          return _Dictionary.Values;
        }
        finally
        {
          _RWLock.ExitReadLock();
        }
      }
    }

    public TValue this[TKey key]
    {
      get
      {
        _RWLock.EnterReadLock();
        try
        {
          return _Dictionary[key];
        }
        finally
        {
          _RWLock.ExitReadLock();
        }
      }
      set
      {
        _RWLock.EnterWriteLock();
        try
        {
          _Dictionary[key] = value;
        }
        finally
        {
          _RWLock.ExitWriteLock();
        }
      }
    }

    #endregion

    #region ICollection<KeyValuePair<TKey,TValue>> Members

    public void Add(KeyValuePair<TKey, TValue> item)
    {
      _RWLock.EnterWriteLock();
      try
      {
        ((ICollection<KeyValuePair<TKey, TValue>>)_Dictionary).Add(item);
      }
      finally
      {
        _RWLock.ExitWriteLock();
      }
    }

    public void Clear()
    {
      _RWLock.EnterWriteLock();
      try
      {
        _Dictionary.Clear();
      }
      finally
      {
        _RWLock.ExitWriteLock();
      }
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
      _RWLock.EnterReadLock();
      try
      {
        return ((ICollection<KeyValuePair<TKey, TValue>>)_Dictionary).Contains(item);
      }
      finally
      {
        _RWLock.ExitReadLock();
      }
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      _RWLock.EnterReadLock();
      try
      {
        ((ICollection<KeyValuePair<TKey, TValue>>)_Dictionary).CopyTo(array, arrayIndex);
      }
      finally
      {
        _RWLock.ExitReadLock();
      }
    }

    public int Count
    {
      get
      {
        _RWLock.EnterReadLock();
        try
        {
          return _Dictionary.Count;
        }
        finally
        {
          _RWLock.ExitReadLock();
        }
      }
    }

    public bool IsReadOnly
    {
      get { return false; }
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
      _RWLock.EnterWriteLock();
      try
      {
        return ((ICollection<KeyValuePair<TKey, TValue>>)_Dictionary).Remove(item);
      }
      finally
      {
        _RWLock.ExitWriteLock();
      }
    }

    #endregion

    #region IEnumerable<KeyValuePair<TKey,TValue>> Members

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      _RWLock.EnterReadLock();
      try
      {
        return ((ICollection<KeyValuePair<TKey, TValue>>)_Dictionary).GetEnumerator();
      }
      finally
      {
        _RWLock.ExitReadLock();
      }
    }

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      _RWLock.EnterReadLock();
      try
      {
        return ((System.Collections.IEnumerable)_Dictionary).GetEnumerator();
      }
      finally
      {
        _RWLock.ExitReadLock();
      }
    }

    #endregion
  }

  public static class SumtExtensions
  {
    public static ThreadSafeDictionary<TKey, TValue> ToThreadSafeDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
    {
      ThreadSafeDictionary<TKey, TValue> tsDictionary = new ThreadSafeDictionary<TKey, TValue>();

      foreach (TKey key in dictionary.Keys)
        tsDictionary.Add(key, dictionary[key]);

      return tsDictionary;
    }
  }
}
