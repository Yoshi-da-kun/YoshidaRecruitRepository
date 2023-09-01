
using UnityEngine;

/// --------------------------------------------------
/// #CharacterBaseParameter.cs
/// 作成者:吉田雄伍
/// 
/// 各キャラクターのパラメータをまとめるスクリプト
/// キャラクターとは、HPをもつもの
/// --------------------------------------------------

public class CharacterBaseParameter : ScriptableObject
{
    [field: SerializeField, Label("キャラクターの最大HP")]
    public int _maxHp { get; private set; }
}