using UnityEngine;
[System.Serializable]
public class DialogMessages : MonoBehaviour
{
    [TextArea(2,10)]
    public string[] messages;
    private Dialog dialog;
    private void OnEnable()
    {
        dialog = FindAnyObjectByType<Dialog>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartTalking();
            gameObject.SetActive(false);
        }
    }
    public void StartTalking()
    {
        dialog.StartDialog(messages);
    }
}
