using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExoSort
{
    class Program
    {
        static string fin = $@"{Directory.GetCurrentDirectory()}\fin.txt";
        static uint length = 10;
        static ulong count = 22;
        static bool release = true;
        static async Task Main(string[] args)
        {
#if DEBUG
            release = false;
#endif
            if (release)
                AskSettings();
            Generator G = new Generator(length, count);
            length = G.Length;
            count = G.Count;
            Console.WriteLine($"Настройка завершена.\nПараметры файла:\nДлина строки: {length}\nКоличество строк: {count}");
            Console.WriteLine($"{DateTime.Now} - Запущена Генерация файла\nДлина строки: {length}\nКол-во строк: {count}");
            G.GetFile(fin);

            if (fin.Contains("GetFileException"))
                Console.WriteLine(fin);
            else
            {
                Console.WriteLine($"{DateTime.Now} - Генерация файла завершена\n{DateTime.Now} - Начало сортировки");
                try
                {
                
                    await FileProcessor.Sort(fin, count);
                    Console.WriteLine($"{DateTime.Now} - Успех\nПуть к отсортированному файлу:\n{fin}");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.ReadKey();
        }
        static void AskSettings()
        {
            Console.Write($"{DateTime.Now} - Настройка Генератора\nВведите длину строки (0 - 1000000000): ");
            while (!UInt32.TryParse(Console.ReadLine(), out length))
                Console.WriteLine($"Неверный формат!\nЧисло должно входить в диапазон от 0 до 1000000000\nВведите длину строки (0 - 1000000000): ");
            Console.Write($"Принято\nВведите количество строк: ");
            while (!UInt64.TryParse(Console.ReadLine(), out count))
                Console.WriteLine($"Неверный формат!\nОжидается целое неорицательное число uInt64\nВведите количество строк: ");
        }
    }
}
