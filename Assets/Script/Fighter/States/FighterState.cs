namespace FighterGame
{
    /// <summary>
    /// ����״̬���ࣺ�����ɫ״̬�Ľӿڣ����о���״̬Ӧ�̳д��ࡣ
    /// </summary>
    public abstract class FighterState
    {
        /// <summary>
        /// �����״̬ʱ���á������ڳ�ʼ��״̬��������ݻ򴥷�������
        /// </summary>
        public abstract void OnEnter(FighterController fighter);

        /// <summary>
        /// �ڸ�״̬��ÿ֡���á�������״̬�����ڼ���߼������ƶ�����������ȡ�
        /// </summary>
        public abstract void UpdateState(FighterController fighter);

        /// <summary>
        /// �뿪��״̬ʱ���á����������������״̬��ص����ݡ�
        /// </summary>
        public abstract void OnExit(FighterController fighter);
    }
}
