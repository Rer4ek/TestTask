using System.Linq;

namespace TestTask
{
    public static class StringExtension
    {
        private const string VOWELS = "AEIOUYАУОИЭЫЯЮЕЁ";
        private const string CONSONANTS = "BCDFGHJYKLMNPQRSTVWXYZБВГДЖЗЙКЛМНПРСТФХЦЧШЩ";

        public static bool IsVowels(this string letter) =>
            letter.All(l => VOWELS.Contains(char.ToUpper(l)));

        public static bool IsConsonants(this string letter) =>
            letter.All(l => CONSONANTS.Contains(char.ToUpper(l)));
    }
}
