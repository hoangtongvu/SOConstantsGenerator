using System;

namespace SOConstantsGenerator;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ConstantFieldAttribute : Attribute
{
    public Type ConverterType {  get; }

    public ConstantFieldAttribute(Type converter = null) : base()
    {
        this.ConverterType = converter;
    }
}