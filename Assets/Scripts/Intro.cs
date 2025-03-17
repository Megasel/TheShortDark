using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private AudioSource ambientSound;
    [SerializeField] private GameObject mainMenu;
    private void ShowGame()
    {
        cam.enabled = true;
        ambientSound.enabled = true;
        mainMenu.SetActive(true);
    }
}
