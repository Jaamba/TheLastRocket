using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float thrustForce;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float stoppingAudioSpeed;

    private Rigidbody _rigidbody;
    private AudioSource _audio;

    private void Start()
    {
        //initializers
        {
            _rigidbody = GetComponent<Rigidbody>();
            _audio = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //movment
        {
            ProcessInput();
        }
    }

    private void ProcessInput()
    {
        //boost
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _rigidbody.AddRelativeForce(Vector3.up * Time.deltaTime * thrustForce);

            _audio.volume = 1;
            if (!_audio.isPlaying)
            {
                _audio.Play();
            }
        }
        else if(_audio.isPlaying)  //audio stop       
        {
            Tools.StopAudio(_audio, stoppingAudioSpeed);
        }

        //rotate
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * Time.deltaTime * rotationSpeed);
        }
    }

}
