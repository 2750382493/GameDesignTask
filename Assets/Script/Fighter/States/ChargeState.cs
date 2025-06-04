using UnityEngine;

namespace FighterGame
{
    /// <summary>
    /// ����״̬����ɫ��������������´ι���������
    /// </summary>
    public class ChargeState : FighterState
    {
        [SerializeField] private float chargeRate = 30f;  // �����ٶȣ�ÿ�����ӵ�����ֵ
        private bool fullyCharged = false;                // �Ƿ��Ѿ�������

        public override void OnEnter(FighterController fighter)
        {
            // ��������״̬��������������/��Ч
            if (fighter.Animator != null)
            {
                fighter.Animator.SetTrigger("Charge");  // *Ҫ��Animator��"Charge"������*
            }
            fullyCharged = false;
            Debug.Log("��ʼ����...");
        }

        public override void UpdateState(FighterController fighter)
        {
            if (!fullyCharged)
            {
                // ������������ֵ
                fighter.AddCharge(chargeRate * Time.deltaTime);
                // ����Ƿ���������
                if (fighter.Charge >= fighter.MaxCharge)
                {
                    fullyCharged = true;
                    // �������󣬿��ڴ˴���һ����ʾ��Ч��
                    Debug.Log("����������");
                    // ע�⣺���ǲ�δ�ڴ��Զ��˳�����״̬���ȴ�����ɿ���������������
                }
            }
            // ����ɿ��������ļ����PlayerController����������������Զ��л�״̬
            // ����ҪAIʹ���������ɿ�����AIController�߼��и�������˳�����״̬
        }

        public override void OnExit(FighterController fighter)
        {
            // �뿪����״̬����ֹͣ������������Ч
            if (fighter.Animator != null)
            {
                // fighter.Animator.ResetTrigger("Charge");
            }
            // ������˳�ʱ��δ������Ҳ�����ڴ�ȡ���ѻ��۵Ĳ���������������ȡ�����������ȣ�
            Debug.Log("������������ǰ����ֵ:" + fighter.Charge);
        }
    }
}
