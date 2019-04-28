using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Helper_JSON
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string FixJsonArray(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
