using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float lookSensitivity = 100f;    // ���������
    public float maxHeadTurnAngle = 22.5f;    // ͷ����Խ�ɫǰ�������ƫ���Ƕ���ֵ����������ת���壩
    public float bodyTurnSpeed = 90f;       // ��ɫ��ת�ٶȣ���/�룩
    public float headHeight = 1f;         // ����� Head �ڵ㣬���������ڽ�ɫ�ײ��ĸ߶ȣ��ף�

    private Transform player;               // ������� Transform
    private Transform headTransform;        // ���ͷ�� Transform ���������
    private float pitch = 0f;               // ����������Ƕ�
    private float yawOffset = 0f;           // �������Խ�ɫ��ƫ���Ƕȣ��ȣ�

    void Start()
    {
        // �������������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ������Ҷ���
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("δ�ҵ���ǩΪ Player �Ķ���");
            return;
        }
        player = playerObj.transform;

        // ���Ի�ȡ��Ϊ "Head" ���ӽڵ���Ϊͷ��
        headTransform = player.Find("Head");
        if (headTransform != null)
        {
            transform.SetParent(headTransform);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            // ���� Head �ڵ㣬��������ҵ�����ϲ�ʹ��Ԥ��ƫ��
            transform.SetParent(player);
            transform.localPosition = new Vector3(0, headHeight, 0);
        }
        transform.localRotation = Quaternion.identity;

        // ��ʼ��ƫ���ǣ����ɫ��ʼ������룩
        yawOffset = 0f;
    }

    void Update()
    {
        // ��ȡ�����������
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;

        // ���������ǶȲ����Ʒ�Χ���������·�ת
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -89f, 89f);

        // �ۼ�ƫ���Ƕ�
        yawOffset += mouseX;
        // �� yawOffset ������ [-180,180] ��Χ��
        if (yawOffset > 180f) yawOffset -= 360f;
        if (yawOffset < -180f) yawOffset += 360f;

        // ���ƫ���Ƕȳ�����ֵ�����Թ̶��ٶ���ת��ɫ����
        if (Mathf.Abs(yawOffset) > maxHeadTurnAngle)
        {
            float sign = Mathf.Sign(yawOffset);
            float rotateAmount = sign * bodyTurnSpeed * Time.deltaTime;
            // ����ת������ʣ��ƫ����������ת��
            if (Mathf.Abs(rotateAmount) > Mathf.Abs(yawOffset))
                rotateAmount = yawOffset;
            // ��ת��ɫ
            player.Rotate(0, rotateAmount, 0);
            // ����ƫ���ǣ�ʹ�������ָ�����ֵ��Χ��
            yawOffset -= rotateAmount;
        }

        // Ӧ��������ֲ���ת���ȸ�����ƫ��ƫ�ƣ�
        transform.localRotation = Quaternion.Euler(pitch, yawOffset, 0);
    }
}
