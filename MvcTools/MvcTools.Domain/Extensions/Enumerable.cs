// MvcTools.Domain.Enumerable.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Domain.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}" />.
    /// </summary>
    public static class Enumerable
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
        /// Creates an <see cref="IList{T}" /> from an <see cref="IEnumerable{T}" />.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> to create an <see cref="IList{T}" /> from.</param>
        /// <returns>An <see cref="IList{T}" /> that contains elements from the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static IList<TSource> ToIList<TSource>(this IEnumerable<TSource> source)
        {
            return source.ToList();
        }
    }
}
