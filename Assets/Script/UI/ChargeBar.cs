using UnityEngine;
using UnityEngine.UI;

namespace FighterGame
{
    /// <summary>
    /// ������UI�ű�����Slider���ɫ����ֵ������ʵʱ������ʾ��
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class ChargeBar : MonoBehaviour
    {
        [SerializeField] private FighterController fighter;  // Ҫ��������ֵ�Ľ�ɫ
        private Slider slider;

        void Awake()
        {
            slider = GetComponent<Slider>();
        }

        void Start()
        {
            if (fighter != null && slider != null)
            {
                slider.maxValue = fighter.MaxCharge;
                slider.value = fighter.Charge;
            }
        }

        void Update()
        {
            if (fighter != null && slider != null)
            {
                slider.value = fighter.Charge;
            }
        }
    }
}
