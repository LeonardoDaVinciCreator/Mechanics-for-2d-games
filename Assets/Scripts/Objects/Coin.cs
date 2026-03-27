using UnityEngine;

public class Coin : InteractObject
{
    protected override void OnInteract()
    {
        Debug.Log("take coin");
    }
}
