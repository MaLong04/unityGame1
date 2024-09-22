using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    public Player Player;
    public List<Enemie> Enemies = new List<Enemie>(); // Инициализация списка врагов
    public GameObject Lose;
    public GameObject Win;

    private int currWave = 0;
    [SerializeField] private LevelConfig Config; // Предполагается, что это скриптableObject для конфигурации уровней

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnWave(); // Запуск первой волны
    }

    public void AddEnemie(Enemie enemie)
    {
        Enemies.Add(enemie);
        Debug.Log($"Враг добавлен. Общее количество врагов: {Enemies.Count}");
    }

    public void RemoveEnemie(Enemie enemie)
    {
        Enemies.Remove(enemie);
        Debug.Log($"Враг удален. Осталось врагов: {Enemies.Count}");

        if (Enemies.Count == 0)
        {
            SpawnWave(); // Запуск новой волны, если врагов больше нет
        }
    }

    private void SpawnWave()
    {
        if (currWave >= Config.Waves.Length)
        {
            Win.SetActive(true);
            return; // Если все волны завершены
        }

        var wave = Config.Waves[currWave];
        foreach (var character in wave.Characters)
        {
            Vector3 pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(character, pos, Quaternion.identity);
        }

        currWave++;
        Debug.Log($"Запущена волна {currWave}."); // Логирование для отслеживания
    }

    public void GameOver()
    {
        Lose.SetActive(true);
        Debug.Log("Игра окончена!");
    }

    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
