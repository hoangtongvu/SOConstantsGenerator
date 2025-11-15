using SOConstantsGenerator.Editor.Common;

namespace SOConstantsGenerator.Editor.FieldHandlers.Common;

public readonly record struct HandleInput(
    CodeWriter Writer,
    MyFieldInfo FieldInfo
);