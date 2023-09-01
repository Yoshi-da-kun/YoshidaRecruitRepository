
using System.Collections.Generic;
using UnityEngine;

/// --------------------------------------------------------
/// #FieldGenerator.cs
/// 
/// フィールドを生成するスクリプト
/// --------------------------------------------------------

public class FieldGenerator : MonoBehaviour
{
    [SerializeField, Label("フィールドのパラメータ")]
    private FieldParameter _fieldParameter;

    #region 各ブロックごとのPrefabや変数
    
    [SerializeField, Header("各ブロックのPrefab"),StringIndex(new string[] { "空白のブロック", "平らな床ブロック", "壊せないブロック", "鉄のブロック", "木材のブロック" })]
    private GameObject[] _blockPrefabs;
    // 各ブロックのObjectPoolのフォルダの名前(※各ブロックの番号と順番を合わせること)
    private readonly string[] _blockPoolNames = {"NothingBlocks", "FlatBlocks", "UnbreakableBlocks", "IronBlocks", "WoodBlocks"};

    // 各ブロックのObjectPoolのフォルダ
    private Transform[] _blockPoolFolderTransforms = default;

    // 各ブロックの番号
    private const sbyte NOTHING_BLOCK_NUMBER = 0;
    private const sbyte FLAT_BLOCK_NUMBER = 1;
    private const sbyte UNBREAKABLE_BLOCK_NUMBER = 2;
    private const sbyte IRON_BLOCK_NUMBER = 3;
    private const sbyte WOOD_BLOCK_NUMBER = 4;

    #endregion 各ブロックごとのPrefabや変数

    // 各マスを番号で格納するフィールドデータ
    private sbyte[,] _fieldDatas = default;

    // フィールド上のブロックのTransform(フィールドを左右半分で２分割しているためListが二つ)
    private List<Transform>[] _blockTransforms = new List<Transform>[2];

    // 次に生成するブロックと、余分生成ブロックのTransformを示すポインタ
    private sbyte _newGenerateBlockTransformsPointer = default;
    private sbyte _extraGenerateBlockTransformsPointer = default;


    // 次のウェーブ(生成)に残す生成済みフィールドの列の大きさ
    private int _rowSizeLeftForNextWave = default;

    // フィールドの列数の最大値
    private int _fieldMaxRowSize = default;

    // フィールドの余分な生成を開始する列
    private int _extraFieldStartRow = default;

    // 区画あたりの列数
    private int _rowSizePerSelection = default;


    private void Start()
    {

        SwitchingNewClimateProcess();
    }

    /// <summary>
    /// 完全新規のフィールド(Scene)になるときの処理
    /// </summary>
    private void SwitchingNewClimateProcess()
    {
        // 区画あたりの列数を求める
        _rowSizePerSelection = _fieldParameter._rowSizeToGoal / _fieldParameter._numberOfFieldSelection;

        // フィールドデータ用の配列の大きさを定義(列の大きさ + 余分に生成する列数(プラス1区画), 段の大きさ)
        _fieldDatas = new sbyte
            [_fieldParameter._rowSizeToGoal * _fieldParameter._numberOfActiveField + _rowSizePerSelection, _fieldParameter._fieldColumnSize];

        // ブロックのObjectの親オブジェクトの配列の大きさを定義
        _blockPoolFolderTransforms = new Transform[_blockPoolNames.Length];

        // 各ブロックのPool用のオブジェクトを生成する
        for (int i = 0; i < _blockPoolNames.Length; i++)
        {
            _blockPoolFolderTransforms[i] = new GameObject(_blockPoolNames[i]).transform;
        }

        // フィールド上のブロックのTransform配列の大きさを定義(表示されているフィールド用 + 余分生成したフィールド用)
        _blockTransforms = new List<Transform>[_fieldParameter._numberOfActiveField + 1];
        for (int i = 0; i < _blockTransforms.Length; i++)
        {
            _blockTransforms[i] = new List<Transform>();
        }
        // 余分に生成されるブロックのTransformを示すポインタを求める
        _extraGenerateBlockTransformsPointer = _newGenerateBlockTransformsPointer;
        _extraGenerateBlockTransformsPointer++;

        // フィールドの列数の最大値を求める
        _fieldMaxRowSize = _fieldDatas.GetLength(0);
        
        // 次のウェーブ(生成)に残す生成済みフィールドの列の大きさを求める
        _rowSizeLeftForNextWave = _fieldMaxRowSize - _fieldParameter._rowSizeToGoal;
        
        // フィールドの余分生成を開始する列を求める
        _extraFieldStartRow = _fieldMaxRowSize - _rowSizePerSelection;
    }

