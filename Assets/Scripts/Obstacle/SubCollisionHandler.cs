using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCollisionHandler : MonoBehaviour
{
    ObstacleController parent;

    private void Start()
    {
        parent = GetComponentInParent<ObstacleController>();                
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(parent != null)
        {
            parent.HandleSubCollision(collision, gameObject);
        }
    }
}
