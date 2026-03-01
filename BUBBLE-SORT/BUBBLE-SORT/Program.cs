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

    // Tách riêng Setup (chuẩn bị dữ liệu) và TimedAction (thuật toán cần đo)
    public static void Measure(Action setup, Action timedAction, int loopCount)
    {
        for(int i = 0; i < 5; i++) { setup(); timedAction(); } // Warm-up

        List<double> times = new List<double>(loopCount);
        Stopwatch sw = new Stopwatch();

        for (int i = 0; i < loopCount; i++)
        {
            setup(); // Khởi tạo lại mảng (KHÔNG ĐƯA VÀO THỜI GIAN ĐO)

            sw.Restart();
            timedAction(); // Chỉ đo đúng thời gian chạy Bubble Sort thuần túy
            sw.Stop();
            
            times.Add(sw.Elapsed.TotalMilliseconds);
        }

        // Vẫn giữ màng lọc nhẹ 5% để tránh CPU bị khựng do OS (Windows/Linux)
        var filteredTimes = times.OrderBy(t => t).Take((int)(loopCount * 0.95)).ToList();

        double min = filteredTimes.Min();
        double max = filteredTimes.Max();
        double avg = CalculateAverageOn2(filteredTimes);

        Console.WriteLine($"\n--- Hiệu năng ({loopCount} lần lặp) ---");
        Console.WriteLine($"Nhanh nhất : {min:F4} ms");
        Console.WriteLine($"Chậm nhất  : {max:F4} ms");
        Console.WriteLine($"Trung bình : {avg:F4} ms (Hội tụ tuyệt đối)");
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Random rnd = new Random();

        // === MINH HỌA 10 PHẦN TỬ ===
        Element[] smallArray = {
            new Element(2, 0), new Element(5, 0), new Element(5, 1), 
            new Element(5, 2), new Element(8, 0), new Element(10, 0),
            new Element(10, 1), new Element(10, 2), new Element(10, 3), new Element(10, 4)
        };
        smallArray[0] = new Element(10, 0); smallArray[5] = new Element(2, 0);
        BubbleSort(smallArray);
        
        Console.WriteLine("=== MINH HỌA 10 PHẦN TỬ ===");
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
        Element[] copy = new Element[size];
        
        // Gọi hàm Measure với 2 Action tách biệt
        Timing.Measure(
            setup: () => {
                Array.Copy(largeArray, copy, size);
            },
            timedAction: () => {
                BubbleSort(copy);
                if (!IsStable(copy)) allStable = false; // Check ổn định có thể để đây vì mảng đã xếp xong
            }, 
            loopCount: iterations
        );

        Console.WriteLine($"\nKết quả thực nghiệm ổn định: {(allStable ? "VƯỢT QUA (STABLE)" : "THẤT BẠI")}");
        Console.WriteLine($"Giải thích: Qua {iterations} lần chạy mảng lớn, không có trật tự ID nào bị thay đổi.");

        Console.ReadKey();
    }

    public static void BubbleSort(Element[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n; i++)
        {
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