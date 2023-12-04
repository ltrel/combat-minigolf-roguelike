using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject enemyPrefab;

    private LevelFrame levelFrame;
    private int score;
    private float timeRemaining;

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
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        scoreText.text = score.ToString("D4") + "\n\n" + FormatTime(timeRemaining); 
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
}
