using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public static Collector Instance { get; private set; }

    [SerializeField] private List<CollectorMask> masks = new List<CollectorMask>();
    [SerializeField] private CollectorMask currentMask;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    public void ActivateMaskEffect(Card card)
    {
        if (currentMask is CM_Accusatore && card.cardData.cardType == CardTypes.Doloroso)
        {
            currentMask.ActivateMaskEffect();
        }

        if (currentMask is CM_Tentatore && HasDrawEffect(card))
        {
            currentMask.ActivateMaskEffect();
        }

        if (currentMask is CM_Martire && HasHealEffect(card))
        {
            currentMask.ActivateMaskEffect();
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
        currentMask = masks[0];

        foreach (CollectorMask mask in masks)
        {
            if (mask.currentCardActivated > currentMask.currentCardActivated)
            {
                currentMask = mask;
            }
        }
    }
}
