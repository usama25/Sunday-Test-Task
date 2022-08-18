using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
public class ButtonDrawer : PropertyDrawer
{

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		string methodName = (attribute as InspectorButtonAttribute)?.MethodName;
		Object target = property.serializedObject.targetObject;
		System.Type type = target.GetType();
		if (methodName != null)
		{
			System.Reflection.MethodInfo method = type.GetMethod(methodName);
			if (method == null)
			{
				GUI.Label(position, "Method could not be found. Is it public?");
				return;
			}

			if (method.GetParameters().Length > 0)
			{
				GUI.Label(position, "Method cannot have parameters.");
				return;
			}

			if (GUI.Button(position, method.Name))
			{
				method.Invoke(target, null);
			}
		}
	}

}
#endif

public class InspectorButtonAttribute : PropertyAttribute
{

	public string MethodName { get; }

	public InspectorButtonAttribute(string methodName)
	{
		MethodName = methodName;
	}

}