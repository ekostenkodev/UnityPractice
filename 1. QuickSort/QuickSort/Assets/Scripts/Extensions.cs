using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static T GetRandomAndRemove<T>(this IList<T> list)
    {
        var randomIndex = Random.Range(0, list.Count);
        var item = list[randomIndex];
        list.RemoveAt(randomIndex);
        
        return item;
    }
}
