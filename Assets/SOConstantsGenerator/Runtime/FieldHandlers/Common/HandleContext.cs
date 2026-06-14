using SOConstantsGenerator.Common;
using System;

namespace SOConstantsGenerator.FieldHandlers.Common;

public readonly record struct HandleContext(
    CodeWriter Writer,
    MyFieldInfo FieldInfo,
    Type[] ConverterTypes
);