using UnityEngine;

public class DNABlueprint
{
    public BodyPartType[] bodyParts = new BodyPartType[4];
    public string team;
    public int generation;
    public bool hasMovement;
    public bool hasVision;
    public bool hasMouth;
    public bool doCoinFlip = true;

    public void BuildBlueprint()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!hasMovement && CoinFlip())
            {
                bodyParts[i] = (BodyPartType)Random.Range(1, 5);
            }
            else if (!hasVision && CoinFlip())
            {
                bodyParts[i] = (BodyPartType)Random.Range(5, 9);
            }
            else if (!hasMouth && CoinFlip())
            {
                bodyParts[i] = (BodyPartType)Random.Range(7, 12);
                hasMouth = true;
            }
            else
                bodyParts[i] = (BodyPartType)Random.Range(0, 17);
            PartsCheckList(bodyParts[i]);
        }
    }

    // The public use of it in Tank might not be good.
    public void Evolve()
    {
        bool hasEvolved = false;
        for (int i = 0; i < bodyParts.Length && !hasEvolved; i++)
        {
            if (bodyParts[i] == BodyPartType.Nothing && CoinFlip())
            {
                bodyParts[i] = (BodyPartType)Random.Range(0, 17);
                hasEvolved = true;
            }
            else if (CoinFlip())
            {
                bodyParts[i] = (BodyPartType)Random.Range(0, 17); hasEvolved = true;
                hasEvolved = true;
            }
        }
    }

    private void PartsCheckList(BodyPartType bodyPart)
    {
        if (bodyPart == BodyPartType.Flagella || bodyPart == BodyPartType.Cilia || bodyPart == BodyPartType.Jet || bodyPart == BodyPartType.Glue)
            hasMovement = true;
        else if (bodyPart == BodyPartType.Eye || bodyPart == BodyPartType.Stalk)
            hasVision = true;
        else if (bodyPart == BodyPartType.Photosynthesizer || bodyPart == BodyPartType.Chemosynthesizer)
        {
            hasMouth = true; hasVision = true;
        }
        else if (bodyPart == BodyPartType.Jaw || bodyPart == BodyPartType.Filter || bodyPart == BodyPartType.Proboscis)
            hasMouth = true;
        else if (bodyPart == BodyPartType.Electric) { hasMovement = true; hasVision = true; }
    }

    public DNABlueprint MixBlueprints(DNABlueprint dad, DNABlueprint mom)
    {
        DNABlueprint child = new DNABlueprint();
        if (dad.generation > mom.generation)
            child.generation = dad.generation + 1;
        else
            child.generation = mom.generation + 1;
        child.team = dad.team;
        for (int i = 0; i < 4; i++)
        {
            if (CoinFlip())
                child.bodyParts[i] = dad.bodyParts[i];
            else
                child.bodyParts[i] = mom.bodyParts[i];
        }
        child.Evolve();
        return child;
    }

    private bool CoinFlip()
    {
        if (doCoinFlip)
            return Random.Range(0, 2) == 0;
        else return true;
    }
}