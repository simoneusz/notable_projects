using System;
using System.Text;
using System.Linq;

namespace TextEditor
{
    public class TextStatistics
    {
        public double SizeInKb { get; }
        public int Symbols { get; }
        public int Paragraphs { get; }
        public int EmptyLineCount { get; }
        public int AuthorPages { get; }
        public int CountVowels { get; }
        public int CountConsonants { get; }
        public int CountNumeric { get; }
        public int CountSpecialSymbols { get; }

        public TextStatistics(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            SizeInKb = bytes.Length / 1024.0;
            Symbols = text.Length;
            Paragraphs = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
            EmptyLineCount = CalculateEmptyLineCount(text);
            AuthorPages = Convert.ToInt32(Symbols / 1800);
            CountVowels = CalculateCountVowels(text);
            CountConsonants = CalculateCountConsonants(text);
            CountNumeric = CalculateCountNumeric(text);
            CountSpecialSymbols = CalculateCountSpecialSymbols(text);
        }

        private int CalculateEmptyLineCount(string text)
        {
            string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            int emptyLineCount = 0;
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    emptyLineCount++;
                }
            }
            return emptyLineCount;
        }

        private int CalculateCountVowels(string text)
        {
            string vowels = "aiueoAIUEO";
            int count = 0;
            for (int i = 0; i < vowels.Length; i++)
            {
                count += text.Count(t => t == vowels[i]);
            }
            return count;
        }

        private int CalculateCountConsonants(string text)
        {
            string consonants = "BCDFGHJKLMNPQRSTVWXYZbcdfghjklmnpqrstvwxyz";
            int count = 0;
            for (int i = 0; i < consonants.Length; i++)
            {
                count += text.Count(t => t == consonants[i]);
            }
            return count;
        }

        private int CalculateCountNumeric(string text)
        {
            string numeric = "0123456789";
            int count = 0;
            for (int i = 0; i < numeric.Length; i++)
            {
                count += text.Count(t => t == numeric[i]);
            }
            return count;
        }

        private int CalculateCountSpecialSymbols(string text)
        {
            int count = 0;
            foreach (char c in text)
            {
                if (!Char.IsLetterOrDigit(c) && !Char.IsWhiteSpace(c))
                {
                    count++;
                }
            }
            return count;
        }
    }
}