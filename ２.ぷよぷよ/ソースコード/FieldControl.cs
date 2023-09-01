
using System.Collections.Generic;
using UnityEngine;

/// --------------------------------------------------
/// #FieldControl.cs
/// 作成者:吉田雄伍
/// 
/// フィールドのオブジェクトにアタッチしてください
/// フィールドのデータと設置済みのぷよを管理するスクリプトです
/// --------------------------------------------------

public class FieldControl : MonoBehaviour
{
    [SerializeField]
    private SummarizeScriptableObjects _summarizeScriptableObjects;

    // フィールドのパラメータ
    private FieldParameter _fieldParameter;

    // ゲーム中のサウンドをまとめたすくりぷと
    private InGameSounds _inGameSounds;

    // ゲーム進行を示すフラグをまとめたスクリプト
    private PlayerFlags _playerFlags;

    // ぷよの設置、消去演出を行うスクリプト
    private PuyoPresentation _puyoPresentation;

    // フィールド上のぷよデータ(ぷよの色番号で格納している)
    private sbyte[,] _puyoColorsData = default;
    private Transform[,] _puyoTransforms = default;

    // ぷよが検索済みかを示す配列
    private bool[,] _isSearchedPuyos = default;

    // フィールドのぷよの高さ
    private sbyte[] _fieldHeightData = default;

    // 消すぷよの位置
    private List<int> _eracePuyoColumns = new List<int>(), _eracePuyoRows = new List<int>();
    // 連結しているぷよの位置
    private List<int> _linkingPuyoRows = new List<int>(), _linkingPuyoColumns = new List<int>();

    private bool _isInstallingPresentation = default;
    private bool _isEracingPresentation = default;

    // ぷよを消すとき、各行の消えた数を格納する
    private sbyte[] _eachRowEraceCount = default;

    // 各行と列の座標
    private float[] _eachRowFieldPositions = default;
    private float[] _eachColumnFieldPositions = default;

    // ぷよの効果音を鳴らすためのオーディオソース
    private AudioSource _puyoSeAudioSource;

    [SerializeField, Header("ゲームオーバー時のテキスト")]
    private GameObject _gameOverText;

    #region ぷよの値や、検索に扱う定数

    // フィールドデータに入れる空マスの値
    private const sbyte EMPTY_PUYO_NUMBER = 0;

    // ぷよがつながっているかを検索する方向とその数
    private const sbyte SEARCH_DIRECTION_NUMBER = 4;
    private readonly sbyte[] SEARCH_DESTINATION_ROW = new sbyte[SEARCH_DIRECTION_NUMBER] { 0, 0, 1, -1 };
    private readonly sbyte[] SEARCH_DESTINATION_COLUMN = new sbyte[SEARCH_DIRECTION_NUMBER] { 1, -1, 0, 0 };

    #endregion


    /// <summary>
    /// フィールドの座標の計算や、ぷよの大きさなどの設定を行う
    /// </summary>
    private void Awake()
    {
        // フィールドに関するパラメータを取得
        _fieldParameter = _summarizeScriptableObjects._fieldParameter;
        // サウンドをまとめたスクリプトを取得
        _inGameSounds = _summarizeScriptableObjects._inGameSounds;

        // ゲームの進行にかかわるフラグを管理するスクリプトを取得
        _playerFlags = this.GetComponent<PlayerFlags>();
        // ぷよの演出を行うスクリプトを取得
        _puyoPresentation = this.GetComponent<PuyoPresentation>();

        // フィールドを示す配列型の変数の大きさを定義する(上には一マス余分に作る、はみ出したぷよを消すため)
        _puyoColorsData = new sbyte[_fieldParameter._fieldRowSize, _fieldParameter._fieldColumnSize];
        _puyoTransforms = new Transform[_fieldParameter._fieldRowSize, _fieldParameter._fieldColumnSize];

        // フィールドや列数からなる配列の大きさを定義する
        _isSearchedPuyos = new bool[_fieldParameter._fieldRowSize, _fieldParameter._fieldColumnSize];
        _eachRowEraceCount = new sbyte[_fieldParameter._fieldRowSize];


        // 各行と列の座標
        _eachRowFieldPositions = new float[_fieldParameter._fieldRowSize];
        _eachColumnFieldPositions = new float[_fieldParameter._fieldColumnSize];

        // フィールドの左下端の座標を求める
        Vector2 bottomLeftEdgeMassPos = this.transform.position - this.transform.lossyScale / 2;
        //　フィールドの左下端のマスの中心を求める
        bottomLeftEdgeMassPos += _fieldParameter._scaleOfOneMass / 2;

        // 行数分繰り返す
        for (int i = 0; i < _fieldParameter._fieldRowSize; i++)
        {
            // 各行の座標を求める
            _eachRowFieldPositions[i] = bottomLeftEdgeMassPos.x + _fieldParameter._scaleOfOneMass.x * i;
        }
        // 列数分繰り返す
        for (int i = 0; i < _fieldParameter._fieldColumnSize; i++)
        {
            // 行の座標を求める
            _eachColumnFieldPositions[i] = bottomLeftEdgeMassPos.y + _fieldParameter._scaleOfOneMass.y * i;
        }

        // フィールドの高さデータの大きさを定義
        _fieldHeightData = new sbyte[_fieldParameter._fieldRowSize];

        // 効果音を鳴らすためオーディオソースを取得
        _puyoSeAudioSource = this.GetComponent<AudioSource>();
    }


