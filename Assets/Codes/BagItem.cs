using UnityEngine;

/// <summary>
/// 对象品质，对应 bg 下标
/// </summary>
public enum BagItemQuality {
    Grey, Green, Blue, Purple, Brown, Red
}

public class BagItem {
    public Bag bag;
    public GameObject go;                       // 显示对象
    public float x, y;                          // 当前坐标
    public float tarX, tarY;                    // 要移动到的目标坐标
    public const float tarSpeed = 10;            // 移动速度( 每帧像素距离 )

    BagItemQuality quality;                     // 物品品质
    int id;                                     // 物品 id
    int quantity;                               // 数量
    // ...

    public BagItem(Bag bag_, int id_, int quality_, int rowIndex = -1, int colIndex = -1) {
        Debug.Assert(rowIndex != -1 && colIndex != -1
            || rowIndex == -1 && colIndex == -1);

        bag = bag_;
        var idx = rowIndex * Bag.numCols + colIndex;
        Debug.Assert(bag.items[idx] == null);

        bag.items[idx] = this;

        id = id_;
        quantity = quality_;

        tarX = x = colIndex * Bag.cellSize + Bag.cellSize_2;
        tarY = y = rowIndex * Bag.cellSize + Bag.cellSize_2;

        InitGameObject();
    }

    public bool Update() {

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
        go.transform.position = new Vector3(x, y);
    }

    public void InitGameObject() {
        go = new GameObject();
        // todo: add bg sprite
        // todo: add item sprite
    }

    public void Show() {
        go.transform.parent = bag.go.transform;
        go.SetActive(true);
    }

    public void Hide() {
        go.transform.parent = null;
        go.SetActive(false);
    }

    public void Destroy() {
        GameObject.Destroy(go);
    }

}
