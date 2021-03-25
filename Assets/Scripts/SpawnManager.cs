using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject gameMenu;
    [SerializeField] Collider[] spawnPoints;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Button restartButton;
    [SerializeField] Camera camera1;
    [SerializeField] Camera camera2;
    public Camera activeCamera;
    int score = 0;
    float spawnRate = 5;
    float spawnTime;
    bool gameOver = false;
    GameObject player;
    List<GameObject> zombies = new List<GameObject>();

    private void Awake()
    {
        restartButton.onClick.AddListener(RestartGame);
        gameMenu.SetActive(false);
        scoreText.gameObject.SetActive(true);
        scoreText.text = "Score: 0";
        spawnTime = Time.time + spawnRate;
        player = Instantiate(playerPrefab);
        activeCamera = camera1;
    }

    void Update()
    {
        if(!gameOver)
        {
            if (spawnTime < Time.time)
            {
                spawnTime = Time.time + spawnRate;
                int index = Random.Range(0, spawnPoints.Length);
                GameObject zombie = Instantiate(zombiePrefab, spawnPoints[index].transform);
                zombies.Add(zombie);
            }
        }
    }

    public void ScoreCount()
    {
        score++;
        scoreText.text = "Score: " + score;
    }
    //смена камеры при заходе на огневую точку
    public void CameraSwitch(bool camSwitch)
    {
        camera1.gameObject.SetActive(!camSwitch);
        camera2.gameObject.SetActive(camSwitch);
        activeCamera = camera2;
    }

    public void GameOver()
    {
        gameOver = true;
        Destroy(player);
        gameMenu.SetActive(true);
        scoreText.gameObject.SetActive(false);
        foreach (GameObject z in zombies)
        {
            Destroy(z);
        }
    }

    void RestartGame()
    {
        gameOver = false;
        gameMenu.SetActive(false);
        scoreText.gameObject.SetActive(true);
        scoreText.text = "Score: 0";
        spawnTime = Time.time + spawnRate;
        player = Instantiate(playerPrefab);
        activeCamera = camera1;
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
    }
}
