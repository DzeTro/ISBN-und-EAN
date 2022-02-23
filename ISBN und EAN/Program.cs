using System.Text;
using System.Text.RegularExpressions;

namespace ISBN
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.Write("ISBN-Nummer oder EAN-Nummer eingeben: ");
            string inputString = Console.ReadLine();
            ISBNOderEANPrüfung(inputString);
            
        }

        private static void ISBNOderEANPrüfung(string inputString)
        {
            
            if (inputString.Contains('-'))
                ISBNPrüfung(inputString);
            else
                EANPrüfung(inputString);
        }

        private static void ISBNPrüfung(string inputString)
        {
            inputString = SonderzeichenEnfernen(inputString);
            //Eingabe nach int[] parsen
            int[] numbers = inputString.ToCharArray() // wird das String in ein Array mit Chars umgewandelt
                .Select(ch => ch == 'X' ? 10 : int.Parse(ch.ToString())).ToArray();

            int sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += numbers[i] * (10 - i);
            }

            var isbnWithoutCheckNumber = numbers.SkipLast(1);
            var eanStartNummer = new int[] { 9, 7, 8 }; // Default  erste 3 Zeichen beim EAN nummer
            var aenWithoutChecknumber = eanStartNummer.Concat(isbnWithoutCheckNumber).ToArray(); // "978"+string.Concat(isbnWithoutCheckNumber);
            var eanChecknumer = ComputeCheckNumberForEan(aenWithoutChecknumber);
            // eanNummer += eanChecknumer;
            var eanNummer = string.Concat(aenWithoutChecknumber) + eanChecknumer.ToString();
            var isValidISBNNumber = sum % 11 == 0;

            if (isValidISBNNumber)
            {
                Console.WriteLine("Prüfziffer ist korrekt!");
                // passende EAN Nummer anzeigen
                Console.WriteLine($"Dazu Passende EAN Nummer: {eanNummer}");
            }
            else
            {
                var isbnWithoutCheckDigit = numbers.SkipLast(1).ToArray();
                var isbnCheckDigit = ComputeCheckNumberForIsbn(isbnWithoutCheckDigit);
                var newISBNNummer = string.Concat(numbers.SkipLast(1)) + isbnCheckDigit;
                Console.WriteLine("ISBN-Nummer ist nicht korrekt!");
                Console.WriteLine($"Richtige ISBN-Nummer ist : {newISBNNummer}");
                Console.WriteLine($"Dazu Passende EAN Nummer: {eanNummer}");
            }

            Console.ReadLine();

        }

        private static string ComputeCheckNumberForIsbn(int[] isbnWithoutCheckDigit)
        {
            int sumWithoutCheckDigit = 0;
            for (int n = 0; n < isbnWithoutCheckDigit.Length; n++)
            {
                sumWithoutCheckDigit += isbnWithoutCheckDigit[n] * (n + 1);
            }

            var isbnCheckDigit = sumWithoutCheckDigit % 11 == 10 ? "X" : (sumWithoutCheckDigit % 11).ToString();
            return isbnCheckDigit;
        }

        private static string SonderzeichenEnfernen(string inputString)
        {
            return inputString.ToUpper().Replace("-", "").Replace(" ", "").Trim();
        }

        private static void EANPrüfung(string inputString)
        {
            inputString = SonderzeichenEnfernen(inputString);
            int[] numberArray = inputString.Select(ch => int.Parse(ch.ToString())).ToArray();
            var eanChekcDigit = numberArray[numberArray.Length - 1];
            var numebersWitoutCheckNumer = numberArray.SkipLast(1).ToArray();
            int computedEanCheckDigit = ComputeCheckNumberForEan(numebersWitoutCheckNumer);

            var isbnWithoutCheckNumber = numberArray.Skip(3).Take(9).ToArray();
            var isbnCheckNumber = ComputeCheckNumberForIsbn(isbnWithoutCheckNumber);
            var isbn = string.Concat(isbnWithoutCheckNumber) + isbnCheckNumber;

            var isValidEANNumber = eanChekcDigit == computedEanCheckDigit;
            if (isValidEANNumber)
            {
                // Meldung anzeigen das EAN Richtig ist
                // und dazu passende ISBN Anzeigen
                Console.WriteLine("EAN-Nummer ist  korrekt!");
                Console.WriteLine($"Dazu passende ISBN-Nummer ist : {isbn}");
            }
            else
            {
                // Richtige Prüfzifferanezeign
                var newEannumer = string.Concat(numebersWitoutCheckNumer) + computedEanCheckDigit;
                Console.WriteLine("EAN-Nummer ist nicht korrekt!");
                Console.WriteLine($"Richtige EAN-Nummer ist : {newEannumer}");
                Console.WriteLine($"Dazu passende ISBN-Nummer ist : {isbn}");
            }
        }

        private static int ComputeCheckNumberForEan(int[] numebersWitoutCheckNumer)
        {
            var sumEan = 0;
            for (int i = 0; i < numebersWitoutCheckNumer.Length; i++)
            {
                var multipülicator = 3;
                if (i == 0 || i % 2 == 0)
                    multipülicator = 1;

                sumEan += numebersWitoutCheckNumer[i] * multipülicator;
            }
            var nD = sumEan; // 21
            while (nD % 10 != 0)
            {
                nD += 1;
            }
            var computedEanCheckDigit = nD - sumEan;
            return computedEanCheckDigit;
        }
    }
}

    
    
