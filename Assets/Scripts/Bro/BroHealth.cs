using UnityEngine;

public class BroHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private CharacterType characterType;
    private int currentHealth;
    private Animator animator;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    [SerializeField] private AudioClip gruntDamageSoundClip;
    [SerializeField] private AudioClip mollyDamageSoundClip;
    [SerializeField] private AudioClip eddieDamageSoundClip;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        currentHealth -= damage;
        PlayDamageSound();

        if (currentHealth <= 0)
        {
            EnemyRegistry.UnregisterBro(transform);
            animator.SetTrigger("Die");
            Destroy(gameObject, 1f);
        }
        else
        {
            rb.AddForce(hitDirection * 2f, ForceMode2D.Impulse);
        }
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
}
