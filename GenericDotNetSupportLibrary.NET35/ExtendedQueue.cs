using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDotNetSupportLibrary
{
  public class ExtendedQueue<T> where T : ExtendedQueueItem
  {
    private int maxSize;
    private int currentSize;
    private int currentIndex;
    private Dictionary<int, T> queueElements;

    public ExtendedQueue()
    {
      this.maxSize = 1024;
      this.currentSize = 0;
      this.currentIndex = 0;
      this.queueElements = new Dictionary<int, T>();
    }

    public void Clear()
    {
      this.queueElements.Clear();
      this.currentIndex = 0;
      this.currentSize = 0;
    }

    public void Enqueue(T wp)
    {
      if (this.currentSize < this.maxSize)
      {
        if (!this.queueElements.Keys.Contains(this.currentIndex + 1))
        {
          this.queueElements.Add(++this.currentIndex, wp);
          this.currentSize++;
        }
        else
          this.currentIndex++;
      }
    }

    public T Seek()
    {
      if (this.currentIndex != -1)
      {
        T wp = this.queueElements[this.currentIndex];
        this.currentIndex--;
        return wp;
      }
      else
        return default(T);
    }

    public void MoveIndex(string currentItemName)
    {
      var item = queueElements.Where(i => i.Value.ItemName.Equals(currentItemName))
                  .OrderBy(wp => wp.Key)
                  .ToList();
      if (item.Any())
        this.currentIndex = item.First().Key - 1;
    }

    public int Size()
    {
      return this.currentIndex;
    }
  }

  public interface ExtendedQueueItem
  {
    string ItemName { get; }
  }
}
