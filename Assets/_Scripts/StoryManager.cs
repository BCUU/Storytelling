using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private OVRInput.Button bButton;
    [SerializeField] private OVRPassthroughLayer _passthroughLayer;
    [SerializeField] private float targetBrightness = -0.2f; 
    [SerializeField] private float transitionSpeed = 3f;

    [SerializeField] private float delayAfterLightOn = 2f; 
    [SerializeField] private float delayAfterFirstEvent = 3f; 

    private float currentBrightness = -1f; 
    private Tween brightnessTween;

    // Unity Event
    [SerializeField] private UnityEvent onBrightnessIncreaseComplete;
    [SerializeField] private UnityEvent onFirstDelayedEvent; 
    [SerializeField] private UnityEvent onSecondDelayedEvent; 
    [SerializeField] private UnityEvent onThirdDelayedEvent; 
    [SerializeField] private UnityEvent onfourdDelayedEvent;
    public Animator characterAnimator;

    private void Start()
    {
        // Ba�lang��ta -1 karanl�k olarak ayarla
        _passthroughLayer.SetBrightnessContrastSaturation(currentBrightness, 0f, 0f);

        // DoTween ile ba�lang��ta -1'den targetBrightness'a yava��a a��lmas�n� sa�la
        brightnessTween = DOTween.To(() => currentBrightness, x =>
        {
            currentBrightness = x;
            _passthroughLayer.SetBrightnessContrastSaturation(currentBrightness, 0f, 0f);
        }, targetBrightness, transitionSpeed)
        .SetEase(Ease.InOutSine)
        .OnComplete(() =>
        {
            // Parlakl�k a��lma i�lemi tamamland���nda �a�r�lacak event
           

            // �lk s�re sonra ilk event �a�r�l�r
            
        });
        
        Invoke(nameof(onBrightnessIncreaseCompletevoid), delayAfterLightOn);
        Invoke(nameof(CallFirstDelayedEvent), delayAfterLightOn*2);
    }

    private void onBrightnessIncreaseCompletevoid()
    {
        onBrightnessIncreaseComplete.Invoke();

    }
    private void CallFirstDelayedEvent()
    {
        onFirstDelayedEvent?.Invoke();

        // �lk event �al��t�ktan sonra ikinci s�reyi bekleyip ikinci event'i �a��r
        Invoke(nameof(CallSecondDelayedEvent), delayAfterFirstEvent);
    }

    private void CallSecondDelayedEvent()
    {
        onSecondDelayedEvent?.Invoke();
    }
    public void CallThirdDelayedevents()
    { onThirdDelayedEvent?.Invoke();
        Invoke(nameof(callFourd),16f);
    }
    private void callFourd()
    {
        characterAnimator.SetTrigger("Dance");
        onfourdDelayedEvent?.Invoke();
    }

    private void Update()
    {
        bool bButtonbool = OVRInput.Get(bButton);

        float targetValue = bButtonbool ? -0.8f : targetBrightness;

        // E�er hali haz�rda bir tween varsa, iptal et
        if (brightnessTween != null && brightnessTween.IsActive())
        {
            brightnessTween.Kill();
        }

        // Butona bas�ld���nda -1'e d��s�n, b�rak�ld���nda tekrar a��lmaya devam etsin
        brightnessTween = DOTween.To(() => currentBrightness, x =>
        {
            currentBrightness = x;
            _passthroughLayer.SetBrightnessContrastSaturation(currentBrightness, 0f, 0f);
        }, targetValue, transitionSpeed)
        .SetEase(Ease.InOutSine);
    }
    public void wolfScenePass()
    {
        targetBrightness = -0.8f;
        _passthroughLayer.SetBrightnessContrastSaturation(targetBrightness, 0f, 0f);
    }
    public void WolfScnenedone()
    {
        targetBrightness = 0.2f;
        _passthroughLayer.SetBrightnessContrastSaturation(targetBrightness, 0f, 0f);

    }
}
