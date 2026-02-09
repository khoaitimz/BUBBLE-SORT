using System;

internal class Program
{
    private static void Main(string[] args)
    {
        int[] array = { 67, 36, 24, 12, 26, 13, 69, 96 };
        Console.WriteLine("Unsorted array:");
        PrintArray(array);
        BubbleSort(array);
        Console.WriteLine("Sorted array:");
        PrintArray(array);
    }
    private static void BubbleSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    // Swap arr[j] and arr[j+1]
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
    }
    private static void PrintArray(int[] arr)
    {
        foreach (var item in arr)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
    }
}