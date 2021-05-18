using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    //method to run on trigger enter
    [SerializeField] private UnityEvent action;

    private void OnTriggerEnter(Collider other)
    {
        action.Invoke();    
    }
}
