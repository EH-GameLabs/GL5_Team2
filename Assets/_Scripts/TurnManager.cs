using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // SINGLETON
    public static TurnManager Instance { get; private set; }

    public TurnType turnType;
    public Turn currentTurn;
    public List<GameObject> cardSlot = new List<GameObject>();

    public int RMAlteration = 0;
    public int altereted = 0;
    private CollectorMask collectorMask;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
        currentTurn = new Turn();
    }

    public void StartGame()
    {
        Time.timeScale = 1.0f;
        DeckManager.Instance.InitializeDeck();

        // Il giocatore sceglie una Maschera, che determina bonus e malus iniziali.

        currentTurn.BeginTurn();


        SoundManager.Instance.PlayMusic(SoundManager.Instance.backgroundMusic);
    }

    public void ActivateCardsEffects()
    {
        if (currentTurn.turnState != Turn.TurnState.MainPahse)
        {
            Debug.LogWarning("Non puoi attivare gli effetti delle carte in questo momento.");
            return;
        }

        //_ = currentTurn.ActivationPhase(cardSlot);
        StartCoroutine(currentTurn.ActivationPhase(cardSlot));
    }

    public void NextTurn()
    {
        FindAnyObjectByType<HudUI>(FindObjectsInactive.Include).HideRMAlteration();

        currentTurn = new Turn();
        collectorMask = null;
        RMAlteration = -altereted;
        altereted = 0;

        StartCoroutine(WaitToStart());
    }

    private IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(1f);
        currentTurn.BeginTurn();
        if (RMAlteration != 0)
        {
            FindAnyObjectByType<HudUI>(FindObjectsInactive.Include).ShowRMAlteration(RMAlteration);
        }
    }

    internal void SetNextMask(CollectorMask collectorMask)
    {
        this.collectorMask = collectorMask;
    }

    public void SetHealed()
    {
        currentTurn.healed = true;
    }
    public void SetHealCheck(int dmg)
    {
        currentTurn.haveToCheckHealed = true;
        currentTurn.damageOnHealed += dmg;
    }

    public void SetCardsCheck()
    {
        currentTurn.haveToCheckCards = true;
        currentTurn.healOnCheckCard++;
    }

    public bool CanEndTurn()
    {
        if (currentTurn.turnState == Turn.TurnState.MainPahse)
        {
            // Il Giocatore può terminare il turno solo se ha giocato almeno una carta.
            foreach (GameObject slot in cardSlot)
            {
                Card card = slot.GetComponentInChildren<Card>();
                if (card != null && card.isPlaced)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

public enum TurnType
{
    Player,
    Enemy,
}