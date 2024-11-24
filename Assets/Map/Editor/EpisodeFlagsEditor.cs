using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Map
{
	[CustomEditor(typeof(EpisodeFlags))]
	public class EpisodeFlagsEditor : Editor
	{
		private EpisodeFlags _target;
		private SerializedProperty _saveDirProperty;
		private SerializedProperty _fileNameProperty;

		private void OnEnable()
		{
			_target = (EpisodeFlags)target;
			_saveDirProperty = serializedObject.FindProperty("_saveDir");
			_fileNameProperty = serializedObject.FindProperty("_fileName");
		}

		public override void OnInspectorGUI()
		{
			if (_target != null)
			{
				serializedObject.Update();

				EditorGUILayout.LabelField("Save Settings", EditorStyles.boldLabel);

				EditorGUILayout.PropertyField(_saveDirProperty);
				EditorGUILayout.PropertyField(_fileNameProperty);

				serializedObject.ApplyModifiedProperties();

				EditorGUILayout.LabelField("Flags", EditorStyles.boldLabel);

				if (_target.FlagList != null)
				{
					foreach (var kvp in _target.FlagList)
					{
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField($"{kvp.Key.Item1}-{kvp.Key.Item2}", GUILayout.Width(100));
						kvp.Value = EditorGUILayout.Toggle(kvp.Value);
						EditorGUILayout.EndHorizontal();
					}
				}

				GUILayout.Space(EditorGUIUtility.singleLineHeight);

				if (GUILayout.Button("Save"))
				{
					_target.Save();
				}

				if (GUILayout.Button("Load"))
				{
					_target.Load();
				}

				AssetDatabase.Refresh();
			}
		}
	}
}