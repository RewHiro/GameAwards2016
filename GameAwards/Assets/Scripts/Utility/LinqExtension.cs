using System;
using System.Collections.Generic;

public static class LinqExtension
{
    public static IEnumerable<TSource> ExecuteAction<TSource>(this IEnumerable<TSource>sources, Action<TSource>action)
    {
        foreach (var source in sources)
        {
            action(source);
        }

        return sources;
    }
}