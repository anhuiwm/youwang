using System;
using System.IO;

namespace System.Net.Json
{
	public class JsonBooleanValue : JsonObject
	{
	    // Fields
	    private bool? _value;
	
	    // Methods
	    public JsonBooleanValue()
	    {
	        this._value = null;
	    }
	
	    public JsonBooleanValue(bool? value)
	    {
	        this._value = null;
	        this.Value = value;
	    }
	
	    public JsonBooleanValue(string name)
	    {
	        this._value = null;
	    }
	
	    public JsonBooleanValue(string name, bool? value)
	    {
	        this._value = null;
	        base.Name = name;
	        this.Value = value;
	    }
	
	    public JsonBooleanValue(string name, string value)
	    {
	        this._value = null;
	        base.Name = name;
	        this._value = null;
	        if (value != null)
	        {
	            value = value.Trim().ToLower();
	            if (value != string.Empty)
	            {
	                switch (value.Trim().ToLower())
	                {
	                    case "null":
	                        this._value = null;
	                        return;
	
	                    case "true":
	                        this._value = true;
	                        return;
	
	                    case "false":
	                        this._value = false;
	                        return;
	                }
	                throw new NotSupportedException();
	            }
	        }
	    }
	
	    public override bool Equals(object obj)
	    {
	        JsonBooleanValue value2 = obj as JsonBooleanValue;
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
	        if (this.Value.HasValue)
	        {
	            writer.Write(this.Value.ToString().ToLower());
	        }
	        else
	        {
	            writer.Write("null");
	        }
	    }
	
	    // Properties
	    public bool? Value
	    {
	        get
	        {
	            return this._value;
	        }
	        set
	        {
	            this._value = value;
	        }
	    }
	}
}