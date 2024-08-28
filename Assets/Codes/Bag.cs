using System;
using UnityEngine;

/// <summary>
/// 背包类型. 通常分为 已装备的( 角色身上的 ) 和 库存
/// </summary>
public enum BagTypes {
    Char, Inventory
}

// 固定大小的包
public class Bag {

    // init 时填充
    public BagTypes type;                                   // 背包类型
    public Bag neighbor;                                    // 指向邻居背包( 可相互拖拽 )  // todo: 可能得是个数组
    public int numRows;                                     // 行数
    public int numCols;                                     // 列数
    public int numMaxItems;                                 // 最大数量
    public float cellSize;                                  // 正方形格子边长( 和显示背景尺寸同步 )
    public float cellSize_2;
    public float gridWidth;                                 // 整个表格的宽度
    public float gridWidth_2;
    public float gridHeight;                                // 整个表格的高度
    public float gridHeight_2;
    public BagItem[] items;                                 // item 数据容器。下标和坐标有对应关系
    public bool[] masks;                                    // 和 items 下标对应，用于标记哪些格子 无效(true)
    public float posX, posY;                                // 存储显示区域左上角坐标( 世界坐标 )

    // mouse down & drag 时填充
    public BagItem selectedItem;                            // mouse down 时对应的格子里面的 item
    public bool dragging;                                   // 是否正在拖拽
    public float draggingX, draggingY;                      // 鼠标拖拽时该 item 的 tarX, Y
    public bool lastMBLeftDown;                             // 上次的鼠标左键按下状态
    public float lastMouseX, lastMouseY;                    // 鼠标拖拽时 mouse 的 X, Y
    public float lastMBLeftDownTime;                        // 上次的鼠标左键按下状态变化的时间点( 秒 )
    public int lastItemIndex;                               // 上次的鼠标左键按下时所在格子下标

    public void Init(BagTypes type_, Bag neighbor_, int numRows_, int numCols_, float cellSize_, float goX, float goY) {
        Debug.Assert(items == null);
        type = type_;
        neighbor = neighbor_;
        numRows = numRows_;
        numCols = numCols_;
        numMaxItems = numRows * numCols_;
        cellSize = cellSize_;
        cellSize_2 = cellSize / 2;
        gridWidth = cellSize * numCols;
        gridWidth_2 = gridWidth / 2;
        gridHeight = cellSize * numRows;
        gridHeight_2 = gridHeight / 2;
        items = new BagItem[numMaxItems];
        masks = new bool[numMaxItems];

        posX = goX - gridWidth_2;
        posY = goY + gridHeight_2;
        Debug.Log(posX + " " + posY);
    }

