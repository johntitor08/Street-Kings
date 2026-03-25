using UnityEngine;
using Unity.Cinemachine;

public class PlayerHealth : MonoBehaviour
{
    private Rigidbody2D rb;
    private int currentHealth = 10;
    private Animator animator;
    private AudioSource audioSource;
    private CinemachineCamera vcam;
    [SerializeField] private CharacterType characterType;
    [SerializeField] private AudioClip gruntDamageSoundClip;
    [SerializeField] private AudioClip mollyDamageSoundClip;
    [SerializeField] private AudioClip eddieDamageSoundClip;
    [SerializeField] private AudioClip gruntHitSoundClip;
    [SerializeField] private AudioClip mollyHitSoundClip;
    [SerializeField] private AudioClip eddieHitSoundClip;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = 10;
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
        else
            rb.AddForce(hitDirection * 2f, ForceMode2D.Impulse);
    }

    private void Die()
    {
        GameObject newPlayer = GameObject.FindGameObjectWithTag("Bro");

        if (newPlayer != null && vcam == null)
        {
            newPlayer.tag = "Player";
            newPlayer.GetComponent<SpriteRenderer>().color = Color.white;

            if (newPlayer.GetComponent<PlayerHealth>() == null)
                newPlayer.AddComponent<PlayerHealth>();

            if (newPlayer.GetComponent<PlayerMovement>() == null)
                newPlayer.AddComponent<PlayerMovement>();

            if (newPlayer.GetComponent<PlayerFlip>() == null)
                newPlayer.AddComponent<PlayerFlip>();

            if (newPlayer.GetComponent<BroFollow>() != null)
                Destroy(newPlayer.GetComponent<BroFollow>());

            if (newPlayer.GetComponent<BroHealth>() != null)
                Destroy(newPlayer.GetComponent<BroHealth>());

            EnemyRegistry.UnregisterBro(newPlayer.transform);
            vcam = Object.FindFirstObjectByType<CinemachineCamera>();

            if (vcam != null)
            {
                vcam.Follow = newPlayer.transform;
                vcam.LookAt = newPlayer.transform;
            }
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
            return;

        Vector2 hitDir = (collision.transform.position - transform.position).normalized;
        collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(1, hitDir);
        GetComponent<PlayerFlip>().FlipOnPunch(hitDir);
        animator.SetTrigger("Hit");
        PlayHitSound(characterType);
        PlayDamageSoundForEnemy(collision.gameObject.GetComponent<CharacterIdentity>()?.characterType);
    }

    private void PlayHitSound(CharacterType type)
    {
        AudioClip clip = type switch
        {
            CharacterType.Grunt => gruntHitSoundClip,
            CharacterType.Molly => mollyHitSoundClip,
            CharacterType.Eddie => eddieHitSoundClip,
            _ => null
        };

        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    private void PlayDamageSoundForEnemy(CharacterType? type)
    {
        if (type == null)
            return;

        AudioClip clip = type switch
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
