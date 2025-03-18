using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    private string[] currentMessages;
    int messageIndex = -1;
    [SerializeField] GameObject bg;
    [SerializeField] AudioSource aud;

    public void StartDialog(string[] messages)
    {
        messageText.enabled = true;
        bg.SetActive(true);
        currentMessages = messages;
        messageText.gameObject.SetActive(true);
        messageIndex = -1; // —брасываем индекс перед началом диалога
        ShowNextMessage();
    }

    public void ShowNextMessage()
    {
        // ѕровер€ем, не вышли ли за пределы массива
        if (messageIndex >= currentMessages.Length - 1)
        {
            StopAllCoroutines();
            EndDialog();
            return;
        }

        messageIndex++;
        messageText.text = "";
        StartCoroutine(TypeMessage(messageIndex));
    }

    IEnumerator TypeMessage(int messageIndex)
    {
        // ѕровер€ем, что индекс в пределах массива
        if (messageIndex < currentMessages.Length)
        {
            for (int i = 0; i < currentMessages[messageIndex].ToCharArray().Length; i++)
            {
                yield return new WaitForSeconds(0.08f);
                //aud.pitch = Random.Range(0.9f, 1.1f);
                aud.Play();
                messageText.text += currentMessages[messageIndex][i];
            }

            // ∆дем 1.5 секунды перед показом следующего сообщени€
            yield return new WaitForSeconds(1.5f);
            ShowNextMessage();
        }
    }

    private void EndDialog()
    {
        messageText.enabled = false;
        bg.SetActive(false);
    }
}