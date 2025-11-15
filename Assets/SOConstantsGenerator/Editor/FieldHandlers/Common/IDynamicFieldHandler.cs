
namespace SOConstantsGenerator.Editor.FieldHandlers.Common;

public interface IDynamicFieldHandler
{
    bool CanHandle(CanHandleInput canHandleInput);

    void HandleDeclarationGeneration(HandleInput handleInput);

    void HandleAssignmentGeneration(HandleInput handleInput);
}