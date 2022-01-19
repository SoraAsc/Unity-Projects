using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    bool canFollowCamera = false;
    [SerializeField] float speed;
    private float length, startPos;
    Camera cam;
    private void Start()
    {
        startPos = transform.position.x;
        cam = Camera.main;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float temp = cam.transform.position.x * (1 - speed);
        float distance = cam.transform.position.x * speed;

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}
