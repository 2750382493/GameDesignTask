using UnityEngine;

namespace FighterGame
{
    /// <summary>
    /// ����״̬����ɫִ�й���������״̬��
    /// </summary>
    public class AttackState : FighterState
    {
        // ��������ʱ�䣨�룩������ģ�⹥������ʱ���Ӳֱ
        private float attackDuration = 0.5f;
        private float timer;

        public override void OnEnter(FighterController fighter)
        {
            // ������ʼ��������������
            if (fighter.Animator != null)
            {
                fighter.Animator.SetTrigger("Attack");  // *Ҫ��Animator��"Attack"������*
            }
            timer = attackDuration;  // ���ü�ʱ��

            // ��鲢Ӧ���˺�
            FighterController target = fighter.target;
            if (target != null)
            {
                // �ж�Ŀ���Ƿ��ڹ�����Χ��
                float dist = Vector3.Distance(fighter.transform.position, target.transform.position);
                if (dist <= (fighter is AIController ai ? ai.AttackPower : fighter.AttackPower))
                {
                    // ����򵥼��蹥�����룬�ɸ�Ϊfighter�Ĺ�����Χ���ԡ����AIController��attackRange������ai.attackRange��

                    // �ж�Ŀ���Ƿ��ڸ�״̬
                    if (target.currentState is BlockState)
                    {
                        // Ŀ���ڸ񵲣������˺�����ȫ�ֵ�
                        int reducedDamage = Mathf.Max(fighter.AttackPower / 2, 1);
                        target.TakeDamage(reducedDamage);
                        // ������²�����ѣ��Ч������������Ѫ���������ڴ���Ӹ���Ч
                        Debug.Log("�������񵲣��˺�����");
                    }
                    else
                    {
                        // Ŀ��δ�񵲣���������˺�
                        target.TakeDamage(fighter.AttackPower);
                        // ����Ƿ�Ϊ���������ػ����������ʹĿ�����ѣ��״̬
                        if (fighter.Charge >= fighter.MaxCharge && target.Health > 0)
                        {
                            // ��Ŀ���л�Ϊѣ��״̬������չ����ߵ�����ֵ
                            target.ChangeState(new StunnedState());
                            fighter.ConsumeAllCharge();
                            Debug.Log("�ػ��ɹ���Ŀ�����ѣ��״̬");
                        }
                    }
                }
            }
        }

        public override void UpdateState(FighterController fighter)
        {
            // ����״̬��ʱ���ȴ���������/Ӳֱ����
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    // ���������������л���Idle����״̬
                    fighter.ChangeState(new IdleState());
                }
            }
        }

        public override void OnExit(FighterController fighter)
        {
            // ����״̬������������Դ��������������β����
            // ���磺���ù���������������ʹAnimator�ص�Idle״̬��IdleState��OnEnterҲ�ᴥ��Idle������
            // fighter.Animator.ResetTrigger("Attack");
        }
    }
}