    /// <summary>
    /// ぷよが消えている間など、フラグに応じて特定の処理を行う
    /// </summary>
    private void FixedUpdate()
    {
        // ぷよの設置演出中の処理
        if (_isInstallingPresentation)
        {
            // ぷよの設置演出が終了したとき、設置処理を行う。
            if (!_puyoPresentation._duringInstallPresentation)
            {

            }
        }

        // ぷよの消去演出中の処理
        if (_isEracingPresentation)
        {
            // ぷよの消去演出が終了したとき、消去処理を行う。
            if (!_puyoPresentation._duringEracePresentation)
            {
                // 消去演出していない状態のフラグをセット
                _isEracingPresentation = false;

                // ぷよを消去したフラグをセット
                _playerFlags._isPuyoEraced = true;

                // ぷよを消去した時の効果音をならす
                _puyoSeAudioSource.PlayOneShot(_inGameSounds._puyoEraceSE);

                // ぷよを消す処理
                PuyoEraceProcess();
            }
        }
    }


    /// <summary>
    /// 消せるぷよがあるか盤面上のすべてを探索する
    /// </summary>
    private void SearchForEracePuyosAllMass()
    {
        // ぷよを消去するかを示すフラグ
        bool isPuyoErace = false;

        // すべての行を検索するまで繰り返す
        for (int i = 0; i < _fieldParameter._fieldRowSize; i++)
        {
            // 検索をするときにスキップするマス数
            sbyte searchSkipMass = 2;
            // 検索を開始する列(段)
            int serchStartColumn = 0;

            // 検索する行が偶数行なら、検索を開始する列を一段上げる
            if (i % 2 == 0)
            {
                serchStartColumn = 1;
            }

            // ぷよが連結しているかをマスごとに(１段飛ばしで下から上に)検索する
            for (int j = serchStartColumn; j < _fieldParameter._fieldColumnSize; j += searchSkipMass)
            {
                // 検索するマスのぷよの色を格納
                sbyte searchColor = _puyoColorsData[i, j];

                // 検索するマスが空なら次の行にいく
                if (searchColor == 0)
                {
                    break;
                }

                // 検索するマスが検索済みなら、次の段にいく
                if (_isSearchedPuyos[i, j])
                {
                    continue;
                }

                // 検索マスのぷよと同じ色のぷよが周りにあるか探索する
                SearchSurroundingSameColor(searchColor, i, j);

                // ぷよの連結数がぷよが消える数を超えたときの処理
                if (_linkingPuyoRows.Count >= _fieldParameter._puyoEraceLinkCount)
                {
                    // ぷよ消去フラグをセット
                    isPuyoErace = true;

                    // 消去するぷよの配列に連結しているぷよを格納する
                    for (int k = 0; k < _linkingPuyoRows.Count; k++)
                    {
                        _eracePuyoRows.Add(_linkingPuyoRows[k]);
                        _eracePuyoColumns.Add(_linkingPuyoColumns[k]);
                    }
                }

                // 連結しているぷよ位置の配列を初期化
                _linkingPuyoRows = new List<int>();
                _linkingPuyoColumns = new List<int>();
            }
        }
        // ぷよが検索済みかを示す配列を初期化する
        _isSearchedPuyos = new bool[_fieldParameter._fieldRowSize, _fieldParameter._fieldColumnSize];

        // ぷよが消えていないなら、高さデータを更新して処理を終える
        if (!isPuyoErace)
        {
            // ゲームオーバーマスに設置しているかを判定する
            if (_puyoColorsData[_fieldParameter._gameOverRow, _fieldParameter._gameOverColumn] != 0)
            {
                // ゲームオーバーの処理を行う
                GameOverProcess();

                return;
            }

            // フィールドの高さデータを更新
            for (int i = 0; i < _fieldParameter._fieldRowSize; i++)
            {
                FieldHeightDataUpdate(i);
            }

            // ぷよが消えたあとに落ちる処理
            PuyoFallProcessAfterErace();

            // 全てのぷよが消え終わったときのフラグをセット
            _playerFlags._isAllPuyoEraced = true;

            return;
        }

        // ぷよ消去中のフラグをセット
        _isEracingPresentation = true;

        // 消去するぷよのTransform
        List<Transform> eracePuyoTransforms = new List<Transform>();

        // 消去するぷよのTransformを格納する
        for (int i = 0; i < _eracePuyoRows.Count; i++)
        {
            eracePuyoTransforms.Add(_puyoTransforms[_eracePuyoRows[i], _eracePuyoColumns[i]]);
        }

        // ぷよの消去時の演出を行う
        _puyoPresentation.PuyoEracePresentationStart(eracePuyoTransforms);
    }


