using System;
using System.Diagnostics;

// 1. Định nghĩa đối tượng để kiểm tra tính ổn định
struct Element
{
    public int Value;
    public char Id; // Dùng để đánh dấu thứ tự ban đầu (A, B, C...)

    public Element(int value, char id)
    {
        Value = value;
        Id = id;
    }

    public override string ToString() => $"{Value}{Id}";
}

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
        Console.WriteLine($"Nhanh nhất : {min:F4} ms | Chậm nhất : {max:F4} ms | Trung bình : {total / loopCount:F4} ms");
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // 2. Khởi tạo mảng 10 phần tử 
        // Cố tình tạo các cặp số giống nhau: 20A, 20B, 20C để test Stability
        Element[] array = new Element[10] {
            new Element(50, ' '), new Element(20, 'A'), new Element(70, ' '),
            new Element(20, 'B'), new Element(10, ' '), new Element(20, 'C'),
            new Element(90, ' '), new Element(40, ' '), new Element(20, 'D'),
            new Element(30, ' ')
        };

        Console.WriteLine("=== MẢNG GỐC BAN ĐẦU ===");
        PrintArray(array);
        Console.WriteLine("Lưu ý: Các số 20 đang có thứ tự là A -> B -> C -> D\n");

        // 3. Đo hiệu năng 
        Timing.Measure(() => {
            Element[] copy = new Element[10];
            Array.Copy(array, copy, 10);
            BubbleSort(copy);
        }, 100);

        // 4. Sắp xếp mảng chính và kiểm tra tính ổn định
        BubbleSort(array);

        Console.WriteLine("\n=== MẢNG SAU KHI SẮP XẾP ===");
        PrintArray(array);

        // 5. Kiểm tra logic tính ổn định
        CheckStability(array);

        Console.ReadKey();
    }

    // Thuật toán Bubble Sort giữ tính ổn định
    public static void BubbleSort(Element[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n; i++)
        {
            bool swapped = false;
            for (int j = 0; j < n - i - 1; j++)
            {
                // Ổn định nhờ dấu '>' (không đổi chỗ khi bằng nhau)
                if (arr[j].Value > arr[j + 1].Value)
                {
                    Element temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                    swapped = true;
                }
            }
            if (!swapped) break;
        }
    }

    static void CheckStability(Element[] arr)
    {
        string result = "";
        foreach (var e in arr) if (e.Value == 20) result += e.Id;

        if (result == "ABCD")
            Console.WriteLine("=> KẾT LUẬN: Thuật toán ỔN ĐỊNH (Thứ tự A-B-C-D được giữ nguyên).");
        else
            Console.WriteLine($"=> KẾT LUẬN: Thuật toán KHÔNG ỔN ĐỊNH (Thứ tự bị đảo thành: {result}).");
    }

    static void PrintArray(Element[] arr)
    {
        foreach (var e in arr) Console.Write($"[{e}] ");
        Console.WriteLine();
    }
}