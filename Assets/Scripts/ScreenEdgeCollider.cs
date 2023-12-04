using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEdgeCollider : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;

    // Start is called before the first frame update
    void Start()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        StartCoroutine(setColliderEdges());
    }
    
    IEnumerator setColliderEdges()
    {
        yield return new WaitForEndOfFrame();

        Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Vector2 bottomRight = new Vector2(topRight.x, bottomLeft.y);
        Vector2 topLeft = new Vector2(bottomLeft.x, topRight.y);

        edgeCollider.SetPoints(new List<Vector2> {bottomLeft, topLeft, topRight, bottomRight, bottomLeft});
    }
}
