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
        enemyObjs = new string[] { "EnemyS", "EnemyM", "EnemyL", "EnemyB"};

        ReadSpawnFile();
    }

    void ReadSpawnFile()
    {
        //  ���� �ʱ�ȭ
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        //  ������ ���� �б�
        TextAsset textFile = Resources.Load<TextAsset>("Stage 0");
        StringReader stringReader = new StringReader(textFile.text);

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();

            Debug.Log(line);

            if (line == null)
            {
                break;
            }

            //  ������ ������ ����
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        //  �ؽ�Ʈ ���� �ݱ�
        stringReader.Close();

        //  ù ��° ���� ������ ����
        nextSpawnDelay = spawnList[0].delay;
    }

    private void Update()
    {
        curSpwanDelay += Time.deltaTime;

        if (curSpwanDelay > nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();
            curSpwanDelay = 0;
        }

        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    private void SpawnEnemy()
    {
        int enemyIndex = 0;

        switch (spawnList[spawnIndex].type)
        {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
            case "B":
                enemyIndex = 3;
                break;
        }

        int enemyPoint = spawnList[spawnIndex].point;

        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;

        if (enemyPoint == 5 || enemyPoint == 6)
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }   //  if : Right Spwan
        else if (enemyPoint == 7 || enemyPoint == 8)
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }   //  else if : Left Spwan
        else
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }   //  else : Front Spwan

        //  ������ �ε��� ����
        spawnIndex++;

        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        //  ���� ������ ������ ����
        nextSpawnDelay = spawnList[spawnIndex].delay;
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
