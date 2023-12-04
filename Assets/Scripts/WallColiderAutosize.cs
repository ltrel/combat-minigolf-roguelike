using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallColiderAutosize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = spriteRenderer.size - Vector2.one * (2f / 16f);
    }
}
