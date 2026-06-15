using System;
using System.IO;

namespace SOConstantsGenerator.Common;

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

    public void Unindent() { if (_indentLevel > 0) _indentLevel--; }

    public void Write() => _writer.Write(new string(' ', _indentLevel * 4));

    public void Write(string line) => _writer.Write(new string(' ', _indentLevel * 4) + line);

    public void WriteNoIndent(string line) => _writer.Write(line);

    public void WriteLine() => _writer.WriteLine();

    public void WriteLine(string line) => _writer.WriteLine(new string(' ', _indentLevel * 4) + line);

    public void WriteLineNoIndent(string line) => _writer.WriteLine(line);

    public void Flush() => _writer.Flush();

    public void Close() => _writer.Close();

    public void Dispose() => _writer.Dispose();

    public IDisposable Block(string opening = "{", string closing = "}")
    {
        WriteLine(opening);
        Indent();
        return new BlockScope(() => { Unindent(); WriteLine(closing); });
    }

    private class BlockScope : IDisposable
    {
        private readonly Action _onDispose;
        public BlockScope(Action onDispose) => _onDispose = onDispose;
        public void Dispose() => _onDispose();
    }
}