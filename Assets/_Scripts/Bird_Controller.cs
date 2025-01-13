using UnityEngine;

public class Bird_Controller : MonoBehaviour
{
    // Hareket parametreleri
    public float minSpeed = 1f;
    public float maxSpeed = 3f;
    public float minRadius = 3f;
    public float maxRadius = 7f;
    public float minVerticalAmplitude = 0.5f;
    public float maxVerticalAmplitude = 2f;
    public float minVerticalSpeed = 0.5f;
    public float maxVerticalSpeed = 2f;

    // Ses için parametreler
    public AudioClip[] birdSounds;
    public float minSoundInterval = 3f;
    public float maxSoundInterval = 7f;

    private float speed;
    private float radius;
    private float verticalAmplitude;
    private float verticalSpeed;

    private Vector3 centerPoint;
    private float angle;

    private AudioSource audioSource;
    private float nextSoundTime;
    private Vector3 lastPosition; // Kuþun önceki pozisyonunu tutar

    void Start()
    {
        // Kuþun baþlangýç pozisyonunu merkez olarak al
        centerPoint = transform.position;

        // Rastgele hareket deðerleri ata
        speed = Random.Range(minSpeed, maxSpeed);
        radius = Random.Range(minRadius, maxRadius);
        verticalAmplitude = Random.Range(minVerticalAmplitude, maxVerticalAmplitude);
        verticalSpeed = Random.Range(minVerticalSpeed, maxVerticalSpeed);

        // AudioSource ekle veya kontrol et
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Ýlk ses çýkýþ zamanýný belirle
        nextSoundTime = Time.time + Random.Range(minSoundInterval, maxSoundInterval);

        // Ýlk pozisyonu kaydet
        lastPosition = transform.position;
    }

    void Update()
    {
        // Kuþ hareketini güncelle
        angle += speed * Time.deltaTime;
        if (angle >= 360f) angle -= 360f;

        float x = centerPoint.x + Mathf.Cos(angle) * radius;
        float z = centerPoint.z + Mathf.Sin(angle) * radius;
        float y = centerPoint.y + Mathf.Sin(Time.time * verticalSpeed) * verticalAmplitude;

        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;

        // Hareket yönünü hesapla
        Vector3 direction = newPosition - lastPosition;
        if (direction.magnitude > 0.01f) // Hareket varsa
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }

        // Son pozisyonu güncelle
        lastPosition = newPosition;

        // Ses çýkarma zamaný geldiyse
        if (Time.time >= nextSoundTime && birdSounds.Length > 0)
        {
            PlayRandomBirdSound();
            nextSoundTime = Time.time + Random.Range(minSoundInterval, maxSoundInterval);
        }
    }

    void PlayRandomBirdSound()
    {
        if (birdSounds.Length > 0)
        {
            AudioClip clip = birdSounds[Random.Range(0, birdSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}
