using System.Text;
using System.Text.RegularExpressions;

namespace ISBN
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.Write("ISBN-Nummer oder EAN-Nummer eingeben: ");
            string number = Console.ReadLine();
            ISBNOderEANPrüfung(number);
            
        }

        private static void ISBNOderEANPrüfung(string number)
        {
            
            if (number.Contains('-'))
            {
                ISBNPrüfung(number);
            }
            else
            {
                EANPrüfung(number);
            }
        }

        private static void ISBNPrüfung(string input)
        {
            //Eingabe nach int[] parsen
            int[] numbers = input.ToCharArray() // wird das String in ein Array mit Chars umgewandelt
                .Select(ch => ch == 'X' ? 10 : int.Parse(ch.ToString())).ToArray();

            int sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += numbers[i] * (10 - i);
            }

            if (sum % 11 == 0)
            {
                Console.WriteLine("ISBN-Nummer ist korrekt!");
            }
            else
            {
                Console.WriteLine("ISBN-Nummer ist nicht korrekt!");
            }

            Console.ReadLine();

        }

        //private static string SonderzeichenEnfernen(string input)
        //{
        //    return input.ToUpper().Replace("-", "").Replace(" ", "").Trim();
        //}

        private static void EANPrüfung(string input)
        {
            string ean = input;
            var checkDigit = char.GetNumericValue(ean[^1]);
            if (!new Regex(@"\b\d{13}\b").IsMatch(ean)) return;
            var sum = Enumerable.Range(0, ean.Length - 1).Select(x => char.GetNumericValue(ean[x]) * Math.Pow(3, x % 2)).Sum();
            var resultCheckDigit = Math.Ceiling(sum / 10) * 10 - sum;
            checkDigit = resultCheckDigit;
            Console.WriteLine("Die Prüfziffer ist "+ checkDigit);
        }

    }
}

    
    
