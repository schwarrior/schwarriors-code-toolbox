<Query Kind="Program" />

void Main()
{
	var nullV = new int?();
	var nullModifyStatus = new ModifyStatus(nullV);
	"Change Status Int: Null".Dump();
	("NoChange: " + nullModifyStatus.NoChange.ToString()).Dump();
	("ContentChange: " + nullModifyStatus.ContentChange.ToString()).Dump();
	("MetaChange: " + nullModifyStatus.ContentChange.ToString()).Dump();
	string.Empty.Dump();
	
	var maxEnumValue = Utilities.GetMaxFlagInt(typeof(ModifyStatus.ModifyStatusEnum));
	
	for(int s = -1; s <= maxEnumValue+1; s++)
	{
		("Change Status Int: " + s.ToString()).Dump();
		try
		{
			var changeStatus = new ModifyStatus(s);
			("NoChange: " + changeStatus.NoChange.ToString()).Dump();
			("ContentChange: " + changeStatus.ContentChange.ToString()).Dump();
			("MetaChange: " + changeStatus.ContentChange.ToString()).Dump();
		}
		catch(Exception ex)
		{
			ex.ToString().Dump();
		}
		string.Empty.Dump();
	}
}

public class ModifyStatus
{
	ModifyStatusEnum _changeStatusEnum = ModifyStatusEnum.Unknown;

	public ModifyStatus(int? changeStatusNullableInt)
	{
		if (!changeStatusNullableInt.HasValue) {_changeStatusEnum = ModifyStatusEnum.Unknown; return;} //treat null same as 0 / Unknown
		int maxFlagValue;
		var valueWithinRange = Utilities.FlagEnumValueWithinRange(typeof(ModifyStatusEnum), changeStatusNullableInt.Value, out maxFlagValue);
		if (!valueWithinRange) 			throw new ArgumentOutOfRangeException("Change Status int must be between 0 and " + maxFlagValue);
		_changeStatusEnum = (ModifyStatusEnum)changeStatusNullableInt.Value;
	}
	
	public bool NoChange 
		{ get { return ((_changeStatusEnum & ModifyStatusEnum.NoChange) != ModifyStatusEnum.Unknown); } }
	
	public bool ContentChange 
		{ get { return ((_changeStatusEnum & ModifyStatusEnum.Content) != ModifyStatusEnum.Unknown); } }
	
	public bool MetaChange 
		{ get { return ((_changeStatusEnum & ModifyStatusEnum.Meta) != ModifyStatusEnum.Unknown); } }
	
	[Flags]
	public enum ModifyStatusEnum
	{
		Unknown = 0,
		NoChange = 1,
		Content = 2,
		Meta = 4
	}
}

public class Utilities
{
	public static bool FlagEnumValueWithinRange(Type flagEnum, int value, out int maxFlagValue)
	{
		maxFlagValue = Utilities.GetMaxFlagInt(flagEnum);
		if(value < 0) return false;
		if(value > maxFlagValue) return false;
		return true;
	}

	public static int GetMaxFlagInt(Type flagEnum)
	{
		var maxFlagInt = 0;
		var enumObjectArray = Enum.GetValues(flagEnum);
		foreach(var enumObject in enumObjectArray)
		{
			var enumValue = (int)enumObject;
			if(enumValue>maxFlagInt) maxFlagInt = enumValue;
		}
		var maxFlagIntChecksum = Math.Pow(2, (enumObjectArray.Length-2));
		Debug.Assert(maxFlagInt == maxFlagIntChecksum, "Flag enum may not be configured correctly");
		return maxFlagInt;
	}
}