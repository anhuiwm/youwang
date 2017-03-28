using System;
using System.IO;

namespace System.Net.Json
{

	public class JsonStringValue : JsonObject
	{
	    // Fields
	    private string _value;
	
	    // Methods
	    public JsonStringValue()
	    {
	    }
	
	    public JsonStringValue(string name)
	    {
	        base.Name = name;
	    }
	
	    public JsonStringValue(string name, string value)
	    {
	        base.Name = name;
	        this.Value = value;
	    }
	
	    public override bool Equals(object obj)
	    {
	        JsonStringValue value2 = obj as JsonStringValue;
	        if (value2 == null)
	        {
	            return false;
	        }
	        return (this.Value == value2.Value);
	    }
	
	    public override int GetHashCode()
	    {
	        return this.Value.GetHashCode();
	    }
	
	    public override object GetValue()
	    {
	        return this.Value;
	    }
	
	    public override string ToString()
	    {
	        return base.ToString();
	    }
	
	    public override void WriteTo(TextWriter writer)
	    {
	        if (base.Name != string.Empty)
	        {
	            writer.Write('"');
	            writer.Write(base.Name);
	            writer.Write('"');
	            writer.Write(':');
	            JsonUtility.WriteSpace(writer);
	        }
	        writer.Write(JsonUtility.EscapeString(this.Value));
	    }
	
	    // Properties
	    public string Value
	    {
	        get
	        {
	            if (this._value != null)
	            {
	                return this._value;
	            }
	            return string.Empty;
	        }
	        set
	        {
	            this._value = value;
	        }
	    }
	}
	
}
