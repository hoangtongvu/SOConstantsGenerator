namespace SOConstantsGenerator.FieldHandlers.Common;

public interface IConstantFieldHandler
{
    bool CanHandle(CanHandleInput canHandleInput);

    void HandleInLineGeneration(HandleInput handleInput);
}