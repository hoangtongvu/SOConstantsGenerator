using SOConstantsGenerator.Common;

namespace SOConstantsGenerator.FieldHandlers.Common;

public readonly record struct HandleInput(
    CodeWriter Writer,
    MyFieldInfo FieldInfo
);