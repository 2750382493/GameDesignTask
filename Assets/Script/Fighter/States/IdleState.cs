namespace FighterGame
{
    /// <summary>
    /// ����״̬����ɫĬ��״̬����ֹ�ȴ������Ŀ�ꡣ
    /// </summary>
    public class IdleState : FighterState
    {
        public override void OnEnter(FighterController fighter)
        {
            // �������״̬������Idle��������
            if (fighter.Animator != null)
            {
                fighter.Animator.SetTrigger("Idle");  // ����Animator����Ϊ"Idle"�Ĵ�����������
            }
            // ��������������ĳЩ״̬���������ƶ��ٶȻָ�������ֹͣ�ƶ���
            // ʾ����ֹͣ��ɫ�ٶȣ����ʹ�ø����ƶ����ɽ��ٶ���Ϊ0��
            // fighter.GetComponent<Rigidbody2D>()?.velocity = Vector2.zero;
        }

        public override void UpdateState(FighterController fighter)
        {
            // ����״̬��һ�㲻����ת��״̬������������AI�߼�������ʱ�л���
            // ����������ջ����һЩ����ʱ����Ϊ��������΢ҡ�ڶ�����۲����ܵȡ�
        }

        public override void OnExit(FighterController fighter)
        {
            // �뿪����״̬��ͨ���������⴦�������Ҫ�����ڴ�������¼״̬�˳���
            // ���磺fighter.Animator.ResetTrigger("Idle");
        }
    }
}
