using System.Linq;

namespace TestTask
{
    /// <summary>
    /// Статистика вхождения буквы/пары букв
    /// </summary>
    public class LetterStats
    {
        /// <summary>
        /// Буква/Пара букв для учёта статистики.
        /// </summary>
        public string Letter { get; private set; }

        /// <summary>
        /// Кол-во вхождений буквы/пары.
        /// </summary>
        public int Count { get; private set; } = 0;


        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданной структуре.
        /// </summary>
        public void IncStatistic()
        {
            Count++;
        }

        public bool TrySetLetter(string letter)
        {

            if (string.IsNullOrEmpty(letter))
            {
                return false;
            }

            if (!letter.All(l => char.IsLetter(l)) || !letter.All(l => l == letter[0]))
            {
                return false;
            }

            Letter = letter;
            return true;
        }
    }
}
