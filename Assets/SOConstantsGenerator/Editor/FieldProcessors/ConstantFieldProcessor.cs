using SOConstantsGenerator.Editor.FieldHandlers.Common;
using System.Collections.Generic;
using SOConstantsGenerator.Editor.FieldHandlers.ConstantFieldHandlers;

namespace SOConstantsGenerator.Editor.FieldProcessors;

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

    public void Process(CanHandleInput canHandleInput, HandleInput handleInput)
    {
        foreach (var handler in fieldHandlers)
        {
            if (!handler.CanHandle(canHandleInput)) continue;

            handler.HandleInLineGeneration(handleInput);
            break;
        }
    }
}