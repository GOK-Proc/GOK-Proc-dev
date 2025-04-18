﻿using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EncryptedScriptableObject), true)]
public class EncryptedScriptableObjectEditor : Editor
{
	private EncryptedScriptableObject _target;
	private SerializedProperty _saveDirProperty;
	private SerializedProperty _fileNameProperty;

	private void OnEnable()
	{
		_target = (EncryptedScriptableObject)target;
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

			GUILayout.Space(EditorGUIUtility.singleLineHeight);

			DrawDefaultInspector();

			GUILayout.Space(EditorGUIUtility.singleLineHeight);

			if (GUILayout.Button("Save"))
			{
				_target.Save();
			}

			if (GUILayout.Button("Load"))
			{
				_target.Load();
			}

		}
	}
}
