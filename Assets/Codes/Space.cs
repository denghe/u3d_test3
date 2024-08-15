using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 要放入 空间索引 容器的对象基类
/// </summary>
public class SpaceItem {
    public SpaceContainer spaceContainer;
    public SpaceItem spacePrev, spaceNext;
    public int spaceIndex = -1;
    public float x, y;
    public float radius;
}

/// <summary>
/// 空间索引容器
/// </summary>
public class SpaceContainer {
    public int numRows, numCols;                // size info
    public float cellSize, _1_cellSize;         // = 1 / cellSize
    public float maxY, maxX;                    // edge position
    public int numItems;                        // for state
    public SpaceItem[] cells;                   // grid container( numRows * numCols )


    /// <summary>
    /// 初始化格子规模，传入  行数， 列数， 每一格的 边长（正方形）
    /// </summary>
    public SpaceContainer(int numRows_, int numCols_, float cellSize_) {
#if UNITY_EDITOR
        Debug.Assert(numRows_ > 0);
        Debug.Assert(numCols_ > 0);
        Debug.Assert(cellSize_ > 0);
#endif
        numRows = numRows_;
        numCols = numCols_;
        cellSize = cellSize_;
        _1_cellSize = 1f / cellSize_;
        maxY = cellSize * numRows;
        maxX = cellSize * numCols;
        if (cells == null) {
            cells = new SpaceItem[numRows * numCols];
        } else {
            Array.Fill(cells, null);
            Array.Resize(ref cells, numRows * numCols);
        }
    }


    /// <summary>
    /// 将对象放入容器
    /// </summary>
    public void Add(SpaceItem c) {
#if UNITY_EDITOR
        Debug.Assert(c != null);
        Debug.Assert(c.spaceContainer == this);
        Debug.Assert(c.spaceIndex == -1);
        Debug.Assert(c.spacePrev == null);
        Debug.Assert(c.spaceNext == null);
        Debug.Assert(c.x >= 0 && c.x < maxX);
        Debug.Assert(c.y >= 0 && c.y < maxY);
#endif

        // calc rIdx & cIdx
        var idx = PosToIndex(c.x, c.y);
#if UNITY_EDITOR
        Debug.Assert(cells[idx] == null || cells[idx].spacePrev == null);
#endif

        // link
        if (cells[idx] != null) {
            cells[idx].spacePrev = c;
        }
        c.spaceNext = cells[idx];
        c.spaceIndex = idx;
        cells[idx] = c;
#if UNITY_EDITOR
        Debug.Assert(cells[idx].spacePrev == null);
        Debug.Assert(c.spaceNext != c);
        Debug.Assert(c.spacePrev != c);
#endif

        // stat
        ++numItems;
    }


    /// <summary>
    /// 将对象从容器移除
    /// </summary>
    public void Remove(SpaceItem c) {
#if UNITY_EDITOR
        Debug.Assert(c != null);
        Debug.Assert(c.spaceContainer == this);
        Debug.Assert(c.spacePrev == null && cells[c.spaceIndex] == c || c.spacePrev.spaceNext == c && cells[c.spaceIndex] != c);
        Debug.Assert(c.spaceNext == null || c.spaceNext.spacePrev == c);
        //Debug.Assert(cells[c.spaceIndex] include c);
#endif

        // unlink
        if (c.spacePrev != null) {  // isn't header
#if UNITY_EDITOR
            Debug.Assert(cells[c.spaceIndex] != c);
#endif
            c.spacePrev.spaceNext = c.spaceNext;
            if (c.spaceNext != null) {
                c.spaceNext.spacePrev = c.spacePrev;
                c.spaceNext = null;
            }
            c.spacePrev = null;
        } else {
#if UNITY_EDITOR
            Debug.Assert(cells[c.spaceIndex] == c);
#endif
            cells[c.spaceIndex] = c.spaceNext;
            if (c.spaceNext != null) {
                c.spaceNext.spacePrev = null;
                c.spaceNext = null;
            }
        }
#if UNITY_EDITOR
        Debug.Assert(cells[c.spaceIndex] != c);
#endif
        c.spaceIndex = -1;
        c.spaceContainer = null;

        // stat
        --numItems;
    }


