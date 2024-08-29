
/// <summary>
/// 背包格子( 逻辑概念，并非真正容器. 用来做筛选 )
/// </summary>
public class BagCell {
    public Bag bag;

    /// <summary>
    /// 是否允许将目标放置到本格
    /// </summary>
    public virtual bool Avaliable(BagItem item) {
        return true;
    }
}

/// <summary>
/// 啥都不能放置
/// </summary>
public class BagCell_Blocked : BagCell {
    public override bool Avaliable(BagItem item) {
        return false;
    }
}

/// <summary>
/// 能放置指定类型
/// </summary>
public class BagCell_Condition : BagCell {
    public ItemTypes typeLimit;
    public BagCell_Condition(ItemTypes typeLimit_) {
        typeLimit = typeLimit_;
    }
    public override bool Avaliable(BagItem item) {
        //return item.cType == typeLimit;
        return false;   // todo
    }
}
