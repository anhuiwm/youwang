using System.Collections.Generic;

namespace System.Net.Json
{
	public class JsonArrayCollection : JsonCollection
	{
	    // Methods
	    public JsonArrayCollection()
	    {
	    }
	
	    public JsonArrayCollection(IEnumerable<JsonObject> collection) : base(collection)
	    {
	    }
	
	    public JsonArrayCollection(string name) : base(name)
	    {
	    }
	
	    public JsonArrayCollection(string name, IEnumerable<JsonObject> collection) : base(name, collection)
	    {
	    }
	
	    // Properties
	    protected override char BeginCollection
	    {
	        get
	        {
	            return '[';
	        }
	    }
	
	    protected override char EndCollection
	    {
	        get
	        {
	            return ']';
	        }
	    }
	}

}