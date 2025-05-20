using System;
using System.Threading.Tasks;
using UnityEngine;

public class Turn
{
    public enum TurnState
    {
        Begin,
        Activation,
        End,
    }

    public TurnState turnState;
    public TurnType turnType;

    public Turn(TurnType turnType)
    {
        turnState = TurnState.Begin;
        this.turnType = turnType;
    }

    public void BeginTurn()
    {
        turnState = TurnState.Begin;

        // Logica per iniziare il turno
        Debug.Log("Inizio turno: " + turnType);
    }

    public void ActivationTurn()
    {
        turnState = TurnState.Activation;

        // Logica di attivazione di effetti delle carte
        Debug.Log("Attivazione carte: " + turnType);

        // Una volta finite le attivazioni possibili delle carte si passa alla fine del turno
        ExecuteAfterDelay();
    }

    public async void ExecuteAfterDelay()
    {
        Debug.Log("Attendo 2 secondi...");
        await Task.Delay(2000);
        Debug.Log("-> End Turn!");
        EndTurn();
    }


    public void EndTurn()
    {
        turnState = TurnState.End;

        // Logica per terminare il turno
        Debug.Log("Fine turno: " + turnType);

        // Passa il turno
        TurnManager.Instance.ChangeTurn();
    }
}
