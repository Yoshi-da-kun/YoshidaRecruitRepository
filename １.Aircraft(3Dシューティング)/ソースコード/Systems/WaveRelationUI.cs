
using UnityEngine;
using UnityEngine.UI;

/// --------------------------------------------------
/// #WaveRelationUI.cs
/// 作成者:吉田雄伍
/// 
/// ウェーブ進行に関連するUIの処理をするスクリプト
/// --------------------------------------------------

public class WaveRelationUI : MonoBehaviour
{
    [SerializeField, Label("目標(mission)を表示するテキスト")]
    private Text _missionText;

    [SerializeField, Label("敵の数を表示するテキスト")]
    private Text _targetCountText;

    [SerializeField, Header("敵の数を表示するテキストの数字の前と後ろにつける文字(上:前　下:後ろ)")]
    private string _startOfTargetCountText;
    [SerializeField]
    private string _endOfTargetCountText;

    [Header("")]
    [SerializeField, Label("ウェーブの終了時に表示するテキスト")]
    private Text _waveFinishText;

    // 現在の残りターゲットの数
    private int _currentRemainingTargetCount;


    private void Start()
    {
        // ウェーブ終了のテキストを非表示にする
        _waveFinishText.enabled = false;

        // 目標のテキストを表示する
        _missionText.enabled = true;
    }


    private void Update()
    {
        // 残りターゲットの数が変化した時の処理
        if (_currentRemainingTargetCount != WaveState._remainingTargets)
        {
            // 現在の残りターゲットの数を更新する
            _currentRemainingTargetCount = WaveState._remainingTargets;

            // 残りターゲットの変化時に行うテキストの変更処理
            RemainingTargetTextUpdate();

            // 残りターゲット数が０になったとき
            if (_currentRemainingTargetCount == 0)
            {
                // ウェーブ終了のテキストを表示する
                _waveFinishText.enabled = true;

                // 目標のテキストを非表示にする
                _missionText.enabled = false;
            }
        }
    }


    /// <summary>
    /// 残りターゲットの変化時に行うテキストの変更処理
    /// </summary>
    private void RemainingTargetTextUpdate()
    {
        // 残り敵数のテキストがない場合以下の処理を行わない
        if (!_targetCountText)
        {
            return;
        }

        // 残り敵数のテキストの数を更新する
        _targetCountText.text = _startOfTargetCountText + _currentRemainingTargetCount + _endOfTargetCountText;
    }
}
