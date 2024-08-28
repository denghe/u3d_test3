using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameObject 对象池
/// </summary>
public struct GO {

    /// <summary>
    /// 存储游戏对象
    /// </summary>
    public GameObject g;

    /// <summary>
    /// 存储精灵渲染器
    /// </summary>
    public SpriteRenderer r;

    /******************************************************************************************/
    /******************************************************************************************/
    // 静态区

    /// <summary>
    /// 对象池
    /// </summary>
    public static Stack<GO> pool;

    /// <summary>
    /// 统一材质
    /// </summary>
    public static Material material;

    /// <summary>
    /// 从对象池拿 GO 并返回. 没有就新建
    /// </summary>
    public static void Pop(ref GO o, int sortingOrder = 0, int layer = 0, string sortingLayerName = "Default") {
#if UNITY_EDITOR
        Debug.Assert(o.g == null);
#endif
        if (!pool.TryPop(out o)) {
            o = New();
        } else {
            o.g.SetActive(true);
            o.r.color = new Color(1f, 1f, 1f, 1f);
        }
        o.g.layer = layer;
        o.r.sortingOrder = sortingOrder;
        o.r.sortingLayerName = sortingLayerName;
    }

    /// <summary>
    /// 将 GO 退回对象池
    /// </summary>
    public static void Push(ref GO o) {
#if UNITY_EDITOR
        Debug.Assert(o.g != null);
#endif
        o.r.material = material;
        o.r.sprite = null;
        var t = o.g.transform;
        t.parent = null;
        t.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        t.localScale = Vector3.one;
        o.g.SetActive(false);
        pool.Push(o);
        o.g = null;
        o.r = null;
    }

    /// <summary>
    /// 新建 GO 并返回( 顺便设置统一的材质球 排序 pivot )
    /// </summary>
    public static GO New() {
        GO o = new();
        o.g = new GameObject();
        o.r = o.g.AddComponent<SpriteRenderer>();
        o.r.material = material;
        o.r.spriteSortPoint = SpriteSortPoint.Pivot;
        return o;
    }

    /// <summary>
    /// 预填充( 可多次调用但参数不能变，方便任意 scene 来初始化 )
    /// </summary>
    public static void Init(Material material, int count) {
        if (GO.material != null) {
            Debug.Assert(GO.material == material);
            Debug.Assert(GO.pool != null);
            return;
        }
        GO.material = material;
        GO.pool = new(count);
        for (int i = 0; i < count; i++) {
            var o = New();
            o.g.SetActive(false);
            pool.Push(o);
        }
    }

    /// <summary>
    /// 释放池资源
    /// </summary>
    public static void Destroy() {
        foreach (var o in pool) {
            GameObject.Destroy(o.g);
        }
        pool.Clear();
    }
}
