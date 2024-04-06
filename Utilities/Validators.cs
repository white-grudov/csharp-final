using System.Text.RegularExpressions;

using csharp_final.Exceptions;

namespace csharp_final.Utilities
{
    internal partial class Validators
    {
        [GeneratedRegex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
        private static partial Regex EmainRegex();

        public static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ShortNameException("Name cannot be empty.");
            }
        }

        public static void ValidateEmail(string email)
        {
            if (!EmainRegex().IsMatch(email))
            {
                throw new InvalidEmailAddressException("Invalid email address.");
            }
        }

        public static void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            int age = DateUtilites.CalculateAge(dateOfBirth);
            if (age < 0)
            {
                throw new IncorrectDateException("You were not born yet!");
            }
            else if (age > 135)
            {
                throw new IncorrectDateException("You are most probably dead by now!");
            }
        }
    }
}
