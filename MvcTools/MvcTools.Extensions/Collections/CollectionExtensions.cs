// MvcTools.MvcTools.Extensions.CollectionExtensions.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Extensions.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods for collections.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Tries to determine whether a sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" />.</param>
        /// <returns>true if the source sequence is null or contains no elements; otherwise, false.</returns>
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source == null || source.None();
        }

        /// <summary>
        /// Tries to determine whether a sequence contains any elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" />.</param>
        /// <returns>true if the source sequence contains elements; otherwise, false.</returns>
        public static bool IsNotEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source != null && source.Any();
        }

        /// <summary>
        /// Determines whether a sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" />.</param>
        /// <returns>true if the source sequence contains no elements; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static bool None<TSource>(this IEnumerable<TSource> source)
        {
            return !source.Any();
        }

        /// <summary>
        /// Determines whether no elements of a sequence satisfy a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}" /> that contains the elements to apply the predicate to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// true if every element of the source sequence fails the test in the specified
        /// predicate, or if the sequence is empty; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
        public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return !source.Any(predicate);
        }

        /// <summary>
        /// Removes the last element of an <see cref="IList{T}" />.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The <see cref="IList{T}" />.</param>
        /// <exception cref="NotSupportedException">The <see cref="IList{T}" /> is read-only.</exception>
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
        /// <exception cref="NotSupportedException">The <see cref="IList{T}" /> is read-only.</exception>
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
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index" /> is not a valid index in the <see cref="IList{T}" />.
        /// </exception>
        /// <exception cref="NotSupportedException">The <see cref="IList{T}" /> is read-only.</exception>
        public static void SwapRemove<TSource>(this IList<TSource> source, int index)
        {
            source.Swap(index, source.Count - 1);
            source.Remove();
        }
    }
}
