
using System.Collections.Generic;
using UnityEngine;

/// --------------------------------------------------
/// #PuyoPresentation.cs
/// 作成者:吉田雄伍
/// 
/// ぷよの演出を行うスクリプトです
/// --------------------------------------------------

public class PuyoPresentation : MonoBehaviour
{
    [SerializeField]
    private SummarizeScriptableObjects _summarizeScriptableObjects;

    // 各パラメータをまとめたスクリプト
    private PresentationParameter _presentationParameter;
    private FieldParameter _fieldParameter;


    // 設置演出を行うぷよ
    private List<Transform> _installPuyosTransforms = new List<Transform>();

    // ぷよ設置の演出時間を計測する
    private List<float> _installPresentationElapsedTimes = new List<float>();

    // ぷよ設置の演出中のフラグ
    public bool _duringInstallPresentation { get; private set; } = default;

    // ぷよが設置演出中の平べったい状態かを示すフラグ
    private bool _isPuyoFlatScale = default;


    // 消える演出を行うぷよ
    private List<SpriteRenderer> _eracePuyosRenderer = new List<SpriteRenderer>();

    // ぷよが消える演出中のフラグ
    public bool _duringEracePresentation { get; private set; } = default;

    // ぷよが表示されているかを示すフラグ
    private bool _isPuyoDisplaying = default;

    // ぷよが消える演出時間を計測する
    private float _eracePresentationElapsedTime = default;


    private void Start()
    {
        // 各パラメータを取得
        _presentationParameter = _summarizeScriptableObjects._prensentationParameter;
        _fieldParameter = _summarizeScriptableObjects._fieldParameter;
    }


    private void FixedUpdate()
    {
        // ぷよを設置したときの演出を行う
        PuyoInstallPresentationProcess();

        // ぷよが消える時の演出を行う
        PuyoEracePresentationProcess();
    }


    /// <summary>
    /// ぷよを設置したときの演出
    /// </summary>
    private void PuyoInstallPresentationProcess()
    {
        // ぷよを設置する演出中じゃなければ処理しない
        if (!_duringInstallPresentation)
        {
            return;
        }

        // ぷよを設置したときの演出を終了するかを判定する
        for (int i = 0; i < _installPuyosTransforms.Count; i++)
        {
            // ぷよを設置したときの演出時間を計測する
            _installPresentationElapsedTimes[i] += Time.fixedDeltaTime;

            // ぷよを設置したときの演出時間が終了時間に達したら終了する
            if (_installPresentationElapsedTimes[i] >= _presentationParameter._installPresentationTime)
            {
                // ぷよの大きさを元に戻す
                _installPuyosTransforms[i].localScale = _fieldParameter._scaleOfOneMass;

                // 削除予定のTransformを末尾の要素と交換する
                Transform fromEndPointerTransform = _installPuyosTransforms[_installPuyosTransforms.Count - 1];
                _installPuyosTransforms[_installPuyosTransforms.Count - 1] = _installPuyosTransforms[i];
                _installPuyosTransforms[i] = fromEndPointerTransform;

                // 削除予定の計測時間を末尾の要素と交換する
                float fromEndPointerTime = _installPresentationElapsedTimes[_installPuyosTransforms.Count - 1];
                _installPresentationElapsedTimes[_installPuyosTransforms.Count - 1] = _installPresentationElapsedTimes[i];
                _installPresentationElapsedTimes[i] = fromEndPointerTime;

                // 末尾の要素を削除する
                _installPuyosTransforms.RemoveAt(_installPuyosTransforms.Count - 1);
                _installPresentationElapsedTimes.RemoveAt(_installPuyosTransforms.Count - 1);

                // 検索する要素のポインタを一つ前に戻す(もう一度同じ要素番号を検索するため)
                i--;

                // 設置演出を行っているぷよがひとつもなければ演出処理を終了する
                if (_installPuyosTransforms.Count == 0)
                {
                    _duringInstallPresentation = false;

                    return;
                }

                continue;
            }
        }

        ///-- 設置時の演出を行う --///

        // ぷよが平べったい状態のとき
        if (_isPuyoFlatScale)
        {
            // ぷよを縦に長い大きさに変える
            for (int i = 0; i < _installPuyosTransforms.Count; i++)
            {
                _installPuyosTransforms[i].localScale = _fieldParameter._scaleOfOneMass;
            }
        }
        // ぷよが縦に長い状態のとき
        else
        {
            // ぷよを平べったい大きさに変える
            for (int i = 0; i < _installPuyosTransforms.Count; i++)
            {
                _installPuyosTransforms[i].localScale = _fieldParameter._scaleOfOneMass;
            }
        }
    }


    /// <summary>
    /// ぷよが消える時の演出
    /// </summary>
    private void PuyoEracePresentationProcess()
    {
        // ぷよが消える演出中じゃなければ処理しない
        if (!_duringEracePresentation)
        {
            return;
        }
        
        // ぷよが表示されているならぷよの表示をやめる
        if (_isPuyoDisplaying)
        {
            // ぷよの表示を消す処理
            for (int i = 0; i < _eracePuyosRenderer.Count; i++)
            {
                _eracePuyosRenderer[i].enabled = false;
            }

            // ぷよの表示フラグを非表示の状態にする
            _isPuyoDisplaying = false;
        }
        // ぷよが表示されていないならぷよを表示する
        else
        {
            // ぷよを表示する処理
            for (int i = 0; i < _eracePuyosRenderer.Count; i++)
            {
                _eracePuyosRenderer[i].enabled = true;
            }
            
            // ぷよの表示フラグをセットする
            _isPuyoDisplaying = true;
        }

        // ぷよが消えるときの演出時間を計測する
        _eracePresentationElapsedTime += Time.fixedDeltaTime;

        // ぷよが消えるときの演出時間が終了時間に達したら終了する
        if (_eracePresentationElapsedTime >= _presentationParameter._eracePresentationTime)
        {
            // ぷよが消えるときの演出を終了する
            _duringEracePresentation = false;

            // ぷよを表示状態にする
            for (int i = 0; i < _eracePuyosRenderer.Count; i++)
            {
                _eracePuyosRenderer[i].enabled = true;
            }

            // 演出の計測時間を初期化
            _eracePresentationElapsedTime = 0;

            // ぷよのSpriteRendererの配列を初期化する
            _eracePuyosRenderer = new List<SpriteRenderer>();
        }
    }


    /// <summary>
    /// ぷよが設置したときの演出
    /// </summary>
    public void PuyoInstallPresentationStart(List<Transform> installPuyos)
    {
        // 設置したぷよのTransformと演出計測時間を格納する
        for (int i = 0; i < installPuyos.Count; i++)
        {
            _installPuyosTransforms.Add(installPuyos[i]);
            _installPresentationElapsedTimes.Add(0);
        }
        
        _duringInstallPresentation = true;
    }


    /// <summary>
    /// ぷよが設置したときの演出
    /// </summary>
    public void PuyoEracePresentationStart(List<Transform> eracePuyos)
    {
        // ぷよのSpriteRendererを取得し、格納する
        for (int i = 0; i < eracePuyos.Count; i++)
        {
            _eracePuyosRenderer.Add(eracePuyos[i].gameObject.GetComponent<SpriteRenderer>());
        }
        
        // ぷよが消えるときの演出を開始する
        _duringEracePresentation = true;

        // ぷよ表示フラグをセットする
        _isPuyoDisplaying = true;
    }
}