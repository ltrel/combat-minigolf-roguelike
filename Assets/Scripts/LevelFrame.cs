using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelFrame : MonoBehaviour
{
    public int Width;
    public int Height;

    [SerializeField] private SpriteRenderer border;
    [SerializeField] private SpriteRenderer backdrop;
    [SerializeField] private EdgeCollider2D edgeCollider;

    void Update()
    {
        border.size = new Vector2(Width * 4 + 2, Height * 4 + 2);
        backdrop.size = new Vector2 (Width * 8, Height * 8);

        Vector2 topRight = (Vector2) transform.position + new Vector2 (Width * 2, Height * 2);
        Vector2 bottomRight = (Vector2) transform.position + new Vector2 (Width * 2, -Height * 2);
        Vector2 bottomLeft = (Vector2) transform.position + new Vector2 (-Width * 2, -Height * 2);
        Vector2 topLeft = (Vector2) transform.position + new Vector2 (-Width * 2, Height * 2);

        edgeCollider.SetPoints(new List<Vector2> { topRight , bottomRight, bottomLeft, topLeft, topRight });
    }

    public Vector3 RandomPositionInside(float xPadding, float yPadding)
    {
        float randomX = Random.Range(-Width * 2 + xPadding, Width * 2 - xPadding);
        float randomY = Random.Range(-Height * 2 + yPadding, Height * 2 - yPadding);
        return new Vector3(randomX, randomY, 0);
    }

    public Vector3 RandomPositionInside()
    {
        return RandomPositionInside(0, 0);
    }
}
