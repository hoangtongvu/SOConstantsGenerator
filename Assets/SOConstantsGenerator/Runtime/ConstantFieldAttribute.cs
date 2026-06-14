using System;

namespace SOConstantsGenerator;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ConstantFieldAttribute : Attribute
{
    public Type[] ConverterTypes {  get; }

    public ConstantFieldAttribute(params Type[] converterTypes) : base()
    {
        if (converterTypes == null) return;

        this.ConverterTypes = converterTypes.Length == 0
            ? null
            : converterTypes;
    }
}