using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    //serializable parms
    [SerializeField] private float thrustForce;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float stoppingAudioSpeed;
    [SerializeField] private float angularVelocityRange;
    [SerializeField] private float maximumAngularVelocity;

    //fields
    private Rigidbody _rigidbody;
    private AudioSource _audio;

    private void Start()
    {
        //initializers
        {
            _rigidbody = GetComponent<Rigidbody>();
            _audio = GetComponent<AudioSource>();
        }

        _rigidbody.maxAngularVelocity = maximumAngularVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        //movment
        {
            Thrust();
            Rotate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "friendly":
                //print("friendly hit");
                break;
            default:
                //print("you dead muahahahahah");
                break;
        }
    }

    //processes rotation input on the rocket
    private void Rotate()
    {
        //rotate
        if (Input.GetKey(KeyCode.A))
        {
            if (_rigidbody.angularVelocity.z < angularVelocityRange)
            {
                _rigidbody.angularVelocity = new Vector3(_rigidbody.angularVelocity.x, _rigidbody.angularVelocity.y, _rigidbody.angularVelocity.z + (rotationSpeed * Time.deltaTime) );
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (_rigidbody.angularVelocity.z > -angularVelocityRange)
            {
                _rigidbody.angularVelocity = new Vector3(_rigidbody.angularVelocity.x, _rigidbody.angularVelocity.y, _rigidbody.angularVelocity.z - (rotationSpeed * Time.deltaTime));
            }
        }
    }

    //processes thrust input on the rocket
    private void Thrust()
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
        else if (_audio.isPlaying)  //audio stop       
        {
            Tools.StopAudio(_audio, stoppingAudioSpeed);
        }
    }
}
