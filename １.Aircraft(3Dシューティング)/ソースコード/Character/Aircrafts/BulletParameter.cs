
using UnityEngine;

/// --------------------------------------------------
/// #AircraftBullet.cs
/// 作成者:吉田雄伍
/// 
/// 弾のパラメータをまとめたスクリプト
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/BulletParameter" ,fileName = "NewBulletParameter")]
public class BulletParameter : ScriptableObject
{
    [field: SerializeField, Label("弾のPrefab")]
    public GameObject _bulletPrefab { get; private set; }

    [field: SerializeField, Label("弾の速度")]
    public float _bulletSpeed { get; private set; }

    [field: SerializeField, Label("弾の攻撃力")]
    public int _bulletPower { get; private set; }

    [field: SerializeField, Label("弾が発射されてから消えるまでの時間")]
    public int _bulletBreakTime { get; private set; }

    [field: SerializeField, Label("弾が当たった時のエフェクト")]
    public GameObject _hitEffect { get; private set; }

    [field: SerializeField, Label("弾が当たった時のSE")]
    public AudioClip _hitSound { get; private set; }

    [field: SerializeField, Label("弾を発射した時のSE")]
    public AudioClip _shotSound { get; private set; }
}
