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

        _ = currentTurn.ActivationPhase(cardSlot);
    }

    public void ChangeTurn()
    {
        if (currentTurn.turnType == TurnType.Player)
        {
            currentTurn = new Turn(TurnType.Enemy);
        }
        else
        {
            currentTurn = new Turn(TurnType.Player);
        }

        StartCoroutine(WaitToStart());
    }

    private IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(1f);
        currentTurn.BeginTurn();
    }
}

public enum TurnType
{
    Player,
    Enemy,
}