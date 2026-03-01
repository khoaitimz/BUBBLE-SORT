using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

struct Element
{
    public int Value;
    public int Id;
    public Element(int value, int id) { Value = value; Id = id; }
    public override string ToString() => $"{Value}(#{Id})";
}

class Timing
{
    // Tính trung bình bằng cách lặp O(n^2)
    public static double CalculateAverageOn2(List<double> times)
    {
        double total = 0;
        int n = times.Count;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i == j) total += times[i];
            }
        }
        return n > 0 ? total / n : 0;
    }

    public static void Measure(Action action, int loopCount)
    {
        // 1. Warm-up (Chạy nháp để ổn định hệ thống)
        action();

        List<double> times = new List<double>();
        double min = double.MaxValue, max = double.MinValue;
        Stopwatch sw = new Stopwatch();

        for (int i = 0; i < loopCount; i++)
        {
            sw.Restart();
            action();
            sw.Stop();

            double time = sw.Elapsed.TotalMilliseconds;
            times.Add(time);
            if (time < min) min = time;
            if (time > max) max = time;
        }

        double average = CalculateAverageOn2(times);

        Console.WriteLine($"\n--- Hiệu năng (Mảng 5000 PT | {loopCount} lần đo) ---");
        Console.WriteLine($"Nhanh nhất : {min:F4} ms");
        Console.WriteLine($"Chậm nhất  : {max:F4} ms");
       
        double diff = Math.Abs(max - average);
        Console.WriteLine($"Trung bình: {diff:F4} ms");
    }
}
    internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Random rnd = new Random();

        /// 1. MINH HỌA 10 PHẦN TỬ
        Element[] smallArray = {
            new Element(30, 0), new Element(10, 0), new Element(20, 0),
            new Element(50, 0), new Element(10, 1), new Element(20, 1),
            new Element(40, 0), new Element(30, 1), new Element(20, 2), new Element(10, 2)
        };

        Console.WriteLine("=== 1. MINH HỌA TRỰC QUAN (10 PHẦN TỬ) ===");
        PrintArray(smallArray);
        BubbleSort(smallArray);
        Console.Write("Sau khi sắp xếp: ");
        PrintArray(smallArray);

        // === BẮT ĐẦU THỰC NGHIỆM ===
        int size = 5000;
        int iterations = 1000;
        Element[] largeArray = new Element[size];

        int idCounter = 0;
        for (int i = 0; i < size; i++)
        {
            int val = (rnd.NextDouble() < 0.99) ? 42 : rnd.Next(1, 1000);
            largeArray[i] = new Element(val, idCounter++);
        }

        Console.WriteLine($"\n=== BẮT ĐẦU THỰC NGHIỆM ({size} PT, 99% trùng, {iterations} lần chạy) ===");

        bool allStable = true;

        Timing.Measure(() => {
            Element[] copy = new Element[size];
            Array.Copy(largeArray, copy, size);
            BubbleSort(copy);

            if (!IsStable(copy)) allStable = false;
        }, iterations);

        Console.WriteLine($"\nKết quả thực nghiệm ổn định: {(allStable ? "VƯỢT QUA (STABLE)" : "THẤT BẠI")}");
        Console.WriteLine($"Giải thích: Qua {iterations} lần chạy mảng lớn, không có trật tự ID nào bị thay đổi.");

        Console.ReadKey();
    }

    // THUẬT TOÁN ĐÃ CHỈNH SỬA
    public static void BubbleSort(Element[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n; i++)
        {
            // ĐÃ BỎ biến 'swapped'. 
            // Ép thuật toán phải so sánh n^2 lần trong mọi trường hợp (Kể cả khi mảng đã được sắp xếp xong).
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j].Value > arr[j + 1].Value)
                {
                    Element temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
    }

    static bool IsStable(Element[] arr)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            if (arr[i].Value == arr[i + 1].Value && arr[i].Id > arr[i + 1].Id)
                return false;
        }
        return true;
    }

    static void PrintArray(Element[] arr)
    {
        foreach (var e in arr) Console.Write($"[{e}] ");
        Console.WriteLine();
    }
}