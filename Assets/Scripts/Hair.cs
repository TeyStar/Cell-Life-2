using UnityEngine;

public class Hair : MonoBehaviour
{
    public string team;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cell"))
        {
            HairFunction(collision);
        }
    }

    private void HairFunction(Collision2D collision)
    {
        Cell otherCell = collision.gameObject.GetComponent<Cell>();
        if (otherCell != null)
        {
            Vector2 direction = (otherCell.transform.position - transform.position).normalized;
            if (otherCell.team != team)
            {
                // Set linearVelocity to push away cells that are not on the same team
                otherCell.rb.linearVelocity = direction * 2;
            }
            else
            {
                // Set linearVelocity to pull in cells that are on the same team
                otherCell.rb.linearVelocity = -direction * 2;
            }
        }
    }
}
