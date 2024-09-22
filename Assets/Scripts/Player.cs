using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float Hp = 100f;  // Здоровье игрока
    public float Damage = 20f;  // Урон игрока
    public float AttackSpeed = 1f; // Период между атаками
    public float AttackRange = 2f; // Дальность атаки
    public float MoveSpeed = 5f;    // Скорость перемещения
    public float SuperAttackDamageMultiplier = 2f;
    public float SuperAttackCooldown = 2f; // Кулдаун супер атаки

    private float lastAttackTime = 0;
    private float lastSuperAttackTime = 0; // Кулдаун супер атаки
    private bool isAttacking = false; // Индикатор атаки
    private bool isDead = false;
    public Animator AnimatorController;

    public Button AttackButton;
    public Button SuperAttackButton;
    public Image SuperAttackCooldownImage;

    private void Start()
    {
        AttackButton.onClick.AddListener(InitiateAttack);
        SuperAttackButton.onClick.AddListener(PerformSuperAttack);
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Hp <= 0)
        {
            Die();
            return;
        }

        // Проверка кулдауна для супер атаки
        if (Time.time - lastSuperAttackTime < SuperAttackCooldown)
        {
            SuperAttackButton.interactable = false;
            SuperAttackCooldownImage.fillAmount = (Time.time - lastSuperAttackTime) / SuperAttackCooldown;
        }
        else
        {
            SuperAttackButton.interactable = true;
            SuperAttackCooldownImage.fillAmount = 1;
        }

        Debug.Log("Игрок HP: " + Hp); // Отладочная информация о здоровье игрока
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized; // Нормализуем
        if (movement != Vector3.zero)
        {
            transform.position += movement * MoveSpeed * Time.fixedDeltaTime;
            transform.rotation = Quaternion.LookRotation(movement);
        }
    }

    private void InitiateAttack()
    {
        if (isAttacking || Time.time - lastAttackTime < AttackSpeed)
        {
            Debug.Log("Кулдаун атаки или атака уже идёт.");
            return;
        }

        lastAttackTime = Time.time;
        AnimatorController.SetTrigger("Attack");
        isAttacking = true;

        StartCoroutine(PerformAttackWithDelay(0.5f)); // Задержка перед атакой
    }

    private IEnumerator PerformAttackWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        var enemies = GameSceneManager.Instance.Enemies;
        Enemie closestEnemie = FindClosestEnemy(enemies);

        if (closestEnemie != null)
        {
            closestEnemie.Hp -= Damage;
            Debug.Log("Враг получил урон: " + Damage);
        }
        else
        {
            Debug.Log("Врагов в зоне атаки нет.");
        }

        isAttacking = false;
    }

    private void PerformSuperAttack()
    {
        if (Time.time - lastSuperAttackTime < SuperAttackCooldown)
        {
            return;
        }

        lastSuperAttackTime = Time.time; // Обновляем время супер атаки
        AnimatorController.SetTrigger("SuperAttack");
        Debug.Log("Супер атака началась!");

        var enemies = GameSceneManager.Instance.Enemies;
        Enemie closestEnemie = FindClosestEnemy(enemies);

        if (closestEnemie != null)
        {
            transform.LookAt(closestEnemie.transform);
            StartCoroutine(ApplyDamageAfterAnimation(closestEnemie, Damage * SuperAttackDamageMultiplier));
        }
        else
        {
            Debug.Log("Врагов в зоне супер атаки нет.");
        }
    }

    private Enemie FindClosestEnemy(List<Enemie> enemies)
    {
        Enemie closestEnemie = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemie in enemies)
        {
            if (enemie == null) continue;

            float distance = Vector3.Distance(transform.position, enemie.transform.position);
            if (distance <= AttackRange)
            {
                if (closestEnemie == null || distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemie = enemie;
                }
            }
        }

        return closestEnemie;
    }

    private IEnumerator ApplyDamageAfterAnimation(Enemie enemie, float damage)
    {
        yield return new WaitForSeconds(0.5f); // Задержка перед нанесением урона
        if (enemie != null)
        {
            enemie.Hp -= damage;
            Debug.Log("Враг получил урон: " + damage);
        }
    }

    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");
        GameSceneManager.Instance.GameOver();
    }

    public void RestoreHealth(float amount)
    {
        Hp += amount;
        Debug.Log("HP восстановлено: " + amount + ". Текущий HP: " + Hp);
    }

}
