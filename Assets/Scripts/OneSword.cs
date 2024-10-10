using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSword : MonoBehaviour
{
    public float speed = 5f;  
    [SerializeField] private int damage = 20;   

    // Update is called once per frame
    void Update()
    {
        // Move the sword across the x-axis by speed every frame
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x > Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 1)
        {
            Destroy(gameObject); // Destroy the sword when offscreen
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            
            Health playerHealth = collision.gameObject.GetComponent<Health>();

            
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, transform.position);
            }
        }
    }
}

