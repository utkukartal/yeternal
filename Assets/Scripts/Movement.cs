using UnityEngine; // MonoBehaviour is from this namespace 

public class Movement : MonoBehaviour // Our class Movement inherits some stuff from MonoBehaviour, which is a class itself
{
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float zThrust = 250f;
    [SerializeField] AudioClip mainEngineClip;
    [SerializeField] AudioClip sideThrusterClip;
    [SerializeField] ParticleSystem mainBoosterParticle;
    [SerializeField] ParticleSystem leftBoosterFrontParticle;
    [SerializeField] ParticleSystem leftBoosterBackParticle;
    [SerializeField] ParticleSystem rightBoosterFrontParticle;
    [SerializeField] ParticleSystem rightBoosterBackParticle;
    
    Rigidbody rigidBody; // A variable named rigidBody, with the type Rigidbody
    AudioSource audioSource;
    public AudioSource sideThrusterAudioSource;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>(); // Here we are cashing a reference to the component
        audioSource = GetComponent<AudioSource>();

        GameObject sideThrusterObject = GameObject.Find("Side Thrusters");
        sideThrusterAudioSource = sideThrusterObject.GetComponentInChildren<AudioSource>();
        
    }

    void Update()
    {
        ProcessThrust();
        ProcessRatation();
    }

    void ProcessThrust() {
        if(Input.GetKey(KeyCode.Space)) {
            StartThrusting();
        }
        else {
            StopThrusting();
        }
    }

    void ProcessRatation() {
        if(! (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))) {
            if(Input.GetKey(KeyCode.A)) {
                StartLeftRotation();
            }
            else if(Input.GetKey(KeyCode.D)) {
                StartRightRotation();
            }else {
                StopRotation();
            }
        }
    }

    void StartThrusting() {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime); // Relative to force to the elements coordinates, so up will be elements up not just increase y position.
        // Vector3 is 3 diff values for coordinates, its both direction and magnitude. .up is pretty much sayin (0,1,0)
        
        if (! audioSource.isPlaying) {
            audioSource.PlayOneShot(mainEngineClip);
        }

        if (! mainBoosterParticle.isPlaying) {
            mainBoosterParticle.Play();
        }
    }

    void StopThrusting() {
        mainBoosterParticle.Stop();
        audioSource.Stop();
    }

    void StartLeftRotation() {
        if(! sideThrusterAudioSource.isPlaying) {
            sideThrusterAudioSource.PlayOneShot(sideThrusterClip);
        }

        if(! rightBoosterFrontParticle.isPlaying) {
            rightBoosterFrontParticle.Play();
            rightBoosterBackParticle.Play();
        }
        ApplyRotation(1);
    }

    void StartRightRotation() {
        if(! sideThrusterAudioSource.isPlaying) {
            sideThrusterAudioSource.PlayOneShot(sideThrusterClip);
        }

        if(! leftBoosterFrontParticle.isPlaying) {
            leftBoosterFrontParticle.Play();
            leftBoosterBackParticle.Play();
        }
        ApplyRotation(-1);
    }

    void StopRotation() {
        rightBoosterFrontParticle.Stop();
        rightBoosterBackParticle.Stop();
        leftBoosterFrontParticle.Stop();
        leftBoosterBackParticle.Stop();
        sideThrusterAudioSource.Stop();
    }

    void ApplyRotation(float rotationDirection) {
        rigidBody.freezeRotation = true; // freezing pyhsiycs rotation when manual ratation happens
        transform.Rotate(rotationDirection * Vector3.forward * zThrust * Time.deltaTime);
        rigidBody.freezeRotation = false;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
    }
}
