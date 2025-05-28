using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; }

    [Header("Deck Settings")]
    [SerializeField] private List<Card> deck = new();

    private List<Card> initialDeck = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
        InitializeDeck();
    }

    private void ShuffleDeck()
    {
        for (int i = 0; i < initialDeck.Count; i++)
        {
            int randomIndex = Random.Range(i, initialDeck.Count);
            (initialDeck[i], initialDeck[randomIndex]) = (initialDeck[randomIndex], initialDeck[i]);
        }
    }

    public void InitializeDeck()
    {
        initialDeck.Clear();
        foreach (Card card in deck)
        {
            initialDeck.Add(card);
        }
        ShuffleDeck();
    }


    public Card DrawCard()
    {
        Card card = initialDeck[^1];
        initialDeck.RemoveAt(initialDeck.Count - 1);
        return card;
    }

    public Card DrawCard(CardTypes cardType)
    {
        Card card = initialDeck.Find(c => c.cardData.cardType == cardType);
        if (card != null)
        {
            initialDeck.Remove(card);
        }
        return card;
    }
}