    /// <summary>
    /// 当对象的 pos 数据发生改变后，需要调用这个函数来同步 自己在容器中的索引
    /// </summary>
    public void Update(SpaceItem c) {
#if UNITY_EDITOR
        Debug.Assert(c != null);
        Debug.Assert(c.spaceContainer == this);
        Debug.Assert(c.spaceIndex > -1);
        Debug.Assert(c.spaceNext != c);
        Debug.Assert(c.spacePrev != c);
        //Debug.Assert(cells[c.spaceIndex] include c);
#endif

        var x = c.x;
        var y = c.y;
#if UNITY_EDITOR
        Debug.Assert(x >= 0 && x < maxX);
        Debug.Assert(y >= 0 && y < maxY);
#endif
        int cIdx = (int)(x * _1_cellSize);
        int rIdx = (int)(y * _1_cellSize);
        int idx = rIdx * numCols + cIdx;
#if UNITY_EDITOR
        Debug.Assert(idx <= cells.Length);
#endif

        if (idx == c.spaceIndex) return;  // no change

        // unlink
        if (c.spacePrev != null) {  // isn't header
#if UNITY_EDITOR
            Debug.Assert(cells[c.spaceIndex] != c);
#endif
            c.spacePrev.spaceNext = c.spaceNext;
            if (c.spaceNext != null) {
                c.spaceNext.spacePrev = c.spacePrev;
                //c.spaceNext = {};
            }
            //c.spacePrev = {};
        } else {
#if UNITY_EDITOR
            Debug.Assert(cells[c.spaceIndex] == c);
#endif
            cells[c.spaceIndex] = c.spaceNext;
            if (c.spaceNext != null) {
                c.spaceNext.spacePrev = null;
                //c.spaceNext = {};
            }
        }
        //c.spaceIndex = -1;
#if UNITY_EDITOR
        Debug.Assert(cells[c.spaceIndex] != c);
        Debug.Assert(idx != c.spaceIndex);
#endif

        // link
        if (cells[idx] != null) {
            cells[idx].spacePrev = c;
        }
        c.spacePrev = null;
        c.spaceNext = cells[idx];
        cells[idx] = c;
        c.spaceIndex = idx;
#if UNITY_EDITOR
        Debug.Assert(cells[idx].spacePrev == null);
        Debug.Assert(c.spaceNext != c);
        Debug.Assert(c.spacePrev != c);
#endif
    }

    // return cells index
    public int PosToIndex(float x, float y) {
#if UNITY_EDITOR
        Debug.Assert(x >= 0 && x < maxX);
        Debug.Assert(y >= 0 && y < maxY);
#endif
        int cIdx = (int)(x * _1_cellSize);
        int rIdx = (int)(y * _1_cellSize);
        int idx = rIdx * numCols + cIdx;
#if UNITY_EDITOR
        Debug.Assert(idx <= cells.Length);
#endif
        return idx;
    }



