using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public int dmg = default;

    [SerializeField] public bool isRotate = default;

    private void Update()
    {
        if (isRotate)
        {
            transform.Rotate(Vector3.forward * 10f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BorderBullet"))
        {
            gameObject.SetActive(false);
        }
    }
}
