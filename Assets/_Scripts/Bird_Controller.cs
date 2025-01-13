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

    // Ses i�in parametreler
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
    private Vector3 lastPosition; // Ku�un �nceki pozisyonunu tutar

    void Start()
    {
        // Ku�un ba�lang�� pozisyonunu merkez olarak al
        centerPoint = transform.position;

        // Rastgele hareket de�erleri ata
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

        // �lk ses ��k�� zaman�n� belirle
        nextSoundTime = Time.time + Random.Range(minSoundInterval, maxSoundInterval);

        // �lk pozisyonu kaydet
        lastPosition = transform.position;
    }

    void Update()
    {
        // Ku� hareketini g�ncelle
        angle += speed * Time.deltaTime;
        if (angle >= 360f) angle -= 360f;

        float x = centerPoint.x + Mathf.Cos(angle) * radius;
        float z = centerPoint.z + Mathf.Sin(angle) * radius;
        float y = centerPoint.y + Mathf.Sin(Time.time * verticalSpeed) * verticalAmplitude;

        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;

        // Hareket y�n�n� hesapla
        Vector3 direction = newPosition - lastPosition;
        if (direction.magnitude > 0.01f) // Hareket varsa
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }

        // Son pozisyonu g�ncelle
        lastPosition = newPosition;

        // Ses ��karma zaman� geldiyse
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
