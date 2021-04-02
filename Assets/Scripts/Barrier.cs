using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (FindObjectOfType<Rocket>() != null)
        {
            FindObjectOfType<Rocket>().StopEngine();
        }
    }
}
