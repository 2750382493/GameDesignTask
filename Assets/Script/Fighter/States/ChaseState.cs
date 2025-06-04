using UnityEngine;

namespace FighterGame
{
    /// <summary>
    /// ׷��״̬��AI��ɫ��Ŀ���ƶ���״̬��
    /// </summary>
    public class ChaseState : FighterState
    {
        public override void OnEnter(FighterController fighter)
        {
            // ����׷��״̬����������/׷������
            if (fighter.Animator != null)
            {
                fighter.Animator.SetTrigger("Run");  // ����Animator����Ϊ"Run"�Ķ���
            }
        }

        public override void UpdateState(FighterController fighter)
        {
            // ������Ŀ���ƶ�
            if (fighter.target != null)
            {
                // ���㳯��Ŀ��ķ�����������λ����
                Vector3 direction = (fighter.target.transform.position - fighter.transform.position).normalized;
                // ������ˮƽ����������ֻ��ˮƽ����׷��������2D��Ϸ��
                direction.y = 0;
                // ���û����ƶ�������Ŀ�귽���ƶ�
                fighter.Move(new Vector2(direction.x, direction.y));
            }
            // *״̬�л��ж�*���Ƿ���AIController�д���ͨ��AIController.Update����ݾ�����������ChangeState�����ڴ��ڲ�ֱ�Ӹı�״̬��
            // ��������������Ӱ�ȫ��飬����Ŀ�궪ʧʱֱ���л���Idle��
            // if (fighter.target == null) { fighter.ChangeState(new IdleState()); }
        }

        public override void OnExit(FighterController fighter)
        {
            // �뿪׷��״̬�����ڴ�ֹͣ�ƶ������ö���״̬
            if (fighter.Animator != null)
            {
                // ֹͣ���ܶ����������л�ΪIdle�������ڽ���IdleStateʱ�ᴥ��Idle�������ѡ��
                // fighter.Animator.ResetTrigger("Run");
            }
        }
    }
}
