
using UnityEngine;

/// --------------------------------------------------
/// #SummarizeScriptableObjects.cs
/// 作成者:吉田雄伍
/// 
/// 各パラメータのScriptableObjectをまとめたスクリプトです
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Others/SummarizeScriptableObjects", fileName = "NewSummarizeScriptableObjects")]
public class SummarizeScriptableObjects : ScriptableObject
{
    [field: SerializeField, Header("フィールドのパラメータ")]
    public FieldParameter _fieldParameter { get; private set; }

    [field: SerializeField, Header("プレイヤーのパラメータ")]
    public PlayerParameter _playerParameter { get; private set; }

    [field: SerializeField, Header("演出のパラメータ")]
    public PresentationParameter _prensentationParameter { get; private set; }

    [field: SerializeField, Header("サウンドをまとめたスクリプト")]
    public InGameSounds _inGameSounds { get; private set; }
}