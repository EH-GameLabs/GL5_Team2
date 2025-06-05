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

    public bool collectorCanActivateEffect = true;
    //public int RMAlteration = 0; // Risorse Mentali (RM) Alteration
    public bool healed = false;
    public bool haveToCheckHealed = false;
    public int damageOnHealed = 0;
    public bool haveToCheckCards = false;
    public int healOnCheckCard = 0;

    private delegate void CardEffect();
    CardEffect previousCardEffect;

    private int previousCardLifeTime = 0;

    public Turn()
    {
        turnState = TurnState.Begin;
    }

    public void BeginTurn()
    {
        turnState = TurnState.Begin;

        // Logica per iniziare il turno
        //Debug.Log("Inizio turno: " + turnType);


        // Il Giocatore pesca 5 carte.
        // Ricarica le 3 Risorse Mentali(RM) all’inizio di ogni turno.

        GameManager.Instance.DrawRandomCards();
        GameManager.Instance.ResetRM();
        Collector.Instance.SetMask();

        //Debug.Log($"Turno Iniziato. RM Alteration: {RMAlteration}");
        //if (RMAlteration != 0)
        //{
        //    GameObject.FindAnyObjectByType<HudUI>(FindObjectsInactive.Include).ShowRMAlteration(RMAlteration);
        //}

        MainPhase();
    }

    public void MainPhase()
    {
        turnState = TurnState.MainPahse;
        //Debug.Log("MainPhase: " + turnType);

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

        foreach (GameObject card in cardSlot)
        {
            Card cardComponent = card.GetComponentInChildren<Card>();

            if (cardComponent != null)
            {
                Debug.Log("Card: " + cardComponent.name);
                cardComponent.lifetime--;
                float t = 0;
                Vector3 startPos = cardComponent.transform.position;
                while (t < 1)
                {
                    t += Time.deltaTime * 4;
                    cardComponent.transform.position = Vector3.Lerp(startPos, startPos + new Vector3(0, 0.5f, 0), t);
                    yield return null;
                }

                Debug.Log("Attivazione effetto carta: "/* + cardComponent.cardData.cardName*/);
                foreach (SO_Effect effect in cardComponent.cardData.effects)
                {
                    if (effect is E_Duplicate)
                    {
                        previousCardEffect?.Invoke();
                        cardComponent.lifetime = previousCardLifeTime;
                        break;
                    }
                    effect.Effect();
                    previousCardEffect = effect.Effect;
                    previousCardLifeTime = cardComponent.cardData.lifeTime;
                    if (cardComponent.cardData.cardType == CardTypes.Doloroso && effect is E_DoDamage)
                    {
                        CM_Accusatore accusatore = GameObject.FindAnyObjectByType<CM_Accusatore>();
                        E_DoDamage e_DoDamage = effect as E_DoDamage;
                        accusatore.damageAmount = e_DoDamage.damageAmount;
                        accusatore.AddCard();
                    }
                }

                if (collectorCanActivateEffect)
                {
                    Collector.Instance.ActivateMaskEffect(cardComponent);
                }

                collectorCanActivateEffect = true;


                yield return new WaitForSeconds(1f);

                t = 0;
                startPos = cardComponent.transform.position;
                while (t < 1)
                {
                    t += Time.deltaTime * 4;
                    Vector3 endPos = cardComponent.lifetime < 1 ? Vector3.zero : startPos + new Vector3(0, -0.5f, 0);
                    cardComponent.transform.position = Vector3.Lerp(startPos, endPos, t);
                    yield return null;
                }

                if (cardComponent.lifetime < 1)
                {
                    DeckManager.Instance.AddCard(cardComponent);
                    GameObject.Destroy(cardComponent.gameObject);
                }
            }
            else
            {
                Debug.LogWarning("Nessun componente Card trovato su: " + card.name);
            }
        }

        if (haveToCheckCards && GameManager.Instance.GetNCards() == 2)
        {
            if (GameManager.Instance.PlayerLife < GameManager.Instance.playerMaxLife)
            {
                healed = true;
                GameManager.Instance.PlayerLife += healOnCheckCard;
                if (collectorCanActivateEffect)
                {
                    Collector.Instance.ActivateMaskEffect();
                }
            }
        }

        if (haveToCheckHealed && healed)
        {
            GameManager.Instance.EnemyLife -= damageOnHealed;
        }

        DeckManager.Instance.ShuffleDeck();

        Debug.Log("-> End Turn!");
        EndTurn();
    }



    public void EndTurn()
    {
        turnState = TurnState.End;

        Debug.Log("Turno Terminato.");
        GameObject.FindAnyObjectByType<HudUI>(FindObjectsInactive.Include).HideRMAlteration();


        // Passa il turno
        TurnManager.Instance.NextTurn();
    }
}
