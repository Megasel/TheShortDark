using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public Transform needle;            
    public Transform player;              
    private void OnEnable()
    {
        player = FindAnyObjectByType<PlayerMoveController>().transform;
    }
    private void Update()
    {
        Vector3 northDirection = new Vector3(0, 0, 1);

        Vector3 playerForward = new Vector3(player.forward.x, 0, player.forward.z).normalized;

        float angle = Vector3.SignedAngle(playerForward, northDirection, Vector3.up);

        needle.localRotation = Quaternion.Euler(0, -angle, 0);
    }
}
