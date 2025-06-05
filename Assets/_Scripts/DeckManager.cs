using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; }

    [Header("Deck Settings")]
    [SerializeField] private List<Card> deck = new();

    [Header("Current Deck")]
    [SerializeField] private List<Card> currentDeck = new();
    [SerializeField] private Transform deckModel;
    [SerializeField] private float cardHeight = 0.1f;

    private void UpdateDeck()
    {
        deckModel.position = Vector3.zero;
        float deckCounter = deck.Count;
        float currentDeckCounter = currentDeck.Count;
        deckModel.position = new Vector3(0, -(deckCounter - currentDeckCounter) * cardHeight, 0);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
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
            UpdateDeck();
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
            UpdateDeck();
        }
    }


    public Card DrawCard()
    {
        Card card = currentDeck[^1];
        currentDeck.RemoveAt(currentDeck.Count - 1);
        UpdateDeck();
        return card;
    }

    public Card DrawCard(CardTypes cardType)
    {
        Card card = currentDeck.Find(c => c.cardData.cardType == cardType);
        if (card != null)
        {
            currentDeck.Remove(card);
            UpdateDeck();
        }
        return card;
    }

    public IEnumerator DiscardCard(Card cardComponent)
    {
        float t = 0;
        Vector3 startPos = cardComponent.transform.position;
        cardComponent.isDraggable = false;
        cardComponent.GetComponent<Collider>().enabled = false;
        cardComponent.transform.rotation = Quaternion.Euler(Vector3.zero);
        while (t < 1)
        {
            t += Time.deltaTime;
            Vector3 endPos = Vector3.zero;
            cardComponent.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        AddCard(cardComponent);
        Destroy(cardComponent.gameObject);
    }
}
