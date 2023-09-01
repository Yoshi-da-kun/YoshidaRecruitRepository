
using UnityEngine;

/// --------------------------------------------------
/// #TargetBombParameter.cs
/// 作成者:吉田雄伍
/// 
/// 爆弾型の的のパラメータをまとめるスクリプト
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/TargetBombParameter", fileName = "NewTargetBombParameter")]
public class TargetBombParameter : CharacterBaseParameter
{
    [field: SerializeField, Label("壊れた時のエフェクトのPrefab")]
    public GameObject _breakedEffectPrefab { get; private set; }

    [field: SerializeField, Label("破壊された時のSE")]
    public AudioClip _breakedSound { get; private set; }
}