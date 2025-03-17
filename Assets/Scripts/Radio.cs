using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Radio : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioSource aud;
    [SerializeField] private List<AudioClip> clips;
    [SerializeField] private List<RadioMessage> messages;

    private Coroutine switchFrequencyCoroutine;
    private Coroutine spawnTextCoroutine;
    private int currentClip = 1;
    private bool isEnabled = false;
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text floatingText;
    [SerializeField] private GameObject textPrefab;   
    [SerializeField] private Transform textSpawnPoint;    

    public void Interact()
    {
        if (switchFrequencyCoroutine == null)
        {
            switchFrequencyCoroutine = StartCoroutine(SwitchFrequency());
            if (!isEnabled)
                animator.enabled = true;

            
        }
    }

    public IEnumerator SwitchFrequency()
    {
        if(switchFrequencyCoroutine != null)
        {
            StopCoroutine(switchFrequencyCoroutine);
            switchFrequencyCoroutine = null;
        }
        aud.PlayOneShot(clips[0]);
        yield return new WaitForSeconds(1);
        if (isEnabled)
            currentClip++;
        isEnabled = true;
        if (currentClip >= clips.Count)
            currentClip = 1;
        aud.clip = clips[currentClip];
        if (spawnTextCoroutine == null)
            spawnTextCoroutine = StartCoroutine(SpawnText());
        aud.Play();
        
        switchFrequencyCoroutine = null;
    }

    private IEnumerator SpawnText()
    {
        int clipQueue = 0;
        while (isEnabled)
        {
            RadioMessage currentMessage = messages[currentClip];
            if (currentMessage.messages.Length > 0)
            {
                GameObject textObject = Instantiate(textPrefab, textSpawnPoint.position, Quaternion.identity, transform);
                TMP_Text textComponent = textObject.GetComponent<TMP_Text>();
                textObject.transform.rotation = textSpawnPoint.rotation;
                textComponent.text = currentMessage.messages[clipQueue];
                textComponent.color = currentMessage.textColor;

                Destroy(textObject, 2f);
            }

            yield return new WaitForSeconds(1.5f);
            clipQueue++;
            if(clipQueue >= currentMessage.messages.Length)
            {
                clipQueue = 0;
            }
        }

        spawnTextCoroutine = null;
    }
}

[Serializable]
public class RadioMessage
{
    public string[] messages;   
    public Color textColor;   
}