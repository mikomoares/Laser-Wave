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

    public Vector3 dir;
    public Vector3 esq;

    public Vector3 cima;

    public Vector3 baixo;

    private Vector3[] posicoes;
    private float timeToSpawn;
    private float timeToWave;
    public GameObject panelWin;
    public GameObject panelLose;
    public bool winned;
    public bool losed;
    public GameObject croc;
    private bool crocked;
    public GameObject player;

    void Start()
    {
        posicoes = new [] { esq, dir, cima, baixo };

        timeToSpawn = 3.5f;
        timeToWave = 2f;

        enemyInWave = 3;
        StartCoroutine(Spawner());
        panelWin.SetActive(false);
        panelLose.SetActive(false);

        player = GameObject.Find("Player");

    }

    void Update()
    {
        
    }

    private IEnumerator Spawner(){
        yield return new WaitForSeconds(3f);
        while (!gameOver){
            for (int i = 0; i < enemyInWave; i++)
            {
                if (enemyInWave == 6 && !crocked){
                    Instantiate(croc);
                    crocked = true;
                }
                if (enemyInWave==3){
                    Instantiate(enemies[0], posicoes[(int)Random.Range(0f,3.99f)], Quaternion.identity);
                }else if (enemyInWave<=5){
                    float ran = Random.Range(0f,1f);
                    if (ran>=0.25){
                        Instantiate(enemies[0], posicoes[(int)Random.Range(0f,3.99f)], Quaternion.identity);
                    }else{
                        Instantiate(enemies[1], posicoes[(int)Random.Range(0f,3.99f)], Quaternion.identity);
                    }
                }else{
                    float ran = Random.Range(0f,1f);
                    if (ran>=0.4){
                        Instantiate(enemies[0], posicoes[(int)Random.Range(0f,3.99f)], Quaternion.identity);
                    }else{
                        Instantiate(enemies[1], posicoes[(int)Random.Range(0f,3.99f)], Quaternion.identity);
                    }
                }
                yield return new WaitForSeconds(timeToSpawn);
            }

            yield return new WaitForSeconds(timeToWave);

            if (enemyInWave<=9){
                enemyInWave++;
            }

            if (timeToWave>=1){
                timeToWave-=0.15f;
            }
            if (timeToSpawn>=2){
                timeToWave-=0.15f;
            }
        }
    }
    public void Win(){
        if (!losed){
            player.GetComponent<SpriteRenderer>().sortingOrder = 1;
            panelWin.SetActive(true);
            winned = true;
        }
    }
    public void Lose(){
        if(!winned){
            player.GetComponent<SpriteRenderer>().sortingOrder = 1;
            panelLose.SetActive(true);
            losed = true;
        }
    }
}
