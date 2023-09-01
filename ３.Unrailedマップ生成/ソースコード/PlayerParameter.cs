
using UnityEngine;

/// --------------------------------------------------------
/// #PlayerParameter.cs
/// 
/// プレイヤーに関するパラメータをまとめたスクリプト
/// --------------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/PlayerParameter", fileName = "NewPlayerParameter")]
public class PlayerParameter : ScriptableObject
{
    [field: SerializeField, Label("プレイヤーの移動速度"), Range(0.01f, 10)]
    public float _moveSpeed { get; private set; } = 1;

    [field: SerializeField, Label("プレイヤーの攻撃力(１を想定)"), Range(1, 10)]
    public int _attackVolume { get; private set; } = 1;


    [field: SerializeField, Label("ものを破壊する間隔"), Range(0.01f, 10)]
    public float _breakIntervalTime { get; private set; } = 1;

    [field: SerializeField, Label("ものを破壊できる距離"), Range(0.01f, 10)]
    public float _breakingDistance { get; private set; } = 1;

}
