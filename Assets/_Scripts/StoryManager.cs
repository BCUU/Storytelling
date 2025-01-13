using UnityEngine;
using UnityEngine.InputSystem;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private OVRInput.Button bButton;
    [SerializeField] private OVRPassthroughLayer _passthroughLayer;

    private void Update()
    {
        bool bButtonbool = OVRInput.Get(bButton);
        if (bButtonbool)
        {
           _passthroughLayer.SetBrightnessContrastSaturation(-1f,0f,0f);
        }
        else
        {
            _passthroughLayer.SetBrightnessContrastSaturation(0.2f,0f,0f);
        }
    }
}
