using UnityEngine;

public class BroFollow : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Transform target;
    private float speed = 5f;
    private float detectionRange = 15f;
    private float stoppingDistance = 2f;
    private float attackTime = 0f;
    private float attackCooldown = 1f;
    [SerializeField] private CharacterType characterType;
    [SerializeField] private AudioClip gruntHitSoundClip;
    [SerializeField] private AudioClip mollyHitSoundClip;
    [SerializeField] private AudioClip eddieHitSoundClip;

    private void OnEnable() => EnemyRegistry.RegisterBro(transform);

    private void OnDisable() => EnemyRegistry.UnregisterBro(transform);

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

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

            if (target == null || !target.CompareTag("Enemy") || attackTime < attackCooldown)
                return;

            var enemyHealth = target.GetComponent<EnemyHealth>();

            if (enemyHealth == null || enemyHealth.isDead)
                return;

            enemyHealth.TakeDamage(1, (target.position - transform.position).normalized);
            animator.SetTrigger("Hit");
            attackTime = 0f;
            PlayHitSound();
        }
    }

    private void FindTarget()
    {
        float closestDistance = detectionRange;
        Transform closestEnemy = null;

        foreach (Transform enemy in EnemyRegistry.Enemies)
        {
            if (enemy == null) 
                continue;

            float dist = Vector2.Distance(transform.position, enemy.position);

            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy;
            return;
        }

        Transform player = EnemyRegistry.GetPlayer();
        target = player;
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
