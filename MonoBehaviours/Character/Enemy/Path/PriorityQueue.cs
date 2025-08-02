using System.Collections.Generic;

// 가장 우선순위가 높은 얘를 가져온다.
public class PriorityQueue<T>
{
    private List<(T item, int priority)> items = new();

    public int Count => items.Count;

    public void Enqueue(T item, int priority)
    {
        items.Add((item, priority));
    }

    public T Dequeue()
    {
        if (items.Count == 0)
        {
            throw new System.InvalidOperationException("큐가 비어 있습니다.");
        }

        int bestIndex = 0;
        int bestPriority = items[0].priority;

        for (int i = 1; i < items.Count; i++)
        {
            if (items[i].priority < bestPriority)
            {
                bestPriority = items[i].priority;
                bestIndex = i;
            }
        }

        T bestItem = items[bestIndex].item;
        items.RemoveAt(bestIndex);
        return bestItem;
    }

    public bool Contains(T item)
    {
        foreach (var pair in items)
        {
            if (EqualityComparer<T>.Default.Equals(pair.item, item))
            {
                return true;
            }
        }
        return false;
    }

    public void Clear()
    {
        items.Clear();
    }
}