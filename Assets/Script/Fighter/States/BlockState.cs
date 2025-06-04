using UnityEngine;

namespace FighterGame
{
    /// <summary>
    /// ��״̬����ɫ���������̬�����ٻ�ֵ������˺���
    /// </summary>
    public class BlockState : FighterState
    {
        private float maxBlockTime = 2f;  // �񵲵�������ʱ�䣨�룩
        private float timer;

        public override void OnEnter(FighterController fighter)
        {
            // �����״̬������������̬����
            if (fighter.Animator != null)
            {
                fighter.Animator.SetTrigger("Block");  // *Ҫ��Animator��"Block"������*
            }
            // ���ø񵲼�ʱ������AI�Զ�����񵲣�
            timer = maxBlockTime;
            // �ڸ�״̬�£����Խ��ͽ�ɫ�ƶ��ٶȻ���ʹ���޷��ƶ�
            // �ڴ�ʾ���У��򵥴���Ϊ�������ƶ���PlayerController/AIController�ڼ�⵽BlockStateʱ�Ѳ�ִ��Move��
        }

        public override void UpdateState(FighterController fighter)
        {
            // �����AI��ɫ����ʹ�ü�ʱ���Զ��˳���
            if (!(fighter is PlayerController))
            {
                if (timer > 0f)
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                    {
                        // AI��һ��ʱ����Զ�������ص�Idle
                        fighter.ChangeState(new IdleState());
                    }
                }
            }
            // ������ҽ�ɫ����״̬���˳������루�ɿ�������������
            // ������ﲻ�Զ��л�״̬������Ȼ������ʱ��Ϊ���ʱ�������ƣ���ѡ����
        }

        public override void OnExit(FighterController fighter)
        {
            // �뿪��״̬�������ø����Ч��
            // ���磺fighter.Animator.ResetTrigger("Block");
            // ���н����ƶ��ٶȻ����ķ�����Ч�������ڴ˻ָ�/�رա�
        }
    }
}
