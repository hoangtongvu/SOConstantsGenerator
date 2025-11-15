using SOConstantsGenerator.Editor.FieldHandlers.Common;
using System.Collections;

namespace SOConstantsGenerator.Editor.FieldHandlers.DynamicFieldHandlers;

public class ArrayDynamicFieldHandler : IDynamicFieldHandler
{
    private IEnumerable enumerable;

    public bool CanHandle(CanHandleInput canHandleInput)
    {
        var fieldInfo = canHandleInput.FieldInfo;

        if (fieldInfo.Value is IEnumerable enumerable && fieldInfo.Type.IsArray)
        {
            this.enumerable = enumerable;
            return true;
        }

        return false;
    }

    public void HandleDeclarationGeneration(HandleInput handleInput)
    {
        var writer = handleInput.Writer;
        var fieldInfo = handleInput.FieldInfo;
        var elementType = fieldInfo.Type.GetElementType();

        writer.WriteLine($"public static {elementType}[] {fieldInfo.Name};");
    }

    public void HandleAssignmentGeneration(HandleInput handleInput)
    {
        var writer = handleInput.Writer;
        var fieldInfo = handleInput.FieldInfo;

        writer.WriteLine($"{fieldInfo.Name} = so.{fieldInfo.Name};");
    }
}