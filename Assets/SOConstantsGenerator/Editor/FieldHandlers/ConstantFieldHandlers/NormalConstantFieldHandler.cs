using SOConstantsGenerator.Editor.FieldHandlers.Common;
using static SOConstantsGenerator.Editor.Utilities;

namespace SOConstantsGenerator.Editor.FieldHandlers.ConstantFieldHandlers;

public class NormalConstantFieldHandler : IConstantFieldHandler
{
    public bool CanHandle(CanHandleInput canHandleInput)
    {
        return true;
    }

    public void HandleInLineGeneration(HandleInput handleInput)
    {
        var writer = handleInput.Writer;
        var fieldInfo = handleInput.FieldInfo;

        if (CanBeConst(fieldInfo.Type))
        {
            // Handle constants
            writer.WriteLine($"public const {fieldInfo.Type} {fieldInfo.Name} = {FormatValue(fieldInfo.Value)};");
        }
        else
        {
            // Handle static readonly
            var bytesString = BoxedStructToBytesString(fieldInfo.Type, fieldInfo.Value);
            writer.WriteLine($"public static readonly {fieldInfo.Type} {fieldInfo.Name} =");
            writer.Indent();
            writer.WriteLine($"Unsafe.As<byte, {fieldInfo.Type}>(ref new byte[] {{ {bytesString} }}[0]);");
            writer.Unindent();
        }
    }
}