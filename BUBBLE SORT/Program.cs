using System;
using System.Diagnostics;

class Timing
{
    public static void Measure(Action action, int loopCount)
    {
        double min = double.MaxValue;
        double max = double.MinValue;
        double total = 0;

        Stopwatch sw = new Stopwatch();

        for (int i = 0; i < loopCount; i++)
        {
            sw.Restart();
            action();
            sw.Stop();

            double time = sw.Elapsed.TotalMilliseconds;
            if (time < min) min = time;
            if (time > max) max = time;
            total += time;
        }

        double average = total / loopCount;

        Console.WriteLine("\n--- Kết quả đo lường hiệu năng ---");
        Console.WriteLine($"Nhanh nhất : {min:F4} ms");
        Console.WriteLine($"Chậm nhất  : {max:F4} ms");
        Console.WriteLine($"Trung bình : {average:F4} ms");
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // 1. Khởi tạo mảng gồm đúng 10 phần tử
        int size = 10;
        int[] array = new int[size];
        Random rnd = new Random();

        for (int i = 0; i < size; i++)
        {
            array[i] = rnd.Next(1, 100); // Giới hạn số nhỏ cho dễ nhìn
        }

        // 2. Hiển thị mảng gốc
        Console.WriteLine("Mảng gốc ban đầu:");
        PrintArray(array);

        // 3. Đo thời gian (vẫn lặp nhiều lần để thấy con số ms vì mảng 10 phần tử cực nhanh)
        Timing.Measure(() =>
        {
            for (int k = 0; k < 10000; k++)
            {
                int[] copy = new int[size];
                Array.Copy(array, copy, size);
                BubbleSort(copy);
            }
        }, 100);

        // 4. Sắp xếp mảng thật và hiển thị kết quả cuối cùng
        BubbleSort(array);

        Console.WriteLine("\n--- Mảng sau khi đã sắp xếp ---");
        PrintArray(array);

    }

    public static void BubbleSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n; i++)
        {
            bool swapped = false;
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                    swapped = true;
                }
            }
            if (!swapped) break;
        }
    }

    private static void PrintArray(int[] arr)
    {
        Console.Write("[ ");
        for (int i = 0; i < arr.Length; i++)
        {
            Console.Write($"{arr[i]}" + (i < arr.Length - 1 ? ", " : ""));
        }
        Console.WriteLine(" ]");
    }
}