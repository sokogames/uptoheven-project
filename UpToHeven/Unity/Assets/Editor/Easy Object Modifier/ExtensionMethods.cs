using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Editor
{
    public static class ExtensionMethods
    {
        public static void PopulateTable<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = value;
        }
    }
}
