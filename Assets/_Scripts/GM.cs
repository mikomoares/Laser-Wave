using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    public GameObject[] enemies;
    private bool gameOver;
    private int enemyInWave;

    private float timeToSpawn;
    private float timeToWave;

    public GameObject panelWin;
    public GameObject panelLose;
    public bool winned;
    public bool losed;

    public GameObject croc;
    private bool crocked;

    public GameObject player;

    [Header("Spawn Settings")]
    public float spawnMargin = 1.5f; // quão fora da tela o inimigo nasce

    private Camera cam;
    private Vector2 camMin;
    private Vector2 camMax;

    void Start()
    {
        timeToSpawn = 1f;
        timeToWave = .8f;
        enemyInWave = 5;

        panelWin.SetActive(false);
        panelLose.SetActive(false);

        player = GameObject.Find("Player");

        cam = Camera.main;
        UpdateCameraBounds();

        StartCoroutine(WaitForGameStart());
    }

    void UpdateCameraBounds()
    {
        camMin = cam.ViewportToWorldPoint(new Vector3(0, 0));
        camMax = cam.ViewportToWorldPoint(new Vector3(1, 1));
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 pos;

        do
        {
            float x = Random.Range(camMin.x - spawnMargin, camMax.x + spawnMargin);
            float y = Random.Range(camMin.y - spawnMargin, camMax.y + spawnMargin);
            pos = new Vector3(x, y, 0f);
        }
        while (pos.x > camMin.x && pos.x < camMax.x &&
               pos.y > camMin.y && pos.y < camMax.y);

        return pos;
    }

    private IEnumerator WaitForGameStart()
    {
        while (GameStateManager.Instance == null || !GameStateManager.Instance.IsGameStarted())
        {
            yield return null;
        }

        StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        yield return new WaitForSeconds(3f);

        while (!gameOver)
        {
            for (int i = 0; i < enemyInWave; i++)
            {
                if (enemyInWave == 12 && !crocked)
                {
                    Instantiate(croc);
                    crocked = true;
                }

                Vector3 spawnPos = GetRandomSpawnPosition();

                if (enemyInWave == 1)
                {
                    Instantiate(enemies[0], spawnPos, Quaternion.identity);
                }
                else if (enemyInWave <= 10)
                {
                    float ran = Random.Range(0f, 1f);
                    Instantiate(ran >= 0.25f ? enemies[0] : enemies[1], spawnPos, Quaternion.identity);
                }
                else
                {
                    float ran = Random.Range(0f, 1f);
                    Instantiate(ran >= 0.4f ? enemies[0] : enemies[1], spawnPos, Quaternion.identity);
                }

                yield return new WaitForSeconds(timeToSpawn);
            }

            yield return new WaitForSeconds(timeToWave);

            if (enemyInWave <= 9)
                enemyInWave++;

            if (timeToWave >= 1f)
                timeToWave -= 0.15f;

            if (timeToSpawn >= 2f)
                timeToSpawn -= 0.15f;
        }
    }

    public void Win()
    {
        if (!losed)
        {
            player.GetComponent<SpriteRenderer>().sortingOrder = 1;
            panelWin.SetActive(true);
            winned = true;
        }
    }

    public void Lose()
    {
        if (!winned)
        {
            player.GetComponent<SpriteRenderer>().sortingOrder = 1;
            panelLose.SetActive(true);
            losed = true;
        }
    }
}
