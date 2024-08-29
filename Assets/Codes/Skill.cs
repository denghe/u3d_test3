/// <summary>
/// 技能配置
/// </summary>
public class SkillConfig {
    // todo: 效果/作用，基础强度，施展条件, 消耗，主动？被动？冷却？....
}

/// <summary>
/// 技能
/// </summary>
public class Skill {
    public SkillConfig config;                          // 指向配置模板
    // todo: skill config + 参数( 等级啥的 )
}

// 技能默认全局可用，主要调用参数：释放者，作用目标
// 释放者本身的一些数据，会影响技能 强度
// 释放者可以是非玩家。比如 item, 机关. 作用目标 也可能是别的范围参数

// todo: 技能配置表
// todo: 技能归属表，分类表
