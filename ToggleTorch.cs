using UnityEngine;
using UnityEngine.InputSystem;

public class TorchToggle : MonoBehaviour
{
    public Light torchLight; // Assign in Inspector
    public AudioSource toggleSound; // Optional
    public InputAction toggleAction; // Define in Inspector or enable via script

    private bool isOn = true;

    void Awake()
    {
        if (torchLight == null)
            torchLight = GetComponent<Light>();

        torchLight.enabled = isOn;

        // Enable the input action
        toggleAction.Enable();
        toggleAction.performed += ctx => ToggleTorch();
    }

    private void ToggleTorch()
    {
        isOn = !isOn;
        torchLight.enabled = isOn;

        if (toggleSound != null)
            toggleSound.Play();
    }

    void OnDisable()
    {
        toggleAction.Disable();
        toggleAction.performed -= ctx => ToggleTorch();
    }
}

