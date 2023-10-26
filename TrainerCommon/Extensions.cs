#nullable enable

using System.Collections;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace TrainerCommon;

public static class Extensions {

    public static (T? head, IEnumerable<T> tail) HeadAndTail<T>(this IEnumerable<T> source) where T: class? {
        using IEnumerator<T> enumerator = source.GetEnumerator();
        return (head: enumerator.MoveNext() ? enumerator.Current : null, tail: new Enumerable<T>(enumerator));
    }

    public static (T? head, IEnumerable<T> tail) HeadAndTailStruct<T>(this IEnumerable<T> source) where T: struct {
        using IEnumerator<T> enumerator = source.GetEnumerator();
        return (head: enumerator.MoveNext() ? enumerator.Current : null, tail: new Enumerable<T>(enumerator));
    }

    public static (T? head, IEnumerable<T?> tail) HeadAndTailStruct<T>(this IEnumerable<T?> source) where T: struct {
        using IEnumerator<T?> enumerator = source.GetEnumerator();
        return (head: enumerator.MoveNext() ? enumerator.Current : null, tail: new Enumerable<T?>(enumerator));
    }

    private class Enumerable<T>: IEnumerable<T> {

        private readonly IEnumerator<T> enumerator;

        public Enumerable(IEnumerator<T> enumerator) {
            this.enumerator = enumerator;
        }

        public IEnumerator<T> GetEnumerator() => enumerator;

        IEnumerator IEnumerable.GetEnumerator() => enumerator;

    }

}