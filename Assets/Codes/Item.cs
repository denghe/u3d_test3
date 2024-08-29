using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 词条配置
/// </summary>
public struct StatConfig {
    public StatTypes type;
    public float valueFrom, valueTo;

    public static StatConfig Make(StatTypes type, float value) {
        return new StatConfig { type = type, valueFrom = value, valueTo = value };
    }
    public static StatConfig Make(StatTypes type, float valueFrom, float valueTo) {
        return new StatConfig { type = type, valueFrom = valueFrom, valueTo = valueTo };
    }
}

/// <summary>
/// 物品配置
/// </summary>
public class ItemConfig {
    public ItemQualities quality;               // 品质
    public ItemTypes type;                      // 佩戴部位
    public int numVariantStats;                 // 可变词条数量( 指生成时要从 avaliableStats 选多少条 )
    public bool allowMultiple;                  // 是否允许堆叠( 数量 > 1 )
    public Sprite res;                          // 指向资源
    public SetsConfig sets;                     // 指向套装套装配置
    public StatConfig[] fixedStats;             // 固定词条列表
    public StatConfig[] avaliableStats;         // 可选词条列表

    // 该函数并不方便填充 avaliableStats
    public static ItemConfig Make(ItemQualities quality, ItemTypes type, Sprite res, int numVariantStats
        , bool allowMultiple, params StatConfig[] fixedStats) {
        return new ItemConfig {
            quality = quality,
            type = type,
            res = res,
            numVariantStats = numVariantStats,
            allowMultiple = allowMultiple,
            fixedStats = fixedStats,
        };
    }

    public void FillAvaliableStats(params StatConfig[] avaliableStats) {
        this.avaliableStats = avaliableStats;
    }
}

/// <summary>
/// 套装配置
/// </summary>
public class SetsConfig {
    public ItemConfig[] items;                  // 套装内有哪些 item
    public StatConfig[] fixedStats;             // 形成套装之后的附加词条列表

    // 该函数并不方便填充 fixedStats
    public static SetsConfig Make(params ItemConfig[] items) {
        return new SetsConfig { items = items };
    }

    public void FillFixedStats(params StatConfig[] fixedStats) {
        this.fixedStats = fixedStats;
    }
}








// todo

/// <summary>
/// 物品词条
/// </summary>
public struct Stat {
    public StatTypes type;
    public float value;
}


public class ItemConfigManager {

    public List<Item> generateResult = new();
    public void Generate() {

    }
}

/// <summary>
/// 物品实例数据
/// </summary>
public class Item {
    public int id;                              // 全局自增 id, 生成时填充
    public double quantity;                     // 数量

    // 词条容器
    Stat[] stats;
}

/// <summary>
/// 掉落到地上的物品( 当从地面拾取时，将转为 BagItem 智能的放入背包 )
/// </summary>
public class FloorItem : Item {
    // todo: 地面坐标 啥的?
}


//[StructLayout(LayoutKind.Explicit)]
//public struct Stat {
//    [FieldOffset(0)]
//    public StatTypes type;

//    [FieldOffset(4)]
//    public float f1;
//    [FieldOffset(8)]
//    public float f2;
//    [FieldOffset(12)]
//    public float f3;

//    [FieldOffset(4)]
//    public int i1;
//    [FieldOffset(8)]
//    public int i2;
//    [FieldOffset(12)]
//    public int i3;

//    [FieldOffset(8)]
//    public double d1;
//    [FieldOffset(8)]
//    public long l1;
//}
