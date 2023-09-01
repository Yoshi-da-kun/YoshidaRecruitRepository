
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// --------------------------------------------------
/// #FadeChangeScene.cs
/// 作成者:吉田雄伍
/// 
/// シーンチェンジするときのフェードイン、アウトとシーン移動を行うスクリプト
/// --------------------------------------------------

public class FadeChangeScene : MonoBehaviour
{
    [SerializeField, Label("フェードイン、アウト用の画像")]
    private Image _fadePanel;

    [SerializeField, Label("フェードイン、アウトするときの速度"), Range(0.001f, 1)]
    private float _fadeSpeed = 0.001f;

    // 現在のアルファ値
    private float _currentAlpha = default;

    // フェードイン中のフラグ
    private bool _isFadeIn = default;

    [SerializeField, Label("Scene開始時にフェードアウトするか")]
    private bool _isFadeOut;

    [SerializeField, Label("移動するシーンの名前")]
    private string _changeSceneName;


    private void Start()
    {
        // フェードアウトするなら
        if (_isFadeOut)
        {
            // アルファ値の最大値
            int maxAlphaValue = 1;

            // フェード用の画像の透明度を最大にする
            _fadePanel.color = new Color(0, 0, 0, maxAlphaValue);
        }

        // 現在のアルファ値を格納
        _currentAlpha = _fadePanel.color.a;
    }


    private void Update()
    {
        // フェードアウト中か
        if (_isFadeOut)
        {
            // 現在のアルファ値から透明になるように減算する
            _currentAlpha -= _fadeSpeed;
        
            // フェードアウト用の画像のアルファ値を更新
            _fadePanel.color = new Color(0, 0, 0, _currentAlpha);

            // フェードアウトが完了したとき
            if (_currentAlpha <= 0)
            {
                // フェードアウトを終了する
                _isFadeOut = false;
            }
        
            return;
        }
        
        // フェードイン中か
        if (_isFadeIn)
        {
            // 現在のアルファ値から不透明になるように加算する
            _currentAlpha += _fadeSpeed;
            
            // フェードイン用の画像のアルファ値を更新
            _fadePanel.color = new Color(0, 0, 0, _currentAlpha);
            
            // アルファ値の最大値
            int maxAlphaValue = 1;

            // フェードインが完了したとき
            if (_currentAlpha >= maxAlphaValue)
            {
                // Sceneを移動する
                SceneManager.LoadScene(_changeSceneName);
            }
        }
    }


    /// <summary>
    /// フェードアウトを開始する
    /// </summary>
    public void FadeOutStart()
    {
        _isFadeOut = true;
    }


    /// <summary>
    /// フェードインを開始する
    /// </summary>
    public void FadeInStart()
    {
        _isFadeIn = true;
    }
}
