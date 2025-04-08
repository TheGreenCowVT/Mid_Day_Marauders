using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerDetector : MonoBehaviour
{
    SphereCollider sphereCollider;
    public bool playerIsDetected = false;
    bool startDetectionCheck = false;
    public float playerDetectionCooldown = 5f;
    float currentCooldown = 0;
    Transform player;
    
    public List<string> secondaryEnemies = new List<string>();

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
    }

    private void OnEnable()
    {
        startDetectionCheck = false;
        playerIsDetected = false;
        currentCooldown = 0;
    }

    private void Update()
    {
        // If we have spotted the player, the sphere collider is turned on, then we start a cooldown.
        // After the cooldown if the player isn't around, set playerIsDetected to false, deactivate
        // the sphere collider and wait until the player re-enters the sight cone. 
        if(startDetectionCheck)
        {
            currentCooldown += Time.deltaTime;
            if(currentCooldown > playerDetectionCooldown )
            {
                if (!playerIsDetected)
                {
                    sphereCollider.enabled = false;
                    currentCooldown = 0;
                    startDetectionCheck = false;
                }
            }
        }
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && !playerIsDetected)
        {
            player = col.transform;
            currentCooldown = 0;
            playerIsDetected = true;
            startDetectionCheck = true;
            sphereCollider.enabled = true;
        }
    }
    
    public Transform GetPlayer() => player;
}