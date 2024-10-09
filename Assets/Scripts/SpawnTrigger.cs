using UnityEngine;

public class PrefabInstantiator : MonoBehaviour
{
    public GameObject prefabToInstantiate;  // Assign the prefab in the inspector
    public Transform instantiateLocation;   // Assign the desired location in the inspector

    private bool hasInstantiated = false;   // To ensure we only instantiate once

    // Method called when another object enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering is the player
        if (other.CompareTag("Player") && !hasInstantiated)
        {
            // Instantiate the prefab at the specified location
            Instantiate(prefabToInstantiate, instantiateLocation.position, Quaternion.identity);
            hasInstantiated = true;  // Prevents multiple instantiations
        }
    }
}
