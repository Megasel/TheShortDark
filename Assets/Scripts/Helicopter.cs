
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Helicopter : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform startPoint; // Начальная точка
    [SerializeField] private Transform endPoint;   // Конечная точка
    [SerializeField] private float speed = 1.0f;   // Скорость движения
    [SerializeField] private List<MeshRenderer> meshRenderers; // Список MeshRenderer'ов
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
    // Метод для начала движения
    public void StartFly()
    {
        // Устанавливаем начальную позицию объекта
        transform.position = startPoint.position;
        aud.Play();
        // Включаем все MeshRenderer'ы
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = true;
        }

        // Вычисляем расстояние между точками
        journeyLength = Vector3.Distance(startPoint.position, endPoint.position);

        // Запоминаем время начала движения
        startTime = Time.time;
    }

    private void Update()
    {
        // Вычисляем пройденное расстояние
        float distanceCovered = (Time.time - startTime) * speed;

        // Нормализованное расстояние (от 0 до 1)
        float fractionOfJourney = distanceCovered / journeyLength;

        // Если движение не завершено
        if (fractionOfJourney < 1.0f)
        {
            // Плавное перемещение от startPoint к endPoint
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, fractionOfJourney);
        }
        else
        {
            // Фиксируем конечную позицию
            transform.position = endPoint.position;
        }
    }
}