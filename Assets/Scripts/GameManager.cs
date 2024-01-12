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
    private List<GameObject> enemies = new List<GameObject>(); 

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
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        timeRemaining = 10;
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
        else if (timeRemaining <= 10)
        {
            scoreText.color = Color.red;
        }

        timeRemaining = Mathf.Max(timeRemaining - Time.deltaTime, 0);
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

    private bool SufficientClearance(Vector3 position, float minSquaredClearance, IEnumerable<GameObject> objects)
    {
       return objects.All((obj) => (obj.transform.position  - position).sqrMagnitude > minSquaredClearance); 
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
            Vector3 position = levelFrame.RandomPositionInside();

            GameObject newWall = Instantiate(wallPrefab, position, rotation);
            SpriteRenderer wallSpriteRenderer = newWall.GetComponent<SpriteRenderer>();
            wallSpriteRenderer.size = new Vector2(width, height);

            float minSquaredClearance = maxSideLength * 4;
            // Just in case any enemies have been destroyed
            enemies.RemoveAll((x) => x == null);
            IEnumerable<GameObject> checkObjects = enemies.Concat(walls).Append(ballController.gameObject);
            if (SufficientClearance(position, minSquaredClearance, checkObjects))
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

    public void SpawnEnemy()
    {
        int attempts = 0;
        while(attempts < 100)
        {
            SpriteRenderer enemySpriteRenderer = enemyPrefab.GetComponent<SpriteRenderer>();
            float enemyWidth = enemySpriteRenderer.bounds.size.x;
            float enemyHeight = enemySpriteRenderer.bounds.size.y;
            Vector3 position = levelFrame.RandomPositionInside(enemyWidth, enemyHeight);
            if(!Physics2D.OverlapCircle(position, enemySpriteRenderer.bounds.size.x * 2))
            {
                GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
                enemies.Add(newEnemy);
                break;
            }
            attempts++;
        }
    }
}
