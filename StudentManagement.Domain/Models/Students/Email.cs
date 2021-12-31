using CSharpFunctionalExtensions;
using StudentManagement.Domain.Utils;
using System.Text.RegularExpressions;

namespace StudentManagement.Domain.Models.Students
{
    public class Email : ValueObject
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email, Error> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Errors.General.ValueIsRequired(nameof(Email));

            email = email.Trim();

            if (email.Length > 200)
                return Errors.General.InvalidLength(nameof(Email));

            if (!Regex.IsMatch(email, @"^(.+)@(.+)$"))
                return Errors.General.ValueIsInvalid(nameof(Email));

            return new Email(email);
        }

        public override string ToString()
        {
            return Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
