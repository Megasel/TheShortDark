using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtshelnikTrigger : MonoBehaviour
{
    [SerializeField] private GameObject othselnikObject;
    [SerializeField] private Transform othselnikSpawnPoint;
    [SerializeField] private DoorBehavior carDoorBehaviour;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartEvent();
        }
    }
    private void StartEvent()
    {
        othselnikObject.SetActive(true);
        carDoorBehaviour.isLocked = false;
        carDoorBehaviour.OpenDoor();
    }
}
