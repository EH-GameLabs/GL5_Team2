using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Enemy Stats")]
    [SerializeField] private int enemyMaxLife;
    [SerializeField] private int enemyLife;
    public int EnemyLife 
    { 
        get { return enemyLife; }
        set 
        { 
            if (value < enemyLife) SoundManager.Instance.PLaySFXSound(SoundManager.Instance.broTakeDamage);
            enemyLife = value > enemyMaxLife ? enemyMaxLife : value;
            hudUI.UpdateEnemyHealth(enemyLife, enemyMaxLife); 
        } 
    }
    [Header("Player Stats")]
    public int playerMaxLife;
    [SerializeField] private int playerLife;
    public int PlayerLife 
    { 
        get { return playerLife; } 
        set 
        { 
            if (value < playerLife) SoundManager.Instance.PLaySFXSound(SoundManager.Instance.playerTakeDamage);
            playerLife = value > playerMaxLife ? playerMaxLife : value;
            hudUI.UpdatePlayerHealth(playerLife, playerMaxLife); 
        } 
    }
    [SerializeField] private int maxPlayerRM = 3;

    [Header("Hand Settings")]
    public int maxCardsInHand;
    public List<Transform> handPositions = new List<Transform>();

    private HudUI hudUI;

    [Header("Debug Variables")]
    [SerializeField] private bool isGameActive;
    [SerializeField] private int currentTurn;
    [SerializeField] private int cardsInHand;
    [SerializeField] private int currentRM;
    public int CurrentRM
    {
        get { return currentRM; }
        set
        {
            currentRM = value;
            hudUI.UpdateRM(currentRM);
        }
    }


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;

        hudUI = FindAnyObjectByType<HudUI>(FindObjectsInactive.Include);

        EnemyLife = enemyMaxLife;
        PlayerLife = playerMaxLife;
    }

    public void DrawCard(CardTypes value, int key)
    {
        if (cardsInHand >= maxCardsInHand) return;

        // Logic to draw a card based on the CardTypes enum
        // ...
    }

    public void ResetRM() { CurrentRM = maxPlayerRM; }

    internal void DrawRandomCards()
    {
        Card currentCard = null;
        for (int i = 0; i < maxCardsInHand; i++)
        {
            if (handPositions[i].GetComponentInChildren<Card>() != null)
            {
                continue;
            }

            currentCard = Instantiate(DeckManager.Instance.DrawCard(), Vector3.zero, Quaternion.identity);

            StartCoroutine(CardAnimation(currentCard.gameObject, i));
        }
    }

    private IEnumerator CardAnimation(GameObject card, int index)
    {
        card.GetComponent<Card>().isDraggable = false;

        Transform position = handPositions[index];
        card.transform.SetParent(position);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 1;
            card.transform.position = Vector3.Lerp(card.transform.position, position.position, t);
            card.transform.rotation = Quaternion.Lerp(card.transform.rotation, position.rotation, t);
            yield return null;
        }
        card.transform.position = position.position;
        card.GetComponent<Card>().SetStartPos(position.position);
        card.GetComponent<Card>().isDraggable = true;
    }

    public bool IsGameActive { get { return isGameActive; } }
    public void SetGameStatus(bool status) { isGameActive = status; }

    //public void DiscardHand()
    //{
    //    foreach (var hand in handPositions)
    //    {
    //        if (hand.childCount > 0)
    //        {
    //            Card card = hand.GetComponentInChildren<Card>();
    //            if (card != null)
    //            {
    //                StartCoroutine(DiscardRoutine(card.gameObject));
    //            }
    //        }
    //    }
    //}

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

    public bool CanPlayCard(Card card)
    {
        if (card == null) return false;
        if (CurrentRM < card.cardData.RMCost + TurnManager.Instance.RMAlteration) return false;
        return true;
    }

    public int GetNCards()
    {
        int count = 0;
        foreach (var card in handPositions)
        {
            if (card.GetComponentInChildren<Card>() != null) count++;
        }
        return count;
    }
}
