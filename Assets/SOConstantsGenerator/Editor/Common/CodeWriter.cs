using System;
using System.IO;

namespace SOConstantsGenerator.Editor.Common;

public class CodeWriter : IDisposable
{
    private StreamWriter _writer;
    private int _indentLevel;

    public StreamWriter InternalWriter => _writer;
    public int IndentLevel => _indentLevel;

    public CodeWriter(StreamWriter writer, int startIndentLevel = 0)
    {
        _writer = writer;
        _indentLevel = startIndentLevel;
    }

    public void Indent() => _indentLevel++;

    public void Unindent() => _indentLevel--;

    public void Write(string line) => _writer.Write(line);

    public void WriteLine() => _writer.WriteLine();

    public void WriteLine(string line) => _writer.WriteLine(new string(' ', _indentLevel * 4) + line);

    public void Flush() => _writer.Flush();

    public void Close() => _writer.Close();

    public void Dispose() => _writer.Dispose();
}