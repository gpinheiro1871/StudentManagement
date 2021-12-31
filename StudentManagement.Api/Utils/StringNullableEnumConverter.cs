using System.Text.Json;
using System.Text.Json.Serialization;

namespace StudentManagement.Api.Utils;

// See https://github.com/dotnet/runtime/issues/30947
public class StringNullableEnumConverter<T> : JsonConverter<T>
{
	private readonly Type _underlyingType;

	public StringNullableEnumConverter()
	{
		_underlyingType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
	}

	public override bool CanConvert(Type typeToConvert)
	{
		return typeof(T).IsAssignableFrom(typeToConvert);
	}

	public override T Read(ref Utf8JsonReader reader,
		Type typeToConvert,
		JsonSerializerOptions options)
	{
		string value = reader.GetString();
		if (string.IsNullOrEmpty(value)) return default;

        if (!Enum.TryParse(_underlyingType, value, ignoreCase: true, out object result) &&
			!Enum.TryParse(_underlyingType, value, ignoreCase: true, out result))
        {
            return default;
        }

        return (T) result;
    }

	public override void Write(Utf8JsonWriter writer,
		T value,
		JsonSerializerOptions options)
	{
		writer.WriteStringValue(value?.ToString());
	}
}