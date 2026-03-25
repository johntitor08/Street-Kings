using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    private readonly float attackCooldown = 0.5f;
    private float attackTime = 0f;
    private bool isAttacking = false;
    [SerializeField] private CharacterType characterType;
    [SerializeField] private AudioClip gruntHitSoundClip;
    [SerializeField] private AudioClip mollyHitSoundClip;
    [SerializeField] private AudioClip eddieHitSoundClip;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        attackTime += Time.deltaTime;

        if (attackTime >= attackCooldown)
            isAttacking = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy") || isAttacking)
            return;

        isAttacking = true;
        attackTime = 0f;
        Vector2 hitDir = (collision.transform.position - transform.position).normalized;
        var enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();

        if (enemyHealth != null && !enemyHealth.isDead)
        {
            enemyHealth.TakeDamage(1, hitDir);
            GetComponent<PlayerFlip>().FlipOnPunch(hitDir);
            animator.SetTrigger("Hit");
            PlayHitSound();
        }
    }

    private void PlayHitSound()
    {
        AudioClip clip = characterType switch
        {
            CharacterType.Grunt => gruntHitSoundClip,
            CharacterType.Molly => mollyHitSoundClip,
            CharacterType.Eddie => eddieHitSoundClip,
            _ => null
        };

        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
