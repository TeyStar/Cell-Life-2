using UnityEngine;

public class PoisonBlob : MonoBehaviour
{
    public string team;
    public Color color;
    public int maxFrames = 50;
    private int framesInside = 0;
    public int lifeSpanMax = 500;
    private int lifeSpan;
    private SpriteRenderer sr;
    private int frameCounter = 0;
    public float slowDownFactor = 0.75f; // Factor to slow down objects

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
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

    private void SlowDownEffect(Rigidbody2D rb)
    {
        rb.linearVelocity *= slowDownFactor;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Cell cell = other.GetComponent<Cell>();
        if (cell != null && cell.team != team && cell.isAlive)
        {
            if (cell.isPoisonImmune)
            {
                Destroy(gameObject); // Other teams can destroy enemy blobs if they're immune.
            }
            else
            {
                SlowDownEffect(cell.rb);
                framesInside++;
                lifeSpan--;
                if (framesInside >= maxFrames)
                {
                    cell.Kill();
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Cell cell = other.GetComponent<Cell>();
        if (cell != null && cell.team != team)
        {
            framesInside = 0;
        }
    }
}