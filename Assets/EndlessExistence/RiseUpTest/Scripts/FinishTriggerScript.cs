using UnityEngine;

public class FinishTriggerScript : MonoBehaviour
{
    public CameraCutEffect cameraCutEffect;   // Reference to the CameraCutEffect
    public Transform respawnPoint;            // The respawn point (starting position)
    public float fadeDuration = 1.0f;        // Duration for the fade effect
    
    public PlayerController playerController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Start the fade-in effect
            cameraCutEffect.FadeIn(fadeDuration);

            // You can subscribe to the events if needed
            cameraCutEffect.onFadeInFinish.AddListener(FinishFadeIn);

            // Start fade-out after a delay
            Invoke(nameof(StartFadeOut), fadeDuration);
        }
    }

    private void FinishFadeIn()
    {
        // This function is called when fade-in is complete
        Debug.Log("Fade In Finished");

        // You can add any actions you need after fade-in
    }

    private void StartFadeOut()
    {
        // Start fade-out effect
        cameraCutEffect.FadeOut(fadeDuration);

        // You can subscribe to the events if needed
        cameraCutEffect.onFadeOutFinish.AddListener(HandleFadeOutFinish);
    }

    private void HandleFadeOutFinish()
    {
        // This function is called when fade-out is complete
        Debug.Log("Fade Out Finished");

        // Move the player to the respawn point
       // MovePlayerToRespawn();
    }

    public void MovePlayerToRespawn()
    {
        playerController.gameObject.SetActive(false);
        playerController.gameObject.transform.position = respawnPoint.position;
        playerController.gameObject.transform.rotation = respawnPoint.rotation; // Optional if you want to reset rotation
        playerController.gameObject.SetActive(true);
    }

    public void DisablePlayer()
    {
        playerController.enabled = false;
    }

    public void EnablePlayer()
    {
        playerController.enabled = true;
    }
}