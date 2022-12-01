using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Shape
{
}

[Serializable]
public class Circle : Shape
{
	public float radius = 1;
}

[Serializable]
public class Rectangle : Shape
{
	public Vector2 size = Vector2.one;
}

public class TestPolymorphism : MonoBehaviour
{
	[SerializeReference, Polymorphic]
	public Shape shapeNull = null;
	[SerializeReference, Polymorphic]
	public Shape shapePreAllocated = new Circle();
}
