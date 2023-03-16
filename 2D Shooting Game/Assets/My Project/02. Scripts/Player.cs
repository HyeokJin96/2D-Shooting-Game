using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = default;
    private float power = default;
    private float maxShotDelay = default;
    private float curShotDelay = default;

    Animator animator = default;

    private bool isTouchTop = default;
    private bool isTouchBottom = default;
    private bool isTouchRight = default;
    private bool isTouchLeft = default;




    [SerializeField] public GameObject bulletObjA = default;
    [SerializeField] public GameObject bulletObjB = default;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        speed = 3f;
        power = 1f;
        maxShotDelay = 0.15f;
    }

    private void Update()
    {
        Move();
        Fire();
        Reload();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1))
        {
            h = 0;
        }

        float v = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1))
        {
            v = 0;
        }

        Vector3 curentPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curentPos + nextPos;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Vertical"))
        {
            animator.SetInteger("Input", (int)h);
        }
    }

    private void Fire()
    {
        if (!Input.GetButton("Fire1"))
        {
            return;
        }

        if (curShotDelay < maxShotDelay)
        {
            return;
        }

        switch (power)
        {
            case 1:
                GameObject bullet = Instantiate(bulletObjA, transform.position, Quaternion.identity);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.2f, Quaternion.identity);
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletL = Instantiate(bulletObjA, transform.position + Vector3.left * 0.2f, Quaternion.identity);
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 3:
                GameObject bulletRR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.35f, Quaternion.identity);
                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletLL = Instantiate(bulletObjA, transform.position + Vector3.left * 0.35f, Quaternion.identity);
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletCC = Instantiate(bulletObjB, transform.position, Quaternion.identity);
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }

        curShotDelay = 0;
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
