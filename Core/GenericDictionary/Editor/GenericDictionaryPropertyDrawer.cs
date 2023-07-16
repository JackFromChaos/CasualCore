using UnityEditor;
using UnityEngine;


  [CustomPropertyDrawer(typeof(GenericDictionary<,>))]
  public class GenericDictionaryPropertyDrawer : PropertyDrawer
  {
    private const float warningBoxHeight = 1.5f;
    private static float lineHeight = EditorGUIUtility.singleLineHeight;
    private static float vertSpace = EditorGUIUtility.standardVerticalSpacing;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) =>
      OnGUIHorrizontal(position, property, label);

    public void OnGUIHorrizontal(Rect position, SerializedProperty property, GUIContent label)
    {
      // Draw list of key/value pairs.
      var list = property.FindPropertyRelative("list");
      EditorGUI.PropertyField(position, list, label, true);

      // Draw key collision warning.
      var keyCollision = property.FindPropertyRelative("keyCollision").boolValue;
      if (keyCollision)
      {
        position.y += EditorGUI.GetPropertyHeight(list, true);
        if (!list.isExpanded)
          position.y += vertSpace;

        position.height = lineHeight * warningBoxHeight;
        position = EditorGUI.IndentedRect(position);
        EditorGUI.HelpBox(position, "Duplicate keys will not be serialized.", MessageType.Warning);
      }
    }

    public void OnGUIVertival(Rect position, SerializedProperty property, GUIContent label)
    {
      // Draw list of key/value pairs.
      var list = property.FindPropertyRelative("list");
      EditorGUI.PropertyField(position, list, label, true);

      // Draw key collision warning.
      var keyCollision = property.FindPropertyRelative("keyCollision").boolValue;
      if (keyCollision)
      {
        position.y += EditorGUI.GetPropertyHeight(list, true);
        if (!list.isExpanded)
          position.y += vertSpace;

        position.height = lineHeight * warningBoxHeight;
        position = EditorGUI.IndentedRect(position);
        EditorGUI.HelpBox(position, "Duplicate keys will not be serialized.", MessageType.Warning);
      }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      // Height of KeyValue list.
      var height = 0f;
      var list = property.FindPropertyRelative("list");
      if (list == null)
        return 100;

      height += EditorGUI.GetPropertyHeight(list, true);

      // Height of key collision warning.
      var keyCollision = property.FindPropertyRelative("keyCollision").boolValue;
      if (keyCollision)
      {
        height += warningBoxHeight * lineHeight;
        if (!list.isExpanded)
          height += vertSpace;
      }

      return height;
    }
  }
