namespace SOConstantsGenerator.FieldHandlers.Common;

public interface IConstantFieldHandler
{
    bool CanHandle(CanHandleContext canHandleContext);

    void HandleInLineGeneration(HandleContext handleContext);
}