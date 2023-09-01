using UnityEngine;

/// --------------------------------------------------
/// #InGameSounds.cs
/// 作成者:吉田雄伍
/// 
/// ゲーム中の音をまとめるスクリプト
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Others/InGameSounds", fileName = "NewInGameSouds")]
public class InGameSounds : ScriptableObject
{
    [field: SerializeField, Label("ぷよが消えたときの音")]
    public AudioClip _puyoEraceSE { get; private set; }

    [field: SerializeField, Label("ぷよが設置した時の音")]
    public AudioClip _puyoInstallSE { get; private set; }

    [field: SerializeField, Label("ぷよを回転させた時の音")]
    public AudioClip _puyoRotationSE { get; private set; }
}
