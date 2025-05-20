using UnityEngine;

public class PointerManager : MonoBehaviour
{
    public static PointerManager Instance { get; private set; }

    // L'interactable su cui siamo ora, oppure null
    private IInteractable currentHovered = null;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    private void Update()
    {
        // 1) Provo il raycast dal mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
        }
        if (Input.GetMouseButtonUp(0) && currentHovered != null)
        {
            currentHovered.OnExitHover(); // oppure un metodo OnRelease se preferisci
            currentHovered = null;
        }
    }
}
