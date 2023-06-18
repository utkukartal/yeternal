using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{   
    [SerializeField] float loadDelay = 2f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem crashParticle;
    [SerializeField] ParticleSystem successParticle;
    
    AudioSource audioSource;
    Movement movementScript;

    bool isTransitioning = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        movementScript = GetComponent<Movement>();
    }

    void OnCollisionEnter(Collision other) {
        if(isTransitioning) { return; }
        switch (other.gameObject.tag) {
            case "Friendly":
                Debug.Log("You hit friendly!");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence() {
        isTransitioning = true;
        successParticle.Play();
        audioSource.Stop();
        movementScript.sideThrusterAudioSource.Stop();
        audioSource.PlayOneShot(successSound);
        movementScript.enabled = false;
        Invoke("LoadNextLevel", loadDelay);
    }

    void StartCrashSequence() {
        isTransitioning = true;
        crashParticle.Play();
        audioSource.Stop();
        movementScript.sideThrusterAudioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        movementScript.enabled = false;
        Invoke("ReloadLevel", loadDelay);
    }

    void LoadNextLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
        };
        SceneManager.LoadScene(nextSceneIndex);
    }
    void ReloadLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}

