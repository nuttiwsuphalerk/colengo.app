namespace colengo.app.Server.Helper
{
    public static class ConvertPatternString
    {
        public static string ToPascalCase(this string value) =>
        value switch
        {
            null => throw new ArgumentNullException(nameof(value)),
            "" => throw new ArgumentException($"{nameof(value)} cannot be empty", nameof(value)),
            _ => string.Concat(value[0].ToString().ToUpper(), value.AsSpan(1))
        };

        public static string ToPascalCaseWithSeparator(this string value, string separator) =>
        value switch
        {
            null => throw new ArgumentNullException(nameof(value)),
            "" => throw new ArgumentException($"{nameof(value)} cannot be empty", nameof(value)),
            _ => string.Join(separator, value.Split(separator).Select(x => string.Concat(x[0].ToString().ToUpper(), x.AsSpan(1))))
        };
    }
}
