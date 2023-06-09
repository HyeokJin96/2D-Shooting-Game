using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed = default;

    public int startIndex = default;
    public int endIndex = default;
    public Transform[] sprites;

    float viewHeight;

    private void Awake()
    {
        viewHeight = Camera.main.orthographicSize * 2;
    }

    private void Update()
    {
        Vector3 curPos = transform.position;
        Vector3 nexPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nexPos;

        if (sprites[endIndex].position.y < viewHeight * (-1))
        {
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            Vector3 frontSpritePos = sprites[endIndex].localPosition;
            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.up * viewHeight;

            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = startIndexSave -1 == -1 ? sprites.Length -1 : startIndexSave -1;
        }
    }
}
