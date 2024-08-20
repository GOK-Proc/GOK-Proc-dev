using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Map
{
	[CustomEditor(typeof(EpisodeFlags))]
	public class EpisodeFlagsEditor : Editor
	{
		private EpisodeFlags _episodeFlags;
		private MethodInfo _saveJsonInfo;
		private MethodInfo _loadJsonInfo;

		private void OnEnable()
		{
			_episodeFlags = (EpisodeFlags)target;
			_saveJsonInfo = typeof(EpisodeFlags).GetMethod("SaveJson", BindingFlags.NonPublic | BindingFlags.Instance);
			_loadJsonInfo = typeof(EpisodeFlags).GetMethod("LoadJson", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		public override void OnInspectorGUI()
		{
			if (_episodeFlags != null)
			{
				EditorGUILayout.LabelField("Flags", EditorStyles.boldLabel);

				if (_episodeFlags.FlagList != null)
				{
					foreach (var kvp in _episodeFlags.FlagList)
					{
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField($"{kvp.Key.Item1}-{kvp.Key.Item2}", GUILayout.Width(100));
						kvp.Value = EditorGUILayout.Toggle(kvp.Value);
						EditorGUILayout.EndHorizontal();
					}
				}

				if (GUILayout.Button("SaveJson"))
				{
					_saveJsonInfo.Invoke(_episodeFlags, null);
				}

				if (GUILayout.Button("LoadJson"))
				{
					_loadJsonInfo.Invoke(_episodeFlags, null);
				}

				if (GUI.changed)
				{
					EditorUtility.SetDirty(target);
					AssetDatabase.SaveAssets();
				}
			}
		}
	}
}