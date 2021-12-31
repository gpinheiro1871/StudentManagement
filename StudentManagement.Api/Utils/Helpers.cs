using System.Text;

namespace StudentManagement.Api.Utils;

public class Helpers
{
    /**
     *  <summary>
     *  Generates a string containing the type name with its generics 
     *  and all its parents in descending order and separated by periods
     *  </summary>
     *  <example>
     *  x = C
     *  returns
     *  "A.B.C<D<E>>"
     *  </example>
     */
    public static string GetNestedDisplayName(Type x)
    {
        string name = x.Name;

        if (x.IsGenericType)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(x.Name.Substring(0, x.Name.IndexOf('`')));
            sb.Append('<');
            bool appendComma = false;
            foreach (Type arg in x.GetGenericArguments())
            {
                if (appendComma) sb.Append(',');
                sb.Append(GetNestedDisplayName(arg));
                appendComma = true;
            }
            sb.Append('>');

            name = sb.ToString();
        }

        if (x.IsNested && x.DeclaringType is not null)
        {
            name = $"{GetNestedDisplayName(x.DeclaringType)}.{name}";
        }

        return name;
    } 
}