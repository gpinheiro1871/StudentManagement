using CSharpFunctionalExtensions;
using StudentManagement.Domain.Utils;
using System.Text.RegularExpressions;
using static StudentManagement.Domain.Utils.Error;

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
                return Errors.General.ValueIsRequired();

            email = email.Trim();

            if (email.Length > 200)
                return Errors.General.InvalidLength(email);

            if (!Regex.IsMatch(email, @"^(.+)@(.+)$"))
                return Errors.General.ValueIsInvalid();

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
