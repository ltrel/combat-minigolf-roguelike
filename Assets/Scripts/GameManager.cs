using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private BallController ballController;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject wallPrefab;

    private LevelFrame levelFrame;
    private int score;
    private float timeRemaining;
    private List<GameObject> walls = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        levelFrame = FindObjectOfType<LevelFrame>();
        timeRemaining = 60;
        score = 0;

        GenerateWalls();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining <= 0)
        {
            ballController.Kill(); 
        }

        timeRemaining -= Time.deltaTime;
        scoreText.text = score.ToString("D4") + "\n\n" + FormatTime(timeRemaining); 

        if (Input.GetKeyDown(KeyCode.R))
        {
            walls.ForEach((wall) => Destroy(wall.gameObject));
            walls.Clear();
            GenerateWalls();
        }
    }

    public void IncrementScore(int amount)
    {
        score += amount;
    }

    private string FormatTime(float seconds)
    {
        int integerSeconds = Mathf.CeilToInt(timeRemaining);
        int minutes = integerSeconds / 60;
        int remainderSeconds = integerSeconds % 60;
        return minutes + ":" + remainderSeconds.ToString("D2");
    }

    private void GenerateWalls()
    {
        float maxSideLength = 7.5f;
        float maxArea = 7.5f;

        int attempts = 0;
        while (walls.Count < 8 && attempts < 100)
        {
            Quaternion rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
            float width = Random.Range(1f, maxSideLength);
            float height = Random.Range(1f, maxArea / width);
            float randomX = Random.Range(-levelFrame.Width * 2, levelFrame.Width * 2);
            float randomY = Random.Range(-levelFrame.Height * 2, levelFrame.Height * 2);
            Vector3 position = new Vector3(randomX, randomY, 0);

            GameObject newWall = Instantiate(wallPrefab, position, rotation);
            SpriteRenderer wallSpriteRenderer = newWall.GetComponent<SpriteRenderer>();
            wallSpriteRenderer.size = new Vector2(width, height);

            float squaredMinClearance = maxSideLength * 4;
            bool playerClearance = (ballController.transform.position - position).sqrMagnitude > squaredMinClearance;
            bool wallsClearance = walls.All((wall) => (wall.transform.position - position).sqrMagnitude > squaredMinClearance);
            if (wallsClearance && playerClearance)
            {
                walls.Add(newWall);
            }
            else
            {
                Destroy(newWall);
            }
            attempts++;
        }
    }
}
