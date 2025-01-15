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
        // Baþlangýçta -1 karanlýk olarak ayarla
        _passthroughLayer.SetBrightnessContrastSaturation(currentBrightness, 0f, 0f);

        // DoTween ile baþlangýçta -1'den targetBrightness'a yavaþça açýlmasýný saðla
        brightnessTween = DOTween.To(() => currentBrightness, x =>
        {
            currentBrightness = x;
            _passthroughLayer.SetBrightnessContrastSaturation(currentBrightness, 0f, 0f);
        }, targetBrightness, transitionSpeed)
        .SetEase(Ease.InOutSine)
        .OnComplete(() =>
        {
            // Parlaklýk açýlma iþlemi tamamlandýðýnda çaðrýlacak event
           

            // Ýlk süre sonra ilk event çaðrýlýr
            
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

        // Ýlk event çalýþtýktan sonra ikinci süreyi bekleyip ikinci event'i çaðýr
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

        // Eðer hali hazýrda bir tween varsa, iptal et
        if (brightnessTween != null && brightnessTween.IsActive())
        {
            brightnessTween.Kill();
        }

        // Butona basýldýðýnda -1'e düþsün, býrakýldýðýnda tekrar açýlmaya devam etsin
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
