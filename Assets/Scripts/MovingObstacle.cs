using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private Vector3 movment;
    [SerializeField] [Range(0, 1)] private float movingRange;
    [SerializeField] private float period = 1f;

    private const float TAU = Mathf.PI * 2;
    private Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!(period <= Mathf.Epsilon))
        {
            float rawSinWave = Mathf.Sin(Time.time / period * TAU); ;
            movingRange = rawSinWave / 2 + 0.5f;

            transform.position = startingPosition + movingRange * movment;
        }
    }
}
