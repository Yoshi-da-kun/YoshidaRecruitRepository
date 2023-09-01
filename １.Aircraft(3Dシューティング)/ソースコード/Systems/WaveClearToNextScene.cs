
using UnityEngine;

/// --------------------------------------------------
/// #WaveClearToNextScene.cs
/// 作成者:吉田雄伍
/// 
/// ウェーブのクリア時のScene移動処理を行うスクリプト
/// --------------------------------------------------

public class WaveClearToNextScene : MonoBehaviour
{
    [SerializeField, Label("FadeChangeSceneのスクリプト")]
    private FadeChangeScene _fadeChangeScene;

    [SerializeField, Label("フェードインを開始するまでにかける時間")]
    private float _startFadeInTime = 1;

    // フェードイン開始するまでの時間計測用の変数
    private float _startFadeInElapsedTime = default;

    // フェードインを開始したかを示すフラグ
    private bool _fadeInStarted = default;


    private void Update()
    {
        // フェードインを開始していたら処理しない
        if (_fadeInStarted)
        {
            return;
        }

        // 全ての敵を破壊しているとき、Sceneを移動を開始する
        if (WaveState._isAllTargetBreaked)
        {
            // 経過時間の計測を行う
            _startFadeInElapsedTime += Time.deltaTime;

            // フェードインの開始時間を超えたとき
            if (_startFadeInElapsedTime >= _startFadeInTime)
            {
                // フェードインを開始してSceneを移動する
                _fadeChangeScene.FadeInStart();

                // フェードイン開始フラグをセット
                _fadeInStarted = true;
            }
        }
    }
}
