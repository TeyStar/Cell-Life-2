using UnityEngine;

public class ElectricZap : MonoBehaviour
{
    public string team;
    public Color color;
    public Cell shooter;
    public float bulletSpeed = .5f;
    public float magnetMagnitude = 1f;
    public LayerMask wallLayer;
    public LayerMask cellLayer;
    public int electrifiedStacks = 25;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private int lifeSpan;
    public int lifeSpanMax = 500;
    private int frameCounter = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = -transform.up * bulletSpeed;
        sr = GetComponent<SpriteRenderer>();

        if (shooter != null)
        {
            Collider2D shooterCollider = shooter.GetComponent<Collider2D>();
            Collider2D zapCollider = GetComponent<Collider2D>();
            if (shooterCollider != null && zapCollider != null)
            {
                Physics2D.IgnoreCollision(zapCollider, shooterCollider);
            }
        }
    }

    private void Update()
    {
        lifeSpan++;
        if (lifeSpan >= lifeSpanMax)
        {
            Destroy(gameObject);
        }
        FlipColor();
        AdjustAlpha();
    }

    private void FlipColor()
    {
        frameCounter += Random.Range(0, 4);
        if (frameCounter < 5)
            return;
        frameCounter = 0;
        Color currentColor = sr.color;
        currentColor.a = color.a; // Ignore alpha in the comparison

        if (currentColor == color)
        {
            sr.color = new Color(1f, 1f, 1f, sr.color.a); // Set to white but keep the current alpha
        }
        else
        {
            sr.color = new Color(color.r, color.g, color.b, sr.color.a); // Set to original color but keep the current alpha
        }
    }

    private void AdjustAlpha()
    {
        float alpha = Mathf.Lerp(1f, 0.25f, (float)lifeSpan / lifeSpanMax);
        Color newColor = sr.color;
        newColor.a = alpha;
        sr.color = newColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && shooter.gameObject != null && collision.gameObject == shooter.gameObject)
        {
            return;
        }

        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            // Bounce off walls
            Vector2 reflectDir = Vector2.Reflect(rb.linearVelocity, collision.contacts[0].normal);
            rb.linearVelocity = -reflectDir.normalized * rb.linearVelocity.magnitude;
        }
        else if (((1 << collision.gameObject.layer) & cellLayer) != 0)
        {
            Cell cell = collision.gameObject.GetComponent<Cell>();
            if (cell != null)
            {
                if (cell.team == team)
                {
                    // Find both cells and drag them together
                    if (shooter != null)
                    {
                        var repel = (cell.isFed && shooter.isFed) ? 1 : -1;
                        Vector2 direction = (cell.transform.position - shooter.transform.position).normalized;
                        shooter.GetComponent<Rigidbody2D>().AddForce(direction * magnetMagnitude * repel, ForceMode2D.Impulse);
                        cell.GetComponent<Rigidbody2D>().AddForce(-direction * magnetMagnitude * repel, ForceMode2D.Impulse);
                    }
                }
                else if (!cell.isElectricImmune)
                {
                    BodyPart bodyPart = collision.gameObject.GetComponent<BodyPart>();
                    if (bodyPart != null && bodyPart.bodyPart == BodyPartType.Shell)
                    {
                        Destroy(gameObject);
                    }
                    else
                        // Add electrified stacks
                        cell.electrifiedStacks += electrifiedStacks;
                }
                Destroy(gameObject);
            }
        }
    }
}