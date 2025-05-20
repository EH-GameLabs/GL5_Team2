using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // SINGLETON
    public static TurnManager Instance { get; private set; }

    public TurnType turnType;
    public Turn currentTurn;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    public void StartGame()
    {
        currentTurn = new Turn(turnType);
        currentTurn.BeginTurn();
    }

    public void ActivateCardsEffects()
    {
        currentTurn.ActivationTurn();
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