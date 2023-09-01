
using UnityEngine;

/// --------------------------------------------------
/// #PlayerParameter.cs
/// 作成者:吉田雄伍
/// 
/// プレイヤーのパラメータをまとめるスクリプト
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/PlayerParameter", fileName = "NewPlayerParameter")]
public class PlayerParameter : AircraftParameter
{
    [field: SerializeField, Label("ワープスキルの設置後から、スキル発動するまでの時間")]
    public float _warpSkillActivateTime { get; private set; } = 3;

    [field: SerializeField, Label("ワープにかかる時間")]
    public float _warpingTime { get; private set; } = 1.3f;

    [field: SerializeField, Label("壊れた時のエフェクトのPrefab")]
    public GameObject _breakedEffectPrefab { get; private set; }
}
