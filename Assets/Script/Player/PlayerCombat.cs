using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerCombat : MonoBehaviour
{

    [Header("����")]
    [SerializeField] private Camera cam;            // ���߳��򡪡��������Զ� Camera.main
    [SerializeField] private LayerMask enemyMask;   // ֻ�����˵Ĳ�
    [SerializeField] private LayerMask obstacleMask;// ���赲���ߵ�ǽ���
    [SerializeField] private Transform muzzle;      // ���߷���㣨Ϊ��Ĭ�������λ�ã�

    [Header("��ͨ����")]
    public int normalDamage = 10;   // ��ͨ�����˺�
    public float attackRange = 2.5f; // ��󹥻����루�ף�
    public float attackRate = 0.5f; // �չ���ȴ���룩
    public float hitForce = 5f;   // ���л������ȣ��Ը�����ˣ�

    [Header("��������")]
    public int heavyDamage = 25;  // ���������˺�
    public float chargeThreshold = 3f;// ������������Ϊ�������
    public float heavyCooldown = 1.0f;// ����������ȴ���룩

    [Header("�� (�Ҽ�����)")]
    [Range(0f, 1f)]
    public float blockDamageRate = 0.15f; // ��ʱ���������˺�(0=ȫ��,1=ȫ��)
    public AudioClip blockStartSfx;        // ��ʼ����
    public AudioClip blockEndSfx;          // ��������

    [Header("ͨ����Ч")]
    public AudioClip swingSfx; // ��ȭ����
    public AudioClip hitSfx;   // ������


    private float nextAttackTime; // ��ǰ��ȴ����ʱ���
    private bool isCharging;     // �Ƿ��������׶�
    private float chargeTimer;    // ������ʱ��
    private bool isBlocking;     // ��ǰ�Ƿ�ס�Ҽ���
    private AudioSource audioSrc; // ����������

    // UI �����¼����������ٷֱ� (0~1)
    public event Action<float> OnChargeUpdate;

    // �ṩ�������ű���Health����ȡ��ֻ������
    public bool IsBlocking => isBlocking;
    public float BlockDamageRate => blockDamageRate;


    private void Start()
    {
        if (!cam) cam = Camera.main;     // �Զ��������
        if (!muzzle) muzzle = cam.transform; // Ĭ�����ߴ��������
        audioSrc = GetComponent<AudioSource>();

        Debug.Log("<color=cyan>[PlayerCombat]</color> ��ʼ�����");
    }

    private void Update() => HandleInput();

    private void HandleInput()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1))
            BeginBlock();

        if (Input.GetKeyUp(KeyCode.Mouse1))
            EndBlock();

        // ��ʱ��ֹ����������
        if (isBlocking) return;

        // ���������£���ʼ��ʱ
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextAttackTime)
        {
            isCharging = true;
            chargeTimer = 0f;
            Debug.Log("<color=yellow>[PlayerCombat]</color> ��ʼ����");
        }

        // ��ס������ۻ�����ʱ�䣨UI �ٷֱȻص���
        if (isCharging && Input.GetKey(KeyCode.Mouse0))
        {
            chargeTimer += Time.deltaTime;
            OnChargeUpdate?.Invoke(Mathf.Clamp01(chargeTimer / chargeThreshold));

            Debug.Log($"<color=yellow>[PlayerCombat]</color> ��������: {chargeTimer:F1}s");
        }

        // ����ɿ����������������жϳ��ַ�ʽ
        if (Input.GetKeyUp(KeyCode.Mouse0) && isCharging)
        {
            if (chargeTimer >= chargeThreshold)
            {
                // ��������
                Debug.Log("<color=lime>[PlayerCombat]</color> ������������");
                StartCoroutine(PerformHeavyAttack());
                nextAttackTime = Time.time + heavyCooldown;
            }
            else
            {
                // ��ͨ����
                Debug.Log("<color=lime>[PlayerCombat]</color> ������ͨ����");
                PerformNormalAttack();
                nextAttackTime = Time.time + attackRate;
            }

            // ��������״̬ & UI
            isCharging = false;
            OnChargeUpdate?.Invoke(0f);
        }
    }

    private void BeginBlock()
    {
        if (isBlocking) return;
        isBlocking = true;
        //PlaySound(blockStartSfx);
        // TODO���ڴ˴����񵲶��� / UI ����
        Debug.Log("<color=orange>[PlayerCombat]</color> �����״̬");
    }

    private void EndBlock()
    {
        if (!isBlocking) return;
        isBlocking = false;
        //PlaySound(blockEndSfx);
        // TODO���رո񵲶��� / UI
        Debug.Log("<color=orange>[PlayerCombat]</color> �˳���״̬");
    }

    private void PerformNormalAttack()
    {
        PlaySound(swingSfx);

        // ���������е��ˣ�ApplyDamage ���� true
        if (RayHitEnemy(out RaycastHit hit))
        {
            Debug.Log("<color=green>[PlayerCombat]</color> �չ����� -> " + hit.collider.name);
            ApplyDamage(hit, normalDamage);
        }
        else
        {
            Debug.Log("<color=grey>[PlayerCombat]</color> �չ�δ����");
        }
    }

    /// <summary>
    /// Э�����ڵȴ�����/��Ч�еġ���ȭ��ֵ��֡
    /// </summary>
    private IEnumerator PerformHeavyAttack()
    {
        //PlaySound(swingSfx);
        yield return new WaitForSeconds(0.1f); // 0.1 ����ж�����

        if (RayHitEnemy(out RaycastHit hit))
        {
            ApplyDamage(hit, heavyDamage);
            Debug.Log("<color=red>[PlayerCombat]</color> �������� -> " + hit.collider.name);
        }
        else
        {
            Debug.Log("<color=grey>[PlayerCombat]</color> ����δ����");
        }
    }

    /// <summary>
    /// ����һ�����ߣ����ȼ���ϰ������ˣ�ֻ�ڿɹ��������ڷ�����
    /// </summary>
    private bool RayHitEnemy(out RaycastHit hitInfo)
    {
        if (Physics.Raycast(muzzle.position, muzzle.forward,
                            out hitInfo,
                            attackRange,
                            obstacleMask | enemyMask,
                            QueryTriggerInteraction.Ignore))
        {
            // �ж��׸������Ƿ���� (�� enemyMask ��)
            bool isEnemy = ((1 << hitInfo.collider.gameObject.layer) & enemyMask.value) != 0;
            return isEnemy;
        }
        return false;
    }

    /// <summary>
    /// �����еĵ��˿�Ѫ / ���ˣ������� UI ���з���
    /// </summary>
    private void ApplyDamage(RaycastHit hit, int dmg)
    {
        PlaySound(hitSfx);

        // ���ˣ������˴����壩
        if (hit.rigidbody)
            hit.rigidbody.AddForce(muzzle.forward * hitForce, ForceMode.Impulse);

        // ��Ѫ
        if (hit.collider.TryGetComponent(out EnemyAI enemy))
            enemy.TakeDamage(dmg);

        // ���� UI ��˸������ UIManager ʵ�֣�
        FindObjectOfType<UIManager>()?.HitFlash();
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip) audioSrc.PlayOneShot(clip);
    }
}
