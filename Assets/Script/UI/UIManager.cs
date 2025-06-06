using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public Slider healthBar;        // ���Ѫ����UI Slider��
    public GameObject winPanel;     // ʤ�����
    public GameObject losePanel;    // ʧ�����

    public Image crosshair;
    public Color hitColor = Color.red;
    public float hitFlashDuration = 0.1f;

    public void HitFlash()
    {
        StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {
        Color ori = crosshair.color;
        crosshair.color = hitColor;
        yield return new WaitForSeconds(hitFlashDuration);
        crosshair.color = ori;
    }


    void Start()
    {
        if (healthBar) healthBar.value = 1f;
        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
    }

    // ����Ѫ��������0~max֮��ĵ�ǰѪ����
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthBar) healthBar.value = (float)currentHealth / maxHealth;
    }

    // ��ʾʤ������
    public void ShowWin()
    {
        if (winPanel) winPanel.SetActive(true);
    }

    // ��ʾʧ�ܽ���
    public void ShowLose()
    {
        if (losePanel) losePanel.SetActive(true);
    }
}
