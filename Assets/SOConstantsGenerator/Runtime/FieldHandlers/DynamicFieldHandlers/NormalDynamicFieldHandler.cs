using SOConstantsGenerator.FieldHandlers.Common;

namespace SOConstantsGenerator.FieldHandlers.DynamicFieldHandlers;

public class NormalDynamicFieldHandler : IDynamicFieldHandler
{
    public bool CanHandle(CanHandleContext canHandleContext)
    {
        return true;
    }

    public void HandleDeclarationGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;

        writer.WriteLine($"public static {fieldInfo.Type} {fieldInfo.Name};");
    }

    public void HandleAssignmentGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;

        writer.WriteLine($"{fieldInfo.Name} = so.{fieldInfo.Name};");
    }
}