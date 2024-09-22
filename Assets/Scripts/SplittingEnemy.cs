using UnityEngine;

public class SplittingEnemy : Enemie
{
    public GameObject smallerEnemyPrefab;  // Префаб меньшего врага
    public int splitCount = 2;             // Количество меньших врагов

    protected override void Die()  // Переопределяем метод Die()
    {
        base.Die();

        // Спавним меньших врагов
        SpawnSmallerEnemies(smallerEnemyPrefab, splitCount);
        
        Destroy(gameObject, 2f);

    }

    private void SpawnSmallerEnemies(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}
