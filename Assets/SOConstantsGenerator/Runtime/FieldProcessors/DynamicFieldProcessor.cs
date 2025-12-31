using SOConstantsGenerator.FieldHandlers.Common;
using SOConstantsGenerator.FieldHandlers.DynamicFieldHandlers;
using System.Collections.Generic;

namespace SOConstantsGenerator.FieldProcessors;

public class DynamicFieldProcessor
{
    private List<IDynamicFieldHandler> fieldHandlers = new();

    public DynamicFieldProcessor()
    {
        fieldHandlers.Add(new HashMapDynamicFieldHandler());
        fieldHandlers.Add(new ArrayDynamicFieldHandler());
        fieldHandlers.Add(new ListDynamicFieldHandler());
        fieldHandlers.Add(new NormalDynamicFieldHandler());
    }

    public void ProcessDeclaration(CanHandleInput canHandleInput, HandleInput handleInput)
    {
        foreach (var handler in fieldHandlers)
        {
            if (!handler.CanHandle(canHandleInput)) continue;

            handler.HandleDeclarationGeneration(handleInput);
            break;
        }
    }

    public void ProcessAssignment(CanHandleInput canHandleInput, HandleInput handleInput)
    {
        foreach (var handler in fieldHandlers)
        {
            if (!handler.CanHandle(canHandleInput)) continue;

            handler.HandleAssignmentGeneration(handleInput);
            break;
        }
    }
}