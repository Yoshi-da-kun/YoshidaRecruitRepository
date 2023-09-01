
using UnityEngine;

/// --------------------------------------------------
/// #CollideJudgementFunctions.cs
/// 作成者:吉田雄伍
/// 
/// コライダーの衝突判定をとる関数をまとめたスクリプト
/// --------------------------------------------------

namespace CollideJudgement
{
    public static class CollideJudgementFunctions
    {
        /// <summary>
        /// 球コライダー同士が当たっているかを判定して、当たっているオブジェクトを返す
        /// </summary>
        public static OriginalCollider SphereToSphereCollision(OriginalSphereCollider collisionCollider, OriginalSphereCollider receiveCollider)
        {
            // 衝突しているコライダー
            OriginalCollider collidingCollider = default;

            // コライダー同士の距離を求める
            float colliderDistance = Vector3.Distance(collisionCollider._thisTransform.position, receiveCollider._thisTransform.position);

            // 求めた距離が各球の半径を足した値より小さい(コライダー同士が衝突している)とき
            if (colliderDistance <= collisionCollider._colliderRadius + receiveCollider._colliderRadius)
            {
                collidingCollider = receiveCollider.GetComponent<OriginalCollider>();
            }

            return collidingCollider;
        }


        /// <summary>
        /// 球コライダーが直方体(Box)コライダーに当たっているかを判定して、当たっているオブジェクトを返す
        /// </summary>
        public static OriginalCollider SphereToBoxCollision(OriginalSphereCollider collisionCollider, OriginalBoxCollider receiveCollider)
        {
            // 衝突しているコライダー
            OriginalCollider collidingCollider = default;

            // 衝突する側のコライダーの中心座標
            Vector3 collisionColliderPosition = collisionCollider._thisTransform.position;

            // 衝突する側のコライダーの中心座標に対して、直方体コライダーの中で最も近い座標
            Vector3 receiveCollideClosestPosition = collisionColliderPosition;

            // 被衝突(Box)コライダーの最小、最大値の座標
            Vector3 maxRcieveCollidePosition = receiveCollider._thisTransform.position + receiveCollider._colliderSize / 2;
            Vector3 minRcieveCollidePosition = receiveCollider._thisTransform.position - receiveCollider._colliderSize / 2;


            ///-- 距離を計算するための、直方体(被衝突)コライダーの中から座標を求める --///
            /// ※被衝突コライダー → 被コライダ

            // 被コライダ最小座標(x)より中心座標(x)が小さいとき、被コライダの最小座標を格納する
            if (collisionColliderPosition.x < minRcieveCollidePosition.x)
            {
                receiveCollideClosestPosition.x = minRcieveCollidePosition.x;
            }
            // 被コライダ最大座標(x)より中心座標(x)が大きいとき、被コライダの最大座標を格納する
            else if (collisionColliderPosition.x > maxRcieveCollidePosition.x)
            {
                receiveCollideClosestPosition.x = maxRcieveCollidePosition.x;
            }
            // 被コライダ最小座標(y)より中心座標(y)が小さいとき、被コライダの最小座標を格納する
            if (collisionColliderPosition.y < minRcieveCollidePosition.y)
            {
                receiveCollideClosestPosition.y = minRcieveCollidePosition.y;
            }
            // 被コライダ最大座標(y)より中心座標(y)が大きいとき、被コライダの最大座標を格納する
            else if (collisionColliderPosition.y > maxRcieveCollidePosition.y)
            {
                receiveCollideClosestPosition.y = maxRcieveCollidePosition.y;
            }
            // 被コライダ最小座標(z)より中心座標(z)が小さいとき、被コライダの最小座標を格納する
            if (collisionColliderPosition.z < minRcieveCollidePosition.z)
            {
                receiveCollideClosestPosition.z = minRcieveCollidePosition.z;
            }
            // 被コライダ最大座標(z)より中心座標(z)が大きいとき、被コライダの最大座標を格納する
            else if (collisionColliderPosition.z > maxRcieveCollidePosition.z)
            {
                receiveCollideClosestPosition.z = maxRcieveCollidePosition.z;
            }

            // コライダー同士の距離を求める
            float colliderDistance = Vector3.Distance(collisionColliderPosition, receiveCollideClosestPosition);

            // 求めた距離が半径より小さい(コライダー同士が衝突している)とき
            if (colliderDistance <= collisionCollider._colliderRadius)
            {
                // 衝突したコライダーを格納する
                collidingCollider = receiveCollider.GetComponent<OriginalCollider>();
            }

            // 衝突したコライダーを返す
            return collidingCollider;
        }
    }
}

