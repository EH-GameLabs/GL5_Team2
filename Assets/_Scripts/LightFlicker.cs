using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light lightToFlicker;     // La luce da sfarfallare
    public float flickerDuration = 1.0f; // Durata dello sfarfallio
    public float minFlickerInterval = 3.0f; // Tempo minimo tra uno sfarfallio e l'altro
    public float maxFlickerInterval = 7.0f; // Tempo massimo tra uno sfarfallio e l'altro

    void Start()
    {
        StartCoroutine(FlickerLoop());
    }

    IEnumerator FlickerLoop()
    {
        while (true)
        {
            // Aspetta un tempo casuale tra uno sfarfallio e l'altro
            float waitTime = Random.Range(minFlickerInterval, maxFlickerInterval);
            yield return new WaitForSeconds(waitTime);

            // Inizia lo sfarfallio
            float startTime = Time.time;
            while (Time.time - startTime < flickerDuration)
            {
                lightToFlicker.enabled = Random.value > 0.5f;
                yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            }

            // 50/50 se rimane accesa o spenta
            lightToFlicker.enabled = Random.value > 0.5f;
        }
    }
}
