using TMPro;
using UnityEngine;

public class AppleAdder : MonoBehaviour
{
    public TextMeshPro textMesh;
    public AudioSource pickupSound;
    public Animator characterAnimator;
    private int appleCount = 0;
    private bool hasDanced = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("apple"))
        {
            appleCount++;
            UpdateText();
            PlaySound();

            if (appleCount >= 5 && !hasDanced)
            {
                HideText();
                PlayDanceAnimation();
                hasDanced = true;
            }
            Destroy(other.gameObject);
        }
    }

    private void UpdateText()
    {
        if (textMesh != null)
        {
            textMesh.text = ""+appleCount;
        }
    }

    private void PlaySound()
    {
        if (pickupSound != null && !pickupSound.isPlaying)
        {
            pickupSound.Play();
        }
    }

    private void HideText()
    {
        if (textMesh != null)
        {
            textMesh.gameObject.SetActive(false);
        }
    }

    private void PlayDanceAnimation()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("Dance");
        }
    }
}
