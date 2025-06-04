using UnityEngine;

namespace FighterGame
{
    /// <summary>
    /// ������ɫ�����ࣺ�ṩ��ɫ���е����Ժ�״̬�����ܣ���Һ�AI���̳��Դˣ���
    /// </summary>
    public class FighterController : MonoBehaviour
    {
        // ���������ֶ� ���� ʹ��[SerializeField]ʹ����Inspector�ɼ����ɵ��������ַ�װ��
        [Header("��ɫ����")]
        [SerializeField] private float moveSpeed = 3f;       // �ƶ��ٶ�
        [SerializeField] private int maxHealth = 100;        // �������ֵ
        [SerializeField] private int health = 100;           // ��ǰ����ֵ
        [SerializeField] private int attackPower = 20;       // ��������ֵ�������˺���
        [SerializeField] private float maxCharge = 100f;     // �������ֵ����������ֵ��
        [SerializeField] private float charge = 0f;          // ��ǰ����ֵ

        [Header("�������")]
        [SerializeField] private Animator animator;          // ����������ã����ڴ���������
        // ���и������ײ�������Ҳ�����ã����磺
        // [SerializeField] private Rigidbody2D rb;

        [Header("Ŀ������")]
        [SerializeField] public FighterController target;    // ����/׷��Ŀ�꣨�����ⲿָ����

        // ��ǰ״̬��״̬���� 
        protected FighterState currentState;

        // ���Է�װ
        public int MaxHealth => maxHealth;
        public int Health => health;
        public float MaxCharge => maxCharge;
        public float Charge => charge;
        public Animator Animator => animator;
        // ���Ը�����Ҫ�ṩ�������Ե�get������������AttackPower��
        public int AttackPower => attackPower;
        public float MoveSpeed => moveSpeed;

        /// <summary>
        /// Unity�������ڣ�Start������Ϸ��ʼʱ��ʼ����ɫ״̬��
        /// </summary>
        protected virtual void Start()
        {
            // ��ʼ��ʱ�趨��ɫΪIdle״̬
            currentState = new IdleState();
            if (currentState != null)
            {
                currentState.OnEnter(this);
            }
        }

        /// <summary>
        /// Unity�������ڣ�Update��ÿ֡���õ�ǰ״̬�ĸ����߼���
        /// </summary>
        protected virtual void Update()
        {
            // ÿ֡���õ�ǰ״̬���߼���������������ж����ƶ����١���ʱ�ȣ�
            if (currentState != null)
            {
                currentState.UpdateState(this);
            }
        }

        /// <summary>
        /// �л���ɫ״̬�ĺ������������״̬�˳�/���뷽�������µ�ǰ״̬���á�
        /// </summary>
        /// <param name="newState">Ҫ�л�������״̬����</param>
        public void ChangeState(FighterState newState)
        {
            // ����е�ǰ״̬���ȵ������˳��߼�
            if (currentState != null)
            {
                currentState.OnExit(this);
            }
            // �л�����״̬
            currentState = newState;
            if (currentState != null)
            {
                currentState.OnEnter(this);
            }
        }

        /// <summary>
        /// �ƶ���ɫ�ķ���������ָ�����������ƶ�������moveSpeed�ٶȣ���
        /// </summary>
        /// <param name="direction">�ƶ����򣨽�XYƽ�棬����2D��Ϸ��X�ᣩ</param>
        public void Move(Vector2 direction)
        {
            // ���շ�����ٶ��ƶ���ɫ���������򵥵������ƶ���û��ʹ���������棩
            transform.Translate(direction * moveSpeed * Time.deltaTime);
            // ��ѡ�������ƶ����������ɫ����
            if (direction.x != 0)
            {
                // ˮƽ��ת��ɫ����ʹ�������ƶ����򣨼����ɫ����Ϊ����
                Vector3 scale = transform.localScale;
                scale.x = direction.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }

        /// <summary>
        /// ��ɫ�ܵ��˺�ʱ���ã���������ֵ�������ܻ�������
        /// </summary>
        /// <param name="damageAmount">�˺�ֵ</param>
        public void TakeDamage(int damageAmount)
        {
            if (damageAmount <= 0) return;
            health = Mathf.Max(health - damageAmount, 0);
            // �����ܻ���������Animator����Ӧ�Ķ������������ã�
            if (animator != null)
            {
                animator.SetTrigger("Hit");  // *Ҫ��Animator������Ϊ"Hit"�Ĵ�������*
            }
            // ������ֵ����0�����£������ɫ����������ֻ�򵥽��ö�����Ϊʾ����
            if (health <= 0)
            {
                // ��ɫ�����߼������Բ����������������ٶ����
                Debug.Log($"{gameObject.name} ����");
                // �ڴ�ʾ���У���ͣ�ý�ɫ
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// ��������ֵ�ķ�������������״̬������ֵ������������ֵ������������������
        /// </summary>
        public void AddCharge(float amount)
        {
            if (amount == 0) return;
            // ��������ֵ��������0��maxCharge��Χ��
            charge = Mathf.Clamp(charge + amount, 0f, maxCharge);
        }

        /// <summary>
        /// ����ȫ�������������ͷ�����������㴦����
        /// </summary>
        public void ConsumeAllCharge()
        {
            charge = 0f;
        }
    }
}
