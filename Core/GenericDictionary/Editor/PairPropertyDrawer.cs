using UnityEditor;
using UnityEngine;


  [CustomPropertyDrawer(typeof(Pair<,>))]
  public class PairPropertyDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      var labelEmpty = new GUIContent();
      var shortName = label.text.Replace("Element ", "");
      var labelShort = new GUIContent(shortName);

      var labelKey = new Rect(position.x, position.y, position.width / 4, position.height);
      var positionKey = new Rect(position.width / 4, position.y, position.width / 4, position.height);
      var positionValue = new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height);
      var key = property.FindPropertyRelative("Key");
      EditorGUI.PrefixLabel(labelKey, labelShort);
      EditorGUI.PropertyField(positionKey, key, labelEmpty, true);
      var value = property.FindPropertyRelative("Value");
      EditorGUI.PropertyField(positionValue, value, labelEmpty, true);
    }

    public void OnGUI1111(Rect position, SerializedProperty property, GUIContent label)
    {
      // Draw list of key/value pairs.
      var key = property.FindPropertyRelative("Key");
      EditorGUI.PropertyField(position, key, label, true);
      var value = property.FindPropertyRelative("Value");
      var emptyLabel = new GUIContent("");
      EditorGUI.PropertyField(position, value, emptyLabel, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      var key = property.FindPropertyRelative("Key");
      var keyHeight = EditorGUI.GetPropertyHeight(key, true);

      var value = property.FindPropertyRelative("Value");
      var valueHeight = EditorGUI.GetPropertyHeight(value, true);

      return Mathf.Max(keyHeight, valueHeight);
    }
  }
