using UnityEngine;

public class ExplosionPiece : MonoBehaviour
{
    public Transform Piece;
    public Vector3 StartPoint;      // 局部坐标起点
    public Vector3 EndPoint;        // 局部坐标终点
    public float ExplosionDistance = 2.0f;  // 爆炸距离

    public void InitializeExplosionPoints()
    {
        Piece = transform;
        // 记录当前位置作为起点
        StartPoint = Piece.localPosition;

        // 计算爆炸方向：从原点指向当前碎片位置（世界坐标）
        Vector3 worldPosition = Piece.position;
        Vector3 direction = (worldPosition - Vector3.zero).normalized;

        // 计算终点（世界坐标）
        Vector3 endWorldPos = worldPosition + direction * ExplosionDistance;

        // 转换为局部坐标
        if (transform.parent != null)
        {
            EndPoint = transform.parent.InverseTransformPoint(endWorldPos);
        }
        else
        {
            EndPoint = endWorldPos;  // 如果没有父物体，就是世界坐标
        }
    }
}