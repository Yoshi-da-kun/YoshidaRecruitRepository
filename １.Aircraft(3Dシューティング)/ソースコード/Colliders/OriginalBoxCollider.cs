
using UnityEngine;

/// --------------------------------------------------
/// #OriginalBoxCollider.cs
/// 作成者:吉田雄伍
/// 
/// 直方体(Box)のコライダーのデータと処理をもつスクリプト
/// --------------------------------------------------

public class OriginalBoxCollider : OriginalCollider
{
    // このコライダーのTransform 
    public Transform _thisTransform { get; private set; } = default;

    [field: SerializeField, Header("当たり判定の大きさ")]
    public Vector3 _colliderSize { get; private set; }


    private void Awake()
    {
        // 自身のコライダーの形状を格納する
        _colliderShape = _typeOfColliderShape.Box;

        _thisTransform = this.transform;
    }
}
