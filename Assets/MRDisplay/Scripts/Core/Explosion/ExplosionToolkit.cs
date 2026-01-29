using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplosionToolkit : MonoBehaviour
{
    private static ExplosionToolkit instance;

    public bool UseUnclampedExplosion;

    public Transform ExplosionParent;

    public ExplosionPiece[] ExplosionPieces;

    private float explosionValue = 0.0f;

    private bool isLoaded = false;

    public static ExplosionToolkit Get()
    {
        if (instance == null)
        {
            instance = FindAnyObjectByType<ExplosionToolkit>();
        }
        return instance;
    }

    public void Load(GameObject displayObject)
    {
        ExplosionParent = displayObject.transform;
        Initialized();
        isLoaded = true;
    }

    public void UnLoad()
    {
        Action unloadAction = () =>
        {
            ExplosionParent.gameObject.SetActive(false);
            ExplosionParent = null;
            isLoaded = false;
        };

        Recovery(unloadAction);
    }

    private void Initialized()
    {
        Vector3 parentWorldPos = ExplosionParent.position;
        Quaternion parentWorldRot = ExplosionParent.rotation;
        Vector3 parentWorldScale = ExplosionParent.localScale;

        ExplosionParent.position = Vector3.zero;
        ExplosionParent.rotation = Quaternion.identity;
        ExplosionParent.localScale = Vector3.one;

        // 获取爆炸碎片
        ExplosionPieces = ExplosionParent.GetComponentsInChildren<ExplosionPiece>();

        // 为每个碎片计算爆炸起点和终点（基于原点坐标系）
        foreach (ExplosionPiece piece in ExplosionPieces)
        {
            // 获取中心点（可以是父物体位置，或者自定义中心）
            Transform explosionCenter = ExplosionParent;  // 使用父物体作为中心  
            piece.InitializeExplosionPoints();
        }

        // 恢复父物体的世界变换
        ExplosionParent.position = parentWorldPos;
        ExplosionParent.rotation = parentWorldRot;
        ExplosionParent.localScale = parentWorldScale;

        // 测试
        //Action explosion = null, backaction = null;
        //explosion += () => { AutoChangedExplosionValue(0.0f, 0.3f, backaction); };
        //backaction += () => { AutoChangedExplosionValue(0.3f, 0.0f, explosion); };
        //explosion?.Invoke();
    }

    public void AutoChangedExplosionValue(float startValue, float endValue, Action callback = null)
    {
        if (isLoaded)
        {
            StartCoroutine(ExplosionPieceCoroutine(startValue, endValue, callback));
        }
    }

    private IEnumerator ExplosionPieceCoroutine(float startValue, float endValue, Action callback)
    {
        explosionValue = 0.0f;
        explosionValue = startValue;
        float offset = startValue < endValue ? 0.01f : -0.01f;
        while (explosionValue.ToString("F2") != endValue.ToString("F2"))
        {
            foreach (ExplosionPiece explosionPiece in ExplosionPieces)
            {
                if (UseUnclampedExplosion)
                    explosionPiece.Piece.localPosition = Vector3.LerpUnclamped(explosionPiece.StartPoint, explosionPiece.EndPoint, explosionValue);
                else
                    explosionPiece.Piece.localPosition = Vector3.Lerp(explosionPiece.StartPoint, explosionPiece.EndPoint, explosionValue);
            }
            explosionValue += offset;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.5f);
        callback?.Invoke();
    }

    public void Explosion(Action action = null) => AutoChangedExplosionValue(0.0f, 1.0f, action);

    public void Recovery(Action action = null) => AutoChangedExplosionValue(1.0f, 0.0f, action);
}