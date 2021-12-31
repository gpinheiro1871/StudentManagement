using StudentManagement.Domain.Utils;

namespace StudentManagement.Api.Utils
{
    public class Envelope<T>
    {
        public T? Data { get; }
        public bool Success { get; }
        public IDictionary<string, Error[]>? Errors { get; }
        public DateTime TimeGenerated { get; }

        public Envelope(T? data, IDictionary<string, Error[]>? errors)
        {
            Data = data;
            Errors = errors;
            TimeGenerated = DateTime.UtcNow;
            Success = !errors?.Any() ?? true;
        }
    }

    public class Envelope : Envelope<string>
    {
        private Envelope(IDictionary<string, Error[]>? errors) 
            : base(null, errors)
        {
        }

        public static Envelope<T> Result<T>(T data)
        {
            return new Envelope<T>(data, null);
        }

        public static Envelope Error(IDictionary<string, Error[]> errors)
        {
            return new Envelope(errors);
        }

        public static Envelope Error(string fieldName, Error error)
        {
            Dictionary<string, Error[]>? errors = new();

            errors.Add(fieldName, new Error[] { error } );

            return new Envelope(errors);
        }
    }
}
