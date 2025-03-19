
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Helicopter : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform startPoint; // ��������� �����
    [SerializeField] private Transform endPoint;   // �������� �����
    [SerializeField] private float speed = 1.0f;   // �������� ��������
    [SerializeField] private List<MeshRenderer> meshRenderers; // ������ MeshRenderer'��
    [SerializeField] private PlayerMoveController moveController;
    private float journeyLength;
    private float startTime;
    [SerializeField] private GameObject winAnim;
    [SerializeField] private AudioSource aud;
    public void Interact()
    {
        moveController.ToggleControls(false);
        StartCoroutine(Restart());
        winAnim.SetActive(true);
    }
    IEnumerator Restart()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
    // ����� ��� ������ ��������
    public void StartFly()
    {
        // ������������� ��������� ������� �������
        transform.position = startPoint.position;
        aud.Play();
        // �������� ��� MeshRenderer'�
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = true;
        }

        // ��������� ���������� ����� �������
        journeyLength = Vector3.Distance(startPoint.position, endPoint.position);

        // ���������� ����� ������ ��������
        startTime = Time.time;
    }

    private void Update()
    {
        // ��������� ���������� ����������
        float distanceCovered = (Time.time - startTime) * speed;

        // ��������������� ���������� (�� 0 �� 1)
        float fractionOfJourney = distanceCovered / journeyLength;

        // ���� �������� �� ���������
        if (fractionOfJourney < 1.0f)
        {
            // ������� ����������� �� startPoint � endPoint
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, fractionOfJourney);
        }
        else
        {
            // ��������� �������� �������
            transform.position = endPoint.position;
        }
    }
}