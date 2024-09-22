using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    public Player Player;
    public List<Enemie> Enemies = new List<Enemie>(); // ������������� ������ ������
    public GameObject Lose;
    public GameObject Win;

    private int currWave = 0;
    [SerializeField] private LevelConfig Config; // ��������������, ��� ��� ������ableObject ��� ������������ �������

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnWave(); // ������ ������ �����
    }

    public void AddEnemie(Enemie enemie)
    {
        Enemies.Add(enemie);
        Debug.Log($"���� ��������. ����� ���������� ������: {Enemies.Count}");
    }

    public void RemoveEnemie(Enemie enemie)
    {
        Enemies.Remove(enemie);
        Debug.Log($"���� ������. �������� ������: {Enemies.Count}");

        if (Enemies.Count == 0)
        {
            SpawnWave(); // ������ ����� �����, ���� ������ ������ ���
        }
    }

    private void SpawnWave()
    {
        if (currWave >= Config.Waves.Length)
        {
            Win.SetActive(true);
            return; // ���� ��� ����� ���������
        }

        var wave = Config.Waves[currWave];
        foreach (var character in wave.Characters)
        {
            Vector3 pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(character, pos, Quaternion.identity);
        }

        currWave++;
        Debug.Log($"�������� ����� {currWave}."); // ����������� ��� ������������
    }

    public void GameOver()
    {
        Lose.SetActive(true);
        Debug.Log("���� ��������!");
    }

    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
