using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float Hp = 100f;  // �������� ������
    public float Damage = 20f;  // ���� ������
    public float AttackSpeed = 1f; // ������ ����� �������
    public float AttackRange = 2f; // ��������� �����
    public float MoveSpeed = 5f;    // �������� �����������
    public float SuperAttackDamageMultiplier = 2f;
    public float SuperAttackCooldown = 2f; // ������� ����� �����

    private float lastAttackTime = 0;
    private float lastSuperAttackTime = 0; // ������� ����� �����
    private bool isAttacking = false; // ��������� �����
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

        // �������� �������� ��� ����� �����
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

        Debug.Log("����� HP: " + Hp); // ���������� ���������� � �������� ������
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized; // �����������
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
            Debug.Log("������� ����� ��� ����� ��� ���.");
            return;
        }

        lastAttackTime = Time.time;
        AnimatorController.SetTrigger("Attack");
        isAttacking = true;

        StartCoroutine(PerformAttackWithDelay(0.5f)); // �������� ����� ������
    }

    private IEnumerator PerformAttackWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        var enemies = GameSceneManager.Instance.Enemies;
        Enemie closestEnemie = FindClosestEnemy(enemies);

        if (closestEnemie != null)
        {
            closestEnemie.Hp -= Damage;
            Debug.Log("���� ������� ����: " + Damage);
        }
        else
        {
            Debug.Log("������ � ���� ����� ���.");
        }

        isAttacking = false;
    }

    private void PerformSuperAttack()
    {
        if (Time.time - lastSuperAttackTime < SuperAttackCooldown)
        {
            return;
        }

        lastSuperAttackTime = Time.time; // ��������� ����� ����� �����
        AnimatorController.SetTrigger("SuperAttack");
        Debug.Log("����� ����� ��������!");

        var enemies = GameSceneManager.Instance.Enemies;
        Enemie closestEnemie = FindClosestEnemy(enemies);

        if (closestEnemie != null)
        {
            transform.LookAt(closestEnemie.transform);
            StartCoroutine(ApplyDamageAfterAnimation(closestEnemie, Damage * SuperAttackDamageMultiplier));
        }
        else
        {
            Debug.Log("������ � ���� ����� ����� ���.");
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
        yield return new WaitForSeconds(0.5f); // �������� ����� ���������� �����
        if (enemie != null)
        {
            enemie.Hp -= damage;
            Debug.Log("���� ������� ����: " + damage);
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
        Debug.Log("HP �������������: " + amount + ". ������� HP: " + Hp);
    }

}
