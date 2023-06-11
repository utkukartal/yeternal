using System.Collections;
using System.Collections.Generic;
using UnityEngine; // MonoBehaviour is from this namespace 

public class Movement : MonoBehaviour // Our class Movement inherits some stuff from MonoBehaviour, which is a class itself
{
    Rigidbody rigidBody; // A variable named rigidBody, with the type Rigidbody
    AudioSource audioSource;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float zThrust = 250f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>(); // Here we are cashing a reference to the component
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRatation();
    }

    void ProcessThrust() {
        if(Input.GetKey(KeyCode.Space)) {
            if(! audioSource.isPlaying) {
                audioSource.Play();
            };
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime); // Relative to force to the elements coordinates, so up will be elements up not just increase y position.
            // Vector3 is 3 diff values for coordinates, its both direction and magnitude. .up is pretty much sayin (0,1,0)
        }else {
            if(audioSource.isPlaying) {
                audioSource.Stop();
            };
        };
    }
    
    void ProcessRatation() {
        if(! (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))) {
            if(Input.GetKey(KeyCode.A)) {
                ApplyRotation(1);
            }
            else if(Input.GetKey(KeyCode.D)) {
                ApplyRotation(-1);
            }
        }
    }

    void ApplyRotation(float rotationDirection) {
        rigidBody.freezeRotation = true; // freezing pyhsiycs rotation when manual ratation happens
        transform.Rotate(rotationDirection * Vector3.forward * zThrust * Time.deltaTime);
        rigidBody.freezeRotation = false;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
    }
}
