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
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        currentTurn = new Turn(turnType);

        // Il giocatore sceglie una Maschera, che determina bonus e malus iniziali.

        currentTurn.BeginTurn();
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

    public void ChangeTurn()
    {
        if (currentTurn.turnType == TurnType.Player)
        {
            currentTurn = new Turn(TurnType.Enemy);
            collectorMask = null;
        }
        else
        {
            RMAlteration = -altereted;
            currentTurn = new Turn(TurnType.Player);
            altereted = 0;
        }

        StartCoroutine(WaitToStart());
    }

    private IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(1f);
        currentTurn.BeginTurn();
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
    }
}

public enum TurnType
{
    Player,
    Enemy,
}