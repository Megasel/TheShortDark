using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    private Tutorial tutorial;
    [SerializeField] private TutorialStepNames tipName;
    [SerializeField] private int delay;
    private void OnEnable()
    {
        tutorial = FindAnyObjectByType<Tutorial>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowTip();
        }
    }
    private void ShowTip()
    {
        tutorial.ShowTutorialStep(tipName, delay);
       
    }
}
