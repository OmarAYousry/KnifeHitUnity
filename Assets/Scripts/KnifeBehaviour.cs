using UnityEngine;

public class KnifeBehaviour : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D knifeRigidBody = null;
    [SerializeField]
    private Collider2D knifeCollider = null;
    [SerializeField]
    [Range(5f, 50f)]
    private float launchPower = 20f;

    private KnifeController knifeController = null;

    private float lodgeFactor = 1.0f;

    private bool isLaunched = false;

    #region audio-fields

    [SerializeField]
    private AudioSource sfxSource = null;
    [SerializeField]
    private AudioClip lodgeEffect = null;
    [SerializeField]
    private AudioClip appleEffect = null;
    [SerializeField]
    private AudioClip clashEffect = null;

    #endregion

    #region deflect-fields

    [SerializeField]
    float timeToWait = 0.5f;

    float timeAlreadyWaited = 0.0f;

    Vector3 finalPosition = new Vector3();

    Vector3 initialPosition = new Vector3();

    bool isDeflecting = false;

    #endregion

    public void Init(KnifeController knifeController, float lodgeFactor)
    {
        this.knifeController = knifeController;
        this.lodgeFactor = lodgeFactor;
    }

    public void Launch()
    {
        if (isLaunched)
            return;

        Vector2 forceVector = new Vector2(0f, launchPower);
        knifeRigidBody.AddForce(forceVector, ForceMode2D.Impulse);
        isLaunched = true;
    }

    private void LodgeKnife(Transform logTransform)
    {
        sfxSource.PlayOneShot(lodgeEffect);
        knifeRigidBody.velocity = new Vector3(0f, 0f, 0f);
        Destroy(knifeRigidBody);
        transform.position += transform.up * lodgeFactor;
        transform.parent = logTransform;
        knifeController.AddNewKnifeScore();
        Destroy(this);
    }

    private void DeflectKnife()
    {
        sfxSource.PlayOneShot(clashEffect);
        initialPosition = transform.position;
        float horizontalDistance = Random.Range(3f, 6f);
        float verticalDistance = Random.Range(1f, 2f);
        finalPosition = transform.position - new Vector3(horizontalDistance, verticalDistance, 0f);
        knifeRigidBody.velocity = new Vector3(0f, 0f, 0f);
        Destroy(knifeCollider);
        isDeflecting = true;
    }

    private void SlashApple(GameObject apple)
    {
        sfxSource.PlayOneShot(appleEffect);
        knifeController.AddNewAppleScore();
        Destroy(apple);
    }

    private void FixedUpdate()
    {
        if (timeAlreadyWaited >= timeToWait)
        {
            EndGame();
        }
        else if (isDeflecting)
        {
            transform.position = Vector3.Lerp(initialPosition, finalPosition, timeAlreadyWaited / timeToWait);
            timeAlreadyWaited += Time.fixedDeltaTime;
            float rotationSpeed = 360f;
            float normalizedRotationSpeed = rotationSpeed * Time.fixedDeltaTime;
            transform.Rotate(Vector3.forward, normalizedRotationSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isLaunched)
            return;

        if (collider.gameObject.CompareTag("Log"))
        {
            LodgeKnife(collider.transform);
        }
        else if (collider.gameObject.CompareTag("Apple"))
        {
            SlashApple(collider.gameObject);
        }
        else if (collider.gameObject.CompareTag("Knife"))
        {
            DeflectKnife();
        }
        else
        {
            Debug.LogError($"Unidentified tag {collider.gameObject.tag} was found on gameobject {collider.gameObject}");
        }
    }

    private void EndGame()
    {
        Time.timeScale = 0.0f;
        knifeController.TriggerGameOver();
    }

    public void OnDestroy()
    {
        if (GameController.IsQuitting)
            return;

        knifeController.lessknives();
        knifeController.GenerateNewKnife();
    }
}
