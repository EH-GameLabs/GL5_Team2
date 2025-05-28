using UnityEngine;

public class Collector : MonoBehaviour
{
    public static Collector Instance { get; private set; }

    [SerializeField] private CollectorMask currentMask;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    public void ActivateMaskEffect()
    {
        currentMask.ActivateMaskEffect();
    }

    public void SetMask(CollectorMask mask)
    {
        currentMask = mask;
    }
}
