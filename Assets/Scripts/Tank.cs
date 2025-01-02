using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tank : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Cell[] cells;

    [SerializeField]
    private int counterMax;

    private int counter;

    [SerializeField]
    private GameObject cellObject;

    private float borders = 0.45f;

    [SerializeField]
    private int cellCount;

    private int teamMax = 6;
    private int teamCounter;
    private DNABlueprint redDNA;
    private DNABlueprint blueDNA;
    private DNABlueprint greenDNA;
    private DNABlueprint yellowDNA;
    private DNABlueprint purpleDNA;
    private DNABlueprint orangeDNA;

    [SerializeField]
    private float gameSpeed;

    [SerializeField]
    private int maxLifeSpan;

    [SerializeField]
    private int maxDeathSpan;

    [SerializeField]
    private WaterFlow[] waterFlows;

    [SerializeField]
    private float waterFlowSpeed;

    private void Start()
    {
        Application.targetFrameRate = 32;
        BuildDNA();
        for (int i = 0; i < cellCount; i++)
        {
            var placement = new Vector3(Random.Range(-borders, borders), Random.Range(-borders, borders), -1);
            var randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f)); // Generate a random rotation
            GameObject cell = Instantiate(cellObject, placement, randomRotation); // Use the random rotation
            cell.transform.SetParent(transform);
            cell.transform.localPosition = placement;
            cell.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            SetTeam(cell);
        }
        StartCoroutine(UpdateCoroutine());
    }

    private void BuildDNA()
    {
        redDNA = new DNABlueprint();
        redDNA.team = "Red";
        redDNA.BuildBlueprint();
        blueDNA = new DNABlueprint();
        blueDNA.team = "Blue";
        blueDNA.BuildBlueprint();
        greenDNA = new DNABlueprint();
        greenDNA.team = "Green";
        greenDNA.BuildBlueprint();
        yellowDNA = new DNABlueprint();
        yellowDNA.team = "Yellow";
        yellowDNA.BuildBlueprint();
        purpleDNA = new DNABlueprint();
        purpleDNA.team = "Purple";
        purpleDNA.BuildBlueprint();
        orangeDNA = new DNABlueprint();
        orangeDNA.team = "Orange";
        orangeDNA.BuildBlueprint();
    }

    private void SetTeam(GameObject gameObject)
    {
        var cell = gameObject.GetComponent<Cell>();
        switch (teamCounter)
        {
            case 0:
                cell.SetTeam("Red");
                cell.dna = redDNA;
                break;

            case 1:
                cell.SetTeam("Blue");
                cell.dna = blueDNA;
                break;

            case 2:
                cell.SetTeam("Green");
                cell.dna = greenDNA;
                break;

            case 3:
                cell.SetTeam("Yellow");
                cell.dna = yellowDNA;
                break;

            case 4:
                cell.SetTeam("Purple");
                cell.dna = purpleDNA;
                break;

            case 5:
                cell.SetTeam("Orange");
                cell.dna = orangeDNA;
                break;
        }

        teamCounter++;
        if (teamCounter >= teamMax)
        {
            teamCounter = 0;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            gameSpeed -= 0.01f;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            gameSpeed += 0.01f;
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Input.GetKeyDown(KeyCode.W))
            _camera.transform.position += Vector3.up * 10;
        if (Input.GetKeyDown(KeyCode.S))
            _camera.transform.position += Vector3.down * 10;
        if (Input.GetKeyDown(KeyCode.A))
            _camera.orthographicSize--;
        if (Input.GetKeyDown(KeyCode.D))
            _camera.orthographicSize++;
        if (Input.GetKeyDown(KeyCode.Q))
            waterFlowSpeed -= 10;
        if (Input.GetKeyDown(KeyCode.E))
            waterFlowSpeed += 10;
    }

    private void LagFix()
    {
        int excessCells = cells.Length - (cellCount *2);

        if (excessCells > 0)
        {
            for (int i = 0; i < excessCells; i++)
            {
                if (cells[i] != null)
                {
                    Destroy(cells[i].gameObject);
                }
            }
        }
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            switch (counter)
            {
                case 0:
                    GetCells();
                    break;

                case 1:
                    Wiggle();
                    break;

                case 2:
                    BodyPartBehavior(0);
                    break;

                case 3:
                    BodyPartBehavior(1);
                    break;

                case 4:
                    BodyPartBehavior(2);
                    break;

                case 5:
                    BodyPartBehavior(3);
                    break;

                case 6:
                    Flow(0);
                    Photosynthesize();
                    break;

                case 7:
                    Flow(1);
                    break;

                case 8:
                    Flow(2);
                    Chemosynthesize();
                    break;

                case 9:
                    Flow(3);
                    break;
            }
            counter++;
            if (counter > counterMax)
            {
                counter = 0;
                foreach (Cell c in cells)
                {
                    if (c != null && c.isAlive)
                    {
                        c.lifeSpan += Random.Range(-1, 5);
                        if (c.lifeSpan > maxLifeSpan)
                            if (c.isFed)
                            {
                                c.isFed = false; c.lifeSpan = 0;
                            } // If the cell is fed, reset the lifeSpan, otherwise kill the cell
                            else
                                c.Kill();
                    }
                    else if (c != null)
                        {
                        c.deathSpan++;
                        if (c.deathSpan > maxDeathSpan)
                        {
                            var cellObj = c.gameObject;
                            if (cellObj)
                            {
                                Destroy(cellObj);
                            }
                        }
                    }
                }
                CheckTeams();
            }

            LagFix();

            yield return new WaitForSeconds(gameSpeed);
        }
    }

    private void GetCells()
    {
        cells = GetComponentsInChildren<Cell>();
    }

    private void Wiggle()
    {
        foreach (Cell cell in cells)
        {
            cell.Wiggle();
        }
    }

    private void BodyPartBehavior(int i)
    {
        foreach (Cell cell in cells)
        {
            cell.BodyPartBehavior(i);
        }
    }

    private void Flow(int i)
    {
        waterFlows[i].FlowTheWater(waterFlowSpeed);
    }

    private void Photosynthesize()
    {
        foreach (Rigidbody2D rd in waterFlows[0].cellsInCollider)
        {
            Cell cell = rd.GetComponent<Cell>();
            if (cell != null)
            {
                foreach (BodyPart bodyPart in cell.bodyParts)
                {
                    if (bodyPart.bodyPart == BodyPartType.Photosynthesizer)
                    {
                        bodyPart.Photosynthesize();
                    }
                }
            }
        }
    }

    private void Chemosynthesize()
    {
        foreach (Rigidbody2D rd in waterFlows[0].cellsInCollider)
        {
            Cell cell = rd.GetComponent<Cell>();
            if (cell != null)
            {
                foreach (BodyPart bodyPart in cell.bodyParts)
                {
                    if (bodyPart.bodyPart == BodyPartType.Chemosynthesizer)
                    {
                        bodyPart.Chemosynthesize();
                    }
                }
            }
        }
    }

    private void Respawn(DNABlueprint dna)
    {
        var startCellCount = cellCount / teamMax;
        for (int i = 0; i < startCellCount; i++)
        {
            var placement = new Vector3(Random.Range(-borders, borders), Random.Range(-borders, borders), -1);
            var randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f)); // Generate a random rotation
            GameObject cell = Instantiate(cellObject, placement, randomRotation); // Use the random rotation
            cell.transform.SetParent(transform);
            cell.transform.localPosition = placement;
            cell.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            cell.GetComponent<Cell>().dna = dna;
        }
    }

    private void CheckTeams()
    {
        if (!CheckTeamLivingStatus("Red"))
        {
            redDNA.Evolve();
            redDNA.BuildBlueprint();
            Respawn(redDNA);
        }
        if (!CheckTeamLivingStatus("Blue"))
        {
            blueDNA.Evolve();
            blueDNA.BuildBlueprint();
            Respawn(blueDNA);
        }
        if (!CheckTeamLivingStatus("Green"))
        {
            greenDNA.Evolve();
            greenDNA.BuildBlueprint();
            Respawn(greenDNA);
        }
        if (!CheckTeamLivingStatus("Yellow"))
        {
            yellowDNA.Evolve();
            yellowDNA.BuildBlueprint();
            Respawn(yellowDNA);
        }
        if (!CheckTeamLivingStatus("Purple"))
        {
            purpleDNA.Evolve();
            purpleDNA.BuildBlueprint();
            Respawn(purpleDNA);
        }
        if (!CheckTeamLivingStatus("Orange"))
        {
            orangeDNA.Evolve();
            orangeDNA.BuildBlueprint();
            Respawn(orangeDNA);
        }
    }

    private bool CheckTeamLivingStatus(string team)
    {
        foreach (Cell cell in cells)
        {
            if (cell.isAlive && cell.team == team)
            {
                return true;
            }
        }
        return false;
    }
}