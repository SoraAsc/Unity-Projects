using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados n�o utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    Transform player;
    float z = -10;
    [SerializeField]
    Vector2 offsetMin, offsetMax;

    public Vector2 OffsetMin { get => offsetMin; set => offsetMin = value; }
    public Vector2 OffsetMax { get => offsetMax; set => offsetMax = value; }

    //[SerializeField] Vector2[] cameraPos;
    [SerializeField] float smooth;

    private void Start()
    {
        z = -10;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void LateUpdate()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 clampPos = new Vector3(Mathf.Clamp(player.position.x, offsetMin.x, offsetMax.x),
            Mathf.Clamp(player.position.y, offsetMin.y, offsetMax.y), z);

        transform.position = Vector3.SmoothDamp(transform.position, clampPos, ref velocity, smooth * Time.deltaTime);
        /*         transform.position = new Vector3(
                    Mathf.Clamp(player.position.x, offsetMin.x, offsetMax.x),
                    Mathf.Clamp(player.position.y, offsetMin.y, offsetMax.y), z); */
    }
}
