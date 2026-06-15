using SOConstantsGenerator;
using Unity.Collections;

public class FixedString64BytesConverter : ITypeConverter<string, FixedString64Bytes>
{
    public FixedString64Bytes Convert(string source)
    {
        return new(source);
    }
}