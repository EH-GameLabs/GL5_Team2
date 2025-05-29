using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IInteractable
{
    [Header("Card Data")]
    public SO_Card cardData;

    [Header("Interactable")]
    [SerializeField] private GameObject interactableObj;
    public bool isDraggable = true;
    public bool isPlaced = false;

    private Vector3 startPos;

    private void Start()
    {
        interactableObj.SetActive(false);
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

        if (isPlaced) return;
        StartCoroutine(MoveDownRoutine());
    }

    public void OnHover()
    {
        if (!isDraggable) return;
        interactableObj.SetActive(true);

        if (isPlaced) return;
        StartCoroutine(MoveUpRoutine());
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
