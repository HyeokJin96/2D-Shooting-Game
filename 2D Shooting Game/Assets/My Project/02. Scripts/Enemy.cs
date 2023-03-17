using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public string enemyName = default;
    [SerializeField] public float speed = default;
    [SerializeField] public float health = default;
    [SerializeField] public Sprite[] sprites = default;

    [SerializeField] public GameObject bulletObjA = default;
    [SerializeField] public GameObject bulletObjB = default;

    [SerializeField] public GameObject itemCoin = default;
    [SerializeField] public GameObject itemPower = default;
    [SerializeField] public GameObject itemBoom = default;


    [SerializeField] public GameObject player = default;

    private SpriteRenderer spriteRenderer = default;

    private float maxShotDelay = default;
    private float curShotDelay = default;

    public int ememyScore = default;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        maxShotDelay = 3;
    }

    private void Update()
    {
        Fire();
        Reload();
    }

    private void Fire()
    {
        if (curShotDelay < maxShotDelay)
        {
            return;
        }

        if (enemyName == "S")
        {
            GameObject bullet = Instantiate(bulletObjA, transform.position, Quaternion.identity);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        else if (enemyName == "L")
        {
            GameObject bulletR = Instantiate(bulletObjB, transform.position + Vector3.right * 0.3f, Quaternion.identity);
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
            rigidR.AddForce(dirVecR.normalized * 7, ForceMode2D.Impulse);

            GameObject bulletL = Instantiate(bulletObjB, transform.position + Vector3.left * 0.3f, Quaternion.identity);
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);
            rigidL.AddForce(dirVecL.normalized * 7, ForceMode2D.Impulse);
        }

        curShotDelay = 0;
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    public void OnHit(int dmg)
    {
        if (health <= 0)
        {
            return;
        }

        health -= dmg;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += ememyScore;

            //  Random Ratio Item Drop
            int ran = Random.Range(1, 10);

            if (ran < 5)
            {
                //  No Item
            }
            else if (ran < 6)
            {
                Instantiate(itemCoin, transform.position, Quaternion.identity);
            }   //  else if : Coin
            else if (ran < 7)
            {
                Instantiate(itemPower, transform.position, Quaternion.identity);
            }   //  else if : Power
            else if (ran < 8)
            {
                Instantiate(itemBoom, transform.position, Quaternion.identity);
            }   //  else if : Boom

            Destroy(gameObject);
        }
    }

    private void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BorderBullet"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("PlayerBullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);

            Destroy(collision.gameObject);
        }
    }
}
