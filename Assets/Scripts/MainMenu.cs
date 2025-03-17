using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool isPlay = false;
    [SerializeField] GameObject fade;
    [SerializeField] AudioClip startCar;
    [SerializeField] AudioSource audioSource;
    public void Play()
    {
        if (!isPlay)
        {
            audioSource.PlayOneShot(startCar);
            StartCoroutine(PlayDelay());
            isPlay = true;
        }
        
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    IEnumerator PlayDelay()
    {
        fade.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }
}
