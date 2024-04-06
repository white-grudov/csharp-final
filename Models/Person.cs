using csharp_final.Utilities;
using csharp_project.Exceptions;
using csharp_project.Utilities;

namespace csharp_final.Models
{
    public class Person
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime _birthDate;
        private readonly int _age;
        private readonly string _westernZodiac;
        private readonly string _chineseZodiac;
        private readonly bool _isAdult;
        private readonly bool _isBirthday;

        public string FirstName
        {
            get => _firstName;
            set { Validators.ValidateName(value); _firstName = value; }
        }

        public string LastName
        {
            get => _lastName;
            set { Validators.ValidateName(value); _lastName = value; }
        }

        public string Email
        {
            get => _email;
            set { Validators.ValidateEmail(value); _email = value; }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set { Validators.ValidateDateOfBirth(value); _birthDate = value; }
        }

        public int Age => _age;
        public string WesternZodiac => _westernZodiac;
        public string ChineseZodiac => _chineseZodiac;
        public bool IsAdult => _isAdult;
        public bool IsBirthday => _isBirthday;

        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;

            _age = DateUtilites.CalculateAge(_birthDate);
            _isAdult = DateTime.Now.Year - _birthDate.Year > 18;
            _westernZodiac = DateUtilites.GetWesternZodiac(_birthDate);
            _chineseZodiac = DateUtilites.GetChineseZodiac(_birthDate);
            _isBirthday = DateUtilites.IsBirthday(_birthDate);
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} {Email} {BirthDate.ToString("dd.MM.yyyy")} {Age} {WesternZodiac} {ChineseZodiac}" +
                   $"{(IsAdult ? "Adult" : "Child")} {(IsBirthday ? "Birthday" : "")}";
        }
    }
}
