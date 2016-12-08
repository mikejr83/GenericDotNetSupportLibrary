using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GenericDotNetSupportLibrary
{
  /// <summary>
  /// Represents a dynamic data collection that provides notifications when any item in the collection raises its PropertyChanged event
  /// or when items get added, removed, or when the whole list is refreshed. 
  /// </summary>
  /// <typeparam name="T">Type which implements INotifyPropertyChanged</typeparam>
  public class NotifiableObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
  {
    /// <summary>
    /// Occurs whenever any item in the collection raises its PropertyChanged event.
    /// </summary>
    public event EventHandler<CollectionItemPropertyChangedEventArgs> CollectionItemPropertyChanged;

    /// <summary>
    /// Initializes a new instance of the collection.
    /// </summary>
    public NotifiableObservableCollection() : base() { }

    /// <summary>
    /// Initializes a new instance of the collection that contains elements copied from the specified collection.
    /// </summary>
    /// <param name="collection">The collection from which the elements are copied</param>
    /// <exception cref="System.ArgumentNullException">The collection parameter cannot be null.</exception>
    public NotifiableObservableCollection(IEnumerable<T> collection)
      : base(collection)
    { }

    /// <summary>
    /// Initializes a new instance of the collection that contains elements copied from the specified list.
    /// </summary>
    /// <param name="list">The list from which the elements are copied.</param>
    /// <exception cref="System.ArgumentNullException">The list parameter cannot be null.</exception>
    public NotifiableObservableCollection(List<T> list)
      : base(list)
    { }

    /// <summary>
    /// Adds an item implementing INotifyPropertyChanged to the end of the collection. Items added in
    /// this manner will cause the CollectionItemPropertyChanged event to fire whenever one of their
    /// properties raise PropertyChanged.
    /// </summary>
    new public void Add(T item)
    {
      item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
      base.Add(item);
    }

    /// <summary>
    /// Inserts an element into the collection at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which item should be inserted.</param>
    /// <param name="item">The object to insert. The value can be null for reference types.</param>
    /// <exception cref="System.ArgumentOutOfRangeException">index is less than zero.-or-index is greater than the collection count.</exception>
    new public void Insert(int index, T item)
    {
      item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
      base.Insert(index, item);
    }

    /// <summary>
    /// Removes the first occurrence of a specific object from the collection.
    /// </summary>
    /// <param name="item">The item of the to remove from the collection.</param>
    new public bool Remove(T item)
    {
      item.PropertyChanged -= item_PropertyChanged;
      return base.Remove(item);
    }

    /// <summary>
    /// Removes the element at the specific index in the collection.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    new public void RemoveAt(int index)
    {
      if (base[index] != null)
        base[index].PropertyChanged -= item_PropertyChanged;

      base.RemoveAt(index);
    }

    /// <summary>
    /// Removes all elements from the collection.
    /// </summary>
    new public void Clear()
    {
      foreach (T item in this)
        item.PropertyChanged -= item_PropertyChanged;

      base.Clear();
    }

    void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (this.CollectionItemPropertyChanged != null)
      {
        this.CollectionItemPropertyChanged(this, new CollectionItemPropertyChangedEventArgs(sender, e));
      }
    }
  }

  /// <summary>
  /// Provides data for the GenericDotNetSupportLibrary.NotifiableObservableCollection<T> CollectionItemPropertyChanged event
  /// </summary>
  public class CollectionItemPropertyChangedEventArgs : EventArgs
  {
    /// <summary>
    /// Gets the name of the property that changed.
    /// </summary>
    public string PropertyName { get; private set; }
    /// <summary>
    /// Gets the item which fired the PropertyChanged event.
    /// </summary>
    public object CollectionItem { get; private set; }

    internal CollectionItemPropertyChangedEventArgs(object sender, PropertyChangedEventArgs originalArgs)
    {
      this.PropertyName = originalArgs.PropertyName;
      this.CollectionItem = sender;
    }
  }
}
