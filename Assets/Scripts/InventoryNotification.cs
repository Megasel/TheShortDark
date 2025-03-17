using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryNotification : MonoBehaviour
{
    public TMP_Text notificationText;
    private float startduration;
    [SerializeField] private float fadeSpeed;
    private void Update()
    {
        startduration += Time.deltaTime;
        if (startduration > 1)
        {
            notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, (notificationText.alpha - Time.deltaTime * fadeSpeed));
        }
        if(notificationText.color.a <= 0)
            gameObject.SetActive(false);
    }
    public void Play()
    {
        gameObject.SetActive(true);
        notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, 1);
    }
}