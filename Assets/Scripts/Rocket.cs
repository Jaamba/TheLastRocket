using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum State { Alive, Transcending, Dead }
public enum Direction { Left, Right, Forward, Inward}

public class Rocket : MonoBehaviour
{
    //serializable parms
    [SerializeField] private float thrustForce;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float stoppingAudioSpeed;
    [SerializeField] private float angularVelocityRange;
    [SerializeField] private float maximumAngularVelocity;
    [SerializeField] private float respawnDelay;
    [SerializeField] private float afterCrushDeathDelay;
    [SerializeField] private RigidbodyConstraints[] localConstraints;

    [SerializeField] private AudioClip thrustSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private ParticleSystem thrustParticles;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private ParticleSystem winParticles;

    //fields
    private Rigidbody _rigidbody;
    private AudioSource _audio;
    private State state = State.Alive;
    private bool hasCrushed = false;

    //start is called at the start of the execution
    private void Start()
    {
        //initializers with GetComponent<>
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
            ThrustInput();
            RotateInput();

            SetLocalConstraints(localConstraints);
        }

        if (hasCrushed && thrustParticles.isPlaying)
            thrustParticles.Stop();

        if(Input.GetKeyDown(KeyCode.P))
        {
            _rigidbody.angularVelocity = new Vector3(_rigidbody.angularVelocity.x, _rigidbody.angularVelocity.y, _rigidbody.angularVelocity.z + 10);
        }
    }

    //when a collision is detected
    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "friendly":
                break;

            case "Finish":
                if(state != State.Transcending)
                    Win();
                break;

            default:
                if (state != State.Dead)
                    Lose();
                break;
        }

        if (hasCrushed)
            Lose();
    }

    //loses and restarts the level
    public void Lose()
    {
        deathParticles.Play();

        _audio.volume = 1;
        _audio.PlayOneShot(deathSound);

        state = State.Dead;
        Invoke("LoadPreviousLevel", respawnDelay);
    }

    //wins the level and starts the next one
    public void Win()
    {
        if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
        {
            state = State.Transcending;

            _audio.volume = 1;
            _audio.PlayOneShot(winSound);

            winParticles.Play();
        }

        Invoke("LoadNextLevel", respawnDelay);
    }

    //loads the next level
    private void LoadNextLevel()
    {
        Tools.StopAudio(_audio, stoppingAudioSpeed);
        if(winParticles.isPlaying)
            winParticles.Stop();

        if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    //loads the previous level
    private void LoadPreviousLevel()
    {
        Tools.StopAudio(_audio, stoppingAudioSpeed);
        if (deathParticles.isPlaying)
            deathParticles.Stop();

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    //processes rotation input on the rocket
    private void RotateInput()
    {
        //rotate
        if (Input.GetKey(KeyCode.A) && state == State.Alive)
        {
            Rotate(Direction.Left);
        }
        else if (Input.GetKey(KeyCode.D) && state == State.Alive)
        {
            Rotate(Direction.Right);
        }
    }

    //gives angualr speed to the rocket
    private void Rotate(Direction direction)
    {
        if (_rigidbody.angularVelocity.z < angularVelocityRange && direction == Direction.Left)
        {
            _rigidbody.angularVelocity = new Vector3(_rigidbody.angularVelocity.x, _rigidbody.angularVelocity.y, _rigidbody.angularVelocity.z + (rotationSpeed * Time.deltaTime));
        }
        if (_rigidbody.angularVelocity.z > -angularVelocityRange && direction == Direction.Right)
        {
            _rigidbody.angularVelocity = new Vector3(_rigidbody.angularVelocity.x, _rigidbody.angularVelocity.y, _rigidbody.angularVelocity.z - (rotationSpeed * Time.deltaTime));
        }
    }

    //processes thrust input on the rocket
    private void ThrustInput()
    {
        //boost
        if (Input.GetKey(KeyCode.Mouse0) && state == State.Alive)
        {
            Thrust();
        }
        else if (_audio.isPlaying)  //audio stop       
        {
            Tools.StopAudio(_audio, stoppingAudioSpeed);
            thrustParticles.Stop();
        }
    }

    //gives thrust to the rocket
    private void Thrust()
    {
        _rigidbody.AddRelativeForce(Vector3.up * Time.deltaTime * thrustForce);

        _audio.volume = 1;
        if (!_audio.isPlaying && !hasCrushed)
        {
            _audio.PlayOneShot(thrustSound);
        }
        if(!thrustParticles.isPlaying && !hasCrushed)
        {
            thrustParticles.Play();
        }
    }

    //stops the engine
    public void StopEngine()
    {
        thrustForce = 0;
        hasCrushed = true;

        Invoke("Lose", afterCrushDeathDelay);
    }

    //set local constraints
    public void SetLocalConstraints(RigidbodyConstraints[] constraints)
    {
        foreach(RigidbodyConstraints cons in constraints)
        {
            SetLocalConstraint(cons);
        }
    }

    //set local constraint
    public void SetLocalConstraint(RigidbodyConstraints constraint)
    {
        Vector3 localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);

        switch (constraint)
        {
            case RigidbodyConstraints.FreezeAll:
                _rigidbody.angularVelocity = new Vector3(0, 0, 0);
                localVelocity.x = 0;
                localVelocity.y = 0;
                localVelocity.z = 0;
                break;
            case RigidbodyConstraints.FreezePosition:
                localVelocity.x = 0;
                localVelocity.y = 0;
                localVelocity.z = 0;
                break;
            case RigidbodyConstraints.FreezePositionX:
                localVelocity.x = 0;
                break;
            case RigidbodyConstraints.FreezePositionY:
                localVelocity.y = 0;
                break;
            case RigidbodyConstraints.FreezePositionZ:
                localVelocity.z = 0;
                break;
            case RigidbodyConstraints.FreezeRotation:
                _rigidbody.angularVelocity = new Vector3(0,0,0);
                break;
            case RigidbodyConstraints.FreezeRotationX:
                _rigidbody.angularVelocity = new Vector3(0, _rigidbody.angularVelocity.y, _rigidbody.angularVelocity.z);
                break;
            case RigidbodyConstraints.FreezeRotationY:
                _rigidbody.angularVelocity = new Vector3(_rigidbody.angularVelocity.x, 0, _rigidbody.angularVelocity.z);
                break;
            case RigidbodyConstraints.FreezeRotationZ:
                _rigidbody.angularVelocity = new Vector3(_rigidbody.angularVelocity.x, _rigidbody.angularVelocity.y, 0);
                break;
            case RigidbodyConstraints.None:
                break;
        }

        _rigidbody.velocity = transform.TransformDirection(localVelocity);
    }
}
