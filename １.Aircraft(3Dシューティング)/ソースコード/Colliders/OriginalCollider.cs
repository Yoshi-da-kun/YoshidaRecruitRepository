
using UnityEngine;
using ColliderDataCollection;

/// --------------------------------------------------
/// #OriginalCollider.cs
/// 作成者:吉田雄伍
/// 
/// 自作のコライダーのデータと処理をもつスクリプト
/// --------------------------------------------------

public class OriginalCollider : MonoBehaviour
{
    // コライダーの形状を示すenum
    public enum _typeOfColliderShape
    {
        Sphere,
        Box
    }

    // このコライダーの形状
    public _typeOfColliderShape _colliderShape { get; protected set; } = default;


    [SerializeField, Header("弾の当たり判定(チェックでON)")]
    private bool _isBulletCollision = false;

    [SerializeField, Header("直接的な衝突判定(チェックでON)")]
    private bool _isPhysicsCollision = false;

    // 各判定ごとのListに格納されている要素番号
    private int _bulletColliderStoredIndex = default;
    private int _physicsColliderStoredIndex = default;


    /// <summary>
    /// オブジェクト有効化時に、コライダー管理スクリプトに自身を格納する
    /// </summary>
    private void OnEnable()
    {
        // 弾の当たり判定がONのとき
        if (_isBulletCollision)
        {
            _bulletColliderStoredIndex = ColliderDatas.AddBulletColliders(this);
        }
        // 直接衝突判定がONのとき
        if (_isPhysicsCollision)
        {
            _physicsColliderStoredIndex = ColliderDatas.AddPhysicsColliders(this);
        }
    }


    /// <summary>
    /// オブジェクト無効化時に、コライダー管理スクリプトから自身を削除する
    /// </summary>
    private void OnDisable()
    {
        // 弾の当たり判定がONのとき
        if (_isBulletCollision)
        {
            ColliderDatas.RemoveBulletColliders(this, _bulletColliderStoredIndex);
        }
        // 物理衝突判定がONのとき
        if (_isPhysicsCollision)
        {
            ColliderDatas.RemovePhysicsColliders(this, _physicsColliderStoredIndex);
        }
    }
}
