using System;
using System.Collections;
using System.Collections.Generic;
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

    public bool collectorCanActivateEffect = true;

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

        if (turnType == TurnType.Player)
        {
            // Il Giocatore pesca 5 carte.
            // Ricarica le 3 Risorse Mentali(RM) all’inizio di ogni turno.
            Debug.Log("Pesca carte e ricarica RM");
            GameManager.Instance.DrawRandomCards();
            MainPhase();
        }
        else
        {
            // L'Avversario pesca 5 carte.
            Debug.Log("TODO: DA CAMBIARE");
            EndTurn();
        }
    }

    public void MainPhase()
    {
        turnState = TurnState.MainPahse;
        Debug.Log("MainPhase: " + turnType);

        // Il Giocatore può giocare massimo 4 carte della sua mano.
        // Può mettere le carte nell’ordine che preferisce.
        // Il Giocatore clicca un determinato oggetto/ pulsante per terminare il turno.

    }

    //public async Task ActivationPhase(List<GameObject> cardSlot)
    //{
    //    turnState = TurnState.ActivationPhase;
    //    // Logica di attivazione di effetti delle carte
    //    Debug.Log("Attivazione carte: " + turnType);

    //    // Esegui gli effetti delle carte
    //    foreach (GameObject card in cardSlot)
    //    {
    //        // Esegui l'effetto della carta
    //        Card cardComponent = card.GetComponentInChildren<Card>();
    //        if (cardComponent != null)
    //        {
    //            // Esegui l'effetto della carta
    //            Debug.Log("Attivazione effetto carta: "/* + cardComponent.cardData.cardName*/);
    //            // cardComponent.ActivateEffect();
    //            await Task.Delay(1000);
    //            GameObject.Destroy(cardComponent.gameObject);
    //            await Task.Delay(1000);
    //        }
    //        else
    //        {
    //            Debug.LogWarning("Nessun componente Card trovato su: " + card.name);
    //        }
    //    }

    //    //await Task.Delay(2000);
    //    Debug.Log("-> End Turn!");
    //    EndTurn();
    //}

    public IEnumerator ActivationPhase(List<GameObject> cardSlot)
    {
        turnState = TurnState.ActivationPhase;
        Debug.Log("Attivazione carte: " + turnType);

        foreach (GameObject card in cardSlot)
        {
            Card cardComponent = card.GetComponentInChildren<Card>();
            if (cardComponent != null)
            {

                float t = 0;
                Vector3 startPos = cardComponent.transform.position;
                while (t < 1)
                {
                    t += Time.deltaTime * 4;
                    cardComponent.transform.position = Vector3.Lerp(startPos, startPos + new Vector3(0, 0.5f, 0), t);
                    yield return null;
                }

                Debug.Log("Attivazione effetto carta: "/* + cardComponent.cardData.cardName*/);
                //foreach (SO_Effect effect in cardComponent.cardData.effects)
                //{
                //    effect.Effect();
                //}

                //if (collectorCanActivateEffect /*&& ha senso attivarla*/)
                //{
                //    Collector.Instance.ActivateMaskEffect();
                //}

                //collectorCanActivateEffect = true;


                yield return new WaitForSeconds(1f);

                t = 0;
                startPos = cardComponent.transform.position;
                while (t < 1)
                {
                    t += Time.deltaTime * 4;
                    cardComponent.transform.position = Vector3.Lerp(startPos, Vector3.zero, t);
                    yield return null;
                }

                GameObject.Destroy(cardComponent.gameObject);
            }
            else
            {
                Debug.LogWarning("Nessun componente Card trovato su: " + card.name);
            }
        }

        Debug.Log("-> End Turn!");
        EndTurn();
    }



    public void EndTurn()
    {
        turnState = TurnState.End;

        if (turnType == TurnType.Player)
        {
            Debug.Log("Il Giocatore termina il turno.");
            GameManager.Instance.DiscardHand();
            DeckManager.Instance.InitializeDeck();
        }
        else
        {
            Debug.Log("L'Avversario termina il turno.");
        }


        // Passa il turno
        TurnManager.Instance.ChangeTurn();
    }
}