    /// <summary>
    /// ぷよが消えた時の処理
    /// </summary>
    private void PuyoEraceProcess()
    {
        // 消えるぷよの数だけ繰り返して、消える処理を行う
        for (int i = 0; i < _eracePuyoColumns.Count; i++)
        {
            // ぷよをScene上から消す
            _puyoTransforms[_eracePuyoRows[i], _eracePuyoColumns[i]].gameObject.SetActive(false);

            // フィールドデータの消えたぷよの位置を空にする
            _puyoColorsData[_eracePuyoRows[i], _eracePuyoColumns[i]] = EMPTY_PUYO_NUMBER;

            // ぷよの消えた数を足す
            _eachRowEraceCount[_eracePuyoRows[i]] += 1;
        }

        // ぷよが消えたあとに落ちる処理
        PuyoFallProcessAfterErace();

        // 格納された消すぷよを初期化
        _eracePuyoRows = new List<int>();
        _eracePuyoColumns = new List<int>();

        // ぷよが検索済みかを示す配列を初期化
        _isSearchedPuyos = new bool[_fieldParameter._fieldRowSize, _fieldParameter._fieldColumnSize];

        // 各行のぷよの消えた数を初期化
        _eachRowEraceCount = new sbyte[_fieldParameter._fieldRowSize];

        // ぷよが消えるかを探索する処理
        SearchForEracePuyosAllMass();
    }


    /// <summary>
    /// ぷよの連結しているかを探索し、その数を返すメソッド
    /// </summary>
    /// <param name="searchColor"> 探索するぷよの色番号 </param>
    /// <param name="searchOriginRow" name="searchOriginColumn"> 探索をする元(中心)になる行と段の位置 </param>
    private void SearchSurroundingSameColor(sbyte searchColor, int searchOriginRow, int searchOriginColumn)
    {
        // 検索開始する位置のぷよを連結ぷよの配列に追加する
        _linkingPuyoRows.Add(searchOriginRow);
        _linkingPuyoColumns.Add(searchOriginColumn);

        // 検索開始位置を検索済みにする
        _isSearchedPuyos[searchOriginRow, searchOriginColumn] = true;

        // 上下左右のぷよの色の確認を行う
        for (sbyte i = 0; i < SEARCH_DIRECTION_NUMBER; i++)
        {
            // 検索する行を求める
            int searchDestinationRow = searchOriginRow + SEARCH_DESTINATION_ROW[i];

            // 存在しない行なら次の要素へ
            if (searchDestinationRow < 0 || searchDestinationRow >= _fieldParameter._fieldRowSize)
            {
                continue;
            }

            // 検索する段を求める
            int searchDestinationColumn = searchOriginColumn + SEARCH_DESTINATION_COLUMN[i];

            // 存在しない段なら次の要素へ
            if (searchDestinationColumn < 0 || searchDestinationColumn >= _fieldParameter._fieldColumnSize)
            {
                continue;
            }

            // 検索したぷよが検索済みまたは、違う色なら次の要素へ
            if (_isSearchedPuyos[searchDestinationRow, searchDestinationColumn] || 
                _puyoColorsData[searchDestinationRow, searchDestinationColumn] != searchColor)
            {
                continue;
            }

            // 検索したぷよからさらに検索する(再帰関数)
            SearchSurroundingSameColor(searchColor, searchDestinationRow, searchDestinationColumn);
        }
    }


