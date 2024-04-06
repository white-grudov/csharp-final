using System.Collections.ObjectModel;
using System.Text;

namespace csharp_final.Models
{
    public class PersonGenerator
    {
        private static readonly Random _random = new();

        private readonly static List<string> _startSyllables = 
            ["Br", "Jen", "Mar", "Lor", "Gr", "Phil", "Chris", "Am", "Al", "Rob", "Car", "Mad", "Ev", "Is", "An"];
        private readonly static List<string> _middleSyllables = 
            ["an", "el", "in", "on", "er", "ar", "or", "th", "dr", "st"];
        private readonly static List<string> _endSyllables = 
            ["son", "man", "wood", "ton", "field", "hill", "dale", "ley", "sy", "ty", "ny", "by", "thy"];
        private readonly static List<string> _lastNameEndings = 
            ["son", "sen", "ton", "dall", "hall", "smith", "ford", "ham", "stone", "worth", "wick", "cock", "well"];
        private readonly static List<string> _emailProviders =
            ["gmail", "yahoo", "outlook", "icloud", "proton", "hotmail", "msn"];
        private readonly static List<string> _emailDomains =
            ["com", "co.uk", "net", "ua", "fr", "de", "pl", "ee", "fi", "io", "ca", "it"];

        private static string GenerateName(List<string> starts, List<string> middles, List<string> ends, int minSyllables, int maxSyllables)
        {
            StringBuilder name = new();
            int syllableCount = _random.Next(minSyllables, maxSyllables + 1);

            if (syllableCount == 1)
            {
                name.Append(starts[_random.Next(starts.Count)]);
            }
            else
            {
                name.Append(starts[_random.Next(starts.Count)]);

                for (int i = 1; i < syllableCount; i++)
                {
                    if (i == syllableCount - 1)
                    {
                        name.Append(ends[_random.Next(ends.Count)]);
                    }
                    else
                    {
                        name.Append(middles[_random.Next(middles.Count)]);
                    }
                }
            }

            return name[0] + name.ToString(1, name.Length - 1).ToLower();
        }

        public static string GenerateFirstName()
        {
            return GenerateName(_startSyllables, _middleSyllables, _endSyllables, 2, 3);
        }

        public static string GenerateLastName()
        {
            return GenerateName(_startSyllables, _middleSyllables, _lastNameEndings, 2, 4);
        }

        public static string GenerateEmail(string firstName, string lastName)
        {
            return $"{firstName.ToLower()}.{lastName.ToLower()}@" + 
                   $"{_emailProviders[_random.Next(_emailProviders.Count)]}.{_emailDomains[_random.Next(_emailDomains.Count)]}";
        }

        public static DateTime GenerateBirthDate()
        {
            DateTime dateStart = new(1950, 1, 1);
            int range = (DateTime.Today - dateStart).Days;
            return dateStart.AddDays(_random.Next(range));
        }
        public static Person GeneratePerson()
        {
            string firstName = GenerateFirstName();
            string lastName = GenerateLastName();
            string email = GenerateEmail(firstName, lastName);
            DateTime birthDate = GenerateBirthDate();

            return new Person(firstName, lastName, email, birthDate);
        }

        public static ObservableCollection<Person> GeneratePeople(int count)
        {
            ObservableCollection<Person> people = [];

            for (int i = 0; i < count; i++)
            {
                people.Add(GeneratePerson());
            }

            return people;
        }   
    }
}