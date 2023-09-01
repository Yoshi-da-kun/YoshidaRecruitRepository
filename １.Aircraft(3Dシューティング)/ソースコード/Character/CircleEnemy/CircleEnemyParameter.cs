
using UnityEngine;

/// --------------------------------------------------
/// #CircleEnemyParameter.cs
/// 作成者:吉田雄伍
/// 
/// CircleEnemyのパラメータをまとめたスクリプト
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/CircleEnemyParameter", fileName = "NewCircleEnemyParameter")]
public class CircleEnemyParameter : CharacterBaseParameter
{
    [field: Header("移動に関する変数")]

    [field: SerializeField, Label("移動速度"), Range(0.01f,30)]
    public float _movingSpeed { get; private set; } = 0.5f;

    [field: SerializeField, Label("旋回速度と方向"), Range(-10, 10)]
    public float _turningSpeed { get; private set; } = 0.2f;

    [field: SerializeField, Label("破壊されたときのエフェクトのPrefab")]
    public GameObject _breakedEffectPrefab { get; private set; }

    [field: SerializeField, Label("破壊された時のSE")]
    public AudioClip _breakedSound { get; private set; }


    private void OnEnable()
    {
        // CircleEnemyが旋回可能な速度になっているかをチェックする
        if (_turningSpeed == 0)
        {
            Debug.LogError("CircleEnemyが旋回を行えません！");
        }
    }
}