    /// <summary>
    /// 在 9 宫内找出 第1个 相交物 并返回
    /// </summary>
    public SpaceItem FindFirstCrossBy9(float x, float y, float radius) {
        // 5
        int cIdx = (int)(x * _1_cellSize);
        if (cIdx < 0 || cIdx >= numCols) return null;
        int rIdx = (int)(y * _1_cellSize);
        if (rIdx < 0 || rIdx >= numRows) return null;
        int idx = rIdx * numCols + cIdx;
        var c = cells[idx];
        while (c != null) {
            var vx = c.x - x;
            var vy = c.y - y;
            var r = c.radius + radius;
            if (vx * vx + vy * vy < r * r) {
                return c;
            }
            c = c.spaceNext;
        }
        // 6
        ++cIdx;
        if (cIdx >= numCols) return null;
        ++idx;
        c = cells[idx];
        while (c != null) {
            var vx = c.x - x;
            var vy = c.y - y;
            var r = c.radius + radius;
            if (vx * vx + vy * vy < r * r) {
                return c;
            }
            c = c.spaceNext;
        }
        // 3
        ++rIdx;
        if (rIdx >= numRows) return null;
        idx += numCols;
        c = cells[idx];
        while (c != null) {
            var vx = c.x - x;
            var vy = c.y - y;
            var r = c.radius + radius;
            if (vx * vx + vy * vy < r * r) {
                return c;
            }
            c = c.spaceNext;
        }
        // 2
        --idx;
        c = cells[idx];
        while (c != null) {
            var vx = c.x - x;
            var vy = c.y - y;
            var r = c.radius + radius;
            if (vx * vx + vy * vy < r * r) {
                return c;
            }
            c = c.spaceNext;
        }
        // 1
        cIdx -= 2;
        if (cIdx < 0) return null;
        --idx;
        c = cells[idx];
        while (c != null) {
            var vx = c.x - x;
            var vy = c.y - y;
            var r = c.radius + radius;
            if (vx * vx + vy * vy < r * r) {
                return c;
            }
            c = c.spaceNext;
        }
        // 4
        idx -= numCols;
        c = cells[idx];
        while (c != null) {
            var vx = c.x - x;
            var vy = c.y - y;
            var r = c.radius + radius;
            if (vx * vx + vy * vy < r * r) {
                return c;
            }
            c = c.spaceNext;
        }
        // 7
        rIdx -= 2;
        if (rIdx < 0) return null;
        idx -= numCols;
        c = cells[idx];
        while (c != null) {
            var vx = c.x - x;
            var vy = c.y - y;
            var r = c.radius + radius;
            if (vx * vx + vy * vy < r * r) {
                return c;
            }
            c = c.spaceNext;
        }
        // 8
        ++idx;
        c = cells[idx];
        while (c != null) {
            var vx = c.x - x;
            var vy = c.y - y;
            var r = c.radius + radius;
            if (vx * vx + vy * vy < r * r) {
                return c;
            }
            c = c.spaceNext;
        }
        // 9
        ++idx;
        c = cells[idx];
        while (c != null) {
            var vx = c.x - x;
            var vy = c.y - y;
            var r = c.radius + radius;
            if (vx * vx + vy * vy < r * r) {
                return c;
            }
            c = c.spaceNext;
        }
        return null;
    }

    /// <summary>
    /// 遍历坐标所在格子 + 周围  九宫. handler 返回 true 结束遍历( Func 可能产生 gc, 但这种应该是无所谓的, 里面只要不含 unity 资源 )
    /// </summary>
    public void Foreach9All(float x, float y, Func<SpaceItem, bool> handler) {
        // 5
        int cIdx = (int)(x * _1_cellSize);
        if (cIdx < 0 || cIdx >= numCols) return;
        int rIdx = (int)(y * _1_cellSize);
        if (rIdx < 0 || rIdx >= numRows) return;
        int idx = rIdx * numCols + cIdx;
        var c = cells[idx];
        while (c != null) {
            var next = c.spaceNext;
            if (handler(c)) return;
            c = next;
        }
        // 6
        ++cIdx;
        if (cIdx >= numCols) return;
        ++idx;
        c = cells[idx];
        while (c != null) {
            var next = c.spaceNext;
            if (handler(c)) return;
            c = next;
        }
        // 3
        ++rIdx;
        if (rIdx >= numRows) return;
        idx += numCols;
        c = cells[idx];
        while (c != null) {
            var next = c.spaceNext;
            if (handler(c)) return;
            c = next;
        }
        // 2
        --idx;
        c = cells[idx];
        while (c != null) {
            var next = c.spaceNext;
            if (handler(c)) return;
            c = next;
        }
        // 1
        cIdx -= 2;
        if (cIdx < 0) return;
        --idx;
        c = cells[idx];
        while (c != null) {
            var next = c.spaceNext;
            if (handler(c)) return;
            c = next;
        }
        // 4
        idx -= numCols;
        c = cells[idx];
        while (c != null) {
            var next = c.spaceNext;
            if (handler(c)) return;
            c = next;
        }
        // 7
        rIdx -= 2;
        if (rIdx < 0) return;
        idx -= numCols;
        c = cells[idx];
        while (c != null) {
            var next = c.spaceNext;
            if (handler(c)) return;
            c = next;
        }
        // 8
        ++idx;
        c = cells[idx];
        while (c != null) {
            var next = c.spaceNext;
            if (handler(c)) return;
            c = next;
        }
        // 9
        ++idx;
        c = cells[idx];
        while (c != null) {
            var next = c.spaceNext;
            if (handler(c)) return;
            c = next;
        }
    }


