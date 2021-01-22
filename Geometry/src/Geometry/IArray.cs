using System.Collections;
using System.Collections.Generic;

namespace Qkmaxware.Geometry {

/// <summary>
/// Interface representing a fixed size array like object
/// </summary>
/// <typeparam name="T">stored type</typeparam>
public interface IArray<T> : IEnumerable<T> {
    int Length {get;}
    T this[int index] {get; set;}
}

/// <summary>
/// A list which has a fixed size behaving like an array, used when Getters and Setters are to be exposed, and adders or removers are private to a controlling class
/// </summary>
/// <typeparam name="T">List type</typeparam>
internal class FixedSizeList<T> : IArray<T> {
    private List<T> list;

    public FixedSizeList(int size) {
        this.list = new List<T>(new T[size]);
    }

    public FixedSizeList(List<T> list) {
        this.list = list;
    }

    public int Length => list.Count;

    public T this[int index] {
        get => list[index];
        set => list[index] = value;
    }

    public IEnumerator<T> GetEnumerator() {
        return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}

}