    public void Update() {
        // 拿到鼠标位于 camera 的坐标
        var mp = Inputs.mousePositionInCamera;
        // 鼠标按钮处于压下状态
        if (Inputs.mouseButtonLeftDown) {
            // 和上次记录的鼠标状态有差异. 是否产生 mouse down 事件呢? 先判断有没有点到东西
            if (!lastMBLeftDown) {
                // 计算鼠标在表格中的逻辑坐标
                var x = mp.x - posX;
                var y = posY - mp.y;        // y 坐标是上大下小
                // 范围合法性判断( 范围外则认为不该产生 mouse down 事件 )
                if (x >= 0 && y >= 0 && x < gridWidth && y < gridHeight) {
                    // 将坐标转为 行列号，数组下标
                    var colIndex = (int)x / (int)cellSize;
                    var rowIndex = (int)y / (int)cellSize;
                    var itemIndex = colIndex + rowIndex * numCols;
                    // 如果对应的格子有效，不空, 则 mouse down 事件成立, 记录上下文
                    if (!masks[itemIndex]) {
                        var item = items[itemIndex];
                        if (item != null) {
                            selectedItem = item;
                            lastMBLeftDown = true;
                            lastMouseX = mp.x;
                            lastMouseY = mp.y;
                            lastMBLeftDownTime = Env.time;
                            lastItemIndex = itemIndex;
                        }
                    }
                }
            }
            // 无差异. 是否产生 mouse drag 事件呢? 先判断 mouse down 事件是否成立，且鼠标坐标是否变化
            else if (selectedItem != null) {
                // 虽然坐标没变，但如果 mouse down 的时间超过 0.2 秒? 还是认为想拖拽
                if (mp.x != lastMouseX || mp.y != lastMouseY || Env.time - lastMBLeftDownTime > 0.2) {
                    dragging = true;
                    draggingX = mp.x;
                    draggingY = mp.y;
                }
            }
            lastMBLeftDown = true;
        } else {    // mouse up 事件判定
            // 如果正在拖拽，那就要判断目标格子是原先的格子还是 新的格子，新格子是空的就移动，有东西就交换
            if (dragging) {
                // 计算鼠标在表格中的逻辑坐标
                var x = mp.x - posX;
                var y = posY - mp.y;        // y 坐标是上大下小
                // 范围合法性判断( 范围外则认为取消拖拽 )
                if (x >= 0 && y >= 0 && x < gridWidth && y < gridHeight) {
                    // 将坐标转为 行列号，数组下标
                    var colIndex = (int)x / (int)cellSize;
                    var rowIndex = (int)y / (int)cellSize;
                    var itemIndex = colIndex + rowIndex * numCols;
                    if (!masks[itemIndex]) {
                        var item = items[itemIndex];
                        // 不空, 且不是自身: 交换逻辑
                        if (item != null) {
                            if (item != selectedItem) {
                                item.SetTarXY(lastItemIndex);
                                selectedItem.SetTarXY(itemIndex);
                                items[itemIndex] = selectedItem;
                                items[lastItemIndex] = item;
                            }
                            // 空：挪
                        } else {
                            selectedItem.SetTarXY(itemIndex);
                            items[itemIndex] = selectedItem;
                            items[lastItemIndex] = null;
                        }
                    }
                } else {
                    // todo: 判断是否进入了邻居背包的范围, 根据某些规则与其交换
                }
            }

            // clean up
            selectedItem = null;
            lastMBLeftDown = false;
            dragging = false;
        }

        if (selectedItem == null) {
            // todo: 如果没有 mouse down 此时应该根据鼠标指到的 item, 显示个 pop up info panel

        }


        for (int i = 0, e = items.Length; i < e; i++) {
            var item = items[i];
            if (item != null && item.Update()) {
                item.Destroy();
                items[i] = null;
            }
        }
    }

    public void Draw() {
        for (int i = 0, e = items.Length; i < e; i++) {
            items[i]?.Draw();
        }
    }

    public void Destroy() {
        for (int i = 0, e = items.Length; i < e; i++) {
            if (items[i] != null) {
                items[i].Destroy();
                items[i] = null;
            }
        }
    }

    public void Show() {
        for (int i = 0, e = items.Length; i < e; i++) {
            items[i]?.Show();
        }
    }

    public void Hide() {
        for (int i = 0, e = items.Length; i < e; i++) {
            items[i]?.Hide();
        }
    }

    public void Sort() {
        Array.Sort(items, static (a, b) => {
            if (a == null && b == null) return 0;
            else if (a == null) return 1;
            else if (b == null) return -1;
            else if (a.quality < b.quality) return -1;
            else if (a.quality > b.quality) return 1;
            else return a.id.CompareTo(b.id);
        });

        for (int rowIndex = 0; rowIndex < numRows; rowIndex++) {
            for (int colIndex = 0; colIndex < numCols; colIndex++) {
                var itemIndex = colIndex + rowIndex * numCols;
                items[itemIndex]?.SetTarXY(rowIndex, colIndex);
            }
        }
    }

    public void Clear() {
        Destroy();
    }


    public void SetMasks(params int[] idxs) {
        foreach (int idx in idxs) {
            masks[idx] = true;
        }
    }

    // todo: add remove
}
