﻿namespace Extentions;

public static class ListExtensions
{
    public static bool IsNullOrEmpty<T>(this List<T> list)
    {
        return list == null || !list.Any();
    }
}