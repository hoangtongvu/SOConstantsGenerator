using SOConstantsGenerator.Editor.FieldHandlers.Common;
using System.Collections;

namespace SOConstantsGenerator.Editor.FieldHandlers.DynamicFieldHandlers;

public class ListDynamicFieldHandler : IDynamicFieldHandler
{
    private IEnumerable enumerable;
    private System.Type[] genericArguments;

    public bool CanHandle(CanHandleInput canHandleInput)
    {
        var fieldInfo = canHandleInput.FieldInfo;
        var args = fieldInfo.Type.GetGenericArguments();

        if (fieldInfo.Value is IEnumerable enumerable &&
            args.Length == 1)
        {
            this.enumerable = enumerable;
            this.genericArguments = args;
            return true;
        }

        return false;
    }

    public void HandleDeclarationGeneration(HandleInput handleInput)
    {
        var writer = handleInput.Writer;
        var fieldInfo = handleInput.FieldInfo;
        var elementType = this.genericArguments[0];

        writer.WriteLine($"public static {elementType}[] {fieldInfo.Name};");
    }

    public void HandleAssignmentGeneration(HandleInput handleInput)
    {
        var writer = handleInput.Writer;
        var fieldInfo = handleInput.FieldInfo;

        writer.WriteLine($"{fieldInfo.Name} = so.{fieldInfo.Name}.ToArray();");
    }
}