using SOConstantsGenerator.FieldHandlers.Common;
using SOConstantsGenerator.FieldHandlers.ConstantFieldHandlers;
using System.Collections.Generic;

namespace SOConstantsGenerator.FieldProcessors;

public class ConstantFieldProcessor
{
    private List<IConstantFieldHandler> fieldHandlers = new();

    public ConstantFieldProcessor()
    {
        fieldHandlers.Add(new HashMapConstantFieldHandler());
        fieldHandlers.Add(new ArrayConstantFieldHandler());
        fieldHandlers.Add(new ListConstantFieldHandler());
        fieldHandlers.Add(new NormalConstantFieldHandler());
    }

    public void Process(CanHandleContext canHandleContext, HandleContext handleContext)
    {
        foreach (var handler in fieldHandlers)
        {
            if (!handler.CanHandle(canHandleContext)) continue;

            handler.HandleInLineGeneration(handleContext);
            break;
        }
    }
}