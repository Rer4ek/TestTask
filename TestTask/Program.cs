using System;
using System.Linq;
using System.Collections.Generic;

namespace TestTask
{
    public class Program
    {

        /// <summary>
        /// Программа принимает на входе 2 пути до файлов.
        /// Анализирует в первом файле кол-во вхождений каждой буквы (регистрозависимо). Например А, б, Б, Г и т.д.
        /// Анализирует во втором файле кол-во вхождений парных букв (не регистрозависимо). Например АА, Оо, еЕ, тт и т.д.
        /// По окончанию работы - выводит данную статистику на экран.
        /// </summary>
        /// <param name="args">Первый параметр - путь до первого файла.
        /// Второй параметр - путь до второго файла.</param>
        static void Main(string[] args)
        {
   
            IReadOnlyStream inputStream1 = GetInputStream(args[0]);
            IReadOnlyStream inputStream2 = GetInputStream(args[1]);

            IList<LetterStats> singleLetterStats = FillSingleLetterStats(inputStream1);
            IList<LetterStats> doubleLetterStats = FillDoubleLetterStats(inputStream2);

            RemoveCharStatsByType(ref singleLetterStats, CharType.Vowel);
            RemoveCharStatsByType(ref doubleLetterStats, CharType.Consonants);

            PrintStatistic(singleLetterStats);
            PrintStatistic(doubleLetterStats);

            Console.ReadKey();
        }

        /// <summary>
        /// Ф-ция возвращает экземпляр потока с уже загруженным файлом для последующего посимвольного чтения.
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        /// <returns>Поток для последующего чтения.</returns>
        private static IReadOnlyStream GetInputStream(string fileFullPath)
        {
            return new ReadOnlyStream(fileFullPath);
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения каждой буквы.
        /// Статистика РЕГИСТРОЗАВИСИМАЯ!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillSingleLetterStats(IReadOnlyStream stream)
        {
            IList<LetterStats> letterStatsResult = new List<LetterStats>();

            stream.ResetPositionToStart();
            using (stream)
            {
                while (!stream.IsEof)
                {
                    char c = stream.ReadNextChar();

                    LetterStats letterStats = letterStatsResult
                        .FirstOrDefault(l => l.Letter == c.ToString());

                    if (letterStats == null)
                    {
                        letterStats = new LetterStats();
                        if (!letterStats.TrySetLetter(c.ToString()))
                        {
                            continue;
                        }
                        letterStatsResult.Add(letterStats);
                    }
                    letterStats.IncStatistic();
                }
            }

            return letterStatsResult;
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения парных букв.
        /// В статистику должны попадать только пары из одинаковых букв, например АА, СС, УУ, ЕЕ и т.д.
        /// Статистика - НЕ регистрозависимая!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillDoubleLetterStats(IReadOnlyStream stream)
        {
            IList<LetterStats> letterStatsResult = new List<LetterStats>();

            stream.ResetPositionToStart();
            using (stream)
            {
                char lastChar = '\0';
                while (!stream.IsEof)
                {
                    char c = stream.ReadNextChar();

                    string letters = (c.ToString() + lastChar.ToString()).ToUpper();
                    lastChar = c;

                    LetterStats letterStats = letterStatsResult
                        .FirstOrDefault(l => l.Letter == letters);

                    if (letterStats == null)
                    {
                        letterStats = new LetterStats();

                        if (!letterStats.TrySetLetter(letters))
                        {
                            continue;
                        }
                        letterStatsResult.Add(letterStats);
                    }
                    letterStats.IncStatistic();

                    lastChar = c;
                }
            }

            return letterStatsResult;
        }

        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром charType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="charType">Тип букв для анализа</param>
        private static void RemoveCharStatsByType(ref IList<LetterStats> letters, CharType charType)
        {
            switch (charType)
            {
                case CharType.Consonants:
                    letters.RemoveAll(l => l.Letter.IsConsonants());
                    break;
                case CharType.Vowel:
                    letters.RemoveAll(l => l.Letter.IsVowels());
                    break;
            }
        }

        /// <summary>
        /// Ф-ция выводит на экран полученную статистику в формате "{Буква} : {Кол-во}"
        /// Каждая буква - с новой строки.
        /// Выводить на экран необходимо предварительно отсортировав набор по алфавиту.
        /// В конце отдельная строчка с ИТОГО, содержащая в себе общее кол-во найденных букв/пар
        /// </summary>
        /// <param name="letters">Коллекция со статистикой</param>
        private static void PrintStatistic(IEnumerable<LetterStats> letters)
        {
            foreach (LetterStats lettersItem in letters.OrderBy(l => l.Letter))
            {
                Console.WriteLine($"{lettersItem.Letter} - {lettersItem.Count}");
            }
            Console.WriteLine($"Итого: {letters.Sum(l => l.Count)}");
        }

    }
}
