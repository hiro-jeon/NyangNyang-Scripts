using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension
{
    public static T[] GetComponentsInDirectChildren<T>(this GameObject obj)
    {
        List<T> results = new List<T>();

        foreach (Transform child in obj.transform)
        {
            T comp = child.GetComponent<T>();
            if (comp != null)
                results.Add(comp);
        }

        return results.ToArray();
    }
}
