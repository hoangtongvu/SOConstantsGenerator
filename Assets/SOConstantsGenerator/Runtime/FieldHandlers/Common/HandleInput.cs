using SOConstantsGenerator.Common;
using System;

namespace SOConstantsGenerator.FieldHandlers.Common;

public readonly record struct HandleInput(
    CodeWriter Writer,
    MyFieldInfo FieldInfo,
    Type[] ConverterTypes
);