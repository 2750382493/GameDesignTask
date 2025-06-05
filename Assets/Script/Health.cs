using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int CurrentHealth { get; private set; }
    public UnityEvent onHealthChanged;  // �����ڸ���UI
    public UnityEvent onDeath;          // �����ڴ��������¼�

    void Awake()
    {
        CurrentHealth = maxHealth;
        if (onHealthChanged == null) onHealthChanged = new UnityEvent();
        if (onDeath == null) onDeath = new UnityEvent();
    }

    // ��Ѫ
    public void TakeDamage(int amount)
    {
        // �� ����Լ��� PlayerCombat ��������ڸ� �� ����
        if (TryGetComponent(out PlayerCombat pc) && pc.IsBlocking)
        {
            amount = Mathf.CeilToInt(amount * pc.BlockDamageRate);
            // �� ͬʱ������Ӳֱ / ѣ���߼�
            //    ������ֱ�Ӳ����� Stun���������� Stun ʱ�䣩
        }

        CurrentHealth -= amount;
        onHealthChanged.Invoke();

        if (CurrentHealth <= 0) Die();
    }

    // ��Ѫ
    public void Heal(int amount)
    {
        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
        onHealthChanged.Invoke();
    }

    void Die()
    {
        onDeath.Invoke();
        // ����ǵ��˿��������壬�������ҿɴ���ʧ��
        // ���崦����� GameManager �����ж��� onDeath �¼�
    }
}
