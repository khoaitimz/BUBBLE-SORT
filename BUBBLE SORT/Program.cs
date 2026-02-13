using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

static class Timing
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

        int size = 100;
        int[] array = new int[size];
        Random rnd = new Random();

        // Khởi tạo mảng với giá trị ngẫu nhiên
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = rnd.Next(1, 1000);
        }

        // Chọn ra 10 vị trí ngẫu nhiên cố định để quan sát trước và sau
        int[] randomIndices = Enumerable.Range(0, size).OrderBy(x => rnd.Next()).Take(10).ToArray();
        Array.Sort(randomIndices); // Sắp xếp chỉ số để dễ nhìn theo thứ tự mảng

        Console.WriteLine($"Mảng gốc ({size} phần tử). Giá trị tại 10 vị trí ngẫu nhiên:");
        PrintRandomSample(array, randomIndices);

        // Đo thời gian (vẫn dùng vòng lặp nội bộ để hiện rõ ms)
        Timing.Measure(() =>
        {
            for (int k = 0; k < 5000; k++)
            {
                int[] copy = (int[])array.Clone();
                BubbleSort(copy);
            }
        }, 50);

        // Sắp xếp mảng thật
        BubbleSort(array);

        Console.WriteLine("\n--- Sau khi sắp xếp ---");
        Console.WriteLine("Giá trị tại cùng 10 vị trí ngẫu nhiên đó:");
        PrintRandomSample(array, randomIndices);

        Console.WriteLine("\nNhấn phím bất kỳ để thoát...");
        Console.ReadKey();
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

    // Hàm in giá trị tại các chỉ số ngẫu nhiên được chỉ định
    private static void PrintRandomSample(int[] arr, int[] indices)
    {
        foreach (int idx in indices)
        {
            Console.Write($" {arr[idx]} | ");
        }
        Console.WriteLine("\n");
    }
}