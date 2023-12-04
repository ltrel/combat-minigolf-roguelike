using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBar : MonoBehaviour
{
    public float ChargeFraction = 0f;

    [SerializeField] private RectTransform fillRect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fillRect.sizeDelta = new Vector2(-400f + ChargeFraction * 400f, 0);
    }
}
