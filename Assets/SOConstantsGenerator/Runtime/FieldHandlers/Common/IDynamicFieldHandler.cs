namespace SOConstantsGenerator.FieldHandlers.Common;

public interface IDynamicFieldHandler
{
    bool CanHandle(CanHandleContext canHandleContext);

    void HandleDeclarationGeneration(HandleContext handleContext);

    void HandleAssignmentGeneration(HandleContext handleContext);
}