    private void Update()
    {
        // スペースキーを押したとき、フィールドを生成する
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateNewWaveField();
        }
    }


    /// <summary>
    /// ウェーブ進行時のフィールドの生成を行う
    /// </summary>
    private void GenerateNewWaveField()
    {
        // 生成済みのフィールドの削除と移動を行う
        RemoveAndMoveOldWaveField();

        // 新しいフィールドのデータをセットする処理
        FieldDataSetter();

        // 生成したブロックを格納する配列のポインタ
        int instantBlockTransformsPointer = _newGenerateBlockTransformsPointer;

        // 新しく生成するフィールドの列数分繰り返す
        for (int i = _rowSizeLeftForNextWave; i < _fieldMaxRowSize; i++)
        {
            // 生成する列が余分生成の列になったらTransformのポインタを切り替える
            if (i == _extraFieldStartRow)
            {
                instantBlockTransformsPointer = _extraGenerateBlockTransformsPointer;
            }
            // 現在の列のすべて段のブロックを生成する
            for (int j = 0; j < _fieldParameter._fieldColumnSize; j++)
            {
                // ブロックの生成位置を求める
                Vector3 blockGeneratePosition = 
                    _fieldParameter._fieldGenerateStartPosition + _fieldParameter._oneBlockSize * new Vector2(i, j);
                
                // ブロックをScene上に生成して、ブロック用の配列に格納
                _blockTransforms[instantBlockTransformsPointer].Add(GenerateBlock(_fieldDatas[i, j], blockGeneratePosition));
            }
        }
    }


    /// <summary>
    /// 次のウェーブ進行時に、生成済みのフィールドの移動と消去を行う
    /// </summary>
    private void RemoveAndMoveOldWaveField()
    {
        // 生成済みの余分生成列の空白ブロックを埋める処理
        for (int i = _extraFieldStartRow; i < _fieldMaxRowSize; i++)
        {
            // 現在の列のすべての段の空白ブロックを平らなブロックに更新する
            for (int j = 0; j < _fieldParameter._fieldColumnSize; j++)
            {
                // 余分生成の位置に空白ブロック以外が格納されていたら次の要素へ
                if (_fieldDatas[i, j] != NOTHING_BLOCK_NUMBER)
                {
                    continue;
                }
                // 平らなブロックを格納する
                _fieldDatas[i, j] = FLAT_BLOCK_NUMBER;
            }
        }
        // 次のウェーブに残すフィールドデータを移動する
        for (int i = 0; i < _rowSizeLeftForNextWave; i++)
        {
            // 現在の列のすべて段のブロック番号(フィールドデータ)を移動する
            for (int j = 0; j < _fieldParameter._fieldColumnSize; j++)
            {
                _fieldDatas[i, j] = _fieldDatas[_fieldParameter._rowSizeToGoal + i, j];
            }
        }
        // 新規生成するフィールドの初期化に使うブロック番号(平らなブロック)
        sbyte _initializeBlockNumber = FLAT_BLOCK_NUMBER;

        // 新規生成するフィールドデータを初期化する
        for (int i = _rowSizeLeftForNextWave; i < _fieldMaxRowSize; i++)
        {
            // 余分生成用の列になったら初期化用のブロック番号を空白ブロックにする
            if (i == _extraFieldStartRow)
            {
                _initializeBlockNumber = NOTHING_BLOCK_NUMBER;
            }
            // 現在の列のすべて段のフィールドデータを初期化する
            for (int j = 0; j < _fieldParameter._fieldColumnSize; j++)
            {
                _fieldDatas[i, j] = _initializeBlockNumber;
            }
        }
        // 生成されたブロックTransformを示すポインタを更新
        _newGenerateBlockTransformsPointer++;
        _extraGenerateBlockTransformsPointer++;

        // 新規生成用のポインタがTransformの配列数を超えたら、配列のはじめに戻す
        if (_newGenerateBlockTransformsPointer >= _blockTransforms.Length)
        {
            _newGenerateBlockTransformsPointer = 0;
        }
        // 余分生成用のポインタがTransformの配列数を超えたら、配列のはじめに戻す
        if (_extraGenerateBlockTransformsPointer >= _blockTransforms.Length)
        {
            _extraGenerateBlockTransformsPointer = 0;
        }

        // 次のウェーブに残さないブロックを削除する処理
        for (int i = 0; i < _blockTransforms[_extraGenerateBlockTransformsPointer].Count; i++)
        {
            _blockTransforms[_extraGenerateBlockTransformsPointer][i].gameObject.SetActive(false);
        }
        // ブロックを削除したListを初期化
        _blockTransforms[_extraGenerateBlockTransformsPointer] = new List<Transform>();

        // 次ウェーブに残すブロックの座標の移動量を求める
        float movemetForBlockToBeLeft = _fieldParameter._rowSizeToGoal * _fieldParameter._oneBlockSize.x;

        // 次ウェーブに残すブロックの移動を行う処理
        for (int i = 0; i < _blockTransforms.Length; i++)
        {
            // 移動するTransformが格納されていなければ、処理を終了する
            if (_blockTransforms[i] == null)
            {
                continue;
            }
            // 次のウェーブに残すブロックの座標を移動する
            for (int j = 0; j < _blockTransforms[i].Count; j++)
            {
                _blockTransforms[i][j].position -= new Vector3(movemetForBlockToBeLeft, 0, 0);
            }
        }
    }


    /// <summary>
    /// ブロックの生成を行う
    /// </summary>
    /// <param name="generateBlockNumber"> 生成するブロック番号 </param>
    /// <param name="generatePosition"> 生成する座標 </param>
    private Transform GenerateBlock(int generateBlockNumber, Vector2 generatePosition)
    {
        // 再使用できるオブジェクトがあるかを検索する(オブジェクトプール)
        for (int i = 0; i < _blockPoolFolderTransforms[generateBlockNumber].childCount; i++)
        {
            // 検索するオブジェクト
            Transform searchObject = _blockPoolFolderTransforms[generateBlockNumber].GetChild(i);

            // 有効化されていたら次の要素へ
            if (searchObject.gameObject.activeSelf == true)
            {
                continue;
            }
            // 再使用するオブジェクトを有効化
            searchObject.gameObject.SetActive(true);

            // 再使用するオブジェクトを生成位置に移動する
            searchObject.position = generatePosition;

            // 再使用するオブジェクトを返す
            return searchObject;
        }

        // オブジェクトを新しく生成する
        Transform instantiateObject = Instantiate
            (_blockPrefabs[generateBlockNumber], generatePosition, Quaternion.identity, _blockPoolFolderTransforms[generateBlockNumber]).transform;

        // 新しく生成したオブジェクトを返す
        return instantiateObject;
    }


    /// <summary>
    /// フィールドデータを格納していく処理
    /// </summary>
    private void FieldDataSetter()
    {
        // 余分生成を除く、新しく生成するフィールドの列数分初期化を行う
        for (int i = 0; i < _fieldParameter._rowSizeToGoal; i++)
        {
            // 段数分繰り返し、平らなブロックの番号を格納する
            for (int j = 0; j < _fieldParameter._fieldColumnSize; j++)
            {
                _fieldDatas[_rowSizeLeftForNextWave + i, j] = FLAT_BLOCK_NUMBER;
            }
        }
        #region 各ブロックチャンクを生成する処理

        // 壊れないブロックを生成する処理
        if (_fieldParameter._maxUnbreakableChunkCount >= _fieldParameter._minUnbreakableChunkCount)
        {
            // 壊れないブロックのチャンク数を抽選する
            int numberOfUnbreakableChunk = Random.Range(_fieldParameter._minUnbreakableChunkCount, _fieldParameter._maxUnbreakableChunkCount + 1);
            // 壊れないブロックの生成を行う
            ChunkDeployment(UNBREAKABLE_BLOCK_NUMBER, numberOfUnbreakableChunk);
        }
        // 鉄のブロックを生成する
        if (_fieldParameter._maxIronChunkCount >= _fieldParameter._minIronChunkCount)
        {
            // 鉄ブロックのチャンク数を抽選する
            int numberOfIronChunk = Random.Range(_fieldParameter._minIronChunkCount, _fieldParameter._maxIronChunkCount + 1);
            // 鉄ブロックの生成を行う
            ChunkDeployment(IRON_BLOCK_NUMBER, numberOfIronChunk);
        }
        // 木のブロックを生成する処理
        if (_fieldParameter._maxWoodChunkCount >= _fieldParameter._minWoodChunkCount)
        {
            // 木のブロックのチャンク数を抽選する
            int numberOfWoodChunk = Random.Range(_fieldParameter._minWoodChunkCount, _fieldParameter._maxWoodChunkCount + 1);
            // 木のブロックの生成を行う
            ChunkDeployment(WOOD_BLOCK_NUMBER, numberOfWoodChunk);
        }

        #endregion 各ブロックチャンクを生成する処理
    }


    /// <summary>
    /// チャンクを配置する
    /// </summary>
    private void ChunkDeployment(sbyte blockNumber, int numberOfGenerateChunk)
    {
        // フィールドを区画毎に分ける
        int[] chunkCountPerSelections = new int[_fieldParameter._numberOfFieldSelection];

        // まだチャンク数が最大に達していない区画番号
        List<int> notMaxSelectionNumbers = new List<int>();

        // 区画番号を格納する
        for (int i = 0; i < _fieldParameter._numberOfFieldSelection; i++)
        {
            notMaxSelectionNumbers.Add(i);
        }

        // 引数に応じてチャンクをランダムに配置する
        for (int i = 0; i < numberOfGenerateChunk; i++)
        {
            // どの区画に生成するかを抽選する
            int generateSelectionPointer = Random.Range(0, notMaxSelectionNumbers.Count);

            // 生成する区画の左端の列を求める
            int generateSelectionRow = _rowSizeLeftForNextWave + notMaxSelectionNumbers[generateSelectionPointer] * _rowSizePerSelection;

            // チャンクの生成位置を求める(生成する区画の列数 + 区画内でのランダムな列数, 段数の最大値以内のランダムな段数)
            Vector2Int generateMatrix = new Vector2Int
                (generateSelectionRow + Random.Range(0, _rowSizePerSelection), Random.Range(0, _fieldParameter._fieldColumnSize));

            // ブロックチャンクを生成する
            GenerateBlockChunk(blockNumber, generateMatrix);

            // 抽選した区画のチャンク数を加算する
            chunkCountPerSelections[notMaxSelectionNumbers[generateSelectionPointer]]++;

            // 生成する区画のチャンク数が最大のとき
            if (chunkCountPerSelections[notMaxSelectionNumbers[generateSelectionPointer]] == _fieldParameter._maxChunkPerFieldSelection)
            {
                // 最大数に達した区画番号(削除する番号)
                int deleteSelectionNumberPointer = generateSelectionPointer;

                // 末尾の要素と削除する区画番号を入れ替え、削除する
                notMaxSelectionNumbers[generateSelectionPointer] = notMaxSelectionNumbers[notMaxSelectionNumbers.Count - 1];
                notMaxSelectionNumbers[notMaxSelectionNumbers.Count - 1] = deleteSelectionNumberPointer;

                notMaxSelectionNumbers.RemoveAt(notMaxSelectionNumbers.Count - 1);
            }
            // すべての区画が最大数に達した時処理を終了する
            if (notMaxSelectionNumbers.Count == 0)
            {
                return;
            }
        }
    }


    /// <summary>
    /// ブロックのひとまとまり(チャンク)を生成する
    /// </summary>
    private void GenerateBlockChunk(sbyte generateBlockNumber, Vector2Int generateStartMatrix)
    {
        // 最後に生成した列の大きさ
        int lastGeneratedRowSize = _fieldParameter._generateMinRowSize;

        // 生成する列数を減らす状態のときフラグ
        bool isGeneratedSizeSmoller = default;

        // 生成する列の生成開始列
        int generateStartRow = generateStartMatrix.x;

        // ブロックひとまとまり生成するまで繰り返す
        for (int i = 0; i < _fieldParameter._blockChunkMaxSize; i++)
        {
            // 最後に生成した列数に対する次の段の列数
            int plannedGenerateBlockSize = default;

            // 列数を増やせる状態のとき
            if (!isGeneratedSizeSmoller)
            {
                // 列数を増減する抽選の値の最小値
                sbyte decreaseLotteryMinValue = 0;

                // 最小の段数に達していないとき、列数を減らす状態にならないようにする
                if (i < _fieldParameter._generateMinColumnSize)
                {
                    decreaseLotteryMinValue = 1;
                }
                // 列数を最後に生成した列数以上にするか、減らすかを抽選する(0:減らす 以外:増やす)
                int increaseOrDecreaseValue = Random.Range(decreaseLotteryMinValue, _fieldParameter._easeOfChunksLager);

                // 列数を増やすときの処理
                if (increaseOrDecreaseValue > 0)
                {
                    // 大きくなる可能性をもつように列数を抽選する
                    plannedGenerateBlockSize = Random.Range(0, _fieldParameter._increasedMaxBlockSizePerColumn + 1);
                }
                // 列数を減らし始めるときの処理
                else
                {
                    // 列数が大きくならないように列数を抽選する
                    plannedGenerateBlockSize = Random.Range(-_fieldParameter._decreasedMaxBlockSizePerColumn, 1); ;

                    // 列数を減らすときのフラグをセット
                    isGeneratedSizeSmoller = true;
                }
            }
            // 列数を減らす状態のとき
            else
            {
                // 列数が大きくならないように列数を抽選する
                plannedGenerateBlockSize = Random.Range(-_fieldParameter._decreasedMaxBlockSizePerColumn, 1);
            }

            // 生成列数が最小数以上のときに、最後に生成した列の範囲内で開始位置を抽選する処理
            // 生成する列数が小さくなったとき、差の範囲で抽選する
            if (plannedGenerateBlockSize < 0)
            {
                generateStartRow += Random.Range(0, -plannedGenerateBlockSize + 1);
            }
            // 生成する列数が大きくなったとき、差の二倍の範囲で抽選する
            else if (plannedGenerateBlockSize > 0)
            {
                generateStartRow += Random.Range(-plannedGenerateBlockSize, 1);
            }
            // 現在の生成する列数を求める
            int currentGenerateBlockSize = lastGeneratedRowSize + plannedGenerateBlockSize;

            // 抽選した列数分のブロックのデータを更新する
            for (int j = 0; j < currentGenerateBlockSize; j++)
            {
                // データを更新する列と段
                int updateRow = generateStartRow + j;
                int updateColumn = generateStartMatrix.y + i;

                // 更新する列か段がフィールドデータの外のとき、データ更新を終了する
                if (updateRow >= _fieldMaxRowSize || updateColumn >= _fieldParameter._fieldColumnSize)
                {
                    break;
                }
                // ブロックデータを更新する
                _fieldDatas[updateRow, updateColumn] = generateBlockNumber;
            }
            // 列数の最小値以下になったら生成を終了する
            if (currentGenerateBlockSize < _fieldParameter._generateMinRowSize)
            {
                return;
            }
            // 最後に生成した列の大きさを更新する
            lastGeneratedRowSize = currentGenerateBlockSize;
        }
    }
}
