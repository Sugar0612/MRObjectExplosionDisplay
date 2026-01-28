using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplosionManager : MonoBehaviour
{
    public static ExplosionManager Instance;

    public bool useUnclampedExplosion;
    public Transform explosionParent;
    public ExplosionPiece[] explosionPieces;
    [Space]
    public bool useNewTransformations;
    public Vector3 positionOffset;
    public Vector3 eulerRotation;
    public Vector3 localScale = Vector3.one;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (useNewTransformations)
        {
            explosionParent.position = positionOffset;
            explosionParent.localEulerAngles = eulerRotation;
            explosionParent.localScale = localScale;
        }

        AutoChangedExplosionValue(0.0f, 0.3f);
    }

    public void AutoChangedExplosionValue(float startValue, float endValue)
    {
        StartCoroutine(ExplosionPieceCoroutine(startValue, endValue));
    }

    public float explosionValue = 0.0f;
    public IEnumerator ExplosionPieceCoroutine(float startValue, float endValue)
    {
        explosionValue = startValue;
        float offset = startValue < endValue ? 0.01f : -0.01f;
        while (explosionValue.ToString("F2") != endValue.ToString("F2"))
        {
            foreach (ExplosionPiece explosionPiece in explosionPieces)
            {
                if (useUnclampedExplosion)
                    explosionPiece.Piece.localPosition = Vector3.LerpUnclamped(explosionPiece.StartPoint, explosionPiece.EndPoint, explosionValue);
                else
                    explosionPiece.Piece.localPosition = Vector3.Lerp(explosionPiece.StartPoint, explosionPiece.EndPoint, explosionValue);
            }
            explosionValue += offset;
            yield return new WaitForSeconds(0.01f);
        }
    }
}