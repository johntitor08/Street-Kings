using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EnemyFollow : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private EnemyHealth enemyHealth;
    private Transform target;
    private float speed = 5f;
    private float detectionRange = 15f;
    private float stoppingDistance = 1.5f;
    private float attackTime = 0f;
    private float attackCooldown = 1f;
    [SerializeField] private CharacterType characterType;
    [SerializeField] private AudioClip gruntHitSoundClip;
    [SerializeField] private AudioClip mollyHitSoundClip;
    [SerializeField] private AudioClip eddieHitSoundClip;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        enemyHealth = GetComponent<EnemyHealth>();

        switch (characterType)
        {
            case CharacterType.Grunt:
                speed = 5f; stoppingDistance = 2f; attackCooldown = 1f;
                break;

            case CharacterType.Molly:
                speed = 6f; stoppingDistance = 10f; attackCooldown = 5f;
                break;

            case CharacterType.Eddie:
                speed = 3f; stoppingDistance = 4f; attackCooldown = 3f;
                break;
        }
    }

    private void Update()
    {
        if (enemyHealth.isDead)
            return;

        FindTarget();
        attackTime += Time.deltaTime;

        if (target != null && Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position += (Vector3)(speed * Time.deltaTime * direction);
            spriteRenderer.flipX = direction.x > 0;
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);

            if (target == null || attackTime < attackCooldown)
                return;

            Vector2 attackDirection = (target.position - transform.position).normalized;

            if (target.CompareTag("Bro"))
                target.GetComponent<BroHealth>().TakeDamage(1, attackDirection);
            else if (target.CompareTag("Player"))
                target.GetComponent<PlayerHealth>().TakeDamage(1, attackDirection);

            animator.SetTrigger("Hit");
            attackTime = 0f;
            PlayHitSound();
        }
    }

    private void FindTarget()
    {
        float closestDistance = detectionRange;
        Transform closestTarget = null;

        foreach (Transform bro in EnemyRegistry.Bros)
        {
            if (bro == null)
                continue;

            float dist = Vector2.Distance(transform.position, bro.position);

            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestTarget = bro;
            }
        }

        Transform player = EnemyRegistry.GetPlayer();

        if (player != null)
        {
            float dist = Vector2.Distance(transform.position, player.position);

            if (dist < closestDistance)
                closestTarget = player;
        }

        target = closestTarget;
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
