using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// On a [SerializeReference] field, draw a popup to allocate an object of the desired type.
/// </summary>
[CustomPropertyDrawer(typeof(PolymorphicAttribute))]
public class PolymorphicDrawer : PropertyDrawer
{
	new PolymorphicAttribute attribute => (PolymorphicAttribute)base.attribute;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property, label);

	public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
	{
		rect.height = EditorGUIUtility.singleLineHeight;
		EditorGUIExt.GetLabelAndValueRect(rect, out var labelRect, out var valueRect);

		// left side: draw the main property label/foldout
		EditorGUI.PropertyField(labelRect, property, label, false);

		// right side: draw a popup that allows to allocate an object of the desired type
		DerivedTypePopup(valueRect, property, attribute.baseType ?? fieldInfo.FieldType);

		// draw the object's children properties (if the object is non-null)
		EditorGUIExt.ChildrenProperties(rect, property);
	}

	/// <summary>
	/// Find (and cache) the derived types of a given base type.
	/// </summary>
	class TypeCache
	{
		public readonly Type[] types;
		public readonly string[] typeNames;

		TypeCache(Type baseType)
		{
			// look for derived types of baseType
			// TODO: look through all loaded assemblies, Unity has helpers for that somewhere
			types = baseType.Assembly.GetTypes()
				.Where(t => t.BaseType == baseType && !t.IsAbstract)
				.ToArray();

			// add the base type itself if non-abstract (could be optional?)
			if (!baseType.IsAbstract)
				ArrayUtility.Insert(ref types, 0, baseType);

			typeNames = Array.ConvertAll(types, t => t.Name);

			// add null entry, to nullify the field (could be optional?)
			ArrayUtility.Insert(ref types, 0, null);
			ArrayUtility.Insert(ref typeNames, 0, "<null>");
		}

		static readonly Dictionary<Type, TypeCache> baseTypeToDerivedTypes = new Dictionary<Type, TypeCache>();
		public static TypeCache Get(Type baseType)
		{
			if (!baseTypeToDerivedTypes.TryGetValue(baseType, out var cache))
			{
				cache = new TypeCache(baseType);
				baseTypeToDerivedTypes.Add(baseType, cache);
			}
			return cache;
		}
	}

	static void DerivedTypePopup(Rect rect, SerializedProperty property, Type baseType)
	{
		var cache = TypeCache.Get(baseType);

		var value = property.managedReferenceValue;
		var index = Array.IndexOf(cache.types, value?.GetType());

		EditorGUI.BeginChangeCheck();
		index = EditorGUI.Popup(rect, index, cache.typeNames);
		if (EditorGUI.EndChangeCheck())
		{
			if (index <= 0)
			{
				property.managedReferenceValue = null;
			}
			else
			{
				var newType = cache.types[index];
				var newValue = Activator.CreateInstance(newType);
				property.managedReferenceValue = newValue;
			}
		}
	}
}
