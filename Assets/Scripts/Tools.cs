using System.Collections;
using System.Collections.Generic;

public static class Tools
{
    public static T[] Populate<T>(T[] array, T value)
    {
        for (int i = 0; i < array.Length; i++) array[i] = value;
        return array;
    }
}
