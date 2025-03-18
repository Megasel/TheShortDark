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
        messageIndex = -1; // ���������� ������ ����� ������� �������
        ShowNextMessage();
    }

    public void ShowNextMessage()
    {
        // ���������, �� ����� �� �� ������� �������
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
        // ���������, ��� ������ � �������� �������
        if (messageIndex < currentMessages.Length)
        {
            for (int i = 0; i < currentMessages[messageIndex].ToCharArray().Length; i++)
            {
                yield return new WaitForSeconds(0.08f);
                //aud.pitch = Random.Range(0.9f, 1.1f);
                aud.Play();
                messageText.text += currentMessages[messageIndex][i];
            }

            // ���� 1.5 ������� ����� ������� ���������� ���������
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