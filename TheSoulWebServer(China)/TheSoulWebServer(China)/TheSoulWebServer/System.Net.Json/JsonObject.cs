using System;
using System.IO;

namespace System.Net.Json
{
	public abstract class JsonObject
	{
	    // Fields
	    private string _name;
	
	    // Methods
	    protected JsonObject()
	    {
	    }
	
	    public abstract override bool Equals(object obj);
	    public abstract override int GetHashCode();
	    public abstract object GetValue();
	    public override string ToString()
	    {
	        StringWriter writer = new StringWriter();
	        this.WriteTo(writer);
	        return writer.ToString();
	    }
	
	    public abstract void WriteTo(TextWriter writer);
	
	    // Properties
	    public string Name
	    {
	        get
	        {
	            if (this._name == null)
	            {
	                return string.Empty;
	            }
	            return this._name;
	        }
	        set
	        {
	            if (value == null)
	            {
	                this._name = string.Empty;
	            }
	            else
	            {
	                this._name = value.Trim();
	            }
	        }
	    }
	}
	
}
