using FastMember;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerDataAccessor
{
    internal static class Extensions
    {
        public static T ConvertToObject<T>(this SqlDataReader reader) where T : class, new()
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers(); // test if a stack would be faster to search; can pop an item once its found
            var t = new T();

            for( int i = 0; i < reader.FieldCount; i++)
            {
                if(!reader.IsDBNull(i))
                {
                    string fieldName = reader.GetName(i).FromSnakeCaseToPascalCase();
                    if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                    {
                        accessor[t, fieldName] = reader.GetValue(i);
                    }
                }
            }
            return t;
        }

        public static void PrintObjects<T>(this List<T> list)
        {
            //var type = typeof(T);
            //var props = type.GetProperties();
            //foreach (var prop in props)
            //{
            //    Console.Write($"{prop.Name} \t");
            //}
            foreach (var item in list)
            {
                
            }
            //Console.WriteLine()



        }

        public static string FromSnakeCaseToPascalCase(this string text)
        {
            if(text == null) throw new ArgumentNullException(nameof(text));
            if(text.Length < 2) return text;

            var sb = new StringBuilder();
            bool isFirstLetterOfWord = false;
            sb.Append(char.ToUpperInvariant(text[0]));

            for( int i = 1; i < text.Length; i++)
            {
                char c = text[i];

                if (isFirstLetterOfWord)
                {
                    sb.Append(char.ToUpperInvariant(c));
                    isFirstLetterOfWord = false;
                    continue;
                }
                
                if (c.Equals('_'))
                {
                    isFirstLetterOfWord = true;
                    continue;
                }
                sb.Append(c);
            }
            return sb.ToString();
        }
        public static string FromPascalToSnakeCase(this string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            var sb = new StringBuilder();
            sb.Append(char.ToLowerInvariant(text[0]));
           
            for (int i = 1; i < text.Length; i++)
            {
                if (!char.IsUpper(text[i])) sb.Append(text[i]);
                else
                {
                    sb.Append('_');
                    sb.Append(char.ToLowerInvariant(text[i]));
                }
            }
            return sb.ToString();
        }
    }
}
