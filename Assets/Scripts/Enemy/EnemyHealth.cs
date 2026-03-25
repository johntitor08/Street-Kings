using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(EnemyFollow))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject broPrefab;
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private CharacterType characterType;
    private int currentHealth;
    private readonly float corpseLifetime = 5f;
    private readonly float interactionRange = 1.5f;
    public bool isDead = false;
    private float deathTime;
    private Rigidbody2D rb;
    private new Collider2D collider;
    private EnemyFollow enemyFollow;
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private AudioClip gruntDamageSoundClip;
    [SerializeField] private AudioClip mollyDamageSoundClip;
    [SerializeField] private AudioClip eddieDamageSoundClip;

    private void OnEnable()
    {
        EnemyRegistry.RegisterEnemy(transform);
    }

    private void OnDisable()
    {
        EnemyRegistry.UnregisterEnemy(transform);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        enemyFollow = GetComponent<EnemyFollow>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        PlayDamageSound();

        if (currentHealth <= 0)
            Die();
        else
            rb.AddForce(hitDirection * 2f, ForceMode2D.Impulse);
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");

        if (enemyFollow != null)
            enemyFollow.enabled = false;

        if (collider != null)
            collider.enabled = false;
    }

    private void Update()
    {
        if (!isDead)
            return;

        deathTime += Time.deltaTime;
        animator.SetBool("Run", false);
        rb.linearVelocity = Vector2.zero;
        Transform player = EnemyRegistry.GetPlayer();

        if (player != null && deathTime < corpseLifetime && Vector2.Distance(transform.position, player.position) <= interactionRange && Input.GetKeyDown(KeyCode.Q))
        {
            ConvertToBro();
            return;
        }

        if (deathTime > corpseLifetime)
        {
            CharismaManager.Instance.AddCharisma(1);
            Destroy(gameObject);
        }
    }

    public void ConvertToBro()
    {
        GameObject bro = Instantiate(broPrefab, transform.position, Quaternion.identity);
        EnemyRegistry.RegisterBro(bro.transform);
        Destroy(gameObject);
    }

    private void PlayDamageSound()
    {
        AudioClip clip = characterType switch
        {
            CharacterType.Grunt => gruntDamageSoundClip,
            CharacterType.Molly => mollyDamageSoundClip,
            CharacterType.Eddie => eddieDamageSoundClip,
            _ => null
        };

        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    public void ResetState()
    {
        currentHealth = maxHealth;
        isDead = false;
        deathTime = 0f;

        if (collider != null)
            collider.enabled = true;

        if (enemyFollow != null)
            enemyFollow.enabled = true;
    }
}
