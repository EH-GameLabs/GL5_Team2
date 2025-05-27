using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Stats")]
    [SerializeField] private int enemyLife;
    public int EnemyLife { get { return enemyLife; } set { enemyLife = value; } }
    [SerializeField] private int playerLife;
    public int PlayerLife { get { return playerLife; } set { playerLife = value; } }

    [Header("Hand Settings")]
    public int maxCardsInHand;
    public List<Transform> handPositions = new List<Transform>();

    [Header("Debug Variables")]
    [SerializeField] private bool isGameActive;
    [SerializeField] private int currentTurn;
    [SerializeField] private int cardsInHand;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    public void DrawCard(CardTypes value, int key)
    {
        if (cardsInHand >= maxCardsInHand) return;

        // Logic to draw a card based on the CardTypes enum
        // ...
    }

    internal void DrawRandomCards()
    {
        Card currentCard = null;
        for (int i = 0; i < maxCardsInHand; i++)
        {
            currentCard = Instantiate(DeckManager.Instance.DrawCard(), Vector3.zero, Quaternion.identity);

            StartCoroutine(CardAnimation(currentCard.gameObject));
        }
    }

    private IEnumerator CardAnimation(GameObject card)
    {
        int i = 0;
        while (i < handPositions.Count)
        {
            if (handPositions[i].GetComponentInChildren<Card>() == null)
            {
                break;
            }
            i++;
        }
        Transform position = handPositions[i];
        card.transform.SetParent(position);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 2;
            card.transform.position = Vector3.Lerp(card.transform.position, position.position, t);
            card.transform.rotation = Quaternion.Lerp(card.transform.rotation, position.rotation, t);
            yield return null;
        }
        card.transform.position = position.position;
        card.GetComponent<Card>().SetStartPos(position.position);
    }

    public bool IsGameActive { get { return isGameActive; } }
    public void SetGameStatus(bool status) { isGameActive = status; }

    public void DiscardHand()
    {
        foreach (var hand in handPositions)
        {
            if (hand.childCount > 0)
            {
                Card card = hand.GetComponentInChildren<Card>();
                if (card != null)
                {
                    StartCoroutine(DiscardRoutine(card.gameObject));
                }
            }
        }
    }

    public IEnumerator DiscardRoutine(GameObject card)
    {
        float t = 0;

        while (t < 1)
        {
            if (card == null)
            {
                Debug.LogWarning("Card is null, cannot discard.");
                yield break;
            }
            t += Time.deltaTime;
            card.transform.position = Vector3.Lerp(card.transform.position, Vector3.zero, t);
            yield return null;
        }
        card.transform.position = Vector3.zero;

        Destroy(card);
    }
}
