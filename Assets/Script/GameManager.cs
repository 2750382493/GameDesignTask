using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject winPanel;    // ʤ������
    public GameObject losePanel;   // ʧ�ܽ���
    private int enemiesRemaining;

    void Awake()
    {
        // ����ģʽ
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // ͳ�Ƴ��������е���
        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
        // ����ʤ������
        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
    }

    // ��������ʱ����
    public void OnEnemyDefeated()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0)
        {
            WinGame();
        }
    }

    // �������ʱ����
    public void OnPlayerDefeated()
    {
        LoseGame();
    }

    void WinGame()
    {
        if (winPanel) winPanel.SetActive(true);
        Time.timeScale = 0f;  // ��ͣ��Ϸ
    }

    void LoseGame()
    {
        if (losePanel) losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // �����ڰ�ť���¿�ʼ��Ϸ
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
