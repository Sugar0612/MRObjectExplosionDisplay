using UnityEngine;

public class SelfRotate : MonoBehaviour
{
    [Tooltip("旋转速度（度/秒），分别绕X、Y、Z轴")]
    public Vector3 rotationSpeed = new Vector3(0, 90, 0); // 默认绕Y轴每秒旋转90度

    [Tooltip("是否使用本地坐标系旋转（勾选：绕自身轴旋转；不勾选：绕世界轴旋转）")]
    public bool local = true;

    void Update()
    {
        // 计算当前帧应该旋转的角度 = 速度 * 时间增量
        Vector3 angles = rotationSpeed * Time.deltaTime;

        if (local)
        {
            // 绕物体的本地轴旋转
            transform.Rotate(angles, Space.Self);
        }
        else
        {
            // 绕世界轴旋转
            transform.Rotate(angles, Space.World);
        }
    }
}