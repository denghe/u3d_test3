/// <summary>
/// 品质，对应 Res.sprites_bg 下标
/// </summary>
public enum ItemQualities {
    Grey, Green, Blue, Purple, Brown, Red
}

/// <summary>
/// 佩戴部位分类
/// </summary>
public enum ItemTypes {
    Hat, Amulet, Ring, Arm, Armor, Belt, Pants, Glove, Boots, Weapon1, Weapon2, Relic, Material
    // ...
}

/// <summary>
/// 物品基础配置
/// </summary>
public class ItemConfig {
    public ItemQualities cQuality;               // 品质
    public ItemTypes cType;                      // 佩戴部位
    public int cResId;                           // 资源 id
    public bool cAllowMultiple;                  // 是否允许堆叠
    // ...
}

/// <summary>
/// 物品实例数据
/// </summary>
public class Item : ItemConfig {
    // todo: 自增 id ?
    public double quantity;                     // 数量( todo: 显示为文字? )

    // 词条容器?
    // ...
}

/// <summary>
/// 掉落到地上的物品( 当从地面拾取时，将转为 BagItem 智能的放入背包 )
/// </summary>
public class FloorItem : Item {
    // todo: 地面坐标 啥的?
}
