
using UnityEngine;

/// --------------------------------------------------
/// #FieldParameter.cs
/// 作成者:吉田雄伍
/// 
/// フィールドの大きさ等、フィールドやぷよの情報。ぷよの状態をまとめたスクリプトです
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/PresentationParamete", fileName = "NewPresentationParameter")]
public class PresentationParameter : ScriptableObject
{
    [field: SerializeField, Label("ぷよの消える演出を行う時間")]
    public float _eracePresentationTime { get; private set; } = 1;

    [field: SerializeField, Label("ぷよを設置する演出を行う時間")]
    public float _installPresentationTime { get; private set; } = 1;

}
