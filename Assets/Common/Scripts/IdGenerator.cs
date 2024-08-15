using Rhythm;
using System.Collections.Generic;
using System.Linq;

public static class IdGenerator
{
	public static void GenerateRhythmId(BeatmapData data)
	{
		var Ids = new List<string> { "None" };
		Ids.AddRange(data.BeatmapDictionary.Values.Select(x => x.Id).ToList());

		EnumCreator.Create(
			enumName: "RhythmId",
			itemNameList: Ids,
			exportPath: "Assets/Common/Scripts/RhythmId.cs"
		);
	}
}