    /// <summary>
    /// 圆形扩散遍历找出 边距最近的 1 个并返回
    /// </summary>
    public SpaceItem FindNearestByRange(SpaceRingDiffuseData d, float x, float y, float maxDistance) {
        int cIdxBase = (int)(x * _1_cellSize);
        if (cIdxBase < 0 || cIdxBase >= numCols) return null;
        int rIdxBase = (int)(y * _1_cellSize);
        if (rIdxBase < 0 || rIdxBase >= numRows) return null;
        var searchRange = maxDistance + cellSize;

        SpaceItem rtv = null;
        float maxV = 0;

        var lens = d.lens;
        var idxs = d.idxs;
        for (int i = 1; i < lens.Count; i++) {
            var offsets = lens[i - 1].count;
            var size = lens[i].count - lens[i - 1].count;
            for (int j = 0; j < size; ++j) {
                var tmp = idxs[offsets + j];
                var cIdx = cIdxBase + tmp.x;
                if (cIdx < 0 || cIdx >= numCols) continue;
                var rIdx = rIdxBase + tmp.y;
                if (rIdx < 0 || rIdx >= numRows) continue;
                var cidx = rIdx * numCols + cIdx;

                var c = cells[cidx];
                while (c != null) {
                    var vx = c.x - x;
                    var vy = c.y - y;
                    var dd = vx * vx + vy * vy;
                    var r = maxDistance + c.radius;
                    var v = r * r - dd;


                    if (v > maxV) {
                        rtv = c;
                        maxV = v;
                    }
                    c = c.spaceNext;
                }
            }
            if (lens[i].radius > searchRange) break;
        }
        return rtv;
    }


    /// <summary>
    /// 圆形扩散遍历 找出范围内 ??? 最多 n 个 的结果容器
    /// </summary>
    public List<DistanceSpaceItem> result_FindNearestN = new();

