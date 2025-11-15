using SOConstantsGenerator.Editor.FieldHandlers.Common;

namespace SOConstantsGenerator.Editor.FieldHandlers.DynamicFieldHandlers;

public class NormalDynamicFieldHandler : IDynamicFieldHandler
{
    public bool CanHandle(CanHandleInput canHandleInput)
    {
        return true;
    }

    public void HandleDeclarationGeneration(HandleInput handleInput)
    {
        var writer = handleInput.Writer;
        var fieldInfo = handleInput.FieldInfo;

        writer.WriteLine($"public static {fieldInfo.Type} {fieldInfo.Name};");
    }

    public void HandleAssignmentGeneration(HandleInput handleInput)
    {
        var writer = handleInput.Writer;
        var fieldInfo = handleInput.FieldInfo;

        writer.WriteLine($"{fieldInfo.Name} = so.{fieldInfo.Name};");
    }
}