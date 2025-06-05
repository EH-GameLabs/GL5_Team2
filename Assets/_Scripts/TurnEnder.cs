using UnityEngine;

public class TurnEnder : MonoBehaviour, IInteractable
{
    [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material hoverMaterial;
    [SerializeField] private MeshRenderer meshRenderer;

    private Animator animator;

    private void Start()
    {
        //meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = defaultMaterial;
        animator = GetComponent<Animator>();
    }
    public void OnHover()
    {
        // Logica per gestire l'hover
        //Debug.Log("Hover su TurnEnder");
        meshRenderer.material = hoverMaterial;

    }
    public void OnExitHover()
    {
        // Logica per gestire l'uscita dall'hover
        //Debug.Log("Uscita hover su TurnEnder");
        meshRenderer.material = defaultMaterial;
    }
    public void OnClick()
    {
        // Logica per gestire il click
        //Debug.Log("Click su TurnEnder");
        if (!TurnManager.Instance.CanEndTurn()) return;
        animator.SetTrigger("EndTurn");
    }

    public void OnEndAnimation()
    {
        TurnManager.Instance.ActivateCardsEffects();
        SoundManager.Instance.PLaySFXSound(SoundManager.Instance.endTurn);
    }
}