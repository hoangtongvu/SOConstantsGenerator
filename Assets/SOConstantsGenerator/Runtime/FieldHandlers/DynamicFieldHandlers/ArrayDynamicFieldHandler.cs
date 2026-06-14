using SOConstantsGenerator.FieldHandlers.Common;
using System.Collections;

namespace SOConstantsGenerator.FieldHandlers.DynamicFieldHandlers;

public class ArrayDynamicFieldHandler : IDynamicFieldHandler
{
    private IEnumerable enumerable;

    public bool CanHandle(CanHandleContext canHandleContext)
    {
        var fieldInfo = canHandleContext.FieldInfo;

        if (fieldInfo.Value is IEnumerable enumerable && fieldInfo.Type.IsArray)
        {
            this.enumerable = enumerable;
            return true;
        }

        return false;
    }

    public void HandleDeclarationGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;
        var elementType = fieldInfo.Type.GetElementType();

        writer.WriteLine($"public static {elementType}[] {fieldInfo.Name};");
    }

    public void HandleAssignmentGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;

        writer.WriteLine($"{fieldInfo.Name} = so.{fieldInfo.Name};");
    }
}