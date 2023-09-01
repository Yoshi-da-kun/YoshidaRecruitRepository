
using UnityEngine;

/// --------------------------------------------------------
/// #FieldParameter.cs
/// 
/// マップに関するパラメータをまとめたスクリプト
/// --------------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/FieldParameter", fileName = "NewFieldParameter")]
public class FieldParameter : ScriptableObject
{
    // １ブロックの大きさ
    public Vector2 _oneBlockSize { get { return new Vector2(1, 1); } }

    // フィールドの生成を始める位置
    public Vector2 _fieldGenerateStartPosition { get { return new Vector2(1, 1); } }

    [field: SerializeField, Label("フィールドの縦(段)の大きさ"), Range(2, 50)]
    public int _fieldColumnSize { get; private set; } = 18;

    [field: SerializeField, Label("ゴールまでの横(列)の大きさ"), Range(10, 100)]
    public int _rowSizeToGoal { get; private set; } = 30;

    [field: SerializeField, Label("Sceneに表示するフィールドの数(２推奨)"), Range(2, 30)]
    public int _numberOfActiveField { get; private set; } = 2;


    [field: Header("ブロックのひとまとまり(チャンク)関連")]

    [field: SerializeField, Label("ブロックひとまとまりの最大の大きさ"), Range(2, 40)]
    public int _blockChunkMaxSize { get; private set; } = 5;

    [field: SerializeField, Label("ブロックひとまとまりの大きくなりやすさ"), Range(2, 10)]
    public int _easeOfChunksLager { get; private set; } = 5;

    [field: SerializeField, Label("ブロック一段当たりの大きくなる最大値(２推奨)"), Range(1, 10)]
    public int _increasedMaxBlockSizePerColumn { get; private set; } = 2;

    [field: SerializeField, Label("ブロック一段当たりの小さくなる最大値"), Range(1, 10)]
    public int _decreasedMaxBlockSizePerColumn { get; private set; } = 4;

    [field: SerializeField, Label("列数の最小値(２推奨)"), Range(1, 4)]
    public int _generateMinRowSize { get; private set; } = 2;

    [field: SerializeField, Label("段数の最小値(２推奨)"), Range(1, 4)]
    public int _generateMinColumnSize { get; private set; } = 2;


    [field: Header("各ブロックチャンクの数")]

    [field: SerializeField, Label("壊せないブロックチャンクの最小数"), Range(1, 30)]
    public int _minUnbreakableChunkCount { get; private set; } = 3;

    [field: SerializeField, Label("壊せないブロックチャンクの最大数"), Range(0, 30)]
    public int _maxUnbreakableChunkCount { get; private set; } = 7;

    [field: SerializeField, Label("木ブロックチャンクの最小数"), Range(1, 30)]
    public int _minWoodChunkCount { get; private set; } = 3;

    [field: SerializeField, Label("木ブロックチャンクの最大数"), Range(0, 30)]
    public int _maxWoodChunkCount { get; private set; } = 7;

    [field: SerializeField, Label("鉄ブロックチャンクの最小数"), Range(1, 30)]
    public int _minIronChunkCount { get; private set; } = 3;

    [field: SerializeField, Label("鉄ブロックチャンクの最大数"), Range(0, 30)]
    public int _maxIronChunkCount { get; private set; } = 7;


    [field: Header("各チャンクの配置関連")]

    [field: SerializeField, Label("マップの区画数"), Range(4, 30)]
    public int _numberOfFieldSelection { get; private set; } = 6;

    [field: SerializeField, Label("各区画のチャンクの最大数"), Range(1, 30)]
    public int _maxChunkPerFieldSelection { get; private set; } = 3;
}