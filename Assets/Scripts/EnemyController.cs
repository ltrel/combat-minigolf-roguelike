using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Random.insideUnitCircle * 4, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude < 4)
        {
            rb.AddForce(rb.velocity.normalized);
        }
    }
}
