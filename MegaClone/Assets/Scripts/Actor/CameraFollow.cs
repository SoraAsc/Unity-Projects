using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados não utilizados", Justification = "<Pendente>")]
[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    Transform player;
    float z = -10;
    [SerializeField]
    Vector2 offsetMin, offsetMax;

    [SerializeField]
    Vector2[] cameraPos;

    private void Awake()
    {
        z = -10;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Clamp(player.position.x,offsetMin.x,offsetMax.x),
            Mathf.Clamp(player.position.y,offsetMin.y,offsetMax.y),z);
    }
}
