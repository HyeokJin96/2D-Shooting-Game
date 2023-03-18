using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Follower : MonoBehaviour
{
    private float maxShotDelay = default;
    private float curShotDelay = default;

    private Vector3 followPos = default;
    private int followDelay = default;
    private Queue<Vector3> parentPos = default;     //  Queue = FIFO (First Input First OutPut)

    [SerializeField] public Transform parent = default;
    [SerializeField] public ObjectManager objectManager = default;

    private void Awake()
    {
        parentPos = new Queue<Vector3>();
    }

    private void Start()
    {
        maxShotDelay = 2f;

        followDelay = 0;
    }

    private void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    private void Watch()
    {
        //  Input Pos
        parentPos.Enqueue(parent.position);

        //  OutPut Pos
        //if (parentPos.Count)
        {

        }
        followPos = parentPos.Dequeue();
    }

    private void Follow()
    {
        transform.position = followPos;
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

        GameObject bullet = objectManager.MakeObj("BulletFollower");
        bullet.transform.position = transform.position;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        curShotDelay = 0;
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
}
