
using System.Collections.Generic;
using UnityEngine;
using CollisionSystem;

/// --------------------------------------------------
/// #ExplosionBulletProcess.cs
/// 作成者:吉田雄伍
/// 
/// 機体の弾の処理を行うスクリプト
/// --------------------------------------------------

public class ExplosionBulletProcess : MonoBehaviour 
{
    [SerializeField, Label("弾のパラメータ")]
    private BulletParameter _bulletParameter;

    // この弾のコライダーとTransform
    private OriginalCollider _thisCollider = default;
    private Transform _bulletTransform = default;

    // 弾の移動方向
    private Vector3 _bulletMoveDirection = default;

    // 弾が発射されてからの計測時間
    private float _shotedElapsedTime = 0;

    // 音を流すスクリプト
    private SoundController _soundController;

    // 弾が当たったときのエフェクト
    private Transform _hitEffectTransform = default;


    private void Start()
    {
        // コライダーとTransformを取得する
        _thisCollider = this.GetComponent<OriginalCollider>();
        _bulletTransform = this.GetComponent<Transform>();
    }


    /// <summary>
    /// 弾の移動、攻撃処理を行う
    /// </summary>
    private void FixedUpdate()
    {
        // 計測時間の加算を行う
        _shotedElapsedTime += Time.fixedDeltaTime;

        // 弾の移動を行う
        _bulletTransform.position += _bulletParameter._bulletSpeed * _bulletMoveDirection;

        // 球が衝突したオブジェクトのリストを格納する
        List<OriginalCollider> collidingObjects = CollisionProcessing.BulletCollision(_thisCollider);

        // 弾が当たり判定にあたっていないとき
        if (collidingObjects.Count == 0)
        {
            // 弾が発射されてから一定時間が経過したら、弾を消す
            if (_shotedElapsedTime >= _bulletParameter._bulletBreakTime)
            {
                breakBullet();
            }

            return;
        }

        // 弾が当たったオブジェクトに体力があったとき、体力を減らす処理
        for (int i = 0; i < collidingObjects.Count; i++)
        {
            // 体力が格納されているスクリプトを取得
            CharacterBase collideCharacter = collidingObjects[i].GetComponent<CharacterBase>();

            // 体力をもっていないなら次の要素へ
            if (!collideCharacter)
            {
                continue;
            }
            // 弾が当たったキャラクターにダメージを与える
            collideCharacter.TakesDamage(_bulletParameter._bulletPower);
        }

        // エフェクトを再生する
        _hitEffectTransform.position = _bulletTransform.position;
        _hitEffectTransform.gameObject.SetActive(true);

        // 弾が当たったときの音を再生する
        _soundController.PlaySeSound(_bulletParameter._hitSound);

        // 弾が当たったときに弾を消す
        breakBullet();
    }


    /// <summary>
    /// 弾が壊された(消された)とき
    /// </summary>
    private void breakBullet()
    {
        // 弾が発射されてからの計測時間
        _shotedElapsedTime = 0;

        // 弾を消す
        this.gameObject.SetActive(false);
    }


    /// <summary>
    /// 弾を発射する
    /// </summary>
    public void ExplosionBulletShot(Vector3 shotDirection, SoundController soundController)
    {
        // エフェクトが生成されていないなら
        if (!_hitEffectTransform)
        {
            // 弾が当たったときのエフェクトを生成
            _hitEffectTransform = Instantiate(_bulletParameter._hitEffect, this.transform.position, Quaternion.identity).transform;
        }

        // エフェクトオブジェクトを無効化
        _hitEffectTransform.gameObject.SetActive(false);

        // 弾の移動方向を格納
        _bulletMoveDirection = shotDirection;

        // 効果音を流すスクリプトを取得
        _soundController = soundController;

        // 発射時の音を出す
        _soundController.PlaySeSound(_bulletParameter._shotSound);
    }
}
