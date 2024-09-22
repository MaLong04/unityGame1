using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Префаб врага
    public TextMeshProUGUI waveInfoText; // Текст для отображения информации о волне

    private int currentWave = 0;
    private const int totalWaves = 5; // Общее количество волн

    private void Start()
    {
        StartNextWave(); // Запуск первой волны
    }

    private void Update()
    {
        UpdateWaveInfo();
        // Проверяем, есть ли враги в игре
        if (GameSceneManager.Instance.Enemies.Count == 0 && currentWave > 0)
        {
            Debug.Log("Нет врагов, запускаем следующую волну.");
            StartNextWave(); // Запускаем следующую волну, если врагов больше нет
        }
    }

    private void StartNextWave()
    {
        currentWave++;
        Debug.Log($"Начинается волна {currentWave}");

        if (currentWave > totalWaves)
        {
            GameOver(); // Если все волны завершены
            return;
        }

        UpdateWaveInfo();
        SpawnEnemiesForCurrentWave();
    }

    private void UpdateWaveInfo()
    {
        waveInfoText.text = $"Текущая волна: {currentWave} из {totalWaves}";
    }

    private void SpawnEnemiesForCurrentWave()
    {
        int enemyCount = currentWave * 2; // Количество врагов для спавна
        Debug.Log($"Спавн {enemyCount} врагов для волны {currentWave}.");

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void GameOver()
    {
        Debug.Log("Игра окончена! Все волны завершены.");
        // Логика завершения игры
    }
}
