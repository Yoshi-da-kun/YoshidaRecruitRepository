
using System.Collections;
using UnityEngine;

/// --------------------------------------------------
/// #InGameFlags.cs
/// 作成者:吉田雄伍
/// 
/// フィールドのオブジェクトにアタッチしてください
/// ゲーム中のスクリプト間で共有するフラグをまとめたスクリプトです
/// --------------------------------------------------

public class PlayerFlags : MonoBehaviour
{
    // ネクストぷよの更新(抽選)を開始するためのフラグ
    [HideInInspector]
    public bool _isNextPuyoUpdate = default;

    // ぷよ消去がされたかを示すフラグ(ぷよが消去される度にtrueになる)
    [HideInInspector]
    public bool _isPuyoEraced = default;

    // ぷよ設置後、すべてのぷよ消去が終わったかを示すフラグ
    [HideInInspector]
    public bool _isAllPuyoEraced = default;
}
