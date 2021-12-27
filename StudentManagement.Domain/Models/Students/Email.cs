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

        public static DomainResult<Email> Create(string email)
        {
            List<Message> messages = new List<Message>();

            if (string.IsNullOrWhiteSpace(email))
                messages.Add(ErrorMessage.Create(nameof(email), "Email should not be empty"));

            email = email.Trim();

            if (email.Length > 200)
                messages.Add(ErrorMessage.Create(nameof(email), "Email is too long"));

            if (!Regex.IsMatch(email, @"^(.+)@(.+)$"))
                messages.Add(ErrorMessage.Create(nameof(email), "Email is invalid"));

            return DomainResult.FromSuccess(messages, new Email(email));
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
