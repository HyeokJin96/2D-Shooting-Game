using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    public string[] enemyObjs;
    public Transform[] spawnPoints;

    public float nextSpawnDelay;
    public float curSpwanDelay;

    public GameObject player = default;
    public TMP_Text scoreText = default;
    public Image[] lifeImage = default;
    public Image[] boomImage = default;

    public GameObject gameOverSet = default;
    public ObjectManager objectManager = default;

    public List<Spawn> spawnList = default;
    public int spawnIndex = default;
    public bool spawnEnd = false;

    private void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjs = new string[] { "EnemyS", "EnemyM", "EnemyL"};


    }

    void ReadSpawnFile()
    {
        //  변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        //  리스폰 파일 읽기
        TextAsset textFile = Resources.Load("Stage 0") as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();

            Debug.Log(line);

            if (line == null)
            {
                break;
            }

            //  리스폰 데이터 생성
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        //  텍스트 파일 닫기
        stringReader.Close();

        //  첫 번째 스폰 딜레이 적용
        nextSpawnDelay = spawnList[0].delay;
    }

    private void Update()
    {
        curSpwanDelay += Time.deltaTime;

        if (curSpwanDelay > nextSpawnDelay)
        {
            SpawnEnemy();

            nextSpawnDelay = Random.Range(0.5f, 3f);
            curSpwanDelay = 0;
        }

        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    private void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 9);

        GameObject enemy = objectManager.MakeObj(enemyObjs[ranEnemy]);
        enemy.transform.position = spawnPoints[ranPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;

        if (ranPoint == 5 || ranPoint == 6)
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }   //  if : Right Spwan
        else if (ranPoint == 7 || ranPoint == 8)
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }   //  else if : Left Spwan
        else
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }   //  else : Front Spwan
    }

    public void UpdateLifeIcon(int life)
    {
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom)
    {
        for (int index = 0; index < 3; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < boom; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void RespawnPlayer()
    {
        Invoke("RespwanPlayerExe", 1f);
    }

    private void RespwanPlayerExe()
    {
        player.transform.position = Vector3.down * 4.0f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
