using System.Text;

namespace SOConstantsGenerators.Common.StringBuilderUtilities;

public class CodeStringBuilder
{
    private readonly StringBuilder _stringBuilder;
    private int _indentLevel;

    public StringBuilder StringBuilder => _stringBuilder;
    public int IndentLevel => _indentLevel;

    public CodeStringBuilder(int initialCap = 16, int startIndentLevel = 0)
    {
        _stringBuilder = new StringBuilder(initialCap);
        _indentLevel = startIndentLevel;
    }

    public void Indent() => _indentLevel++;

    public void Unindent() => _indentLevel--;

    public void Append(string line) => _stringBuilder.Append(line);

    public void AppendLine() => _stringBuilder.AppendLine();

    public void AppendLine(string line) => _stringBuilder.AppendLine(new string(' ', _indentLevel * 4) + line);

    public override string ToString() => _stringBuilder.ToString();
}