    /// <summary>
    /// ぷよが消えたあとに落ちる処理
    /// </summary>
    private void PuyoFallProcessAfterErace()
    {
        // 行数分繰り返す
        for (int i = 0; i < _fieldParameter._fieldRowSize; i++)
        {
            // この行の、ぷよの消した数が０なら次の行へ
            if (_eachRowEraceCount[i] == 0)
            {
                continue;
            }

            // ぷよの落下地点の高さ
            sbyte puyoDropRow = 0;

            // 段数分繰り返す(jは検索する段)
            for (int j = 0; j < _fieldParameter._fieldColumnSize; j++)
            {
                // この行のぷよの消えた数がゼロになり、その後空マスがあったら次の行へ
                if (_eachRowEraceCount[i] < 0)
                {
                    break;
                }

                // 検索マスが空マスのとき
                if (_puyoColorsData[i, j] == EMPTY_PUYO_NUMBER)
                {
                    // 各行の消えたぷよの数を減らす
                    _eachRowEraceCount[i] -= 1;

                    continue;
                }


                ///-- 浮いているぷよを落下させる処理 --///

                // ぷよが浮いていないときの処理
                if(puyoDropRow == j)
                {
                    // ぷよの落下地点を一段上げる
                    puyoDropRow++;

                    continue;
                }

                // ぷよを落下地点まで移動する
                _puyoTransforms[i, j].position = new Vector3(_eachRowFieldPositions[i], _eachColumnFieldPositions[puyoDropRow], 0);

                // ぷよデータをもっとも下の段まで移動する
                _puyoColorsData[i, puyoDropRow] = _puyoColorsData[i, j];
                _puyoTransforms[i, puyoDropRow] = _puyoTransforms[i, j];

                // 検索するぷよの場所を空にする
                _puyoColorsData[i, j] = EMPTY_PUYO_NUMBER;
                _puyoTransforms[i, j] = null;

                // ぷよの落下地点を一段上げる
                puyoDropRow++;
            }
        }

        // フィールドの高さデータを更新
        for (int i = 0; i < _fieldParameter._fieldRowSize; i++)
        {
            FieldHeightDataUpdate(i);
        }
    }


    /// <summary>
    /// フィールドの高さデータを更新する
    /// </summary>
    /// <param name="updateRow"> 更新する列 </param>
    private void FieldHeightDataUpdate(int updateRow)
    {
        // 列の高さ(列数)
        sbyte verticalNumber = 0;

        // フィールドデータからぷよの高さを数えて、格納する
        for (sbyte i = 0; _puyoColorsData[updateRow, i] != EMPTY_PUYO_NUMBER; i++)
        {
            verticalNumber++;

            // フィールドの最上部に達している場合、高さの計測を終了する
            if (i == _fieldParameter._fieldColumnSize - 1)
            {
                break;
            }
        }

        // 更新する列の高さ(列数)を更新する
        _fieldHeightData[updateRow] = verticalNumber;
    }


