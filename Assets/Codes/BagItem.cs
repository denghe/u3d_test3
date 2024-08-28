using UnityEngine;

/// <summary>
/// 对象品质，对应 bg 下标
/// </summary>
public enum BagItemQuality {
    Grey, Green, Blue, Purple, Brown, Red
}

public class BagItem {
    public Bag bag;
    public GO goBG, goItem, goShadow;                   // 显示对象
    public float x, y;                                  // 当前坐标( world )
    public float tarX, tarY;                            // 要移动到的目标坐标( world )
    public const float tarSpeed = 80 * 60 / Env.FPS;    // 移动速度( 每帧像素距离 )

    public BagItemQuality quality;                      // 物品品质
    public int id;                                      // 物品 id
    public double quantity;                             // 数量( todo: 显示为文字? )
    // ...

    public BagItem(Bag bag_, int id_, BagItemQuality quality_, double quantity_ = 1, int rowIndex = -1, int colIndex = -1) {
        Debug.Assert(rowIndex != -1 && colIndex != -1
            || rowIndex == -1 && colIndex == -1);

        bag = bag_;
        var idx = rowIndex * bag.numCols + colIndex;
        Debug.Assert(bag.items[idx] == null);

        bag.items[idx] = this;

        id = id_;
        quality = quality_;
        quantity = quantity_;

        SetTarXY(rowIndex, colIndex);
        x = bag.posX + bag.cellSize_2;
        y = bag.posY - bag.cellSize_2;

        GO.Pop(ref goBG);
        GO.Pop(ref goItem);
        GO.Pop(ref goShadow);
        goBG.r.sprite = Res.sprites_bg[(int)quality];
        goItem.r.sprite = Res.sprites_item[id];
        goShadow.r.sprite = Res.sprites_item[id];
        goShadow.r.color = new Color(0, 0, 0, 127);
        goShadow.g.transform.position = new Vector3(6, -5);
        goItem.g.transform.parent = goBG.g.transform;
        goShadow.g.transform.parent = goBG.g.transform;
    }

    public void SetTarXY(int rowIndex, int colIndex) {
        tarX = bag.posX + colIndex * bag.cellSize + bag.cellSize_2;
        tarY = bag.posY - rowIndex * bag.cellSize - bag.cellSize_2;
    }
    public void SetTarXY(int itemIndex) {
        var rowIndex = itemIndex / bag.numCols;
        var colIndex = itemIndex - rowIndex * bag.numCols;
        SetTarXY(rowIndex, colIndex);
    }

    public bool Update() {

        // 计算目标坐标
        float tx, ty;
        if (bag.dragging && bag.selectedItem == this) {
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
            if (mag2 <= tarSpeed * tarSpeed) {
                x = tx;
                y = ty;
            } else {
                var ss = 1f / Mathf.Sqrt(mag2) * tarSpeed;
                x += dx * ss;
                y += dy * ss;
            }
            return false;
        }

        // todo: another logic  

        return false;
    }

    public void Draw() {
        goBG.g.transform.position = new Vector3(x, y);
        if (bag.dragging && bag.selectedItem == this) {
            if (goBG.r.sortingOrder != 4) {
                goBG.r.sortingOrder = 4;
                goItem.r.sortingOrder = 6;
                goShadow.r.sortingOrder = 5;
            }
        } else {
            if (goBG.r.sortingOrder != 1) {
                goBG.r.sortingOrder = 1;
                goItem.r.sortingOrder = 3;
                goShadow.r.sortingOrder = 2;
            }
        }
    }

    public void Destroy() {
        GO.Push(ref goShadow);
        GO.Push(ref goItem);
        GO.Push(ref goBG);
    }

    public void Show() {
        goBG.g.SetActive(true);
    }

    public void Hide() {
        goBG.g.SetActive(false);
    }
}
