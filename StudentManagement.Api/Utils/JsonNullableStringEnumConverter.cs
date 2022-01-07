using System.Text.Json;
using System.Text.Json.Serialization;

namespace StudentManagement.Api.Utils;

// See https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-6-0#sample-factory-pattern-converter
public class JsonNullableStringEnumConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
		JsonConverter? converter = (JsonConverter?) Activator.CreateInstance(
			typeof(NullableStringEnumConverter<>).MakeGenericType(typeToConvert));

		return converter;
    }

	// See https://github.com/dotnet/runtime/issues/30947
	private class NullableStringEnumConverter<T> : JsonConverter<T> where T : Enum
	{
		// private readonly JsonConverter<T> _valueConverter;
		private readonly Type _underlyingType;

		public NullableStringEnumConverter()
		{
			_underlyingType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
		}

		public override bool CanConvert(Type typeToConvert)
		{
			return typeof(T).IsAssignableFrom(typeToConvert);
		}

		public override T? Read(ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			string? value = reader.GetString();
			if (string.IsNullOrEmpty(value)) return default;

			if (!Enum.TryParse(_underlyingType, value, ignoreCase: true, out object? result))
			{
				return default;
			}

			return (T?) result;
		}

		public override void Write(Utf8JsonWriter writer,
			T value,
			JsonSerializerOptions options)
		{
			writer.WriteStringValue(value?.ToString());
		}
	}
}