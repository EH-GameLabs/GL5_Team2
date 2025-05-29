using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public static Collector Instance { get; private set; }

    [SerializeField] private List<CollectorMask> masks = new List<CollectorMask>();
    [SerializeField] private CollectorMask currentMask;
    public CollectorMask CurrentMask
    {
        get => currentMask;
        set
        {
            if (value != null)
            {
                currentMask = value;
                FindAnyObjectByType<HudUI>(FindObjectsInactive.Include).UpdateMaskText(currentMask.name);
            }
        }
    }


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    public void ActivateMaskEffect(Card card)
    {
        if (CurrentMask is CM_Accusatore && card.cardData.cardType == CardTypes.Doloroso)
        {
            CurrentMask.ActivateMaskEffect();
        }

        if (CurrentMask is CM_Tentatore && HasDrawEffect(card))
        {
            CurrentMask.ActivateMaskEffect();
        }

        if (CurrentMask is CM_Martire && HasHealEffect(card))
        {
            CurrentMask.ActivateMaskEffect();
        }
    }

    private bool HasHealEffect(Card card)
    {
        foreach (var effect in card.cardData.effects)
        {
            if (effect is E_GainLife)
            {
                return true;
            }
        }
        return false;
    }

    private bool HasDrawEffect(Card card)
    {
        foreach (var effect in card.cardData.effects)
        {
            if (effect is E_DrawCards)
            {
                return true;
            }
        }
        return false;
    }

    public void SetMask()
    {
        CurrentMask = masks[0];

        foreach (CollectorMask mask in masks)
        {
            if (mask.currentCardActivated > currentMask.currentCardActivated)
            {
                CurrentMask = mask;
            }
        }
    }
}
