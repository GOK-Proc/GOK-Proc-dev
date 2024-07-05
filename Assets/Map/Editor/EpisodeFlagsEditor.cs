using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EpisodeFlags))]
public class EpisodeFlagsEditor : Editor
{
	private EpisodeFlags _episodeFlags;

	private void OnEnable()
	{
		_episodeFlags = (EpisodeFlags)target;
	}

	public override void OnInspectorGUI()
	{
		if (_episodeFlags != null)
		{
			EditorGUILayout.LabelField("Flags", EditorStyles.boldLabel);

			if (_episodeFlags.FlagsList != null)
			{
				foreach(var kvp  in _episodeFlags.FlagsList) 
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField($"{kvp.Key.Item1}-{kvp.Key.Item2}", GUILayout.Width(100));
					kvp.Value = EditorGUILayout.Toggle(kvp.Value);
					EditorGUILayout.EndHorizontal();
				}
			}

			if (GUI.changed)
			{
				EditorUtility.SetDirty(target);
			}
		}
	}
}
