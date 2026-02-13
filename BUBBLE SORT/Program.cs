using System;
using System.Diagnostics;

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

        // 1. Khởi tạo mảng chính
        for (int i = 0; i < size; i++)
        {
            array[i] = rnd.Next(1, 1000);
        }

        // 2. Lấy 10 vị trí ngẫu nhiên không dùng LINQ
        int[] allIndices = new int[size];
        for (int i = 0; i < size; i++) allIndices[i] = i;

        // Trộn mảng chỉ số (Fisher-Yates Shuffle)
        for (int i = size - 1; i > 0; i--)
        {
            int j = rnd.Next(0, i + 1);
            int temp = allIndices[i];
            allIndices[i] = allIndices[j];
            allIndices[j] = temp;
        }

        // Lấy 10 phần tử đầu tiên sau khi trộn
        int[] randomIndices = new int[10];
        for (int i = 0; i < 10; i++) randomIndices[i] = allIndices[i];

        // Sắp xếp lại các chỉ số để khi in ra màn hình theo thứ tự từ nhỏ đến lớn
        BubbleSort(randomIndices);

        // 3. Hiển thị trước khi sắp xếp
        Console.WriteLine($"Mảng gốc ({size} phần tử). Giá trị tại 10 vị trí ngẫu nhiên:");
        PrintRandomSample(array, randomIndices);

        // 4. Đo thời gian (Lặp lại 5000 lần mỗi vòng đo để thấy ms)
        Timing.Measure(() =>
        {
            for (int k = 0; k < 5000; k++)
            {
                int[] copy = new int[size];
                Array.Copy(array, copy, size); 
                BubbleSort(copy);
            }
        }, 50);

        // 5. Sắp xếp mảng thật và hiển thị
        BubbleSort(array);

        Console.WriteLine("\n--- Sau khi sắp xếp ---");
        PrintRandomSample(array, randomIndices);

        Console.ReadKey();
    }

    // Tận dụng hàm BubbleSort của bạn cho cả mảng dữ liệu và mảng chỉ số
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

    private static void PrintRandomSample(int[] arr, int[] indices)
    {
        for (int i = 0; i < indices.Length; i++)
        {
            int idx = indices[i];
            Console.Write($"{arr[idx]} | ");
        }
        Console.WriteLine();
    }
}