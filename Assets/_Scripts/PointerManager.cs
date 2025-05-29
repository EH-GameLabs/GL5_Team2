using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PointerManager : MonoBehaviour
{
    public static PointerManager Instance { get; private set; }

    // L'interactable su cui siamo ora, oppure null
    private IInteractable currentHovered = null;
    bool dragging = false;
    Vector3 rayHitPosition = Vector3.zero;
    Vector3 originRotation = Vector3.zero;
    Transform card;
    public Vector3 offset;
    bool hitting = false;
    public LayerMask layerMask;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    private void Update()
    {
        // 1) Provo il raycast dal mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!dragging)
        {
            Hover(ray);
            return;
        }

        hitting = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~layerMask);

        if (Input.GetMouseButton(0) && hitting)
        {
            rayHitPosition = hit.point;
            card.position = rayHitPosition + offset;
        }

        if (Input.GetMouseButtonUp(0))
        {
            currentHovered = null;
            dragging = false;

            if (hitting && hit.collider.CompareTag("Slot") && hit.transform.GetComponentInChildren<Card>() == null)
            {
                card.position = hit.transform.position;
                card.transform.SetParent(hit.transform);
                card.GetComponent<Card>().SetStartPos(card.position);
                if (!card.GetComponent<Card>().isPlaced)
                {
                    GameManager.Instance.CurrentRM -= card.GetComponent<Card>().cardData.RMCost;
                    card.GetComponent<Card>().isPlaced = true;
                }
                card.GetComponent<Collider>().enabled = true;
            }
            else
            {
                //card.SetPositionAndRotation(originPosition, Quaternion.Euler(originRotation));
                card.rotation = Quaternion.Euler(originRotation);
                card.GetComponent<Card>().GetBack();
                card.GetComponent<Card>().SetIntectableObj(false); // Riattivo l'hover della carta
            }
        }
    }



    private void Hover(Ray ray)
    {
        RaycastHit hit;
        IInteractable hitInt = null;

        if (Physics.Raycast(ray, out hit))
        {
            hitInt = hit.collider.GetComponent<IInteractable>();
        }

        // 2) Se cambia l'interactable, mando Exit sul vecchio e Enter (Hover) sul nuovo
        if (hitInt != currentHovered)
        {
            currentHovered?.OnExitHover();

            currentHovered = hitInt;

            currentHovered?.OnHover();
        }

        // 3) Gestione click/selezione separata, senza ricollegarsi all'hover
        if (Input.GetMouseButtonDown(0) && currentHovered != null)
        {
            currentHovered.OnClick();

            if (currentHovered is not Card) return;

            card = ((MonoBehaviour)currentHovered).transform;

            Card cardComponent = card.GetComponent<Card>();

            if (cardComponent.cardData.RMCost > GameManager.Instance.CurrentRM && !cardComponent.isPlaced) return;
            if (!cardComponent.isDraggable) return;

            originRotation = card.rotation.eulerAngles; // Salvo la rotazione originale
            dragging = true; // Inizio a trascinare
            card.GetComponent<Collider>().enabled = false; // Disabilito il collider della carta mentre la trascino
            card.rotation = Quaternion.Euler(Vector3.zero);
        }

        if (Input.GetMouseButtonUp(0) && currentHovered != null)
        {
            currentHovered.OnExitHover(); // oppure un metodo OnRelease se preferisci
            currentHovered = null;
        }
    }
}
