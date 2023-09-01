
using UnityEngine;

/// --------------------------------------------------
/// #WaveStateManagement.cs
/// 作成者:吉田雄伍
/// 
/// ウェーブの状態をSceneの状態合わせて管理するためのスクリプト
/// --------------------------------------------------

public class WaveStateManagement : MonoBehaviour
{
    private void Start()
    {
        // 新たにウェーブを開始するときの処理
        WaveState.TaragetCountUpdate();
    }
}