    /// <summary>
    /// 圆形扩散遍历 找出范围内 边缘最近的 最多 n 个, 返回实际个数。searchRange 决定了要扫多远的格子. maxDistance 限制了结果集最大边距
    /// </summary>
    public int FindNearestNByRange(SpaceRingDiffuseData d, float x, float y, float maxDistance, int n) {
        int cIdxBase = (int)(x * _1_cellSize);
        if (cIdxBase < 0 || cIdxBase >= numCols) return 0;
        int rIdxBase = (int)(y * _1_cellSize);
        if (rIdxBase < 0 || rIdxBase >= numRows) return 0;
        var searchRange = maxDistance + cellSize;

        var os = result_FindNearestN;
        os.Clear();

        var lens = d.lens;
        var idxs = d.idxs;
        for (int i = 1; i < lens.Count; i++) {
            var offsets = lens[i - 1].count;
            var size = lens[i].count - lens[i - 1].count;
            for (int j = 0; j < size; ++j) {
                var tmp = idxs[offsets + j];
                var cIdx = cIdxBase + tmp.x;
                if (cIdx < 0 || cIdx >= numCols) continue;
                var rIdx = rIdxBase + tmp.y;
                if (rIdx < 0 || rIdx >= numRows) continue;
                var cidx = rIdx * numCols + cIdx;

                var c = cells[cidx];
                while (c != null) {
                    var vx = c.x - x;
                    var vy = c.y - y;
                    var dd = vx * vx + vy * vy;
                    var r = maxDistance + c.radius;
                    var v = r * r - dd;

                    if (v > 0) {
                        if (os.Count < n) {
                            os.Add(new DistanceSpaceItem { distance = v, item = c });
                            if (os.Count == n) {
                                Quick_Sort(0, os.Count - 1);
                            }
                        } else {
                            if (os[0].distance < v) {
                                os[0] = new DistanceSpaceItem { distance = v, item = c };
                                Quick_Sort(0, os.Count - 1);
                            }
                        }
                    }

                    c = c.spaceNext;
                }
            }
            if (lens[i].radius > searchRange) break;
        }
        return os.Count;
    }

    // 内部函数。排序 以取代 .net Sort 函数( 会造成 128 byte gc )
    private void Quick_Sort(int left, int right) {
        if (left < right) {
            int pivot = Partition(left, right);
            if (pivot > 1) {
                Quick_Sort(left, pivot - 1);
            }
            if (pivot + 1 < right) {
                Quick_Sort(pivot + 1, right);
            }
        }
    }
    // 内部函数
    private int Partition(int left, int right) {
        var arr = result_FindNearestN;
        var pivot = arr[left];
        while (true) {
            while (arr[left].distance < pivot.distance) {
                left++;
            }
            while (arr[right].distance > pivot.distance) {
                right--;
            }
            if (left < right) {
                if (arr[left].distance == arr[right].distance) return right;
                var temp = arr[left];
                arr[left] = arr[right];
                arr[right] = temp;
            } else return right;
        }
    }

}


public struct SpaceCountRadius {
    public int count;
    public float radius;
};

public struct SpaceXYi {
    public int x, y;
}

public struct DistanceSpaceItem : IComparable<DistanceSpaceItem> {
    public float distance;
    public SpaceItem item;

    public int CompareTo(DistanceSpaceItem o) {
        return this.distance.CompareTo(o.distance);
    }
}

/// <summary>
/// 填充 圆形扩散的 格子偏移量 数组. 主用于 更高效的范围内找最近
/// </summary>
public class SpaceRingDiffuseData {
    public List<SpaceCountRadius> lens = new();
    public List<SpaceXYi> idxs = new();

    public SpaceRingDiffuseData(int gridNumRows, int cellSize) {
        lens.Add(new SpaceCountRadius { count = 0, radius = 0f });
        idxs.Add(new SpaceXYi());
        HashSet<ulong> set = new();
        set.Add(0);
        for (float radius = 0; radius < cellSize * gridNumRows; radius += cellSize) {
            var lenBak = idxs.Count;
            var radians = Mathf.Asin(0.5f / radius) * 2;
            var step = (int)(Mathf.PI * 2 / radians);
            var inc = Mathf.PI * 2 / step;
            for (int i = 0; i < step; ++i) {
                var a = inc * i;
                var cos = Mathf.Cos(a);
                var sin = Mathf.Sin(a);
                var ix = (int)(cos * radius) / cellSize;
                var iy = (int)(sin * radius) / cellSize;
                var key = ((ulong)iy << 32) + (ulong)ix;
                if (set.Add(key)) {
                    idxs.Add(new SpaceXYi { x = ix, y = iy });
                }
            }
            if (idxs.Count > lenBak) {
                lens.Add(new SpaceCountRadius { count = idxs.Count, radius = radius });
            }
        }
    }

}

