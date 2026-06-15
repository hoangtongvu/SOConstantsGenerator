namespace SOConstantsGenerator;

public interface ITypeConverter
{
    public static TDest[] Convert<TSource, TDest>(TSource[] sourceArray, ITypeConverter<TSource, TDest> converter)
    {
        int length = sourceArray.Length;
        var destArray = new TDest[length];

        for (int i = 0; i < length; i++)
        {
            destArray[i] = converter.Convert(sourceArray[i]);
        }

        return destArray;
    }
}

public interface ITypeConverter<TSource, TDest> : ITypeConverter
{
    TDest Convert(TSource source);
}