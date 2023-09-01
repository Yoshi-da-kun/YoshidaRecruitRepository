
using UnityEngine;
using ControllerInput;

/// --------------------------------------------------
/// #TitleScript.cs
/// 作成者:吉田雄伍
/// 
/// タイトルでの処理を行うスクリプト
/// --------------------------------------------------

public class TitleScript : MonoBehaviour
{
    [SerializeField, Label("FadeChangeSceneのスクリプト")]
    private FadeChangeScene _fadeChangeScene;


    private void Update()
    {
        // 次のSceneにいくための入力をしたとき
        if (PlayerInput.TitlePressInput())
        {
            // フェードインを開始してSceneを移動する
            _fadeChangeScene.FadeInStart();
        }

        // ゲーム終了の入力をしたとき
        if (PlayerInput.EscapeInput())
        {
            // アプリケーションを終了する
            Application.Quit();
        }
    }
}
