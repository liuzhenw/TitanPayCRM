namespace Astra.Common;

public class FixedSizedQueue<T>(int capacity)
{
    private readonly Queue<T> _queue = new(capacity + 1);
    public int Capacity { get; } = capacity;
    public int Count => _queue.Count;

    public T Enqueue(T item)
    {
        _queue.Enqueue(item);
        return Count > Capacity ? Dequeue() : item;
    }
    public T Dequeue()
    {
        return _queue.Dequeue();
    }
}