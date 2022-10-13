using System.Diagnostics;
using System.Reflection;
using System.Collections.Concurrent;
using WordCalc;

namespace MainApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Чтение файла
            string filePath = "text.txt";
            string text = "";
            using (StreamReader stream = new StreamReader(filePath))
            {
                text = stream.ReadToEnd();
            }

            //Немного рефлексивной магии
            Type type = typeof(WordCalc.Calc);
            var a = type.GetMethod("CalculateWordSingle", BindingFlags.NonPublic | BindingFlags.Instance);

            //Ставим часики и считаем в один поток
            Stopwatch calcWatchSingle = Stopwatch.StartNew();
            ConcurrentDictionary<string, int> map = (ConcurrentDictionary<string, int>)a.Invoke(new Calc(), new object[] { text });
            calcWatchSingle.Stop();

            //Ставим часики и считаем в много поток
            Stopwatch calcWatchMulti = Stopwatch.StartNew();
            map = new Calc().CalculateWordMulti(text);
            calcWatchMulti.Stop();

            Console.WriteLine(calcWatchSingle.ElapsedMilliseconds + "мс потребовалось для операции на одном потоке");
            Console.WriteLine(calcWatchMulti.ElapsedMilliseconds + "мс потребовалось для операции на нескольких потоках");

            //Сортировка
            var sort = map.OrderByDescending(x => x.Value);

            //Вывод в файл
            using (StreamWriter sw = File.CreateText("textOut.txt"))
            {
                foreach (var x in sort)
                {
                    sw.WriteLine(x.Key + " " + x.Value);
                }
            }
        }
    }
}