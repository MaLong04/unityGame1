using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab; // ������ �����
    public TextMeshProUGUI waveInfoText; // ����� ��� ����������� ���������� � �����

    private int currentWave = 0;
    private const int totalWaves = 5; // ����� ���������� ����

    private void Start()
    {
        StartNextWave(); // ������ ������ �����
    }

    private void Update()
    {
        UpdateWaveInfo();
        // ���������, ���� �� ����� � ����
        if (GameSceneManager.Instance.Enemies.Count == 0 && currentWave > 0)
        {
            Debug.Log("��� ������, ��������� ��������� �����.");
            StartNextWave(); // ��������� ��������� �����, ���� ������ ������ ���
        }
    }

    private void StartNextWave()
    {
        currentWave++;
        Debug.Log($"���������� ����� {currentWave}");

        if (currentWave > totalWaves)
        {
            GameOver(); // ���� ��� ����� ���������
            return;
        }

        UpdateWaveInfo();
        SpawnEnemiesForCurrentWave();
    }

    private void UpdateWaveInfo()
    {
        waveInfoText.text = $"������� �����: {currentWave} �� {totalWaves}";
    }

    private void SpawnEnemiesForCurrentWave()
    {
        int enemyCount = currentWave * 2; // ���������� ������ ��� ������
        Debug.Log($"����� {enemyCount} ������ ��� ����� {currentWave}.");

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void GameOver()
    {
        Debug.Log("���� ��������! ��� ����� ���������.");
        // ������ ���������� ����
    }
}
