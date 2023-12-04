using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform followPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 target = followPoint.position * 0.5f;
        target.z = transform.position.z;

        // transform.position = Vector3.Lerp(transform.position, target, 0.003f);
        transform.position = target;
    }
}
