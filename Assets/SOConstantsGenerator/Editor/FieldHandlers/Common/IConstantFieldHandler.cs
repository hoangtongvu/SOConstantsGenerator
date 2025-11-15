
namespace SOConstantsGenerator.Editor.FieldHandlers.Common;

public interface IConstantFieldHandler
{
    bool CanHandle(CanHandleInput canHandleInput);

    void HandleInLineGeneration(HandleInput handleInput);
}