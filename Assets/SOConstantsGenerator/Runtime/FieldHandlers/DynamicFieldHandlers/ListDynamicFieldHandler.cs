using SOConstantsGenerator.FieldHandlers.Common;
using System.Collections;

namespace SOConstantsGenerator.FieldHandlers.DynamicFieldHandlers;

public class ListDynamicFieldHandler : IDynamicFieldHandler
{
    private IEnumerable enumerable;
    private System.Type[] genericArguments;

    public bool CanHandle(CanHandleContext canHandleContext)
    {
        var fieldInfo = canHandleContext.FieldInfo;
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

    public void HandleDeclarationGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;
        var elementType = this.genericArguments[0];

        writer.WriteLine($"public static {elementType}[] {fieldInfo.Name};");
    }

    public void HandleAssignmentGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;

        writer.WriteLine($"{fieldInfo.Name} = so.{fieldInfo.Name}.ToArray();");
    }
}