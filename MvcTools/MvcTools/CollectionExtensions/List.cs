// MvcTools.List.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.CollectionExtensions
{
    using System.Collections.Generic;

    /// <summary>
    /// Extensions for <see cref="IList{T}" />.
    /// </summary>
    public static class List
    {
        /// <summary>
        /// Removes the <see cref="IList{T}" /> item at the last index (source.Count - 1).
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The <see cref="IList{T}" />.</param>
        /// <exception cref="System.NotSupportedException">The <see cref="IList{T}" /> is read-only.</exception>
        public static void Remove<TSource>(this IList<TSource> source)
        {
            source.RemoveAt(source.Count - 1);
        }

        /// <summary>
        /// Swaps the elements at the indicated indices.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The <see cref="IList{T}" />.</param>
        /// <param name="a">The first index.</param>
        /// <param name="b">The second index.</param>
        /// <exception cref="System.NotSupportedException">The <see cref="IList{T}" /> is read-only.</exception>
        public static void Swap<TSource>(this IList<TSource> source, int a, int b)
        {
            var swap = source[a];
            source[a] = source[b];
            source[b] = swap;
        }

        /// <summary>
        /// Removes the <see cref="IList{T}" /> item at the specified index by swapping
        /// it with the last item then removing the it.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The <see cref="IList{T}" />.</param>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> is not a valid index in the <see cref="IList{T}" />.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The <see cref="IList{T}" /> is read-only.</exception>
        public static void SwapRemove<TSource>(this IList<TSource> source, int index)
        {
            source.Swap(index, source.Count - 1);
            source.Remove();
        }
    }
}
