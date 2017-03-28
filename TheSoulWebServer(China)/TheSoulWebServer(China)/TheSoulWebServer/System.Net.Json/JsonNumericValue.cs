using System.IO;

namespace System.Net.Json
{
	public class JsonNumericValue : JsonObject
	{
	    // Fields
	    private double _value;
	
	    // Methods
	    public JsonNumericValue()
	    {
	    }
	
	    public JsonNumericValue(double value)
	    {
	        this.Value = value;
	    }
	
	    public JsonNumericValue(int value)
	    {
	        this.Value = value;
	    }
	
	    public JsonNumericValue(long value)
	    {
	        this.Value = value;
	    }

        public JsonNumericValue(ulong value)
        {
            this.Value = value;
        }
	
	    public JsonNumericValue(float value)
	    {
	        this.Value = value;
	    }
	
	    public JsonNumericValue(string name)
	    {
	        base.Name = name;
	    }
	
	    public JsonNumericValue(string name, double value)
	    {
	        base.Name = name;
	        this.Value = value;
	    }
	
	    public JsonNumericValue(string name, int value)
	    {
	        base.Name = name;
	        this.Value = value;
	    }
	
	    public JsonNumericValue(string name, long value)
	    {
	        base.Name = name;
	        this.Value = value;
	    }

        public JsonNumericValue(string name, ulong value)
        {
            base.Name = name;
            this.Value = value;
        }
	
	    public JsonNumericValue(string name, float value)
	    {
	        base.Name = name;
	        this.Value = value;
	    }
	
	    public override bool Equals(object obj)
	    {
	        JsonNumericValue value2 = obj as JsonNumericValue;
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
	        writer.Write(this.Value.ToString("g", JsonUtility.CultureInfo));
	    }
	
	    // Properties
	    public double Value
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
