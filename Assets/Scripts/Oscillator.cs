﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 transformMovment;
    [SerializeField] private Vector3 rotationMovment;
    [SerializeField] [Range(0, 1)] private float movingRange;
    [SerializeField] [Range(0, 1)] private float rotationRange;
    [SerializeField] private float transformPeriod = 1f;
    [SerializeField] private float rotationPeriod = 1f;
    [SerializeField] private bool isLocalTransform = false;
    [SerializeField] private bool isLocalRotation = false;

    private const float TAU = Mathf.PI * 2;
    private Vector3 startingPosition;
    private Quaternion startingRotation;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!(transformPeriod <= Mathf.Epsilon))
        {
            float rawSinWave = Mathf.Sin(Time.time / transformPeriod * TAU); ;
            movingRange = rawSinWave / 2 + 0.5f;

            if (isLocalTransform)
                transform.position = startingPosition + movingRange * transform.TransformDirection(transformMovment);
            else
                transform.position = startingPosition + movingRange * transformMovment;
        }

        if (!(rotationPeriod <= Mathf.Epsilon))
        {
            float rawSinWave = Mathf.Sin(Time.time / rotationPeriod * TAU); ;
            rotationRange = rawSinWave / 2 + 0.5f;

            if (isLocalRotation)
                transform.rotation = Quaternion.Euler(startingRotation.eulerAngles + rotationRange * transform.TransformDirection(rotationMovment));
            else
                transform.rotation = Quaternion.Euler(startingRotation.eulerAngles + rotationRange * rotationMovment);
        }
    }
}
