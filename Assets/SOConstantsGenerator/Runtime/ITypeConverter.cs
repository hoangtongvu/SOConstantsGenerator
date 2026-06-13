namespace SOConstantsGenerator;

public interface ITypeConverter<TSource, TDest>
{
    TDest Convert(TSource source);
}