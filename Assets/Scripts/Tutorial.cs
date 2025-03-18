using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialStepNames
{
    wasd,
    breath,
    pick,
    reload,
    inventory,
    map,
    loot
        
}
public class Tutorial : MonoBehaviour
{
    [SerializeField] private List<TutorialStep> steps;
    [SerializeField] private Animator tutorialAnimator;
    private TutorialStep currentStep;
    private Coroutine stepCoroutine;
    public void ShowTutorialStep(TutorialStepNames stepName, bool isActive)
    {
        foreach(TutorialStep step in steps)
        {
            print(step.tutorialStepName + " | " + stepName);
            if (step.tutorialStepName == stepName)
            {
               
                if (step.isCompleted) return;
                tutorialAnimator.SetBool(step.animationTransitionName, isActive);
                if (isActive)
                {
                    step.stepObject.SetActive(true);
                    currentStep = step;
                }
                   
               
                break;
            }
        }
    }
    public void ShowTutorialStep(TutorialStepNames stepName, int delay)
    {
        if(stepCoroutine == null)
            stepCoroutine = StartCoroutine(ShowTutorialStepDelayed(stepName,delay));
    }
    public IEnumerator ShowTutorialStepDelayed(TutorialStepNames stepName, int delay)
    {
        foreach (TutorialStep step in steps)
        {
            print(step.tutorialStepName + " | " + stepName);
            if (step.tutorialStepName == stepName)
            {

                if (step.isCompleted) StopCoroutine(stepCoroutine);
                tutorialAnimator.SetBool(step.animationTransitionName, true);
                
                    step.stepObject.SetActive(true);
                    currentStep = step;
                yield return new WaitForSeconds(delay);
                tutorialAnimator.SetBool(step.animationTransitionName, false);
                yield return new WaitForSeconds(1.5f);
                HideStepObject();

                break;
            }
        }
        stepCoroutine = null;
    }
    public void HideStepObject()
    {

        currentStep.isCompleted = true;
        currentStep.stepObject.SetActive(false);
        currentStep = null;
    }
}
[Serializable]
public class TutorialStep
{
    public TutorialStepNames tutorialStepName;
    public GameObject stepObject;
    public string animationTransitionName;
    public bool isCompleted;
}