using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace FighterGame
{
    /// <summary>
    /// AI���ƽű������ڿ��Ƶз���ɫ����Ϊ�߼����̳��Ի�����ɫ�����ࡣ
    /// </summary>
    public class AIController : FighterController
    {
        [SerializeField] private float detectionRange = 5f;  // ׷����������
        [SerializeField] private float attackRange = 1.2f;   // ��������

        // ���Ը�����Ҫ��Ӹ���AI������ز���������񵲼��ʡ��������ʵ�

        protected override void Start()
        {
            base.Start();
            // ���δ��Inspector��ָ��Ŀ�꣬�����Զ����ҳ����е������ΪĿ��
            if (target == null)
            {
                PlayerController player = FindObjectOfType<PlayerController>();
                if (player != null)
                    target = player;
            }
        }

        /// <summary>
        /// ÿ֡���£�������Ŀ��ľ����л�AI״̬��ִ��AI��Ϊ��
        /// </summary>
        protected override void Update()
        {
            if (target != null)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);

                // �����ǰ״̬��Idle����ҽ���׷����Χ�����л�Ϊ׷��״̬
                if (currentState is IdleState && distance <= detectionRange)
                {
                    ChangeState(new ChaseState());
                }
                // �����ǰ״̬��Chase��׷������
                else if (currentState is ChaseState)
                {
                    // ����ҽ��빥����Χ���л�Ϊ����״̬
                    if (distance <= attackRange)
                    {
                        ChangeState(new AttackState());
                    }
                    // ������ܳ�׷����Χ���л�Idleֹͣ׷��
                    else if (distance > detectionRange * 1.5f)
                    {
                        // *ע��ʹ��1.5��׷����Χ��Ϊ��ʧĿ�����ֵ������Ƶ�������л����ɸ�����Ҫ����*
                        ChangeState(new IdleState());
                    }
                }
            }

            // ��ѡ���������AI��Ϊ���ߣ������������񵲻�����
            // if(currentState is IdleState && Random.value < 0.01f) { ChangeState(new BlockState()); }

            // ���û���Update����ǰ״̬���ڲ������߼�
            base.Update();
        }
    }
}
