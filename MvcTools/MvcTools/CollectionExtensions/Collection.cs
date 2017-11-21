// MvcTools.Collection.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.CollectionExtensions
{
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods for <see cref="ICollection{T}" />.
    /// </summary>
    public static class Collection
    {
        /// <summary>
        /// Iterates over <paramref name="toAdd" /> and adds each item to <paramref name="source" />.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The <see cref="ICollection{T}" />.</param>
        /// <param name="toAdd">The items to add to <paramref name="source" />.</param>
        public static void AddMany<TSource>(this ICollection<TSource> source, IEnumerable<TSource> toAdd)
        {
            foreach (var item in toAdd) source.Add(item);
        }

        /// <summary>
        /// Iterates over <paramref name="toRemove" /> and removes each item from <paramref name="source" />.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The <see cref="ICollection{T}" />.</param>
        /// <param name="toRemove">The items to remove from <paramref name="source" />.</param>
        public static void RemoveMany<TSource>(this ICollection<TSource> source, IEnumerable<TSource> toRemove)
        {
            foreach (var item in toRemove) source.Remove(item);
        }
    }
}
