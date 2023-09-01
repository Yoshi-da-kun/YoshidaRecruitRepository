
using UnityEngine;

/// --------------------------------------------------
/// #OriginalSphereCollider.cs
/// 作成者:吉田雄伍
/// 
/// 球(Sphere)のコライダーのデータと処理をもつスクリプト
/// --------------------------------------------------

public class OriginalSphereCollider : OriginalCollider
{
    // このコライダーのTransform 
    public Transform _thisTransform { get; private set; } = default;

    [field: SerializeField, Header("当たり判定の大きさ")]
    public float _colliderRadius { get; private set; }


    private void Awake()
    {
        // 自身のコライダーの形状を格納する
        _colliderShape = _typeOfColliderShape.Sphere;

        _thisTransform = this.transform;
    }
}
