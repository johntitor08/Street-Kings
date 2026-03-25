using UnityEngine;

public class PlayerFlip : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");

        if (h > 0f)
            spriteRenderer.flipX = true;
        else if (h < 0f)
            spriteRenderer.flipX = false;
    }

    public void FlipOnPunch(Vector2 punchDirection)
    {
        if (punchDirection.x > 0)
            spriteRenderer.flipX = true;
        else if (punchDirection.x < 0)
            spriteRenderer.flipX = false;
    }
}
