using System.Collections.Concurrent;
namespace WordCalc
{
    public class Calc
    {
        ConcurrentDictionary<string, int> CalculateWordSingle(string text)
        {
            ConcurrentDictionary<string, int> map = new ConcurrentDictionary<string, int>();
            string[] separators = { ",", ".", " ", " ", ";", ":", "?", "!", "\"", "(", ")", "—", "–", "[", "]", "»", "«" };

            string[] masWord = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < masWord.Length; i++)
            {
                map.AddOrUpdate(masWord[i], 1, (key, value) => value + 1);
            }
            return map;
        }

        public ConcurrentDictionary<string, int> CalculateWordMulti(string text)
        {
            ConcurrentDictionary<string, int> map = new ConcurrentDictionary<string, int>();
            string[] separators = { ",", ".", " ", " ", ";", ":", "?", "!", "\"", "(", ")", "—", "–", "[", "]", "»", "«" };

            string[] masWord = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            Parallel.For(0, masWord.Length-1, n =>
            {
                map.AddOrUpdate(masWord[n], 1, (key, value) => value + 1);
            });


            //string? s = String.Empty;
            //while ((s = sr.ReadLine()) != null)
            //{
            //    string[] mas = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            //    foreach (string ma in mas)
            //        if (map.ContainsKey(ma)) map[ma]++;
            //        else map.Add(ma, 1);
            //}


            return map;
        }
    }
}