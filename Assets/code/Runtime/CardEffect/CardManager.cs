using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	public Card[] deck;

	public int life;
	List<Card> cards = new List<Card>();

	public IEnumerator PlayCard(Card card)
	{
		if (card.preCardGain != 0)
		{
			DrawCard();
			yield break;
		}

		if (card.lifeGain != 0)
		{
			life += card.lifeGain;
			yield break;
		}

		Consume(card.copyCardEffect);

		if (card.postCardGain != 0)
		{
			DrawCard();
		}
	}

	void DrawCard()
	{
		cards.Add(null);
	}

	void Copy(Card original)
	{
	}

	void Consume(CopyCardEffect effect)
	{
		//if (effect.cardToCopy == null)
		//	return;

		//var cardToCopy = effect.FindCard(this);
	}
}
