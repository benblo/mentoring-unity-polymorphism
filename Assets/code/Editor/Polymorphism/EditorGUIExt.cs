using UnityEditor;
using UnityEngine;

public static class EditorGUIExt
{
	public static void GetLabelAndValueRect( Rect totalRect, out Rect labelRect, out Rect valueRect )
	{
		float labelWidth = EditorGUIUtility.labelWidth;
		labelRect = new Rect(totalRect.x, totalRect.y, labelWidth, totalRect.height);
		valueRect = new Rect(totalRect.x + labelWidth, totalRect.y, totalRect.width - labelWidth, totalRect.height);
	}

	/// <summary>
	/// If the property is expanded, draw its children properties (as usual, indented etc).
	/// </summary>
	public static void ChildrenProperties( Rect rect, SerializedProperty property )
	{
		if (!property.isExpanded)
		{
			return;
		}

		int prevIndent = EditorGUI.indentLevel;
		EditorGUI.indentLevel++;

		bool enterChildren = true;
		var endProperty = property.GetEndProperty();
		while (property.NextVisible(enterChildren) &&
		       !SerializedProperty.EqualContents(property, endProperty))
		{
			rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
			rect.height = EditorGUI.GetPropertyHeight(property, null, true);
			EditorGUI.PropertyField(rect, property, null, true);
			enterChildren = false;
		}

		EditorGUI.indentLevel = prevIndent;
	}
}
