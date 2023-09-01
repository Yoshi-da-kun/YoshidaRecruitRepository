
using UnityEngine;

/// --------------------------------------------------
/// #AircraftParameter.cs
/// 作成者:吉田雄伍
/// 
/// 機体のパラメータをまとめたスクリプト
/// --------------------------------------------------

public class AircraftParameter : CharacterBaseParameter
{
    [field: SerializeField, Label("移動速度の初期値"), Range(0.001f, 100)]
    public float _initialMovementSpeed { get; private set; } = 4;

    [field: SerializeField, Label("ブースト時の移動速度"), Range(0.001f, 100)]
    public float _boostMovementSpeed { get; private set; } = 7;

    [field: SerializeField, Label("減速時の移動速度"), Range(0.001f, 100)]
    public float _slowMovementSpeed { get; private set; } = 1;

    [field: SerializeField, Label("加速度"), Range(0.0001f, 20)]
    public float _movementAcceleration { get; private set; } = 1;
    
    [field: SerializeField, Label("減速度"), Range(0.0001f, 20)]
    public float _movementDeceleration { get; private set; } = 1;

    [field: SerializeField, Label("機体のロール速度")]
    public float _rollingSpeed { get; private set; }

    [field: SerializeField, Label("機体の旋回速度")]
    public float _turningSpeed { get; private set; }


    private void OnEnable()
    {
        // 移動速度の設定が想定と合っているかをチェックする
        if (_boostMovementSpeed <= _slowMovementSpeed)
        {
            Debug.LogError("機体のブースト時の速度が、減速時の速度より小さいよ！");
        }

        // 移動速度の設定が想定と合っているかをチェックする
        if (_boostMovementSpeed <= _initialMovementSpeed || _initialMovementSpeed <= _slowMovementSpeed)
        {
            Debug.LogError("機体の初期移動速度が最大値を超えているか、減速時の値より小さいよ！");
        }
    }
}
