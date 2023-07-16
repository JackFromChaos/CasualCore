using System;
using System.Collections.Generic;
using System.Globalization;
using GTools;
using UnityEngine;

namespace OEditor
{


    public class EditorForAttribute : Attribute
    {
        public Type type;

        public EditorForAttribute(Type type)
        {
            this.type = type;
        }
    }

    public struct ObjectEditorContextData
    {

    }

    public class ObjectEditorContext
    {
        public ObjectEditorContextData data;

    }

    public class StringEditor : SimpleObjectEditor<string>
    {
        public override string Edit(string value)
        {
            return GUILayout.TextField(value);
        }
    }

    public class FloatEditor : SimpleObjectEditor<float>
    {
        public override float Edit(float value)
        {
            return OnGUILayoutExt.FloatField(value);
        }
    }

    public interface IObjectEditor
    {
        void EditObject(ref object value, string title, ObjectEditorContext context);
    }

    public abstract class SimpleObjectEditor<T> : IObjectEditor
    {
        public abstract T Edit(T value);

        public void EditObject(ref object value, string title, ObjectEditorContext context)
        {
            T data = (T)value;
            data = Edit(data);
            value = data;
        }
    }

    public class ObjectEditorManager
    {
        private static ObjectEditorManager instance;

        public static ObjectEditorManager Get
        {
            get
            {
                if (instance == null)
                {
                    instance = new ObjectEditorManager();
                    instance.Init();
                }

                return instance;
            }
        }

        private void Init()
        {

        }
    }
}

namespace GTools
{
    public class HorizontalLayer : IDisposable
    {
        public HorizontalLayer(params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
        }
        public HorizontalLayer(GUIStyle style, Color color, params GUILayoutOption[] options)
        {

            Color save = GUI.color;
            GUI.color = color;
            GUILayout.BeginHorizontal(style, options);
            GUI.color = save;
        }
        public void Dispose()
        {
            GUILayout.EndHorizontal();
        }
    }
    public class VerticalLayer : IDisposable
    {
        public VerticalLayer(params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
        }

        public VerticalLayer(GUIStyle style, Color color, params GUILayoutOption[] options)
        {
            Color save = GUI.color;
            GUI.color = color;
            GUILayout.BeginVertical(style, options);
            GUI.color = save;
        }
        public void Dispose()
        {
            GUILayout.EndVertical();
        }
    }


    public static class OnGUILayoutExt
    {
        public static int labelWidth = 150;

        public static float FloatField(string label, float value, params GUILayoutOption[] options)
        {
            GUIStyle style = GUI.skin.textField;
#if UNITY_EDITOR
            return UnityEditor.EditorGUILayout.FloatField(label, value, style, options);
#else
			GUILayout.Label(label,GUILayout.Width(labelWidth));
			Rect rect = GetLayoutRect(value.ToString(), style/*, GUILayout.MaxWidth(100)*/);
			return OnGUIExt.FloatField(rect, value, style);
#endif
        }

        public static float FloatField(float value, params GUILayoutOption[] options)
        {
            GUIStyle style = GUI.skin.textField;
#if UNITY_EDITOR
            return UnityEditor.EditorGUILayout.FloatField(value, style, options);
#else
			Rect rect = GetLayoutRect(value.ToString(), style/*, GUILayout.MaxWidth(100)*/);
			return OnGUIExt.FloatField(rect, value, style);
#endif
        }

        static GUIContent cacheTextContent = new GUIContent();

        public static Rect GetLayoutRect(string text, GUIStyle style, params GUILayoutOption[] options)
        {
            cacheTextContent.text = text;
            return GUILayoutUtility.GetRect(cacheTextContent, style, options);
        }

    }

    public static class OnGUIExt
    {
        private static int activeFloatField = -1;
        private static float activeFloatFieldLastValue = 0;
        private static string activeFloatFieldString = "";

        /// <summary>
        /// Forces to parse to float by cleaning string if necessary
        /// </summary>
        public static float ForceParse(string str)
        {
            // try parse
            float value;
            if (float.TryParse(str, out value))
                return value;

            // Clean string if it could not be parsed
            bool recordedDecimalPoint = false;
            List<char> strVal = new List<char>(str);
            for (int cnt = 0; cnt < strVal.Count; cnt++)
            {
                UnicodeCategory type = CharUnicodeInfo.GetUnicodeCategory(str[cnt]);
                if (type != UnicodeCategory.DecimalDigitNumber)
                {
                    strVal.RemoveRange(cnt, strVal.Count - cnt);
                    break;
                }
                else if (str[cnt] == '.')
                {
                    if (recordedDecimalPoint)
                    {
                        strVal.RemoveRange(cnt, strVal.Count - cnt);
                        break;
                    }

                    recordedDecimalPoint = true;
                }
            }

            // Parse again
            if (strVal.Count == 0)
                return 0;
            str = new string(strVal.ToArray());
            if (!float.TryParse(str, out value))
                Debug.LogError("Could not parse " + str);
            return value;
        }

        public static float FloatField(Rect pos, float value, GUIStyle style, params GUILayoutOption[] options)
        {

#if UNITY_EDITOR
            return UnityEditor.EditorGUI.FloatField(pos, value, style);
#else
			int floatFieldID = GUIUtility.GetControlID("FloatField".GetHashCode(), FocusType.Keyboard, pos) + 1;
			if (floatFieldID == 0)
				return value;

			bool recorded = activeFloatField == floatFieldID;
			bool active = floatFieldID == GUIUtility.keyboardControl;

			if (active && recorded && activeFloatFieldLastValue != value)
			{ // Value has been modified externally
				activeFloatFieldLastValue = value;
				activeFloatFieldString = value.ToString();
			}

			// Get stored string for the text field if this one is recorded
			string str = recorded ? activeFloatFieldString : value.ToString();

			string strValue = GUI.TextField(pos, str,style);
			if (recorded)
				activeFloatFieldString = strValue;

			// Try Parse if value got changed. If the string could not be parsed, ignore it and keep last value
			bool parsed = true;
			if (strValue == "")
				value = activeFloatFieldLastValue = 0;
			else if (strValue != value.ToString())
			{
				float newValue;
				parsed = float.TryParse(strValue, out newValue);
				if (parsed)
					value = activeFloatFieldLastValue = newValue;
			}

			if (active && !recorded)
			{ // Gained focus this frame
				activeFloatField = floatFieldID;
				activeFloatFieldString = strValue;
				activeFloatFieldLastValue = value;
			}
			else if (!active && recorded)
			{ // Lost focus this frame
				activeFloatField = -1;
				if (!parsed)
					value = ForceParse(strValue);
			}

			return value;
#endif

        }



    }
}