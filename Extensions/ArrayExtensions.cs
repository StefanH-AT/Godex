namespace Godex.Extensions;

public static class ArrayExtensions
{
    public static T[] CloneFrom<T>(this T[] array, int index)
    {
        if (index > array.Length - 1)
        {
            throw new ArgumentException("Index is greater than input array length");
        }

        T[] arr = new T[array.Length - index];

        for (int i = 0; i < array.Length - index; i++)
        {
            arr[i] = array[i + index];
        }

        return arr;
    }

    /// <summary>
    /// Removes all the items from the array described by the indexes and shifts all items left to patch holes
    /// filling the end of the array with emptyValue
    /// </summary>
    /// <param name="array">Array to mutate</param>
    /// <param name="indexes">The item indexes to remove. Make sure this is sortest lowest to highest!</param>
    /// <param name="emptyValue">Value that should be assigned to the remaining items on the right</param>
    /// <typeparam name="T">The type</typeparam>
    public static void Condense<T>(this T[] array, int[] indexes, T emptyValue)
    {
        for (int i = 0; i < indexes.Length; i++)
        {
            int index = indexes[i] - i;

            for (int j = index; j < array.Length - i; j++)
            {
                int g = j + 1;
                if (g >= array.Length)
                {
                    array[j] = emptyValue;
                    continue;
                }
                array[j] = array[g];
            }
        }
    }

    public static unsafe T[] PointerToArray<T>(T* array, int size) where T : unmanaged
    {
        T[] arr = new T[size];
        for (int i = 0; i < size; i++)
        {
            arr[i] = array[i];
        }

        return arr;
    }

    public static unsafe void FillPointer<T>(T* pointer, T[] array) where T : unmanaged
    {
        for (int i = 0; i < array.Length; i++)
        {
            pointer[i] = array[i];
        }
    }
}