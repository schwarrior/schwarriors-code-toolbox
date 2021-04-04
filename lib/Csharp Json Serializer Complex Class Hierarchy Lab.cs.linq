<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
</Query>

void Main()
{
	//this lab was created after tried to serialize / deserialize via json
	//a class hierarchy composed of classes inheriting list<t>
	//the json service didnt understand that type of class
	//so this lab proves that json ser can work as long as child collections are properties
	
	var myLevelA = new LevelA {Text = "Root"};
	var myLevelB1 = new LevelB {Text = "B1"};//myLevelA.LevelBs.Add();
	myLevelA.LevelBs.Add(myLevelB1);
	var myLevelB2 = new LevelB{Text = "B2"};
	myLevelA.LevelBs.Add(myLevelB2);
	var myLevelC1 = new LevelC{Text = "C1"};
	myLevelB1.LevelCs.Add(myLevelC1);

	myLevelA.Dump();
	
	var jsonService = new JsonSerializationService<LevelA>();
	var json = jsonService.GetJsonStringOfObject(myLevelA);
	json.Dump();
	
	var myLevelADeserialized = jsonService.GetNewObjectFromJsonString(json);
	
	myLevelADeserialized.Dump();
}

public class LevelA
{
	public LevelA()
	{
		LevelBs = new List<LevelB>();
	}

	public string Text = "Hello from A";
	
	public List<LevelB> LevelBs {get; set;}
}

public class LevelB
{
	public LevelB()
	{
		LevelCs = new List<LevelC>();
	}
	
	public string Text = "Hello from B";
	
	public List<LevelC> LevelCs {get; set;}
}

public class LevelC
{
	public string Text = "Hello from C";
}

// Define other methods and classes here
public class JsonSerializationService<T>
    {
        public string GetJsonStringOfObject(T sourceObject)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return serializer.Serialize(sourceObject);
        }

        public T GetNewObjectFromJsonString(string json)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var deserializedObject = serializer.Deserialize<T>(json);
            return deserializedObject;
        }
    }