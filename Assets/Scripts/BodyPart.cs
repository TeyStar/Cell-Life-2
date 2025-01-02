using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField]
    public BodyPartType bodyPart;

    [SerializeField]
    private Cell cell;

    [SerializeField]
    private SpriteRenderer sr;

    [SerializeField]
    private BodyParts bodyParts;

    [SerializeField]
    private float flagellaMagnitude;

    [SerializeField]
    private int ciliaCounterMax;

    private int ciliaCounter;

    [SerializeField]
    private float ciliaMagnitude;

    [SerializeField]
    private int jetCounterMax;

    private int jetCounter;

    [SerializeField]
    private float jetMagnitude;

    [SerializeField]
    private bool isGlued;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private int eyeCounterMax;

    private int eyeCounter;

    [SerializeField]
    private float torquePower;

    [SerializeField]
    private CircleCollider2D stalkCircle;

    [SerializeField]
    private int stalkCounterMax;

    private int stalkCounter;

    [SerializeField]
    private GameObject hairPrefab;

    [SerializeField]
    private int photoMax;

    private int photoCounter;

    [SerializeField]
    private int chemoMax;

    private int chemoCounter;

    private Cell proboscisTaste;
    private int proboscisCounter;

    [SerializeField]
    private int proboscisMax;

    [SerializeField]
    private int poisonCounterMax;

    private int poisonCounter;

    [SerializeField]
    private GameObject poisonBlobPrefab;

    [SerializeField]
    private int electricCounterMax;

    private int electricCounter;

    [SerializeField]
    private GameObject electricZapPrefab;

    private void Start()
    {
        //SpawnPart();
        SetPart();
    }

    public void SpawnPart()
    {
        if (!cell.hasMovement && CoinFlip())
            bodyPart = (BodyPartType)Random.Range(0, 4);
        else if (!cell.hasVision && CoinFlip())
            bodyPart = (BodyPartType)Random.Range(4, 7);
        else if (!cell.hasMouth && CoinFlip())
            bodyPart = (BodyPartType)Random.Range(7, 12);
        else
            bodyPart = (BodyPartType)Random.Range(0, 16);
        SetPart();

        if (bodyPart == BodyPartType.Nothing)
            SpawnPart();
    }

    private bool CoinFlip()
    {
        return Random.Range(0, 2) == 0;
    }

    private void SetPart()
    {
        var oldCollider = GetComponent<Collider2D>();
        if (oldCollider != null)
            Destroy(oldCollider);

        switch (bodyPart)
        {
            case BodyPartType.Nothing:
                sr.sprite = null;
                break;

            case BodyPartType.Flagella:
                sr.sprite = bodyParts.flagella;
                cell.hasMovement = true;
                break;

            case BodyPartType.Cilia:
                sr.sprite = bodyParts.cilia;
                cell.hasMovement = true;
                break;

            case BodyPartType.Jet:
                sr.sprite = bodyParts.jet;
                cell.hasMovement = true;
                break;

            case BodyPartType.Glue:
                sr.sprite = bodyParts.glue;
                cell.hasMovement = true;
                gameObject.AddComponent<BoxCollider2D>();
                var glueBox = gameObject.GetComponent<BoxCollider2D>();
                glueBox.size = new Vector2(0.33f, 0.33f);
                break;

            case BodyPartType.Eye:
                sr.sprite = bodyParts.eye;
                cell.hasVision = true;
                break;

            case BodyPartType.Stalk:
                sr.sprite = bodyParts.stalk;
                cell.hasVision = true;
                gameObject.AddComponent<CircleCollider2D>();
                stalkCircle = gameObject.GetComponent<CircleCollider2D>();
                stalkCircle.radius = 5;
                stalkCircle.isTrigger = true;
                break;
            /*
        case BodyPartType.Hair:
            /* Back to the drawing board with this one.
            sr.sprite = bodyParts.hair;
            cell.hasVision = true;
            var hair = Instantiate(hairPrefab, transform.position, Quaternion.identity);
            hair.transform.parent = transform;
            foreach (var hairPart in hair.GetComponentsInChildren<SpriteRenderer>())
                hairPart.color = sr.color;
            foreach (var hairHindge in hair.GetComponents<HingeJoint2D>())
                if (hairHindge.connectedBody == null)
                    hairHindge.connectedBody = cell.rb;
            foreach (var hairComponent in hair.GetComponentsInChildren<Hair>())
                hairComponent.team = cell.team;
            SpawnPart();
            break;
            */
            case BodyPartType.Jaw:
                sr.sprite = bodyParts.jaw;
                cell.hasMouth = true;
                gameObject.AddComponent<BoxCollider2D>();
                var jawBox = gameObject.GetComponent<BoxCollider2D>();
                jawBox.size = new Vector2(0.25f, 0.25f);
                break;

            case BodyPartType.Filter:
                sr.sprite = bodyParts.filter;
                cell.hasMouth = true;
                gameObject.AddComponent<BoxCollider2D>();
                var filterBox = gameObject.GetComponent<BoxCollider2D>();
                filterBox.size = new Vector2(0.75f, 0.75f);
                break;

            case BodyPartType.Photosynthesizer:
                sr.sprite = bodyParts.photosynthesizer;
                cell.hasMouth = true;
                cell.hasVision = true;
                break;

            case BodyPartType.Chemosynthesizer:
                sr.sprite = bodyParts.chemosynthesizer;
                cell.hasMouth = true;
                cell.hasVision = true;
                break;

            case BodyPartType.Proboscis:
                sr.sprite = bodyParts.proboscis;
                cell.hasMouth = true;
                gameObject.AddComponent<BoxCollider2D>();
                var proboscisBox = gameObject.GetComponent<BoxCollider2D>();
                proboscisBox.size = new Vector2(0.1f, 2f);
                proboscisBox.isTrigger = true;
                break;

            case BodyPartType.Spike:
                sr.sprite = bodyParts.spike;
                gameObject.AddComponent<BoxCollider2D>();
                var spikeBox = gameObject.GetComponent<BoxCollider2D>();
                spikeBox.size = new Vector2(0.1f, 0.15f);
                cell.transform.localScale *= 0.75f;
                break;

            case BodyPartType.Shell:
                sr.sprite = bodyParts.shell;
                gameObject.AddComponent<BoxCollider2D>();
                var shellBox = gameObject.GetComponent<BoxCollider2D>();
                shellBox.size = new Vector2(0.3f, 0.3f);
                cell.transform.localScale *= 1.25f;
                break;

            case BodyPartType.Poison:
                sr.sprite = bodyParts.poison;
                cell.isPoisonImmune = true;
                break;

            case BodyPartType.Electric:
                sr.sprite = bodyParts.electric;
                cell.isElectricImmune = true;
                break;

            case BodyPartType.Egg:
                sr.sprite = bodyParts.egg;
                cell.eggSacks++;
                break;
        }
    }

    public void BodyPartBehavior()
    {
        if (Electrified() || cell == null || !cell.isAlive)
            return;
        if (cell.isAlive)
            switch (bodyPart)
            {
                case BodyPartType.Nothing:
                    break;

                case BodyPartType.Flagella:
                    Flagella();
                    break;

                case BodyPartType.Cilia:
                    Cilia();
                    break;

                case BodyPartType.Jet:
                    Jet();
                    break;

                case BodyPartType.Glue:
                    break;

                case BodyPartType.Eye:
                    Eye();
                    break;

                case BodyPartType.Stalk:
                    Stalk();
                    break;
                /*
                            case BodyPartType.Hair:
                                break;
                */
                case BodyPartType.Jaw:
                    break;

                case BodyPartType.Filter:
                    break;

                case BodyPartType.Photosynthesizer:
                    Photosynthesizer();
                    break;

                case BodyPartType.Chemosynthesizer:
                    Chemosynthesizer();
                    break;

                case BodyPartType.Proboscis:
                    break;

                case BodyPartType.Spike:
                    break;

                case BodyPartType.Shell:
                    break;

                case BodyPartType.Poison:
                    Poison();
                    break;

                case BodyPartType.Electric:
                    Electric();
                    break;

                case BodyPartType.Egg:
                    break;
            }
    }

    private void Flagella()
    {
        Vector2 forceDirection = cell.transform.position - transform.position;
        cell.rb.AddForce(forceDirection.normalized * flagellaMagnitude);
    }

    private void Cilia()
    {
        ciliaCounter++;
        if (ciliaCounter >= ciliaCounterMax)
        {
            ciliaCounter = 0;
            Vector2 forceDirection = cell.transform.position - transform.position;
            cell.rb.AddForce(forceDirection.normalized * ciliaMagnitude);
        }
    }

    private void Jet()
    {
        jetCounter++;
        if (jetCounter >= jetCounterMax)
        {
            jetCounter = 0;
            Vector2 forceDirection = cell.transform.position - transform.position;
            cell.rb.AddForce(forceDirection.normalized * jetMagnitude);
        }
    }

    private void Eye()
    {
        if (cell == null || !cell.isAlive)
            return;
        eyeCounter++;
        if (!target || eyeCounter >= eyeCounterMax)
        {
            eyeCounter = 0;
            // Define the raycast direction and distance
            Vector2 rayDirection = transform.up;
            float rayDistance = 100f;

            // Perform the raycast
            foreach (var hit in Physics2D.RaycastAll(transform.position, rayDirection, rayDistance))
            {
                Cell hitCell = hit.collider.GetComponent<Cell>();
                if (hitCell != null)
                {
                    if (cell.isFed)
                    {
                        // Look for a cell of the same team and generation
                        if (hitCell.team == cell.team && hitCell.generation == cell.generation)
                        {
                            target = hitCell.gameObject;
                            break;
                        }
                    }
                    else
                    {
                        // Look for a cell of a different team
                        if (hitCell.team != cell.team)
                        {
                            var hasFilter = false;
                            foreach (var part in cell.bodyParts)
                            {
                                if (part.bodyPart == BodyPartType.Filter)
                                    hasFilter = true;
                            }
                            if (hasFilter && !hitCell.isAlive || !hasFilter && hitCell.isAlive)
                            {
                                target = hitCell.gameObject;
                                break;
                            }
                        }
                    }
                }
            }
        }
        if (target != null)
        {
            Vector2 directionToTarget = (target.transform.position - transform.position).normalized;

            if (cell.hasMovement)
            {
                // Find the movement body part
                BodyPart movementPart = null;
                foreach (var part in cell.bodyParts)
                {
                    if (part.bodyPart == BodyPartType.Flagella || part.bodyPart == BodyPartType.Cilia || part.bodyPart == BodyPartType.Jet)
                    {
                        movementPart = part;
                        break;
                    }
                }

                if (movementPart != null)
                {
                    // Calculate the direction from the movement part to the target
                    Vector2 movementPartDirection = (target.transform.position - movementPart.transform.position).normalized;
                    float angle = Vector2.SignedAngle(cell.transform.up, movementPartDirection);
                    cell.rb.AddTorque(angle * torquePower); // Apply torque to rotate gradually
                }
                else
                {
                    // Fallback to the original direction if no movement part is found
                    float angle = Vector2.SignedAngle(cell.transform.up, directionToTarget);
                    cell.rb.AddTorque(angle * torquePower); // Apply torque to rotate gradually
                }
            }
            else
            {
                // Set the cell's rotation so that this eye is towards its target
                float angle = Vector2.SignedAngle(transform.up, directionToTarget);
                cell.rb.AddTorque(angle * torquePower); // Apply torque to rotate gradually
            }
        }
    }

    private void Stalk()
    {
        if (!stalkCircle) return;
        stalkCounter++;
        if (stalkCounter == stalkCounterMax)
            stalkCircle.enabled = true;
        else if (stalkCounter > stalkCounterMax)
        {
            stalkCircle.enabled = false;
            stalkCounter = 0;
        }
    }

    private void Hair(Collision2D collision)
    {
        Cell otherCell = collision.gameObject.GetComponent<Cell>();
        if (otherCell != null)
        {
            Vector2 direction = (otherCell.transform.position - transform.position).normalized;
            if (otherCell.team != cell.team)
            {
                // Push away cells that are not on the same team
                otherCell.rb.AddForce(direction * flagellaMagnitude, ForceMode2D.Impulse);
            }
            else
            {
                // Pull in cells that are on the same team
                otherCell.rb.AddForce(-direction * flagellaMagnitude, ForceMode2D.Impulse);
            }
        }
    }

    private void Jaw(Collision2D collision)
    {
        Cell otherCell = collision.gameObject.GetComponent<Cell>();
        if (otherCell != null)
        {
            if (otherCell.team != cell.team && !cell.isFed && otherCell.isAlive)
            {
                Poisoning(otherCell);
                Eggified(otherCell);
                cell.isFed = true;
                otherCell.Kill();
                cell.lifeSpan = 0;
            }
        }
    }

    private void Filter(Collision2D collision)
    {
        Cell otherCell = collision.gameObject.GetComponent<Cell>();
        if (otherCell != null)
        {
            if (otherCell.team != cell.team && !cell.isFed && !otherCell.isAlive)
            {
                Poisoning(otherCell);
                Eggified(otherCell);
                cell.isFed = true;
                otherCell.transform.position = new Vector2(1000, 1000);
                cell.lifeSpan = 0;
            }
        }
    }

    public void Photosynthesize()
    {
        if (!cell.isFed)
        {
            photoCounter++;
            if (cell.lifeSpan > 0)
                cell.lifeSpan--;

            if (photoCounter >= photoMax)
            {
                cell.isFed = true;
                cell.lifeSpan = 0;
                photoCounter = 0;
            }
        }
    }

    private void Photosynthesizer()
    {
        Vector2 directionToMove = cell.isFed ? Vector2.down : Vector2.up;

        if (cell.hasMovement)
        {
            // Find the movement body part
            BodyPart movementPart = null;
            foreach (var part in cell.bodyParts)
            {
                if (part.bodyPart == BodyPartType.Flagella || part.bodyPart == BodyPartType.Cilia || part.bodyPart == BodyPartType.Jet)
                {
                    movementPart = part;
                    break;
                }
            }

            if (movementPart != null)
            {
                // Calculate the direction from the movement part to the desired direction
                Vector2 movementPartDirection = (movementPart.transform.position - transform.position).normalized;
                float angle = Vector2.SignedAngle(cell.transform.up, directionToMove);
                cell.rb.AddTorque(angle * torquePower); // Apply torque to rotate gradually
            }
            else
            {
                // Fallback to the original direction if no movement part is found
                float angle = Vector2.SignedAngle(cell.transform.up, directionToMove);
                cell.rb.AddTorque(angle * torquePower); // Apply torque to rotate gradually
            }
        }
        else
        {
            // Set the cell's rotation so that it moves in the desired direction
            float angle = Vector2.SignedAngle(transform.up, directionToMove);
            cell.rb.AddTorque(angle * torquePower); // Apply torque to rotate gradually
        }
    }

    public void Chemosynthesize()
    {
        if (!cell.isFed)
        {
            chemoCounter++;
            if (cell.lifeSpan > 0)
                cell.lifeSpan--;

            if (chemoCounter >= chemoMax)
            {
                cell.isFed = true;
                cell.lifeSpan = 0;
                chemoCounter = 0;
            }
        }
    }

    private void Chemosynthesizer()
    {
        Vector2 directionToMove = cell.isFed ? Vector2.up : Vector2.down;

        if (cell.hasMovement)
        {
            // Find the movement body part
            BodyPart movementPart = null;
            foreach (var part in cell.bodyParts)
            {
                if (part.bodyPart == BodyPartType.Flagella || part.bodyPart == BodyPartType.Cilia || part.bodyPart == BodyPartType.Jet)
                {
                    movementPart = part;
                    break;
                }
            }

            if (movementPart != null)
            {
                // Calculate the direction from the movement part to the desired direction
                Vector2 movementPartDirection = (movementPart.transform.position - transform.position).normalized;
                float angle = Vector2.SignedAngle(cell.transform.up, directionToMove);
                cell.rb.AddTorque(angle * torquePower); // Apply torque to rotate gradually
            }
            else
            {
                // Fallback to the original direction if no movement part is found
                float angle = Vector2.SignedAngle(cell.transform.up, directionToMove);
                cell.rb.AddTorque(angle * torquePower); // Apply torque to rotate gradually
            }
        }
        else
        {
            // Set the cell's rotation so that it moves in the desired direction
            float angle = Vector2.SignedAngle(transform.up, directionToMove);
            cell.rb.AddTorque(angle * torquePower); // Apply torque to rotate gradually
        }
    }

    private void Poison()
    {
        poisonCounter++;
        if (poisonCounter >= poisonCounterMax)
        {
            poisonCounter = 0;
            var poisonBlob = Instantiate(poisonBlobPrefab, transform.position, Quaternion.identity);

            var poison = poisonBlob.GetComponent<PoisonBlob>();
            poison.team = cell.team;
            poison.color = sr.color;
        }
    }

    private void Electric()
    {
        electricCounter++;
        if (electricCounter >= electricCounterMax)
        {
            electricCounter = 0;
            var electricZap = Instantiate(electricZapPrefab, transform.position, transform.rotation);
            var electric = electricZap.GetComponent<ElectricZap>();
            electric.shooter = cell;
            electric.team = cell.team;
            electric.color = sr.color;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cell"))
        {
            if (cell.isAlive)
            {
                if (!isGlued && bodyPart == BodyPartType.Glue)
                {
                    Glue(collision);
                }
                /*
                if (bodyPart == BodyPartType.Hair)
                {
                    Hair(collision);
                }
                */
                if (bodyPart == BodyPartType.Jaw)
                {
                    Jaw(collision);
                }
                if (bodyPart == BodyPartType.Filter)
                {
                    Filter(collision);
                }
            }
            if (bodyPart == BodyPartType.Spike)
            {
                Spike(collision);
            }
            if (bodyPart == BodyPartType.Shell)
            {
                Shell(collision);
            }
        }
    }

    private void Glue(Collision2D collision)
    {
        var otherCell = collision.gameObject.GetComponent<Cell>();
        if (otherCell != null)
        {
            if (otherCell.team != cell.team)
            {
                HingeJoint2D joint = cell.gameObject.AddComponent<HingeJoint2D>();
                joint.connectedBody = otherCell.gameObject.GetComponent<Rigidbody2D>();
                joint.autoConfigureConnectedAnchor = true;
                joint.useMotor = false;
                joint.useLimits = true;
                JointAngleLimits2D limits = joint.limits;
                limits.min = -10; // Lower limit
                limits.max = 10;  // Upper limit
                joint.limits = limits;
                joint.enableCollision = false;
                joint.breakTorque = 500;
                joint.breakForce = 500;
                isGlued = true;
            }
        }
    }

    private void Spike(Collision2D collision)
    {
        var otherCell = collision.gameObject.GetComponent<Cell>();
        if (otherCell != null)
            if (otherCell.team != cell.team)
            {
                otherCell.Kill();
                gameObject.SetActive(false);
            }
    }

    private void Shell(Collision2D collision)
    {
        var otherCell = collision.gameObject.GetComponent<Cell>();
        if (otherCell != null)
            if (otherCell.team != cell.team)
            {
                // Apply the collision's relative velocity to the other cell in the opposite direction like it hit a wall.
                Vector2 relativeVelocity = collision.relativeVelocity;
                Vector2 forceDirection = -relativeVelocity.normalized;
                float magnitudeCheck = relativeVelocity.magnitude * 0.025f;
                float forceMagnitude = magnitudeCheck < 1 ? magnitudeCheck : 1;

                otherCell.rb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);

                // Also apply the same force to the other cell's torque to make it spin.
                float torque = forceMagnitude * 10f; // Adjust the multiplier as needed
                otherCell.rb.AddTorque(torque, ForceMode2D.Impulse);
            }
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        isGlued = false;
        SetPart();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (cell.isAlive)
        {
            if (bodyPart == BodyPartType.Stalk)
                HandleThreatDetection(other);
            if (bodyPart == BodyPartType.Proboscis)
                Proboscis(other);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (cell.isAlive)
        {
            if (bodyPart == BodyPartType.Stalk)
                HandleThreatDetection(other);
            if (bodyPart == BodyPartType.Proboscis)
                Proboscis(other);
        }
    }

    private void Proboscis(Collider2D collision)
    {
        if (cell.isFed) return;
        Cell otherCell = collision.gameObject.GetComponent<Cell>();
        if (otherCell != null)
        {
            if (otherCell.team != cell.team && !cell.isFed && otherCell.isFed)
            {
                if (proboscisTaste != otherCell)
                {
                    proboscisTaste = otherCell;
                    proboscisCounter = 0;
                }
                else
                {
                    proboscisCounter++;
                    if (proboscisCounter >= proboscisMax)
                    {
                        cell.isFed = true;
                        otherCell.isFed = false;
                        proboscisCounter = 0;
                    }
                }
            }
        }
    }

    private void Poisoning(Cell otherCell)
    {
        if (cell.isPoisonImmune) return;
        foreach (var part in otherCell.bodyParts)
            if (part.bodyPart == BodyPartType.Poison)
                cell.poisonedStacks++;
    }

    private bool Electrified()
    {
        if (cell.isElectricImmune || cell.electrifiedStacks <= 0) return false;
        if (cell.electrifiedStacks >= 100)
        {
            cell.electrifiedStacks--;
            cell.Wiggle();
            return true;
        }
        if (Random.Range(1, 100) <= cell.electrifiedStacks)
        {
            cell.electrifiedStacks--;
            cell.Wiggle();
            return true;
        }
        else
        {
            cell.electrifiedStacks--;
            return false;
        }
    }

    private void Eggified(Cell otherCell)
    {
        int eggsEaten = 0;
        foreach (var part in otherCell.bodyParts)
            if (part.bodyPart == BodyPartType.Egg)
                eggsEaten++;
        if (eggsEaten > 0)
        {
            for (int i = 0; i < eggsEaten; i++)
                cell.eatenDNA.Add(otherCell.dna);
            cell.eggsEaten += eggsEaten;
        }
    }

    private void HandleThreatDetection(Collider2D other)
    {
        Cell otherCell = other.GetComponent<Cell>();
        if (otherCell != null && otherCell.team != cell.team)
        {
            foreach (var part in otherCell.bodyParts)
            {
                if (part.bodyPart == BodyPartType.Jaw || part.bodyPart == BodyPartType.Spike ||
                    part.bodyPart == BodyPartType.Poison || part.bodyPart == BodyPartType.Electric ||
                    part.bodyPart == BodyPartType.Glue)
                {
                    Vector2 directionAwayFromThreat = (transform.position - other.transform.position).normalized;
                    float angle = Mathf.Atan2(directionAwayFromThreat.y, directionAwayFromThreat.x) * Mathf.Rad2Deg - 90f;
                    cell.rb.rotation = angle; // Instantly set the rotation to face away from the threat
                    cell.rb.angularVelocity = 0f;
                    break;
                }
                Stalk();
            }
        }
    }

    public void SetTeam(string s)
    {
        switch (s)
        {
            case "Red":
                sr.color = Color.red;
                break;

            case "Blue":
                sr.color = Color.blue;
                break;

            case "Green":
                sr.color = Color.green;
                break;

            case "Yellow":
                sr.color = Color.yellow;
                break;

            case "Purple":
                sr.color = new Color(0.5f, 0, 0.5f);
                break;

            case "Orange":
                sr.color = new Color(1, 0.5f, 0);
                break;
        }
    }
}

public enum BodyPartType
{
    Nothing,
    Flagella,
    Cilia,
    Jet,
    Glue,
    Eye,
    Stalk,
    Photosynthesizer,
    Chemosynthesizer,

    // Hair,
    Jaw,

    Filter,

    Proboscis,
    Spike,
    Shell,
    Poison,
    Electric,
    Egg
}