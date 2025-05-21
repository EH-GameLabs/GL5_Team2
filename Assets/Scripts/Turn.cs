using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Turn
{
    public enum TurnState
    {
        Begin,
        MainPahse,
        ActivationPhase,
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

        // Il giocatore pesca 5 carte.
        // Ricarica le 3 Risorse Mentali(RM) all’inizio di ogni turno.

        MainPhase();
    }

    public void MainPhase()
    {
        turnState = TurnState.MainPahse;
        Debug.Log("MainPhase: " + turnType);

        // Il Giocatore può giocare massimo 4 carte della sua mano.
        // Può mettere le carte nell’ordine che preferisce.
        // Il Giocatore clicca un determinato oggetto/ pulsante per terminare il turno.

    }

    public void ActivationPhase()
    {
        turnState = TurnState.ActivationPhase;
        // Logica di attivazione di effetti delle carte
        Debug.Log("Attivazione carte: " + turnType);

        //await Task.Delay(2000);
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
