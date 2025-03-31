using System;
using System.Collections.Generic;

namespace Queue.Models;

public class CustomQueue<T> where T : class // Ограничиваем T ссылочными типами
{
    private readonly LinkedList<T> _items = new();

    public T? CurrentItem => _items.First?.Value; // Теперь это допустимо
    public int Count => _items.Count;
    public bool IsEmpty => _items.Count == 0;

    public void Enqueue(T item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
            
        _items.AddLast(item);
    }

    public T Dequeue()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Queue is empty");

        var item = _items.First!.Value;
        _items.RemoveFirst();
        return item;
    }

    public void Clear()
    {
        _items.Clear();
    }
}