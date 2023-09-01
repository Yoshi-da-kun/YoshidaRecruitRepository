
using UnityEngine;


/// --------------------------------------------------
/// #PuyoTwoScripts.cs
/// 作成者:吉田雄伍
/// 
/// ぷよぷよ通ルール用のスクリプトです
/// --------------------------------------------------

public class PuyoPuyoTwoRule : MonoBehaviour
{
    // ゲーム内の進行を管理するフラグをまとめたスクリプトです
    private PlayerFlags _playerFlags;


    private void Start()
    {
        // ゲーム内の進行を管理するフラグをまとめたスクリプトを取得
        _playerFlags = this.GetComponent<PlayerFlags>();
        
    }


    /// <summary>
    /// 全てのぷよが消えた時次のぷよを抽選するメソッド
    /// </summary>
    private void Update()
    {
        // 全てのぷよが消去された時
        if (_playerFlags._isAllPuyoEraced)
        {
            // 次のぷよの抽選を開始する
            _playerFlags._isNextPuyoUpdate = true;

            // 全てのぷよが消えたときのフラグをもとに戻す
            _playerFlags._isAllPuyoEraced = false;
        }
    }
}
