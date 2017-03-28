using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace System.Net.Json
{

    public static class JsonUtility
	{
	    // Fields
	    internal const char begin_array = '[';
	    internal const char begin_object = '{';
	    internal static readonly CultureInfo CultureInfo = new CultureInfo("en-US", false);
	    internal const char end_array = ']';
	    internal const char end_object = '}';
	    public static bool GenerateIndentedJsonText = true;
	    internal const char indent = '\t';
	    internal static readonly SortedDictionary<int, int> IndentDepthCollection = new SortedDictionary<int, int>();
	    public static int MaxDepthNesting = -1;
	    public static int MaxStringLength = 0x400;
	    public static int MaxTextLength = -1;
	    public const string MimeType = "application/json";
	    internal const char name_separator = ':';
	    internal const char quote = '"';
	    internal const char space = ' ';
	    internal const char value_separator = ',';
	
	    // Methods
	    internal static string EscapeNonPrintCharacter(char c)
	    {
	        int num = c;
	        return (@"\u" + num.ToString("x4"));
	    }
	
	    internal static string EscapeString(string text)
	    {
	        StringBuilder builder = new StringBuilder();
	        builder.Append('"');
	        foreach (char ch in text)
	        {
	            switch (ch)
	            {
	                case '"':
	                    builder.Append("\\\"");
	                    break;
	
	                case '\\':
	                    builder.Append(@"\\");
	                    break;
	
	                case '\b':
	                    builder.Append(@"\b");
	                    break;
	
	                case '\f':
	                    builder.Append(@"\f");
	                    break;
	
	                case '\n':
	                    builder.Append(@"\n");
	                    break;
	
	                case '\r':
	                    builder.Append(@"\r");
	                    break;
	
	                case '\t':
	                    builder.Append(@"\t");
	                    break;
	
	                default:
	                    if (char.IsLetterOrDigit(ch))
	                    {
	                        builder.Append(ch);
	                    }
	                    else if (char.IsPunctuation(ch))
	                    {
	                        builder.Append(ch);
	                    }
	                    else if (char.IsSeparator(ch))
	                    {
	                        builder.Append(ch);
	                    }
	                    else if (char.IsWhiteSpace(ch))
	                    {
	                        builder.Append(ch);
	                    }
	                    else if (char.IsSymbol(ch))
	                    {
	                        builder.Append(ch);
	                    }
	                    else
	                    {
	                        builder.Append(EscapeNonPrintCharacter(ch));
	                    }
	                    break;
	            }
	        }
	        builder.Append('"');
	        return builder.ToString();
	    }
	
	    internal static string GetIndentString()
	    {
	        int indentDepth = IndentDepth;
	        if (indentDepth <= 0)
	        {
	            return string.Empty;
	        }
	        return new string('\t', indentDepth);
	    }
	
	    internal static string UnEscapeString(string text)
	    {
	        text = text.Trim();
	        if (text.StartsWith("\""))
	        {
	            text = text.Remove(0, 1);
	        }
            if (text.EndsWith("\"") && !text.EndsWith("\\\""))
	        {
	            text = text.Remove(text.Length - 1, 1);
	        }
	        StringBuilder builder = new StringBuilder();
	        try
	        {
	            for (int i = 0; i < text.Length; i++)
	            {
	                char ch = text[i];
	                if (ch == '\\')
	                {
	                    i++;
	                    if ((text[i] != 'u') && (text[i] != 'U'))
	                    {
	                        if (text[i] != 'n')
	                        {
	                            if (text[i] != 'r')
	                            {
	                                if (text[i] != 't')
	                                {
	                                    if (text[i] != 'f')
	                                    {
	                                        if (text[i] != 'b')
	                                        {
	                                            if (text[i] != '\\')
	                                            {
	                                                if (text[i] != '/')
	                                                {
	                                                    if (text[i] != '"')
	                                                    {
	                                                        throw new FormatException(string.Concat(new object[] { @"Unrecognized escape sequence '\", text[i], "' in position: ", i, "." }));
	                                                    }
	                                                    builder.Append('"');
	                                                }
	                                                else
	                                                {
	                                                    builder.Append('/');
	                                                }
	                                            }
	                                            else
	                                            {
	                                                builder.Append('\\');
	                                            }
	                                        }
	                                        else
	                                        {
	                                            builder.Append('\b');
	                                        }
	                                    }
	                                    else
	                                    {
	                                        builder.Append('\f');
	                                    }
	                                }
	                                else
	                                {
	                                    builder.Append('\t');
	                                }
	                            }
	                            else
	                            {
	                                builder.Append('\r');
	                            }
	                        }
	                        else
	                        {
	                            builder.Append('\n');
	                        }
	                    }
	                    else
	                    {
	                        char ch2 = (char) int.Parse(text.Substring(i + 1, 4), NumberStyles.HexNumber);
	                        i += 4;
	                        builder.Append(ch2);
	                    }
	                }
	                else
	                {
	                    builder.Append(ch);
	                }
	            }
	        }
	        catch (Exception e)
	        {
	            throw e;
	        }
	        return builder.ToString();
	    }
	
	    internal static void WriteIndent(TextWriter writer)
	    {
	        if (GenerateIndentedJsonText)
	        {
	            writer.Write(GetIndentString());
	        }
	    }
	
	    internal static void WriteLine(TextWriter writer)
	    {
	        if (GenerateIndentedJsonText)
	        {
	            writer.Write(Environment.NewLine);
	        }
	    }
	
	    internal static void WriteSpace(TextWriter writer)
	    {
	        if (GenerateIndentedJsonText)
	        {
	            writer.Write(' ');
	        }
	    }
	
	    // Properties
	    internal static int IndentDepth
	    {
	        get
	        {
	            int threadId = ThreadId;
	            try
	            {
	                return IndentDepthCollection[threadId];
	            }
	            catch (KeyNotFoundException)
	            {
	                return 0;
	            }
	        }
	        set
	        {
	            IndentDepthCollection[ThreadId] = value;
	        }
	    }
	
	    internal static int ThreadId
	    {
	        get
	        {
	            return Thread.CurrentThread.ManagedThreadId;
	        }
	    }
	}
}
