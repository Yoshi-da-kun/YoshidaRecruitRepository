
using System.Collections.Generic;
using UnityEngine;
using ColliderDataCollection;
using CollideJudgement;

/// --------------------------------------------------
/// #CollisionProcessing.cs
/// 作成者:吉田雄伍
/// 
/// コライダーの衝突検知や、めり込まないため処理を行うスクリプト
/// --------------------------------------------------

namespace CollisionSystem
{
    public static class CollisionProcessing
    {
        /// <summary>
        /// 弾の当たり判定との衝突処理を行う
        /// </summary>
        public static List<OriginalCollider> BulletCollision(OriginalCollider collisionCollider)
        {
            // 衝突しているコライダーのList
            List<OriginalCollider> collidingObjects = new List<OriginalCollider>();

            // 弾の当たり判定をもつコライダーすべてを取得
            ColliderDatas.ColliderLists bulletColliders = ColliderDatas.GetBulletColliderList();

            // 衝突しているかを検知して、衝突したオブジェクトを格納する
            collidingObjects = CollisionCheck(collisionCollider, bulletColliders);

            // 衝突したオブジェクトを返す
            return collidingObjects;
        }


        /// <summary>
        /// 直接的な当たり判定との衝突処理を行う
        /// </summary>
        /// <param name="collisionCollider"> オブジェクトのコライダー </param>
        /// <param name="collisionTransform"> オブジェクトのTransform </param>
        /// <param name="movementDistance"> 移動量 </param>
        public static List<OriginalCollider> PhysicsCollision(OriginalCollider collisionCollider, Transform collisionTransform, Vector3 movementDistance)
        {
            // 衝突検知を行うSphereコライダー
            OriginalSphereCollider collisionSphereCollider = collisionCollider.GetComponent<OriginalSphereCollider>();
            
            // 衝突しているコライダーのList
            List<OriginalCollider> collidingColliders = new List<OriginalCollider>();

            // 弾の当たり判定をもつコライダーすべてを取得
            ColliderDatas.ColliderLists physicsColliders = ColliderDatas.GetPhysicsColliderList();

            // コライダーの現在座標
            Vector3 currentCollisionPosition = collisionTransform.position;
            Vector3 destinationCollisionPosition = collisionTransform.position + movementDistance;

            // 移動予定座標に移動する処理
            collisionTransform.position = destinationCollisionPosition;

            // 衝突しているかを検知して、衝突したオブジェクトを格納する
            collidingColliders = CollisionCheck(collisionCollider, physicsColliders);

            // オブジェクトが衝突していなければ以降の処理を行わない
            if (collidingColliders.Count == 0)
            {
                // 衝突したオブジェクトを返す
                return collidingColliders;
            }

            ///-- 衝突したときに、めり込まないようにする処理 --///

            // 当たっているコライダー分繰り返す
            for (int i = 0; i < collidingColliders.Count; i++)
            {
                // 衝突したコライダーの形状に応じた処理
                switch (collidingColliders[i]._colliderShape)
                {
                    // 球(Sphere)のとき
                    case OriginalCollider._typeOfColliderShape.Sphere:

                        // 移動前の位置に戻す(仮置き、改善可)
                        collidingColliders[i].transform.position = currentCollisionPosition;

                        break;

                    // 直方体(Box)のとき
                    case OriginalCollider._typeOfColliderShape.Box:

                        // 衝突したコライダーの座標を格納する
                        Vector3 collidingColliderPosition = collidingColliders[i].transform.position;
 
                        OriginalBoxCollider collidingBoxCollider = collidingColliders[i].GetComponent<OriginalBoxCollider>();

                        // 衝突したコライダーの頂点の座標
                        Vector3 collidingColliderMinPosition = collidingColliderPosition - collidingBoxCollider._colliderSize / 2;
                        Vector3 collidingColliderMaxPosition = collidingColliderPosition + collidingBoxCollider._colliderSize / 2;


                        ///-- 移動方向に応じてコライダーのめり込んだ距離を求める --///

                        // めり込んでいる距離
                        Vector3 SinkIntoDistance = new Vector3(0, 0, 0);

                        // コライダーのめり込んだ距離を求める処理
                        // x軸の正方向に移動し、コライダーに埋まっているとき
                        if (movementDistance.x > 0 && currentCollisionPosition.x + collisionSphereCollider._colliderRadius >= collidingColliderMinPosition.x)
                        {
                            SinkIntoDistance.x = Mathf.Abs(currentCollisionPosition.x + collisionSphereCollider._colliderRadius - collidingColliderMinPosition.x);
                        }
                        // x軸の負方向に移動し、コライダーに埋まっているとき
                        else if (movementDistance.x < 0 && currentCollisionPosition.x - collisionSphereCollider._colliderRadius <= collidingColliderMaxPosition.x)
                        {
                            SinkIntoDistance.x = Mathf.Abs(collidingColliderMaxPosition.x - currentCollisionPosition.x - collisionSphereCollider._colliderRadius);
                        }
                        // y軸の正方向に移動し、コライダーに埋まっているとき
                        if (movementDistance.y > 0 && currentCollisionPosition.y + collisionSphereCollider._colliderRadius >= collidingColliderMinPosition.y)
                        {
                            SinkIntoDistance.y = Mathf.Abs(currentCollisionPosition.y + collisionSphereCollider._colliderRadius - collidingColliderMinPosition.y);
                        }
                        // y軸の負方向に移動し、コライダーに埋まっているとき
                        else if (movementDistance.y < 0 && currentCollisionPosition.y - collisionSphereCollider._colliderRadius <= collidingColliderMaxPosition.y)
                        {
                            SinkIntoDistance.y = Mathf.Abs(collidingColliderMaxPosition.y - currentCollisionPosition.y - collisionSphereCollider._colliderRadius);
                        }
                        // z軸の正方向に移動し、コライダーに埋まっているとき
                        if (movementDistance.z > 0 && currentCollisionPosition.z + collisionSphereCollider._colliderRadius >= collidingColliderMinPosition.z)
                        {
                            SinkIntoDistance.z = Mathf.Abs(currentCollisionPosition.z + collisionSphereCollider._colliderRadius - collidingColliderMinPosition.z);
                        }
                        // z軸の負方向に移動し、コライダーに埋まっているとき
                        else if (movementDistance.z < 0 && currentCollisionPosition.z - collisionSphereCollider._colliderRadius <= collidingColliderMaxPosition.z)
                        {
                            SinkIntoDistance.z = Mathf.Abs(collidingColliderMaxPosition.z - currentCollisionPosition.z - collisionSphereCollider._colliderRadius);
                        }


                        ///-- めり込まない座標に修正する処理を行う --///

                        // x軸が最もめり込んでいないとき
                        if (SinkIntoDistance.x < SinkIntoDistance.y && SinkIntoDistance.x < SinkIntoDistance.z)
                        {
                            // x軸の正方向に移動していたら、コライダーの最小座標にめり込まないように修正
                            if (movementDistance.x > 0)
                            {
                                destinationCollisionPosition.x = collidingColliderMinPosition.x - collisionSphereCollider._colliderRadius;
                            }
                            // x軸の負方向に移動していたら、コライダーの最大座標にめり込まないように修正
                            else if (movementDistance.x < 0)
                            {
                                destinationCollisionPosition.x = collidingColliderMaxPosition.x + collisionSphereCollider._colliderRadius;
                            }
                        }
                        // y軸が最もめり込んでいないとき
                        else if (SinkIntoDistance.y < SinkIntoDistance.z)
                        {
                            // y軸の正方向に移動していたら、コライダーの最小座標にめり込まないように修正
                            if (movementDistance.y > 0)
                            {
                                destinationCollisionPosition.y = collidingColliderMinPosition.y - collisionSphereCollider._colliderRadius;
                            }
                            // y軸の負方向に移動していたら、コライダーの最大座標にめり込まないように修正
                            else if (movementDistance.y < 0)
                            {
                                destinationCollisionPosition.y = collidingColliderMaxPosition.y + collisionSphereCollider._colliderRadius;
                            }
                        }
                        // z軸が最もめり込んでいないとき
                        else
                        {
                            // z軸の正方向に移動していたら、コライダーの最小座標にめり込まないように修正
                            if (movementDistance.z > 0)
                            {
                                destinationCollisionPosition.z = collidingColliderMinPosition.z - collisionSphereCollider._colliderRadius;
                            }
                            // z軸の負方向に移動していたら、コライダーの最大座標にめり込まないように修正
                            else if (movementDistance.z < 0)
                            {
                                destinationCollisionPosition.z = collidingColliderMaxPosition.z + collisionSphereCollider._colliderRadius;
                            }
                        }

                        // コライダーの座標を更新する
                        collisionTransform.position = destinationCollisionPosition;

                        break;
                }
            }
            // 衝突したオブジェクトを返す
            return collidingColliders;
        }


