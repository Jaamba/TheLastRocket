using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    Rocket rocket;

    private void OnTriggerEnter(Collider other)
    {
        rocket = FindObjectOfType<Rocket>();

        if (rocket.gameObject.tag == other.gameObject.tag)
        {
            rocket.Win();
        } 
    }
}
