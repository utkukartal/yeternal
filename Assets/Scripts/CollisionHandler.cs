using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{   
    [SerializeField] float loadDelay = 2f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;

    AudioSource audioSource;
    Movement movementScript;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        movementScript = GetComponent<Movement>();
    }

    void OnCollisionEnter(Collision other) {
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
        if(movementScript.enabled) {
            audioSource.Stop();
        };
        // add SFX and visiuals
        if(! audioSource.isPlaying) {
            audioSource.PlayOneShot(successSound);
        }
        movementScript.enabled = false;
        Invoke("LoadNextLevel", loadDelay);
    }

    void StartCrashSequence() {
        if(movementScript.enabled) {
            audioSource.Stop();
        };
        // add SFX and visiuals
        if(! audioSource.isPlaying) {
            audioSource.PlayOneShot(crashSound);
        }
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

