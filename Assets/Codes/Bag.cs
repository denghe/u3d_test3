using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包内的对象基类. 坐标使用相对于 bag 内部空间的本地坐标
/// </summary>
public class BagItem : SpaceItem {
    public Bag bag;

    public const float speed = 10;              // 移动速度( 每帧像素距离 )
    public float tarX, tarY;                    // 目标坐标

    /// <summary>
    /// 返回 -1 表示自杀, 0 表示坐标没变, 1 表示坐标有变化
    /// </summary>
    public int Update() {

        // 计算目标坐标
        float tx, ty;
        if (bag.draggingItem == this) {
            tx = bag.draggingX;
            ty = bag.draggingY;
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

/// <summary>
/// 正方形格子背包
/// </summary>
public class Bag {
    public const int numRows = 10;                          // 行数
    public const int numCols = 10;                          // 列数
    public const float cellSize = 64;                       // 正方形格子边长
    public const float cellSize_2 = cellSize / 2;
    public const float centerX = cellSize * numCols / 2;    // 中心点坐标
    public const float centerY = cellSize * numRows / 2;

    public List<BagItem> items;
    public SpaceContainer spaceContainer;                   // 背包内部空间索引
    public BagItem draggingItem;                            // 鼠标正在拖拽的 item ( 为空就是没有拖拽 )
    public float draggingX, draggingY;                      // 鼠标拖拽时该 item 的 tarX, Y
    public bool lastMBLeftDown;                             // 上次的鼠标左键按下状态

    public void Init() {
        // 初始化各种容器
        items = new();
        spaceContainer = new(numRows, numCols, cellSize);

        // 先随便生成一些 item for test
        for (int rowIndex = 0; rowIndex < 10; rowIndex++) {
            for (int colIndex = 0; colIndex < 10; colIndex++) {
                if (Random.value > 0.5f) {
                    MakeItem(rowIndex, colIndex);
                }
            }
        }
    }

    /// <summary>
    /// 格子下标转为坐标
    /// </summary>
    public void FillPos(int rowIndex, int colIndex, ref float x, ref float y) {
        x = colIndex * cellSize + cellSize_2;
        y = rowIndex * cellSize + cellSize_2;
    }

    /// <summary>
    /// 创建一个 item 并返回
    /// </summary>
    public BagItem MakeItem(int rowIndex, int colIndex) {
        // 创建并放入主容器
        var o = new BagItem();
        o.bag = this;
        items.Add(o);

        // 计算坐标并放入空间容器( 从中心点出现 移动过去? )
        o.x = centerX;
        o.y = centerY;
        FillPos(colIndex, rowIndex, ref o.tarX, ref o.tarY);
        o.spaceContainer = spaceContainer;
        spaceContainer.Add(o);

        return o;
    }

    public void Update() {
        // todo: 响应 mouse 行为. 判断是在 bag 区域内 button left down
        // 进而计算 选中了哪一个 item, 填充 draggingXXXXXXXX ( bag 屏幕坐标 - mouse 屏幕坐标? )

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
