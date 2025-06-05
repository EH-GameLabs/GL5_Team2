using UnityEngine;

[RequireComponent(typeof(Light))]
public class FireLightFlareSequenced : MonoBehaviour
{
    [Header("Base Light Settings")]
    public float baseIntensity = 1.5f;

    [Header("Flare Settings")]
    public float minFlareIntensity = 2.5f;
    public float maxFlareIntensity = 4.0f;
    public float minFlareDuration = 0.1f;
    public float maxFlareDuration = 0.3f;
    public float returnSpeed = 5f; // velocità con cui torna alla base

    [Header("Burst Settings")]
    public float minTimeBetweenBursts = 2f;
    public float maxTimeBetweenBursts = 5f;
    public int minFlareInBurst = 1;
    public int maxFlareInBurst = 4;

    private Light fireLight;
    private enum FlareState { Idle, Flaring, Returning }
    private FlareState state = FlareState.Idle;

    private float flareTimer = 0f;
    private float nextBurstTimer = 0f;
    private int flaresRemaining = 0;
    private float targetIntensity = 0f;

    void Start()
    {
        fireLight = GetComponent<Light>();
        fireLight.intensity = baseIntensity;
        targetIntensity = baseIntensity;
        ScheduleNextBurst();
    }

    void Update()
    {
        switch (state)
        {
            case FlareState.Idle:
                nextBurstTimer -= Time.deltaTime;
                if (nextBurstTimer <= 0f)
                {
                    flaresRemaining = Random.Range(minFlareInBurst, maxFlareInBurst + 1);
                    TriggerFlare();
                }
                break;

            case FlareState.Flaring:
                flareTimer -= Time.deltaTime;
                if (flareTimer <= 0f)
                {
                    targetIntensity = baseIntensity;
                    state = FlareState.Returning;
                }
                break;

            case FlareState.Returning:
                fireLight.intensity = Mathf.Lerp(fireLight.intensity, baseIntensity, Time.deltaTime * returnSpeed);
                if (Mathf.Abs(fireLight.intensity - baseIntensity) < 1f)
                {
                    fireLight.intensity = baseIntensity;
                    if (flaresRemaining > 0)
                    {
                        TriggerFlare();
                    }
                    else
                    {
                        ScheduleNextBurst();
                        state = FlareState.Idle;
                    }
                }
                break;
        }

        // In fase di flare o idle, aggiornamento normale
        if (state != FlareState.Returning)
        {
            fireLight.intensity = Mathf.Lerp(fireLight.intensity, targetIntensity, Time.deltaTime * 10f);
        }
    }

    void TriggerFlare()
    {
        targetIntensity = Random.Range(minFlareIntensity, maxFlareIntensity);
        flareTimer = Random.Range(minFlareDuration, maxFlareDuration);
        flaresRemaining--;
        state = FlareState.Flaring;
    }

    void ScheduleNextBurst()
    {
        nextBurstTimer = Random.Range(minTimeBetweenBursts, maxTimeBetweenBursts);
    }
}
