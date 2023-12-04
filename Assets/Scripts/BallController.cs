using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float shotCooldown;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ChargeBar chargeBar;
    [SerializeField] private ChargeBar powerBar;
    [SerializeField] private Animator transitionAnimator;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;

    private float powerUpTime = 0f;
    private float timeUntilReady = 0f;
    private Vector2 mouseStartPos;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = false;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeUntilReady > 0)
        {
            timeUntilReady -= Time.deltaTime;
        }
        if (powerUpTime > 0)
        {
            powerUpTime -= Time.deltaTime;
        }
        chargeBar.ChargeFraction = timeUntilReady / shotCooldown;
        powerBar.ChargeFraction = powerUpTime / 2;

        if (Input.GetMouseButtonDown(0) && timeUntilReady <= 0 && !dead)
        {
            lineRenderer.enabled = true;
            mouseStartPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mouseCurrentPos = Input.mousePosition;
            Vector2 mouseMoveVector = mouseCurrentPos - mouseStartPos;
            Vector2 lineStart = (Vector2)transform.position + mouseMoveVector.normalized;
            Vector2 lineEnd = lineStart + mouseMoveVector * 0.01f;
            lineRenderer.SetPositions(new Vector3[] { lineStart, lineEnd });
        }

        if (Input.GetMouseButtonUp(0) && timeUntilReady <= 0 && lineRenderer.enabled)
        {
            timeUntilReady = shotCooldown;
            lineRenderer.enabled = false;

            Vector2 mouseEndPos = Input.mousePosition;
            Vector2 mouseMoveVector = mouseEndPos - mouseStartPos;
            rb.AddForce(-mouseMoveVector * force, ForceMode2D.Impulse);
        }

        spriteRenderer.color = powerUpTime > 0 ? new Color32(213, 140, 226, 255) : Color.white;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (powerUpTime > 0)
            {
                Destroy(collision.gameObject);
                AudioManager.Instance.PlayEffect("Kill");
                powerUpTime = Mathf.Clamp(powerUpTime + 0.4f, 0, 2);
                GameManager.Instance.IncrementScore(100);
            }
            else
            {
                StartCoroutine(GameOver());
                AudioManager.Instance.PlayEffect("Death");
            }
        }
        else if (collision.gameObject.tag == "Wall")
        {
            powerUpTime = 2;
            AudioManager.Instance.PlayEffect("Bounce");
        }
    }

    private IEnumerator GameOver()
    {
        dead = true;
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;
        yield return new WaitForSeconds(1);
        yield return SceneTransition.Run(transitionAnimator, "Menu");
    }
}
