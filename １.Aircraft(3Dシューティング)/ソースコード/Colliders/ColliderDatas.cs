
using System.Collections.Generic;
using UnityEngine;

/// --------------------------------------------------
/// #ColliderDatas.cs
/// 作成者:吉田雄伍
/// 
/// 全てのコライダーをリストにもつスクリプト
/// 各メソッドでリストに追加、削除や管理を行う
/// --------------------------------------------------

namespace ColliderDataCollection
{
    public static class ColliderDatas
    {
        private const int OUT_OF_RANGE_LIST_NUMBER = -1;


        // 弾に対する当たり判定をもつ球コライダーのリスト
        private static List<OriginalSphereCollider> _bulletSphereColliders = new List<OriginalSphereCollider>();
        // 直接的な衝突判定をもつ球コライダーのリスト
        private static List<OriginalSphereCollider> _physicsSphereColliders = new List<OriginalSphereCollider>();

        // 弾に対する当たり判定をもつ直方体コライダーのリスト
        private static List<OriginalBoxCollider> _bulletBoxColliders = new List<OriginalBoxCollider>();
        // 直接的な衝突判定をもつ直方体コライダーのリスト
        private static List<OriginalBoxCollider> _physicsBoxColliders = new List<OriginalBoxCollider>();


        #region コライダーをリストに追加する処理


        /// <summary>
        /// 弾に対する当たり判定をもつColliderを格納する処理
        /// </summary>
        public static int AddBulletColliders(OriginalCollider colliderToAdd)
        {
            // 各コライダーの形状に応じた処理を行う
            switch (colliderToAdd._colliderShape)
            {
                // 球(Sphere)のコライダーとき
                case OriginalCollider._typeOfColliderShape.Sphere:

                    // SphereColliederをListに追加する処理
                    (List<OriginalSphereCollider>, int) addedSphereList = AddSphereCollider(_bulletSphereColliders, colliderToAdd);

                    // SphereCollider追加後のリストを格納
                    _bulletSphereColliders = addedSphereList.Item1;

                    // SphereColliderが格納された要素番号を返す
                    return addedSphereList.Item2;


                // 直方体(Box)のコライダーとき
                case OriginalCollider._typeOfColliderShape.Box:

                    // BoxColliederをListに追加し、そのListとColliderの格納番号を取得する
                    (List<OriginalBoxCollider>, int) addedBoxList = AddBoxCollider(_bulletBoxColliders, colliderToAdd);

                    // BoxCollider追加後のリストを格納
                    _bulletBoxColliders = addedBoxList.Item1;

                    // BoxColliderが格納された要素番号を返す
                    return addedBoxList.Item2;
            }

            // 例外の処理(絶対に来ない)
            Debug.LogError("弾のColliderListにコライダーが追加できませんでした");

            return OUT_OF_RANGE_LIST_NUMBER;
        }


        /// <summary>
        /// 直接的な衝突判定をもつColliderを格納する処理
        /// </summary>
        public static int AddPhysicsColliders(OriginalCollider colliderToAdd)
        {
            // 各コライダーの形状に応じた処理を行う
            switch (colliderToAdd._colliderShape)
            {
                // 球(Sphere)のコライダーとき
                case OriginalCollider._typeOfColliderShape.Sphere:

                    // SphereColliederをListに追加する処理
                    (List<OriginalSphereCollider>, int) addedSphereList = AddSphereCollider(_physicsSphereColliders, colliderToAdd);

                    // SphereCollider追加後のリストを格納
                    _physicsSphereColliders = addedSphereList.Item1;

                    // SphereColliderが格納された要素番号を返す
                    return addedSphereList.Item2;


                // 直方体(Box)のコライダーとき(未実装)
                case OriginalCollider._typeOfColliderShape.Box:

                    // BoxColliederをListに追加し、そのListとColliderの格納番号を取得する
                    (List<OriginalBoxCollider>, int) addedBoxList = AddBoxCollider(_physicsBoxColliders, colliderToAdd);

                    // BoxCollider追加後のリストを格納
                    _physicsBoxColliders = addedBoxList.Item1;

                    // BoxColliderが格納された要素番号を返す
                    return addedBoxList.Item2;
            }

            // 例外の処理(絶対に来ない)
            Debug.LogError("直接的な衝突判定用のColliderListにコライダーが追加できませんでした");

            return OUT_OF_RANGE_LIST_NUMBER;
        }


