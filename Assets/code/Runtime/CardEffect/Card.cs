using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Card //: MonoBehaviour
{
	public string label;

	public int preCardGain;

	public int lifeGain;

	[SerializeReference, Polymorphic]
	public CopyCardEffect copyCardEffect = null;

	public int postCardGain;

	[SerializeReference, Polymorphic]
	public BaseEffect postAttackEffect;

	public int dummy;
}

[Serializable]
public abstract class BaseEffect
{
}

[Serializable]
public class CopyCardEffect : BaseEffect
{
	//public Card cardToCopy;
	public enum PowerSource
	{
		Weakest,
		Strongest,
	}
	public PowerSource powerSource;
}

[Serializable]
public class DrawCardEffect : BaseEffect
{
	public int cardCount = 1;
}
