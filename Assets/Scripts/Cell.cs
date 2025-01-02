using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField]
    public BodyPart[] bodyParts;

    [SerializeField]
    public bool isAlive;

    [SerializeField]
    public string team;

    [SerializeField]
    public bool isFed;

    [SerializeField]
    public int generation;

    private int mateCounter;

    public Rigidbody2D rb;

    [SerializeField]
    private SpriteRenderer sr;

    [SerializeField]
    private float wiggleSpeed;

    [SerializeField]
    public bool hasMovement;

    [SerializeField]
    public bool hasVision;

    [SerializeField]
    public bool hasMouth;

    public bool isPoisonImmune;
    public int poisonedStacks;

    public bool isElectricImmune;
    public int electrifiedStacks;

    public int eggSacks;
    public int eggsEaten;
    public List<DNABlueprint> eatenDNA = new List<DNABlueprint>();

    public DNABlueprint dna;

    public int lifeSpan;
    public int deathSpan;

    private void Start()
    {
        for (int i = 0; i < bodyParts.Length; i++)
            bodyParts[i].bodyPart = dna.bodyParts[i];
        SetTeam(dna.team);
        generation = dna.generation;
    }

    private float RandomWiggle()
    {
        return Random.Range(-wiggleSpeed, wiggleSpeed);
    }

    public void Wiggle()
    {
        if (isAlive && rb != null)
        {
            rb.AddForce(Vector2.right * RandomWiggle(), ForceMode2D.Impulse);
            rb.AddForce(Vector2.up * RandomWiggle(), ForceMode2D.Impulse);
        }
    }

    public void BodyPartBehavior(int i)
    {
        if (isAlive)
            bodyParts[i].BodyPartBehavior();
    }

    public void SetTeam(string s)
    {
        switch (s)
        {
            case "Red":
                team = "Red";
                sr.color = Color.red;
                break;

            case "Blue":
                team = "Blue";
                sr.color = Color.blue;
                break;

            case "Green":
                team = "Green";
                sr.color = Color.green;
                break;

            case "Yellow":
                team = "Yellow";
                sr.color = Color.yellow;
                break;

            case "Purple":
                team = "Purple";
                sr.color = new Color(0.5f, 0, 0.5f);
                break;

            case "Orange":
                team = "Orange";
                sr.color = new Color(1, 0.5f, 0);
                break;
        }

        foreach (BodyPart bodyPart in bodyParts)
            bodyPart.SetTeam(team);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cell") && isFed)
        {
            var otherCell = collision.gameObject.GetComponent<Cell>();
            if (otherCell.team == team && otherCell.isFed)
            {
                isFed = false;
                otherCell.isFed = false;
                Mate(otherCell.dna);
            }
        }
    }

    private void Mate(DNABlueprint otherCell)
    {
        bool willSurvive = eatenDNA.Count < 1;
        for (float i = 0; i < CountEggs(); i++)
        {
            var childDNA = new DNABlueprint();
            childDNA = childDNA.MixBlueprints(DNABlueprint(), otherCell);
            var myPosition = transform.position;

            var childPosition = new Vector2(Random.Range(-i, i) + myPosition.x, Random.Range(-i, i) + myPosition.y);
            var child = Instantiate(gameObject, transform.position, Quaternion.identity);
            var cell = child.GetComponent<Cell>();
            child.transform.SetParent(GetComponentInParent<Tank>().transform);
            child.transform.localScale = transform.localScale * Random.Range(0.9f, 1.1f);
            cell.dna = childDNA;
        }
        if (!willSurvive)
            Kill();
        if (mateCounter < 1)
            mateCounter++;
        else Kill();
        lifeSpan = 0;
    }

    private float CountEggs()
    {
        var eggCount = 2;
        foreach (BodyPart bodyPart in bodyParts)
            if (bodyPart.bodyPart == BodyPartType.Egg)
                eggCount++;
        eggCount -= poisonedStacks;
        return eggCount;
    }

    private DNABlueprint DNABlueprint()
    {
        if (eatenDNA.Count == 0)
        {
            return dna;
        }
        else
        {
            DNABlueprint firstEatenDNA = eatenDNA[0];
            eatenDNA.RemoveAt(0);
            return firstEatenDNA;
        }
    }

    public void Kill()
    {
        isAlive = false;

        Color originalColor = sr.color;
        Color grayColor = Color.gray;
        Color blendedColor = new Color(
            (2 * grayColor.r + originalColor.r) / 3,
            (2 * grayColor.g + originalColor.g) / 3,
            (2 * grayColor.b + originalColor.b) / 3
        );

        sr.color = blendedColor;

        foreach (BodyPart bodyPart in bodyParts)
        {
            if (bodyPart.bodyPart == BodyPartType.Glue)
            {
                HingeJoint2D joint = bodyPart.GetComponent<HingeJoint2D>();
                if (joint != null)
                    Destroy(joint);
            }
        }
    }
}