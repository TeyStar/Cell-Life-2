using System.Collections.Generic;
using UnityEngine;

public class WaterFlow : MonoBehaviour
{
    public List<Rigidbody2D> cellsInCollider = new List<Rigidbody2D>();
    public FlowDirection flowDirection;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cell"))
        {
            Rigidbody2D cellRigidbody = other.GetComponent<Rigidbody2D>();
            if (cellRigidbody != null && !cellsInCollider.Contains(cellRigidbody))
            {
                cellsInCollider.Add(cellRigidbody);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cell"))
        {
            Rigidbody2D cellRigidbody = other.GetComponent<Rigidbody2D>();
            if (cellRigidbody != null)
            {
                cellsInCollider.Remove(cellRigidbody);
            }
        }
    }

    public void FlowTheWater(float flowPower)
    {
        Vector2 forceDirection = Vector2.zero;
        switch (flowDirection)
        {
            case FlowDirection.Up:
                forceDirection = Vector2.up;
                break;

            case FlowDirection.Down:
                forceDirection = Vector2.down;
                break;

            case FlowDirection.Left:
                forceDirection = Vector2.left;
                break;

            case FlowDirection.Right:
                forceDirection = Vector2.right;
                break;
        }
        foreach (Rigidbody2D cell in cellsInCollider)
            cell.AddForce(forceDirection * flowPower);
    }
}

public enum FlowDirection
{
    Up,
    Down,
    Left,
    Right
}