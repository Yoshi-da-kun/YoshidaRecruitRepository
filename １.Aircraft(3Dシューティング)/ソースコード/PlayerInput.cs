
using UnityEngine;

/// --------------------------------------------------
/// #PlayerInput.cs
/// 作成者:吉田雄伍
/// 
/// プレイヤーの入力処理の関数をまとめたスクリプト
/// --------------------------------------------------

namespace ControllerInput
{
    public static class PlayerInput
    {
        /// <summary>
        /// 速度調整を行う入力値を返す(負なら減速、正なら加速)
        /// </summary>
        public static float SpeedControlInput()
        {
            return Input.GetAxis("SpeedControl");
        }


        /// <summary>
        /// 旋回を行うためのスティックとキーの入力値を返す
        /// </summary>
        public static Vector2 TurningInput()
        {
            // スティックかキーの入力値を格納
            float horizontalInputVolume = Input.GetAxisRaw("Horizontal");
            float verticalInputVolume = Input.GetAxisRaw("Vertical");

            // 旋回量を格納する
            return new Vector2(horizontalInputVolume, verticalInputVolume);
        }


        /// <summary>
        /// 弾を発射するボタンの入力を返す
        /// </summary>
        public static bool BulletShotInput()
        {
            return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0");
        }


        /// <summary>
        /// ワープスキルの入力
        /// </summary>
        public static bool TimeWarpBombInput()
        {
            //// 仮置き
            return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown("joystick button 3");
        }


        /// <summary>
        /// タイトルでスタートするための入力
        /// </summary>
        public static bool TitlePressInput()
        {
            return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0");
        }

        
        /// <summary>
        /// ゲーム終了やメニューを開く入力
        /// </summary>
        public static bool EscapeInput()
        {
            return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7");
        }
    }
}
