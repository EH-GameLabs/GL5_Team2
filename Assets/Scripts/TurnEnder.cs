using UnityEngine;

public class TurnEnder : MonoBehaviour, IInteractable
{
    [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material hoverMaterial;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = defaultMaterial;
    }
    public void OnHover()
    {
        // Logica per gestire l'hover
        Debug.Log("Hover su TurnEnder");
        meshRenderer.material = hoverMaterial;

    }
    public void OnExitHover()
    {
        // Logica per gestire l'uscita dall'hover
        Debug.Log("Uscita hover su TurnEnder");
        meshRenderer.material = defaultMaterial;
    }
    public void OnClick()
    {
        // Logica per gestire il click
        Debug.Log("Click su TurnEnder");
    }
}