    /// <summary>
    /// 新たにぷよを設置したときの処理
    /// </summary>
    /// <param name="installlPuyoDatas"> ぷよの 色, Transform, 行, 列 </param>
    public void PuyoInstallProcess(List<PuyoStructs.ControlPuyoData> installlPuyoDatas)
    {
        // ぷよを設置したときの効果音をならす
        _puyoSeAudioSource.PlayOneShot(_inGameSounds._puyoInstallSE);

        // フィールドデータの更新を行う
        for (int i = 0; i < installlPuyoDatas.Count; i++)
        {
            // ぷよの設置位置がフィールドの高さ以上なら、そのぷよを消して次の要素へ
            if (installlPuyoDatas[i].Column >= _fieldParameter._fieldColumnSize)
            {
                installlPuyoDatas[i].Transform.gameObject.SetActive(false);

                continue;
            }

            // フィールドデータを更新する
            _puyoColorsData[installlPuyoDatas[i].Row, installlPuyoDatas[i].Column] = installlPuyoDatas[i].Color;
            _puyoTransforms[installlPuyoDatas[i].Row, installlPuyoDatas[i].Column] = installlPuyoDatas[i].Transform;

            // フィールドの高さデータを更新
            FieldHeightDataUpdate(installlPuyoDatas[i].Row);
        }


        // 空中に浮いているぷよがあったときにぷよを落下させる処理
        for (int i = 0; i < installlPuyoDatas.Count; i++)
        {
            // 設置されたぷよのひとつ下の段
            int underInstallPuyoColumn = installlPuyoDatas[i].Column - 1;

            // 設置ぷよの一段下が配列外または、なにかの要素が入ってれば次の要素へ
            if (underInstallPuyoColumn < 0 || _puyoColorsData[installlPuyoDatas[i].Row, installlPuyoDatas[i].Column - 1] != EMPTY_PUYO_NUMBER)
            {
                continue;
            }

            ///-- 空中にぷよがあった場合の処理 --///

            // 空中に浮いているぷよの上にぷよがある間繰り返し、落下させる
            for (int j = 0; installlPuyoDatas[i].Column + j < _fieldParameter._fieldColumnSize; j++)
            {
                // 落とすぷよの段と、ぷよが着地する段
                int toBeDropedColumn = installlPuyoDatas[i].Column + j;
                int toBeInstallColumn = _fieldHeightData[installlPuyoDatas[i].Row] + j;

                // 空中に浮いているぷよがなくなったら処理をやめる
                if (_puyoColorsData[installlPuyoDatas[i].Row, toBeDropedColumn] == EMPTY_PUYO_NUMBER)
                {
                    break;
                }

                // ぷよを落下させる
                _puyoTransforms[installlPuyoDatas[i].Row, toBeDropedColumn].position = new Vector3(_eachRowFieldPositions[installlPuyoDatas[i].Row], _eachColumnFieldPositions[toBeInstallColumn], 0);

                // ぷよのデータを落下位置に移動する
                _puyoColorsData[installlPuyoDatas[i].Row, toBeInstallColumn] = _puyoColorsData[installlPuyoDatas[i].Row, toBeDropedColumn];
                _puyoTransforms[installlPuyoDatas[i].Row, toBeInstallColumn] = _puyoTransforms[installlPuyoDatas[i].Row, toBeDropedColumn];

                // もともとぷよがあった場所を空にする
                _puyoColorsData[installlPuyoDatas[i].Row, toBeDropedColumn] = EMPTY_PUYO_NUMBER;
                _puyoTransforms[installlPuyoDatas[i].Row, toBeDropedColumn] = null;
            }

            // フィールドの高さデータを更新
            FieldHeightDataUpdate(installlPuyoDatas[i].Row);
        }

        // 消えるぷよがあるかを探索する
        SearchForEracePuyosAllMass();
    }


    /// <summary>
    /// ゲームオーバー時の処理を行う
    /// </summary>
    private void GameOverProcess() 
    {
        // ゲームオーバーテキストを表示する
        _gameOverText.SetActive(true);
    }


    /// <summary>
    /// フィールド上のぷよの高さを返すメソッド
    /// </summary>
    public sbyte[] GetFieldHeightData()
    {
        return _fieldHeightData;
    }


    /// <summary>
    /// フィールドの列と行の中心座標を返す
    /// </summary>
    public (float[], float[]) GetFieldMassPos()
    {
        return (_eachRowFieldPositions, _eachColumnFieldPositions);
    }
}