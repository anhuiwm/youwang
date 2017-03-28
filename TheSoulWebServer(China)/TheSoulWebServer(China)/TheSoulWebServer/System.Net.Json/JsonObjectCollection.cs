using System.Collections.Generic;

namespace System.Net.Json
{
	public class JsonObjectCollection : JsonCollection
	{
	    // Methods
	    public JsonObjectCollection()
	    {
	    }
	
	    public JsonObjectCollection(IEnumerable<JsonObject> collection) : base(collection)
	    {
	    }
	
	    public JsonObjectCollection(string name) : base(name)
	    {
	    }
	
	    public JsonObjectCollection(string name, IEnumerable<JsonObject> collection) : base(name, collection)
	    {
	    }
	
	    // Properties
	    protected override char BeginCollection
	    {
	        get
	        {
	            return '{';
	        }
	    }
	
	    protected override char EndCollection
	    {
	        get
	        {
	            return '}';
	        }
	    }
	
	    public JsonObject this[string name]
	    {
	        get
	        {
	            for (int i = 0; i < base.Count; i++)
	            {
	                if (base[i].Name == name)
	                {
	                    return base[i];
	                }
	            }
	            return null;
	        }
	    }
	}
	
}
