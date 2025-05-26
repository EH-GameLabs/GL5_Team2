using UnityEngine;

[CreateAssetMenu(fileName = "New Effect 1", menuName = "ScriptableObject/Effect1")]
public class SO_Effect1 : SO_Effect
{
    public override void Effect()
    {
        // Implement the specific effect logic here
        Debug.Log("Effect1 executed!");
    }
}
