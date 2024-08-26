using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Bag {

    public const int numRows = 7;                           // 行数
    public const int numCols = 10;                          // 列数
    public const int numMaxItems = numRows * numCols;       // 最大数量
    public const float cellSize = 132;                      // 正方形格子边长( 和显示背景尺寸同步 )
    public const float cellSize_2 = cellSize / 2;
    public const float gridWidth = cellSize * numCols;
    public const float gridWidth_2 = gridWidth / 2;
    public const float gridHeight = cellSize * numRows;
    public const float gridHeight_2 = gridHeight / 2;

    public GameObject go;                                   // 指向背包显示区域背景 go
    public float posX, posY;                                // 存储显示区域左上角坐标( 世界坐标 )

    public BagItem[] items;
    public BagItem draggingItem;                            // 鼠标正在拖拽的 item ( 为空就是没有拖拽 )
    public float draggingX, draggingY;                      // 鼠标拖拽时该 item 的 tarX, Y
    public bool lastMBLeftDown;                             // 上次的鼠标左键按下状态

    public Bag() {
        items = new BagItem[numMaxItems];

        // 先随便生成一些 item for test
        for (int rowIndex = 0; rowIndex < 10; rowIndex++) {
            for (int colIndex = 0; colIndex < 10; colIndex++) {
                if (Random.value > 0.5f) {
                    new BagItem(this, rowIndex, colIndex);
                }
            }
        }
    }

    public void Show(GameObject goBag_) {
        Debug.Assert(go == null);
        go = goBag_;
        posX = go.transform.position.x;
        posY = go.transform.position.y + gridHeight;
        for (int i = 0, e = items.Length; i < e; i++) {
            items[i]?.Show();
        }
    }

    public void Hide() {
        Debug.Assert(go != null);
        go = null;
        for (int i = 0, e = items.Length; i < e; i++) {
            items[i]?.Hide();
        }
    }

    public void Update() {
        // todo: 响应 mouse 行为. 判断是在 bag 区域内 button left down
        // 进而计算 选中了哪一个 item, 填充 draggingXXXXXXXX ( bag 屏幕坐标 - mouse 屏幕坐标? )

        for (int i = 0, e = items.Length; i < e; i++) {
            var item = items[i];
            if (item.Update()) {
                item.Destroy();
                items[i] = null;
            }
        }
    }

    public void Draw() {
        Debug.Assert(go != null);
        for (int i = 0, e = items.Length; i < e; i++) {
            items[i]?.Draw();
        }
    }

}
