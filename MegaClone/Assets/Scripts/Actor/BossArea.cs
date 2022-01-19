using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : MonoBehaviour
{
    [SerializeField] GameObject[] colliders;
    [SerializeField] Enemy boss;
    [SerializeField] Vector2 cameraOriginalOffsetMax;
    [SerializeField] Vector2 cameraOriginalOffsetMin;
    [SerializeField] Vector2 cameraNewOffset;

    bool actived = false;
    CameraFollow cameraFollow;

    private void Start()
    {
        actived = false;
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !actived)
        {
            actived = true;
            cameraOriginalOffsetMin = cameraFollow.OffsetMin;
            cameraOriginalOffsetMax = cameraFollow.OffsetMax;

            cameraFollow.OffsetMin = cameraNewOffset;
            cameraFollow.OffsetMax = cameraNewOffset;
            foreach (GameObject col in colliders)
            {
                col.SetActive(true);

            }
        }
    }

    private void Update()
    {
        if (actived && !boss)
        {
            ReturnDefaultCameraValues();
        }
    }

    private void ReturnDefaultCameraValues()
    {
        cameraFollow.OffsetMin = cameraOriginalOffsetMin;
        cameraFollow.OffsetMax = cameraOriginalOffsetMax;
        actived = false;
        Destroy(gameObject);
    }
}
