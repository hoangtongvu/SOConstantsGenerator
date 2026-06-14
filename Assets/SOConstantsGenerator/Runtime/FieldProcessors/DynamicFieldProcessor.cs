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

    public void ProcessDeclaration(CanHandleContext canHandleContext, HandleContext handleContext)
    {
        foreach (var handler in fieldHandlers)
        {
            if (!handler.CanHandle(canHandleContext)) continue;

            handler.HandleDeclarationGeneration(handleContext);
            break;
        }
    }

    public void ProcessAssignment(CanHandleContext canHandleContext, HandleContext handleContext)
    {
        foreach (var handler in fieldHandlers)
        {
            if (!handler.CanHandle(canHandleContext)) continue;

            handler.HandleAssignmentGeneration(handleContext);
            break;
        }
    }
}