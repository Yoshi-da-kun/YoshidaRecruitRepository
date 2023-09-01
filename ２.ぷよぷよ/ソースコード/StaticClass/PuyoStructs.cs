
using UnityEngine;

/// --------------------------------------------------
/// #PuyoStructs.cs
/// 作成者:吉田雄伍
/// 
/// ぷよを管理する構造体をまとめるスクリプトです
/// --------------------------------------------------

public static class PuyoStructs
{
    // 操作するぷよの情報を格納する構造体
    public struct ControlPuyoData
    {
        public sbyte Color;
        public Transform Transform;
        public int Row;
        public int Column;
    }
}
