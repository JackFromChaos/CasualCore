using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//[CustomPropertyDrawer(typeof(GMGUIStyleCache))]
public class GMGUIStyleCacheDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);


		EditorGUI.EndProperty();
	}
}
