using UnityEngine;

public class SplittingEnemy : Enemie
{
    public GameObject smallerEnemyPrefab;  // ������ �������� �����
    public int splitCount = 2;             // ���������� ������� ������

    protected override void Die()  // �������������� ����� Die()
    {
        base.Die();

        // ������� ������� ������
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
