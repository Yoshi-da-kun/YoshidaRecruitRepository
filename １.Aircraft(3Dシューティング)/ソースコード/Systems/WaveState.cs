
using System.Collections.Generic;
using UnityEngine;

/// --------------------------------------------------
/// #WaveState.cs
/// 作成者:吉田雄伍
/// 
/// ウェーブの状態をフラグ等で保持するスクリプト
/// --------------------------------------------------

public static class WaveState
{
    // ターゲット(敵)オブジェクトを格納しているリスト
    private static List<GameObject> _targetObjects  = new List<GameObject>();

    // 残りのターゲット(敵)の数
    public static int _remainingTargets { get; private set; } = default;

    // ターゲットすべてが壊されているか
    public static bool _isAllTargetBreaked { get; private set; } = default;


    /// <summary>
    /// ターゲットの数を更新する処理
    /// </summary>
    public static void TaragetCountUpdate()
    {
        // 残りのターゲットの数を初期化
        _remainingTargets = 0;

        // ターゲットオブジェクトの数繰り返す
        for (int i = 0; i < _targetObjects.Count; i++)
        {
            // ターゲットオブジェクトが有効か
            if (_targetObjects[i].activeSelf)
            {
                // 残りのターゲット数を加算して次の要素へ
                _remainingTargets++;
                
                continue;
            }

            // ターゲットオブジェクトが無効化されていたら、リストからも削除する
            _targetObjects.RemoveAt(i);

            // 削除後のポインタの位置を修正する
            i--;
        }

        // ターゲットの数が０のとき
        if (_remainingTargets == 0)
        {
            // 全てのターゲットが破壊されているフラグをセット
            _isAllTargetBreaked = true;
        }
        else
        {
            // ターゲットがまだ残っているフラグをセット
            _isAllTargetBreaked = false;
        }
    }
    

    /// <summary>
    /// ターゲットオブジェクトを新しく追加する
    /// </summary>
    public static void NewTargetAdd(GameObject targetToAdd)
    {
        _targetObjects.Add(targetToAdd);
    }
}
