using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public enum State { Idle, Chase, Attack, Stunned, Dead }
    public State currentState = State.Idle;

    public Transform player;            // ���Ŀ��
    public float chaseDistance = 10f;   // ׷����������
    public float attackDistance = 2f;   // ������������
    public float moveSpeed = 3f;        // �ƶ��ٶ�
    public float turnSpeed = 5f;        // ת���ٶ�
    public bool hardMode = false;       // ����ģʽ����

    private Animator animator;
    private Health healthComponent;
    private float stunTimer = 0f;
    private float maxStunTime = 1.0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        healthComponent = GetComponent<Health>();
    }

    void Update()
    {
        if (currentState == State.Dead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                animator.SetBool("IsMoving", false);
                // �����ҿ��������׷��״̬
                if (distanceToPlayer < chaseDistance)
                {
                    currentState = State.Chase;
                }
                break;

            case State.Chase:
                // ��ת�������
                Vector3 dir = (player.position - transform.position).normalized;
                Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * turnSpeed);
                // �ƶ��ӽ����
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
                animator.SetBool("IsMoving", true);
                // �����㹻�����л�������״̬
                if (distanceToPlayer < attackDistance)
                {
                    currentState = State.Attack;
                }
                break;

            case State.Attack:
                animator.SetBool("IsMoving", false);
                animator.SetTrigger("Attack");
                // ������������ʱ��ͨ�� Animation Event ���� DealDamage()
                // ���������󷵻�׷�������
                // �˴�����Э����ʱģ�⶯�����
                StartCoroutine(ResetAfterAttack(1.0f));
                currentState = State.Stunned; // ��ʱ��Stunned״̬�ȴ��������
                break;

            case State.Stunned:
                stunTimer += Time.deltaTime;
                if (stunTimer >= maxStunTime)
                {
                    stunTimer = 0f;
                    // �����ҽ�����������������׷��
                    currentState = (distanceToPlayer < attackDistance) ? State.Attack : State.Chase;
                }
                break;
        }
    }

    // �ܵ��˺�ʱ����
    public void TakeDamage(int damage)
    {
        if (currentState == State.Dead) return;
        healthComponent.TakeDamage(damage);
        animator.SetTrigger("Hit");
        currentState = State.Stunned;
        stunTimer = 0f;
        // ���Ѫ����Ϊ0����������
        if (healthComponent.CurrentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator ResetAfterAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        // �ص�׷��״̬
        currentState = State.Chase;
    }

    void Die()
    {
        currentState = State.Dead;
        animator.SetBool("IsDead", true);
        // ���ڶ���������¼����ٻ��������
        Destroy(gameObject, 2f);
    }
}
