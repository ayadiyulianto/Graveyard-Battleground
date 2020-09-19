using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public float timerWaveInSec;
    public int totalEnemyToSpawn;
}

public class GameplayManager : MonoBehaviour
{
    public GameObject[] prefabEnemy;
    public Transform[] enemySpawnPoint;
    public Wave[] enemyWave;
    public bool timerON = true;
    public AudioClip audioNewWave;
    public AudioClip audioWin;

    PlayerStats playerStats;
    AudioSource audioSource;
    float timer = 0;
    int currentWave = 0;
    int enemyToSpawn = 0;
    float spawnCooldown = 0;
    float spawnTimer = 0;
    int totalKill = 0;
    bool levelFinished = false;

    public Text textTimer;
    public Text textWave;
    public GameObject gameOverPanel;
    public Text gameOverTitle;
    public Text gameOverKills;

    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        audioSource = GetComponent<AudioSource>();
        textTimer.text = "";
        textWave.text = "";
        gameOverPanel.SetActive(false);
        foreach (Wave item in enemyWave) {
            totalKill += item.totalEnemyToSpawn;
        }
    }

    void Update()
    {
        if (timerON) {
            if (timer > 0) {
                timer -= Time.deltaTime;
            } else {
                if (currentWave < enemyWave.Length) {
                    currentWave++;
                    timer = enemyWave[currentWave-1].timerWaveInSec;
                    enemyToSpawn = enemyWave[currentWave-1].totalEnemyToSpawn;
                    spawnCooldown = timer / 2 / enemyToSpawn;
                    textWave.text = "WAVE " + currentWave;
                    audioSource.PlayOneShot(audioNewWave);
                } else {
                    timerON = false;
                }
            }
            int minute = (int)timer / 60;
            int second = (int)timer % 60;
            textTimer.text = "`` " + minute.ToString("D2") + " : " + second.ToString("D2");

            if (spawnTimer > 0) {
                spawnTimer -= Time.deltaTime;
            } else if (enemyToSpawn > 0) {
                spawnTimer = spawnCooldown;
                int randomSP = Random.Range(0, enemySpawnPoint.Length);
                Instantiate(prefabEnemy[0], enemySpawnPoint[randomSP].position, Quaternion.identity);
                enemyToSpawn--;
            }
        }
        
        if (playerStats.kills == totalKill && !levelFinished) {
            levelFinished = true;
            textWave.text = "YOU WIN!";
            textWave.color = Color.blue;
            if (audioWin != null) audioSource.PlayOneShot(audioWin);
            StartCoroutine(GameOver(true));
        }

        if (playerStats.isDead && !levelFinished) {
            levelFinished = true;
            textWave.text = "GAME OVER!";
            textWave.color = Color.red;
            audioSource.Play();
            StartCoroutine(GameOver(false));
        }
    }

    IEnumerator GameOver(bool win)
    {
        timerON = false;
        yield return new WaitForSeconds(3f);
        if (win) {
            gameOverTitle.text = "Winner Winner Chicken Dinner!";
        } else {
            gameOverTitle.text = "Loser Go To The HELL!";
        }
        gameOverKills.text = "Total Kills : " + playerStats.kills;
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && !audioSource.isPlaying) {
            audioSource.Play();
        }
    }
}
