using UnityEngine;
using UnityEngine.UI;

namespace FighterGame
{
    /// <summary>
    /// Ѫ����UI�ű�����Slider���ɫ����ֵ������ʵʱ������ʾ��
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private FighterController fighter;  // Ҫ��������ֵ�Ľ�ɫ
        private Slider slider;

        void Awake()
        {
            // ��ȡSlider�������
            slider = GetComponent<Slider>();
        }

        void Start()
        {
            if (fighter != null && slider != null)
            {
                // ��ʼ��Slider�����ֵ�͵�ǰֵ
                slider.maxValue = fighter.MaxHealth;
                slider.value = fighter.Health;
            }
        }

        void Update()
        {
            if (fighter != null && slider != null)
            {
                // ÿ֡����Sliderֵ�Է�ӳ��ɫ��ǰ����ֵ
                slider.value = fighter.Health;
            }
        }
    }
}
