using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the playerObject at the PlayerSpawner's position with the playerObject's rotation
        Instantiate(playerObject, transform.position, playerObject.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

