using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; }

    [Header("Deck Settings")]
    [SerializeField] private List<Card> deck = new();

    [Header("Current Deck")]
    [SerializeField] private List<Card> currentDeck = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
        InitializeDeck();
    }

    public void ShuffleDeck()
    {
        SoundManager.Instance.PLaySFXSound(SoundManager.Instance.shuffle);
        for (int i = 0; i < currentDeck.Count; i++)
        {
            int randomIndex = Random.Range(i, currentDeck.Count);
            (currentDeck[i], currentDeck[randomIndex]) = (currentDeck[randomIndex], currentDeck[i]);
        }
    }

    public void InitializeDeck()
    {
        currentDeck.Clear();
        foreach (Card card in deck)
        {
            currentDeck.Add(card);
        }
        ShuffleDeck();
    }

    public void AddCard(Card card)
    {
        Card c = null;

        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i].cardData.cardName == card.cardData.cardName)
            {
                c = deck[i];
                break;
            }
        }

        if (c != null && !currentDeck.Contains(c))
        {
            Debug.Log($"Added: {c}");
            currentDeck.Add(c);
        }
    }


    public Card DrawCard()
    {
        Card card = currentDeck[^1];
        currentDeck.RemoveAt(currentDeck.Count - 1);
        return card;
    }

    public Card DrawCard(CardTypes cardType)
    {
        Card card = currentDeck.Find(c => c.cardData.cardType == cardType);
        if (card != null)
        {
            currentDeck.Remove(card);
        }
        return card;
    }
}
