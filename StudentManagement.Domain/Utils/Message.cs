namespace StudentManagement.Domain.Utils
{
    public class Message
    {
        public string? Title { get; }
        public string Body { get; }

        protected Message(string? title, string body)
        {
            Title = title;
            Body = body;
        }

        public static Message Create(string? title, string body)
        {
            return new Message(title, body);
        }
    }

    public class ErrorMessage : Message
    {
        protected ErrorMessage(string? title, string body)
            : base(title, body)
        {
        }

        public static ErrorMessage Create(string? title, string body)
        {
            return new ErrorMessage(title, body);
        }
    }
}
