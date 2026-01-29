using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmartAbacusExplosion : MonoBehaviour
{
    public Transform abacusModel;

    [System.Serializable]
    public class ExplosionPart
    {
        public Transform transform;
        public PartType type;
        public Vector3 originalPosition;
        public Vector3 explosionDirection;
        public float explosionDistance;
        public bool isActive = true;
    }

    public enum PartType
    {
        Bead,
        Rod,
        Frame,
        Unknown
    }

    private List<ExplosionPart> allParts = new List<ExplosionPart>();

    [Header("全局爆炸控制")]
    [Range(0, 1)] public float explosionProgress = 0f;

    [Header("默认爆炸参数")]
    public float defaultBeadDistance = 0.3f;
    public float defaultRodDistance = 0.5f;
    public float defaultFrameDistance = 0.8f;

    public Vector3 defaultBeadDirection = Vector3.right;
    public Vector3 defaultRodDirection = Vector3.up;
    public Vector3 defaultFrameDirection = Vector3.forward;

    void Start()
    {
        InitializeParts();
    }

    void Update()
    {
        UpdateExplosion();
    }

    private void InitializeParts()
    {
        allParts.Clear();

        // 收集所有需要爆炸的部件
        Transform[] allChildren = abacusModel.GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren)
        {
            // 跳过根物体
            if (child == abacusModel) continue;

            PartType type = IdentifyPartType(child);

            // 如果无法识别类型，可以跳过或设为Unknown
            if (type == PartType.Unknown)
            {
                // 可以根据需要选择是否包含未知类型
                continue;
            }

            ExplosionPart part = new ExplosionPart
            {
                transform = child,
                type = type,
                originalPosition = child.localPosition,
                isActive = true
            };

            // 根据类型设置默认爆炸参数
            SetDefaultExplosionParams(part);

            allParts.Add(part);
        }

        Debug.Log($"初始化完成：找到 {allParts.Count} 个部件");
        LogPartStatistics();
    }

    private PartType IdentifyPartType(Transform transform)
    {
        string name = transform.name.ToLower();
        MeshRenderer renderer = transform.GetComponent<MeshRenderer>();

        // 通过名称识别
        if (name.Contains("pTorus"))
            return PartType.Bead;

        if (name.Contains("pCylinder"))
            return PartType.Rod;

        if (name.Contains("polySurface"))
            return PartType.Frame;

        // 通过形状识别（如果有MeshFilter）
        MeshFilter meshFilter = transform.GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            // 根据bounds大小判断
            Bounds bounds = renderer != null ? renderer.bounds : meshFilter.sharedMesh.bounds;

            // 算珠通常是接近球形的
            float sizeX = bounds.size.x;
            float sizeY = bounds.size.y;
            float sizeZ = bounds.size.z;

            // 判断是否为近似球形
            if (Mathf.Abs(sizeX - sizeY) < 0.01f && Mathf.Abs(sizeY - sizeZ) < 0.01f)
                return PartType.Bead;

            // 杆子通常是细长的圆柱体
            if ((sizeY > sizeX * 3 && sizeY > sizeZ * 3) ||
                (sizeX > sizeY * 3 && sizeX > sizeZ * 3) ||
                (sizeZ > sizeX * 3 && sizeZ > sizeY * 3))
                return PartType.Rod;
        }

        return PartType.Unknown;
    }

    private void SetDefaultExplosionParams(ExplosionPart part)
    {
        switch (part.type)
        {
            case PartType.Bead:
                part.explosionDirection = defaultBeadDirection;
                part.explosionDistance = defaultBeadDistance;
                break;

            case PartType.Rod:
                part.explosionDirection = defaultRodDirection;
                part.explosionDistance = defaultRodDistance;
                break;

            case PartType.Frame:
                part.explosionDirection = defaultFrameDirection;
                part.explosionDistance = defaultFrameDistance;
                break;

            default:
                part.explosionDirection = Vector3.up;
                part.explosionDistance = 0.5f;
                break;
        }
    }

    private void UpdateExplosion()
    {
        foreach (ExplosionPart part in allParts)
        {
            if (part.transform == null || !part.isActive) continue;

            Vector3 offset = part.explosionDirection * part.explosionDistance * explosionProgress;
            part.transform.localPosition = part.originalPosition + offset;
        }
    }

    private void LogPartStatistics()
    {
        int beadCount = allParts.Count(p => p.type == PartType.Bead);
        int rodCount = allParts.Count(p => p.type == PartType.Rod);
        int frameCount = allParts.Count(p => p.type == PartType.Frame);
        int unknownCount = allParts.Count(p => p.type == PartType.Unknown);

        Debug.Log($"部件统计：算珠={beadCount}, 杆子={rodCount}, 框架={frameCount}, 未知={unknownCount}");
    }

    [ContextMenu("重新初始化")]
    public void Reinitialize()
    {
        InitializeParts();
    }

    [ContextMenu("显示部件类型")]
    public void ShowPartTypes()
    {
        foreach (ExplosionPart part in allParts)
        {
            Debug.Log($"{part.transform.name}: {part.type}");
        }
    }

    // 在编辑器中可视化爆炸方向
    void OnDrawGizmosSelected()
    {
        if (allParts == null || allParts.Count == 0) return;

        foreach (ExplosionPart part in allParts)
        {
            if (part.transform == null) continue;

            // 设置颜色
            switch (part.type)
            {
                case PartType.Bead:
                    Gizmos.color = Color.red;
                    break;
                case PartType.Rod:
                    Gizmos.color = Color.green;
                    break;
                case PartType.Frame:
                    Gizmos.color = Color.blue;
                    break;
                default:
                    Gizmos.color = Color.gray;
                    break;
            }

            // 绘制爆炸方向
            Vector3 start = part.transform.position;
            Vector3 end = start + part.transform.TransformDirection(part.explosionDirection) * part.explosionDistance;

            Gizmos.DrawLine(start, end);
            Gizmos.DrawSphere(start, 0.01f);
        }
    }
}