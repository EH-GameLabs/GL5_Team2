using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IInteractable
{
    [Header("Card Data")]
    public SO_Card cardData;
    public bool alreadyDuplicated = false;
    public Action effectToActivate;

    [Header("Interactable")]
    [SerializeField] private GameObject interactableObj;
    public bool isDraggable = true;
    public bool isPlaced = false;
    public int lifetime = 1;

    private Vector3 startPos;

    private void Start()
    {
        interactableObj.SetActive(false);
        lifetime = cardData.lifeTime;
    }

    public void SetStartPos(Vector3 pos) => startPos = pos;

    public void OnClick()
    {
        // visualizza la carta
    }

    public void OnExitHover()
    {
        if (!isDraggable) return;
        interactableObj.SetActive(false);

        if (isPlaced)
        {
            HudUI hudUI = FindAnyObjectByType<HudUI>();

            hudUI.cardHover.sprite = null;
            hudUI.cardHover.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(MoveDownRoutine());
        }
    }

    public void OnHover()
    {
        if (!isDraggable) return;
        interactableObj.SetActive(true);

        if (isPlaced)
        {
            HudUI hudUI = FindAnyObjectByType<HudUI>();

            hudUI.cardHover.sprite = GetComponentInChildren<SpriteRenderer>().sprite;
            hudUI.cardHover.gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(MoveUpRoutine());
            SoundManager.Instance.PLaySFXSound(SoundManager.Instance.hoverCard);
        }
    }

    private IEnumerator MoveUpRoutine()
    {
        float t = 0;

        Vector3 startPos = this.startPos;
        Vector3 endPos = startPos + new Vector3(0, 0.3f, 0);
        while (t < 1)
        {
            t += Time.deltaTime * 5;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        transform.position = endPos;
    }

    private IEnumerator MoveDownRoutine()
    {
        float t = 0;

        Vector3 startPos = transform.position;
        Vector3 endPos = this.startPos;
        while (t < 1)
        {
            t += Time.deltaTime * 5;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        transform.position = endPos;
        GetComponent<Collider>().enabled = true;
    }

    public void SetIntectableObj(bool input) => interactableObj.SetActive(input);
    public void GetBack() => StartCoroutine(MoveDownRoutine());
}