        /// <summary>
        /// 球コライダーをリストに追加し、そのリストと格納した要素番号を返す
        /// </summary>
        /// <param name="colliderList"> コライダーのリスト </param>
        /// <param name="colliderToAdd"> リストに追加するコライダー </param>
        private static (List<OriginalSphereCollider>, int) AddSphereCollider(List<OriginalSphereCollider> colliderList, OriginalCollider colliderToAdd)
        {
            // SphereColliderのインスタンスを取得する
            OriginalSphereCollider sphereToAdd = colliderToAdd.GetComponent<OriginalSphereCollider>();

            // List内のnullを探索し、その要素番号を取得する
            for (int i = 0; i < colliderList.Count; i++)
            {
                // List内の見つけたnullにColliderを格納する処理
                if (colliderList[i] == null)
                {
                    colliderList[i] = sphereToAdd;

                    // Colliderを格納した要素番号を返す
                    return (colliderList, i);
                }
            }
            // 配列の末尾にColliderを格納する
            colliderList.Add(sphereToAdd);

            // 配列の末尾の番号を返す
            return (colliderList, colliderList.Count - 1);
        }


        /// <summary>
        /// 直方体(Box)コライダーをリストに追加し、そのリストと格納した要素番号を返す
        /// </summary>
        /// <param name="colliderList"> コライダーのリスト </param>
        /// <param name="colliderToAdd"> リストに追加するコライダー </param>
        private static (List<OriginalBoxCollider>, int) AddBoxCollider(List<OriginalBoxCollider> colliderList, OriginalCollider colliderToAdd)
        {
            // BoxColliderのインスタンスを取得する
            OriginalBoxCollider boxToAdd = colliderToAdd.GetComponent<OriginalBoxCollider>();

            // List内のnullを探索し、その要素番号を取得する
            for (int i = 0; i < colliderList.Count; i++)
            {
                // List内の見つけたnullにColliderを格納する処理
                if (colliderList[i] == null)
                {
                    colliderList[i] = boxToAdd;

                    // Colliderを格納した要素番号を返す
                    return (colliderList, i);
                }
            }
            // 配列の末尾にColliderを格納する
            colliderList.Add(boxToAdd);

            // 配列の末尾の番号を返す
            return (colliderList, colliderList.Count - 1);
        }


        #endregion コライダーをリストに追加する処理


        #region コライダーをリストから削除する処理


        /// <summary>
        /// 弾に対する当たり判定をもつColliderをListから消去する処理
        /// </summary>
        public static void RemoveBulletColliders(OriginalCollider removeCollider, int removeColliderIndex)
        {
            // 各コライダーの形状に応じたリストから、コライダーを消去する
            switch (removeCollider._colliderShape)
            {
                // 球(Sphere)のコライダーとき
                case OriginalCollider._typeOfColliderShape.Sphere:

                    // コライダーをListから消去する
                    _bulletSphereColliders[removeColliderIndex] = null;

                    break;

                // 直方体(Box)のコライダーとき
                case OriginalCollider._typeOfColliderShape.Box:

                    // コライダーをListから消去する
                    _bulletBoxColliders[removeColliderIndex] = null;

                    break;
            }
        }


        /// <summary>
        /// 直接的な衝突判定をもつColliderをListから消去する処理
        /// </summary>
        public static void RemovePhysicsColliders(OriginalCollider removeCollider, int removeColliderIndex)
        {
            // 各コライダーの形状に応じたリストから、コライダーを消去する
            switch (removeCollider._colliderShape)
            {
                // 球(Sphere)のコライダーとき
                case OriginalCollider._typeOfColliderShape.Sphere:

                    // コライダーをListから消去する
                    _physicsSphereColliders[removeColliderIndex] = null;

                    break;

                // 直方体(Box)のコライダーとき
                case OriginalCollider._typeOfColliderShape.Box:

                    // コライダーをListから消去する
                    _physicsBoxColliders[removeColliderIndex] = null;

                    break;
            }
        }


        #endregion コライダーをリストから削除する処理


        /// <summary>
        /// コライダーの種類でリストをまとめることができる構造体
        /// </summary>
        public struct ColliderLists
        {
            public List<OriginalSphereCollider> sphereList;
            public List<OriginalBoxCollider> boxList;
        }


        /// <summary>
        /// 弾の当たり判定をもつColliderのList返すメソッド
        /// </summary>
        public static ColliderLists GetBulletColliderList()
        {
            ColliderLists returnList;

            // 各形のコライダーを格納
            returnList.sphereList = _bulletSphereColliders;
            returnList.boxList = _bulletBoxColliders;

            return returnList;
        }


        /// <summary>
        /// 直接的な衝突判定をもつColliderのList返すメソッド
        /// </summary>
        public static ColliderLists GetPhysicsColliderList()
        {
            ColliderLists returnList;

            // 各形のコライダーを格納
            returnList.sphereList = _physicsSphereColliders;
            returnList.boxList = _physicsBoxColliders;

            return returnList;
        }


        /// <summary>
        /// 全てのコライダーのリストの初期化
        /// </summary>
        public static void AllColliderListClear()
        {
            _bulletSphereColliders = new List<OriginalSphereCollider>();
            _physicsSphereColliders = new List<OriginalSphereCollider>();;
            _bulletBoxColliders = new List<OriginalBoxCollider>();
            _physicsBoxColliders = new List<OriginalBoxCollider>();
        }
    }
}