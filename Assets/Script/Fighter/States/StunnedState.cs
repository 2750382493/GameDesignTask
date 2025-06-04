using UnityEngine;

namespace FighterGame
{
    /// <summary>
    /// ѣ��״̬����ɫ��ʱ�޷��ж���ͨ���������ػ��󴥷���
    /// </summary>
    public class StunnedState : FighterState
    {
        private float stunDuration = 2f;  // ѣ�γ���ʱ��
        private float timer;

        public override void OnEnter(FighterController fighter)
        {
            // ����ѣ��״̬������ѣ�ζ���
            if (fighter.Animator != null)
            {
                fighter.Animator.SetTrigger("Stunned");  // *Ҫ��Animator��"Stunned"������*
            }
            // �����ڴ˲���ѣ����Ч������������ת��
            timer = stunDuration;
            Debug.Log($"{fighter.gameObject.name} ����ѣ��״̬��");
        }

        public override void UpdateState(FighterController fighter)
        {
            // ����ʱѣ�γ���ʱ��
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    // ѣ��ʱ��������ָ�Idle״̬
                    fighter.ChangeState(new IdleState());
                }
            }
        }

        public override void OnExit(FighterController fighter)
        {
            // �뿪ѣ��״̬��������ѣ����Ч
            Debug.Log($"{fighter.gameObject.name} ����ѣ�Σ��ָ��ж�");
        }
    }
}
