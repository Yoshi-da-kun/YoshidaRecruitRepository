
using UnityEngine;

/// --------------------------------------------------
/// #CircleEnemy.cs
/// 作成者:吉田雄伍
/// 
/// CircleEnemyの挙動を行うスクリプト
/// --------------------------------------------------

public class CircleEnemy : TargetManager
{
    // CircleEnemyのTransform
    private Transform _thisTransform;

    [SerializeField, Label("CircleEnemyのパラメータ")]
    private CircleEnemyParameter _circleEnemyParameter;

    [SerializeField, Label("音を流すスクリプト")]
    private SoundController _soundController;

    // CircleEnemyの正面方向のベクトル
    private Vector3Int AIRCRAFT_FRONT_DIRECTION { get { return new Vector3Int(0, 0, 1); } }

    [SerializeField, Header("CircleEnemyの旋回の中心位置")]
    private Transform _turningCenterTransform;

    [SerializeField, Header("CircleEnemyの初期位置を半周した状態にするか")]
    private bool _startToHalfCirclePosition;


    void Start()
    {
        // CircleEnemyのTransformを取得
        _thisTransform = this.GetComponent<Transform>();

        // CircleEnemyを初期位置に移動する
        CiecleEnemyInitialPosition();
    }


    private void FixedUpdate()
    {
        // CircleEnemyの円状に移動する
        CiecleEnemyMoving();

        // 体力がなくなったとき、キャラクターを消す
        if (_isDead)
        {
            // エフェクトを出す
            Instantiate(_circleEnemyParameter._breakedEffectPrefab, this.transform.position, Quaternion.identity);

            // 音を出す
            _soundController.PlaySeSound(_circleEnemyParameter._breakedSound);

            // オブジェクトを無効化する
            this.gameObject.SetActive(false);
        }
    }

    
    /// <summary>
    /// CircleEnemyの円周状に移動する
    /// </summary>
    private void CiecleEnemyMoving()
    {
        // 旋回を行う
        _thisTransform.rotation *= Quaternion.Euler(0, _circleEnemyParameter._turningSpeed, 0);

        // CircleEnemyの正面方向に前進する
        _thisTransform.position += _thisTransform.rotation * AIRCRAFT_FRONT_DIRECTION * _circleEnemyParameter._movingSpeed;
    }


    /// <summary>
    /// 円状に移動するCircleEnemyを初期位置に移動する
    /// </summary>
    private void CiecleEnemyInitialPosition()
    {
        // 半周にかかる角度
        int halfRoundDegrees = 180;

        // 半周の移動にかかる旋回回数
        float halfRoundMoveCount = halfRoundDegrees / Mathf.Abs(_circleEnemyParameter._turningSpeed);

        // 半周の長さ(円周の2分の1)
        float halfCircumference = halfRoundMoveCount * _circleEnemyParameter._movingSpeed;

        // 半径を求める
        float radius = halfCircumference / Mathf.PI;

        // 旋回方向によって移動する方向を変える
        if (_circleEnemyParameter._turningSpeed > 0)
        {
            radius = -radius;
        }

        // 初期位置がそのままなとき
        if (_startToHalfCirclePosition)
        {
            // CircleEnemyの旋回の中心位置から旋回の半径分離した位置に移動する
            _thisTransform.position = new Vector3(_turningCenterTransform.position.x + radius, _thisTransform.position.y, _thisTransform.position.z);
        }
        // 初期位置が半周した状態のとき
        else
        {
            // CircleEnemyの旋回の中心位置から旋回の半径分離した位置に移動する
            _thisTransform.position = new Vector3(_turningCenterTransform.position.x - radius, _thisTransform.position.y, _thisTransform.position.z);

            // 半周の角度分回転する
            _thisTransform.rotation *= Quaternion.Euler(0, halfRoundDegrees, 0);
        }
        
    }
}
