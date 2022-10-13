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
            string filePath = "text.txt";
            string text = "";
            using (StreamReader stream = new StreamReader(filePath))
            {
                text = stream.ReadToEnd();
            }

            Type type = typeof(WordCalc.Calc);
            var a = type.GetMethod("CalculateWordSingle", BindingFlags.NonPublic | BindingFlags.Instance);

            Stopwatch calcWatchSingle = Stopwatch.StartNew();
            ConcurrentDictionary<string, int> map = (ConcurrentDictionary<string, int>)a.Invoke(new Calc(), new object[] { text });
            calcWatchSingle.Stop();


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

        public static ConcurrentDictionary<string, int> CalculateWord(string text)
        {
            ConcurrentDictionary<string, int> map = new ConcurrentDictionary<string, int>();
            string[] separators = { ",", ".", " ", " ", ";", ":", "?", "!", "\"", "(", ")", "—", "–", "[", "]", "»", "«" };

            string[] masWord = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            Parallel.For(0, masWord.Length - 1, n =>
            {
                map.AddOrUpdate(masWord[n], 1, (key, value) => value + 1);
            });

            return map;
        }
    }
}