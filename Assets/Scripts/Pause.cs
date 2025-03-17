using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private PlayerMoveController moveController;
    [SerializeField] private GameObject metrics;
    [SerializeField] private PlayerUi playerUi;
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0; 
        metrics.SetActive(false);
        //moveController.enabled = false;
    }
    private void OnDisable()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        metrics.SetActive(true);

        Time.timeScale = 1;
       // moveController.enabled = true;
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Continue()
    {
        playerUi.Toggle(gameObject);
    }

}
