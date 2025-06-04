using UnityEngine;
using UnityEngine.UI;

namespace FighterGame
{
    /// <summary>
    /// ��ҿ��ƽű����������������ƽ�ɫ��Ϊ���̳��Ի�����ɫ�����ࡣ
    /// </summary>
    public class PlayerController : FighterController
    {
        // ������Inspector�����ö�Ӧ�����밴ť���ƣ�������Ŀ��Ҫ������
        [SerializeField] private string horizontalAxis = "Horizontal";  // ˮƽ�ƶ�������
        [SerializeField] private string attackButton = "Fire1";        // ������ť����
        [SerializeField] private string blockButton = "Fire2";         // ������ť����
        [SerializeField] private string chargeButton = "Fire3";        // ������ť����

        /// <summary>
        /// ÿ֡���£���ȡ������벢��������ı��ɫ״̬���ƶ���ɫ��
        /// </summary>
        protected override void Update()
        {
            // ��ȡˮƽ�ƶ�����
            float moveInput = Input.GetAxis(horizontalAxis);
            // ֻ����Idle״̬�½�ɫ���������ƶ������⹥�����ܻ���״̬���ƶ���
            if (currentState is IdleState && Mathf.Abs(moveInput) > 0.01f)
            {
                // �����뷽���ƶ���ɫ
                Vector2 moveDir = new Vector2(moveInput, 0f);
                Move(moveDir);
            }

            // �������룺�����¹������ҵ�ǰ���ڹ�����ѣ��״̬ʱ�����빥��״̬
            if (Input.GetButtonDown(attackButton))
            {
                // ֻ���ڷǹ�������ѣ��״̬�²�����Ӧ��������
                if (!(currentState is AttackState) && !(currentState is StunnedState))
                {
                    ChangeState(new AttackState());
                }
            }

            // �������룺�����·������ҵ�ǰ���ڷ���״̬ʱ�������״̬
            if (Input.GetButtonDown(blockButton))
            {
                if (!(currentState is BlockState) && !(currentState is StunnedState))
                {
                    ChangeState(new BlockState());
                }
            }
            // �������ɿ��������ǰ�ڸ�״̬���ɿ�������ָ�Idle״̬
            if (Input.GetButtonUp(blockButton))
            {
                if (currentState is BlockState)
                {
                    ChangeState(new IdleState());
                }
            }

            // �������룺�������������ҵ�ǰ������������������״̬ʱ����������״̬
            if (Input.GetButtonDown(chargeButton))
            {
                if (!(currentState is ChargeState) && !(currentState is StunnedState))
                {
                    ChangeState(new ChargeState());
                }
            }
            // �������ɿ��������ǰ������״̬���ɿ�������������ص�Idle״̬
            if (Input.GetButtonUp(chargeButton))
            {
                if (currentState is ChargeState)
                {
                    ChangeState(new IdleState());
                }
            }

            // ���û�����Update�Լ�������ǰ״̬�ĸ����߼������繥����ʱ��ѣ�λָ��ȣ�
            base.Update();
        }
    }
}
