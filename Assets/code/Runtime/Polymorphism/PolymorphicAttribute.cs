using System;
using UnityEngine;

/// <summary>
/// Add this to a [SerializeReference] field to get a popup in the inspector to allocate an object of the desired type.
/// </summary>
public class PolymorphicAttribute : PropertyAttribute
{
	/// <summary>
	/// Optional: specify an arbitrary baseType for this field, otherwise the field's type will be used.
	/// </summary>
	public readonly Type baseType;

	public PolymorphicAttribute(Type baseType = null)
	{
		this.baseType = baseType;
	}
}
