using System.Linq;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public static Collector Instance { get; private set; }

    [Header("Maschere Disponibili")]
    [SerializeField] private CollectorMask[] masks;            // Suppongo che masks[0] sia la maschera di default
    [SerializeField] private CollectorMask currentMask;
    [SerializeField] private int maxMaskTurnCount = 3;         // Massimo turni per una maschera

    [Header("Maschere")]
    [SerializeField] private GameObject martireMask;
    [SerializeField] private GameObject accusatoreMask;
    [SerializeField] private GameObject tentatoreMask;

    public CollectorMask CurrentMask
    {
        get => currentMask;
        private set
        {
            if (value != null && currentMask != value)
            {
                currentMask = value;
                currentMaskTurnCount = 0; // Resetto il contatore quando cambio maschera

                // Aggiorna la UI
                martireMask.SetActive(value is CM_Martire);
                accusatoreMask.SetActive(value is CM_Accusatore);
                tentatoreMask.SetActive(value is CM_Tentatore);
            }
            else if (value != null && currentMask == value)
            {
                // Se è la stessa maschera, non resettare il contatore qui: 
                // lo incrementiamo in SetMask() dopo aver confermato la scelta.
            }
        }
    }

    private CM_Martire martire;
    private CM_Accusatore accusatore;
    private CM_Tentatore tentatore;

    // Conteggio di quanti turni la CurrentMask è rimasta attiva
    private int currentMaskTurnCount = 0;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;

        martire = GetComponent<CM_Martire>();
        accusatore = GetComponent<CM_Accusatore>();
        tentatore = GetComponent<CM_Tentatore>();
    }


    /// <summary>
    /// Chiamato ogni volta che si gioca una carta (o comunque nel momento in cui si verifica l'effetto della maschera).
    /// </summary>
    public void ActivateMaskEffect(Card card)
    {
        if (CurrentMask is CM_Accusatore && card.cardData.cardType == CardTypes.Doloroso)
        {
            CurrentMask.ActivateMaskEffect();
        }
        else if (CurrentMask is CM_Tentatore && HasDrawEffect(card))
        {
            CurrentMask.ActivateMaskEffect();
        }
        else if (CurrentMask is CM_Martire && HasHealEffect(card))
        {
            CurrentMask.ActivateMaskEffect();
        }
    }

    private bool HasHealEffect(Card card)
    {
        foreach (var effect in card.cardData.effects)
        {
            if (effect is E_Foreach_Do)
            {
                E_Foreach_Do doEffect = (E_Foreach_Do)effect;
                if (doEffect.GetEffectToActivate() is E_GainLife)
                {
                    return true;
                }
            }
            if (effect is E_GainLife)
                return true;
        }
        return false;
    }

    private bool HasDrawEffect(Card card)
    {
        foreach (var effect in card.cardData.effects)
        {
            if (effect is E_Foreach_Do)
            {
                E_Foreach_Do doEffect = (E_Foreach_Do)effect;
                if (doEffect.GetEffectToActivate() is E_DrawCards)
                {
                    return true;
                }
            }
            if (effect is E_DrawCards)
                return true;
        }
        return false;
    }


    /// <summary>
    /// Questo metodo va chiamato all’inizio di ogni turno (o nel punto in cui desideri verificare/​cambiare maschera).
    /// </summary>
    public void SetMask()
    {
        if (currentMask == null)
        {
            CurrentMask = accusatore;
            return;
        }

        // 1) Controllo se devo forzare il cambio di maschera perché ho superato maxMaskTurnCount
        if (currentMask != null && currentMaskTurnCount >= maxMaskTurnCount)
        {
            Debug.Log($"primo controllo: {currentMask != null}");
            Debug.Log($"Secondo controllo: {currentMaskTurnCount} >= {maxMaskTurnCount}? {currentMaskTurnCount >= maxMaskTurnCount}");
            ForceRotateMask();
            return;
        }

        // 3) Resetto i contatori interni di ciascuna maschera
        accusatore.ResetCard();
        tentatore.ResetCard();
        martire.ResetCard();

        // 4) Conto tutte le carte in scena e popolo i contatori interni
        Card[] cards = FindObjectsByType<Card>(FindObjectsSortMode.None);
        //Debug.Log($"Trovate {cards.Length} carte in scena per il conteggio delle maschere.");
        foreach (Card card in cards)
        {
            if (card.cardData.cardType == CardTypes.Doloroso)
            {
                //Debug.Log($"Carta Dolorosa trovata: {card.cardData.cardName}");
                accusatore.AddCard();
            }
            if (HasDrawEffect(card))
            {
                //Debug.Log($"Carta con effetto di pesca trovata: {card.cardData.cardName}");
                tentatore.AddCard();
            }
            if (HasHealEffect(card))
            {
                //Debug.Log($"Carta con effetto di cura trovata: {card.cardData.cardName}");
                martire.AddCard();
            }
        }

        // 5) Ricavo quante carte ha ciascuna maschera
        int countMartire = martire.GetCards();
        int countAccusatore = accusatore.GetCards();
        int countTentatore = tentatore.GetCards();

        // 6) La maschera Martire non è mai eleggibile se il giocatore ha la vita al massimo
        bool playerAtMaxLife = GameManager.Instance.PlayerLife == GameManager.Instance.playerMaxLife;
        bool canMartire = !playerAtMaxLife && countMartire >= 2;
        bool canAccusatore = countAccusatore >= 2;
        bool canTentatore = countTentatore >= 2;

        // 7) Se nessuna maschera può attivarsi (soglia di 2 carte), rimango con quella corrente
        if (!canMartire && !canAccusatore && !canTentatore)
        {
            currentMaskTurnCount++;
            return;
        }

        // 8) Ottengo il numero massimo di carte tra le maschere eleggibili
        int maxCount = Mathf.Max(
            canMartire ? countMartire : 0,
            canAccusatore ? countAccusatore : 0,
            canTentatore ? countTentatore : 0
        );

        // 9) Scelgo la maschera più forte con regola di priorità:
        //     - Se Martire eleggibile e countMartire == maxCount, scelgo Martire.
        //     - Altrimenti, se Accusatore eleggibile e countAccusatore == maxCount, scelgo Accusatore.
        //     - Altrimenti, scegli Tentatore.
        bool hasChangedMask = false;

        if (canMartire && countMartire == maxCount)
        {
            if (CurrentMask != martire)
            {
                CurrentMask = martire;
                hasChangedMask = true;
            }
            else
                currentMaskTurnCount++;
        }
        else if (canAccusatore && countAccusatore == maxCount)
        {
            if (CurrentMask != accusatore)
            {
                CurrentMask = accusatore;
                hasChangedMask = true;
            }
            else
                currentMaskTurnCount++;
        }
        else // canTentatore && countTentatore == maxCount
        {
            if (CurrentMask != tentatore)
            {
                CurrentMask = tentatore;
                hasChangedMask = true;
            }
            else
                currentMaskTurnCount++;
        }

        if (hasChangedMask)
        {
            SoundManager.Instance.PLaySFXSound(SoundManager.Instance.changeMask);
        }
    }


    /// <summary>
    /// Forza un cambio di CurrentMask quando il contatore di turni è scaduto, 
    /// scegliendo la maschera con priorità indipendentemente da currentMaskTurnCount.
    /// Si basa sulle stesse regole di priorità usate in SetMask().
    /// </summary>
    private void ForceRotateMask()
    {
        // 1) Resetto il contatore in modo che la nuova maschera ricominci da 0
        currentMaskTurnCount = 0;

        // 2) Resetto i contatori interni per valutare da zero le percentuali delle maschere
        accusatore.ResetCard();
        tentatore.ResetCard();
        martire.ResetCard();

        // 3) Riconto tutte le carte
        Card[] cards = FindObjectsByType<Card>(FindObjectsSortMode.None);
        foreach (Card card in cards)
        {
            if (card.cardData.cardType == CardTypes.Doloroso)
            {
                accusatore.AddCard();
            }
            if (HasDrawEffect(card))
            {
                tentatore.AddCard();
            }
            if (HasHealEffect(card))
            {
                martire.AddCard();
            }
        }

        // 4) Conteggio delle carte per maschera
        int countMartire = martire.GetCards();
        int countAccusatore = accusatore.GetCards();
        int countTentatore = tentatore.GetCards();

        // 5) La maschera Martire non è eleggibile se il player è a vita piena
        bool playerAtMaxLife = GameManager.Instance.PlayerLife == GameManager.Instance.playerMaxLife;
        bool canMartire = !playerAtMaxLife && countMartire >= 2;
        bool canAccusatore = countAccusatore >= 2;
        bool canTentatore = countTentatore >= 2;

        // 6) Se nessuna è eleggibile (>= 2 carte), assegno la maschera di default (masks[0])
        if (!canMartire && !canAccusatore && !canTentatore)
        {
            CurrentMask = masks[0];
            return;
        }

        // 7) Scelgo la maschera con maggior numero di carte e in caso di parità applico la priorità:
        int maxCount = Mathf.Max(
            canMartire ? countMartire : 0,
            canAccusatore ? countAccusatore : 0,
            canTentatore ? countTentatore : 0
        );

        if (canMartire && countMartire == maxCount)
        {
            CurrentMask = martire;
        }
        else if (canAccusatore && countAccusatore == maxCount)
        {
            CurrentMask = accusatore;
        }
        else
        {
            CurrentMask = tentatore;
        }

        SoundManager.Instance.PLaySFXSound(SoundManager.Instance.changeMask);

    }
}
