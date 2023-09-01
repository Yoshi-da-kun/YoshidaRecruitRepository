
using System.Collections.Generic;
using UnityEngine;

/// --------------------------------------------------
/// #WarpSkill.cs
/// 作成者:吉田雄伍
/// 
/// ワープのスキルを制御するスクリプト
/// --------------------------------------------------

public class WarpSkill : MonoBehaviour
{
    [SerializeField]
    private PlayerParameter _playerParameter = default;

    // ワープスキル中か(設置されたか)を示すフラグ
    public bool _inWarpSkill { get; private set; }

    // ワープ中かを示すフラグ
    public bool _isWarping { get; private set; }

    // ワープスキル設置後の移動座標や姿勢などを保存するリスト
    private List<Vector3> _cordinatesDuringWarpSkill = new List<Vector3>();
    private List<Quaternion> _attributesDuringWarpSkill = new List<Quaternion>();

    // ワープで元の位置に戻っていくときの、移動座標姿勢リストの１要素あたりの経過秒数
    private float _warpElapsedTimePerIndex = default;

    // ワープスキルに関する計測時間
    private float _elapsedTimeDuringWarpSkill = default;


    /// <summary>
    /// ワープスキル使用中のときの処理
    /// </summary>
    public void WarpSkillProcessing(Vector3 _aircraftPosition, Quaternion _aircraftRotation)
    {
        // ワープ中の機体の座標と姿勢を記録する
        _cordinatesDuringWarpSkill.Add(_aircraftPosition);
        _attributesDuringWarpSkill.Add(_aircraftRotation);

        // ワープスキル設置後からの経過時間を加算
        _elapsedTimeDuringWarpSkill += Time.fixedDeltaTime;

        // 経過時間が発動時間を超えたときの処理
        if (_elapsedTimeDuringWarpSkill >= _playerParameter._warpSkillActivateTime)
        {
            // ワープ中のフラグをセットし、計測時間を初期化
            _isWarping = true;
            _elapsedTimeDuringWarpSkill = 0;

            // 移動座標姿勢リストの１要素あたりの経過秒数を求める
            _warpElapsedTimePerIndex = _playerParameter._warpingTime / _cordinatesDuringWarpSkill.Count;
        }
    }


    /// <summary>
    /// ワープ中の処理(ワープスキル設置位置に戻る処理)
    /// </summary>
    public void WarpingProcessing(Transform _aircraftTransform)
    {
        // ワープしている時間を計測する
        _elapsedTimeDuringWarpSkill += Time.fixedDeltaTime;

        // 現在の機体の座標と姿勢がある要素番号を求める
        int currentListIndex = _cordinatesDuringWarpSkill.Count - 1 - (int)(_elapsedTimeDuringWarpSkill / _warpElapsedTimePerIndex);

        // 要素番号が番外にならないようにする
        if (currentListIndex < 0)
        {
            currentListIndex = 0;
        }

        // ワープ中現在の機体の位置と姿勢にする
        _aircraftTransform.position = _cordinatesDuringWarpSkill[currentListIndex];
        _aircraftTransform.rotation = _attributesDuringWarpSkill[currentListIndex];

        // 計測時間がワープにかかる時間を超えたらワープを終了する
        if (_elapsedTimeDuringWarpSkill >= _playerParameter._warpingTime)
        {
            // ワープに関するフラグを初期化
            _isWarping = false;
            _inWarpSkill = false;

            // ワープに関するリストを初期化
            _cordinatesDuringWarpSkill.Clear();
            _attributesDuringWarpSkill.Clear();
        }
    }


    /// <summary>
    /// ワープスキルの発動をする
    /// </summary>
    public void WarpSkillStart()
    {
        // ワープスキルが設置されていないならスキル発動
        if (!_inWarpSkill)
        {
            Debug.Log("スキル発動！");

            // スキル設置フラグをセット
            _inWarpSkill = true;
        }
    }
}
