using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包内的对象基类. 坐标使用相对于 bag 内部空间的本地坐标
/// </summary>
public class BagItem : SpaceItem {
    public Bag bag;

    public const float speed = 10;              // 移动速度( 每帧像素距离 )
    public float tarX, tarY;                    // 目标坐标
    public bool dragging;

    public void Init() {
        // todo
    }

    /// <summary>
    /// 返回 -1 表示自杀, 0 表示坐标没变, 1 表示坐标有变化
    /// </summary>
    public int Update() {

        // 计算目标坐标
        float tx, ty;
        if (dragging) {
            // todo: 响应鼠标 drag 逻辑? 算出 tx, ty ( bag 屏幕坐标 - mouse 屏幕坐标? )
            tx = 0;
            ty = 0;
        } else {
            tx = tarX;
            ty = tarY;
        }

        // 往目标坐标移动?
        if (x != tx || y != ty) {
            var dx = tx - x;
            var dy = ty - y;
            var mag2 = dx * dx + dy * dy;
            if (mag2 <= speed * speed) {
                x = tx;
                y = ty;
            } else {
                var ss = 1f / Mathf.Sqrt(mag2) * speed;
                x += dx * ss;
                y += dy * ss;
            }
            return 1;   // moved
        }

        // todo: 别的逻辑

        return 0;   // no move
    }
}

public class Bag {
    public List<BagItem> items;
    public SpaceContainer spaceContainer;       // 背包内部空间索引

    public void Init() {
        items = new();
        spaceContainer = new(10, 10, 64);

        // todo: 先随便生成点 item for test
    }

    public void MakeItem() {
        var o = new BagItem();
        o.bag = this;
        o.spaceContainer = spaceContainer;
        o.Init();
        spaceContainer.Add(o);
    }

    public void Update() {
        // todo: 响应 mouse 行为. 判断是在 bag 区域内 button left down
        // 进而计算 选中了哪一个 item, 标其 dragging 为 true

        for (int i = items.Count - 1; i >= 0; i--) {
            var item = items[i];
            var rtv = item.Update();
            if (rtv == -1) {
                var lastIndex = items.Count - 1;
                items[i] = items[lastIndex];
                items.RemoveAt(lastIndex);
                spaceContainer.Remove(item);
            } else if (rtv == 1) {
                spaceContainer.Update(item);
            }
        }
    }

    public void Draw() {
        // todo
    }
}
