using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraController: MonoBehaviour
{
    [Header("��Ŀ��")]
    [Tooltip("��Ҫ�����Ŀ������")]
    public Transform target;

    [Header("λ�ò���")]
    [Tooltip("�����Ŀ���ˮƽ����")]
    public float distance = 10f;
    [Tooltip("����Ĺ̶��߶�")]
    public float height = 5f;
    [Tooltip("��ֱ�۲�ƫ�ƣ�����Ŀ���Ϸ���")]
    public float verticalLookOffset = 1.5f;

    [Header("�ƶ�����")]
    [Tooltip("����ƽ��ʱ��")]
    public float smoothTime = 0.15f;
    [Tooltip("Ԥ�о���")]
    public float lookAheadDist = 2f;
    [Tooltip("Ԥ����Ӧ�ٶ�")]
    public float lookAheadSpeed = 3f;

    [Header("����ǽ����")]
    [Tooltip("������ײ���")]
    public bool enableCollisionDetection = true;
    [Tooltip("��ײ���뾶")]
    public float collisionRadius = 0.5f;
    [Tooltip("��С�������")]
    public float minDistance = 2f;
    [Header("��Ļ��")]
    [Tooltip("����𶯷���")]
    public float maxShakeMagnitude = 0.5f;
    [Tooltip("��˥������")]
    public AnimationCurve shakeCurve = AnimationCurve.Linear(0, 1, 1, 0);

    private Vector3 originalLocalPosition;
    private Coroutine shakeCoroutine;

    private Vector3 velocity;
    private Vector3 currentLookAhead;
    private Vector3 lastTargetPos;
    private float originalDistance;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("δ��Ŀ�����壡");
            enabled = false;
            return;
        }

        originalDistance = distance;
        lastTargetPos = target.position;
        InitializeCameraPosition();
    }

    void LateUpdate()
    {
        if (target == null) return;

        UpdateLookAhead();
        Vector3 targetPosition = CalculateIdealPosition();

        if (enableCollisionDetection)
            HandleObstacles(ref targetPosition);

        SmoothFollow(targetPosition);
        LookAtTarget();

        lastTargetPos = target.position;
    }

    void InitializeCameraPosition()
    {
        Vector3 backDirection = -target.forward.normalized;
        transform.position = target.position +
                            backDirection * originalDistance +
                            Vector3.up * height;

        transform.LookAt(target.position + Vector3.up * verticalLookOffset);
    }

    void UpdateLookAhead()
    {
        Vector3 delta = target.position - lastTargetPos;
        Vector3 horizontalDelta = new Vector3(delta.x, 0, delta.z);

        if (horizontalDelta.magnitude > 0.1f)
        {
            Vector3 direction = horizontalDelta.normalized;
            currentLookAhead = Vector3.Lerp(
                currentLookAhead,
                direction * lookAheadDist,
                Time.deltaTime * lookAheadSpeed);
        }
        else
        {
            currentLookAhead = Vector3.Lerp(
                currentLookAhead,
                Vector3.zero,
                Time.deltaTime * lookAheadSpeed);
        }
    }

    Vector3 CalculateIdealPosition()
    {
        Vector3 backDirection = -target.forward.normalized;
        return target.position +
              backDirection * distance +
              Vector3.up * height +
              currentLookAhead;
    }

    void HandleObstacles(ref Vector3 targetPos)
    {
        RaycastHit hit;
        Vector3 direction = (targetPos - target.position).normalized;
        float maxDistance = Vector3.Distance(target.position, targetPos);

        if (Physics.SphereCast(
            target.position,
            collisionRadius,
            direction,
            out hit,
            maxDistance))
        {
            distance = Mathf.Clamp(
                hit.distance - collisionRadius,
                minDistance,
                originalDistance);
        }
        else
        {
            distance = originalDistance;
        }
    }

    void LookAtTarget()
    {
        Vector3 lookPoint = target.position +
                           Vector3.up * verticalLookOffset +
                           currentLookAhead * 0.3f;

        transform.LookAt(lookPoint);
    }

    // ���Ի���
    void OnDrawGizmosSelected()
    {
        if (target == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(target.position, transform.position);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CalculateIdealPosition(), 0.5f);

        if (enableCollisionDetection)
        {
            Gizmos.color = Color.cyan;
            Vector3 direction = (transform.position - target.position).normalized;
            Gizmos.DrawLine(target.position, target.position + direction * distance);
        }
    }

    /// <summary>
    /// ������Ļ��
    /// </summary>
    /// <param name="duration">����ʱ��</param>
    /// <param name="magnitude">��ǿ�ȣ�0~1��</param>
    public void TriggerShake(float duration = 0.5f, float magnitude = 0.3f)
    {
        // ȷ��ǿ���ڰ�ȫ��Χ��
        magnitude = Mathf.Clamp01(magnitude);

        // �������������ִ�У���ֹͣ
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(DoShake(duration, magnitude * maxShakeMagnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        originalLocalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // ʹ�����߿���˥��
            float curveProgress = shakeCurve.Evaluate(elapsed / duration);

            // �������ƫ�ƣ���ά�𶯣�
            Vector3 offset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-0.2f, 0.2f)) * magnitude * curveProgress;

            // Ӧ��ƫ�ƣ�����ԭ�и����߼���
            transform.localPosition = originalLocalPosition + offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // �ָ�ԭʼλ��
        transform.localPosition = originalLocalPosition;
        shakeCoroutine = null;
    }

    // �޸�ԭ��SmoothFollow�������������𶯳�ͻ
    void SmoothFollow(Vector3 targetPosition)
    {
        // ��ִ�л�������
        Vector3 smoothPosition = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime);

        // ������ƫ����
        if (shakeCoroutine != null)
        {
            smoothPosition += transform.localPosition - originalLocalPosition;
        }

        transform.position = smoothPosition;
    }
}