        /// <summary>
        /// 衝突しているかを検知して、衝突したオブジェクトを返す
        /// </summary>
        /// <param name="collisionCollider"> 衝突しているかを検知するコライダー </param>
        /// <param name="recieveColliderList"> 被衝突側のコライダーのリスト </param>
        private static List<OriginalCollider> CollisionCheck(OriginalCollider collisionCollider, ColliderDatas.ColliderLists recieveColliderList)
        {
            // 衝突しているコライダーのList
            List<OriginalCollider> collidingColliders = new List<OriginalCollider>();

            // 各コライダーの形状に応じた処理を行う
            switch (collisionCollider._colliderShape)
            {
                // 衝突検知するコライダーが球(Sphere)のコライダーのとき
                case OriginalCollider._typeOfColliderShape.Sphere:

                    // 衝突検知する球(Sphere)コライダーを取得する
                    OriginalSphereCollider collisionSphereCollider = collisionCollider.GetComponent<OriginalSphereCollider>();

                    // 直方体(Box)コライダーと衝突しているかを検知する
                    for (int i = 0; i < recieveColliderList.boxList.Count; i++)
                    {
                        // 被衝突側のコライダーのListがnullなら次の要素へ
                        if (!recieveColliderList.boxList[i])
                        {
                            continue;
                        }

                        // 衝突したオブジェクトを取得
                        OriginalCollider collidingCollider = CollideJudgementFunctions.SphereToBoxCollision(collisionSphereCollider, recieveColliderList.boxList[i]);

                        // 衝突しているオブジェクトがなかったら次の要素へ
                        if (!collidingCollider)
                        {
                            continue;
                        }

                        // 衝突したオブジェクトを格納する
                        collidingColliders.Add(collidingCollider);
                    }

                    // 球(Sphere)コライダーと衝突しているかを検知する
                    for (int i = 0; i < recieveColliderList.sphereList.Count; i++)
                    {
                        // 被衝突側のコライダーのListがnullなら次の要素へ
                        if (!recieveColliderList.sphereList[i])
                        {
                            continue;
                        }

                        // 衝突したオブジェクトを取得
                        OriginalCollider collidingCollider = CollideJudgementFunctions.SphereToSphereCollision(collisionSphereCollider, recieveColliderList.sphereList[i]);

                        // 衝突しているオブジェクトがなかったら次の要素へ
                        if (!collidingCollider)
                        {
                            continue;
                        }

                        // 衝突したオブジェクトを格納する
                        collidingColliders.Add(collidingCollider);
                    }

                    break;


                // 直方体(Sphere)のコライダーのとき(未実装:Box形状のものを動かしていないため)
                //case OriginalCollider._typeOfColliderShape.Box:
                //
                //    break;
            }

            // 衝突したオブジェクトを返す
            return collidingColliders;
        }
    }
}
