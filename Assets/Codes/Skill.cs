using System.Collections.Generic;

/*
// todo: 先分析一波 技能

技能实质是针对 场景内对象的一些创建行为，或是属性的一些短期或长期修改行为，可能会有后续连锁反应。可理简单解为 角色( 玩家, 怪, 机关... ) 能在游戏里做什么

技能需要有一个 “承载物”，可想象成 技能卷轴，放置于特殊的技能背包
技能的 “释放” 接口，需要传入 释放者，目标( 可能是具体角色，或是可供查询的条件，范围什么的 )
释放者本身的一些数据，会影响技能 强度，在某些设计中，技能释放成功后，还会修改释放者的一些数据。比如蓝
释放者可以是非玩家。比如机关. 可理解为，怪，机关，玩家，可能都具备相同基类，这个基类需要传递到技能，作为释放者使用
注意，就算游戏里开了 “自动攻击 自动施放”，也不会影响主动被动性质。因为这属于 外挂性质，是 AI逻辑在 帮忙手动触发

从分类来看，会有多种分法。
按 释放者 是否有好处来讲，可以是 增益 buff( 增加数值，通常作用于友方 ), 或 减益 debuff( 减少数值，通常作用于敌方 )
按 及时性 来讲 ，可以是 主动 active( 手动触发 )，被动 passive( 每帧自动触发 ) 
按 功能性质 来讲，可以是 直接加减血的，上 buff / dot( 周期 recurrent 影响属性强度 / 减血 ) 的，召唤( 创造新对象 ) 的，等等

这里先列举一些常见的典型的技能案例

受伤减血后脱离战斗了，血慢慢恢复
    buff( 持续多帧生效 ) passive( 每帧自动触发. 但会检查条件：脱离战斗后 )
    性质： 周期加血( 可能每间隔几帧加一点直到加满? )
    目标：释放者
    实现：当目标创建成功时，立即在目标的 buff/debuff 容器中，创建一个 恢复buff。每次目标 update 时也会调用该 buff update

战斗中直接喝血瓶子，自己加了一截血
    buff( 当前帧生效 ) active( 手动触发 )
    性质：一次性加血
    目标：释放者
    实现：target.health += xxxx

砍怪一刀，怪减了一截血
    debuff( 当前帧生效 ) active( 手动触发 )
    性质：一次性减血
    目标：怪
    实现：target.health -= xxxx 可能会有联动 比如目标死亡？

召唤出一只骷髅怪
    buff( 当前帧生效 ) active( 手动触发 )
    性质：一次性召唤
    目标：无
    实现：新创建 owner = 释放者 的 骷髅怪 到场景

发射一枚火球
    buff( 当前帧生效 ) active( 手动触发 )
    性质：一次性召唤
    目标：无( 就算是对着怪, 那也只是取其坐标，或令火球在飞行途中去跟随，不能算成当前的释放目标 )
    实现：新创建 owner = 释放者 的 火球 到场景

用法术点燃怪，怪持续几秒都在掉血
    debuff( 持续多帧生效 ) active( 手动触发 )
    性质：周期性减血
    目标：怪
    实现：立即在目标的 buff/debuff 容器中，创建一个 减血buff。每次目标 update 时也会调用该 buff update

*/

/// <summary>
/// 技能配置
/// </summary>
public class SkillConfig {
    // todo
}

/// <summary>
/// 技能某种分类( 可能就是按资源图标来 )
/// </summary>
public enum SkillTypes {
    // todo
}

/// <summary>
/// 技能
/// </summary>
public class Skill {
    public SkillConfig config;
    // todo
}


///// <summary>
///// 技能容器
///// </summary>
//public class SkillContainer {
//    // todo
//}

/// <summary>
/// Buff 或 Debuff
/// </summary>
public class Buff {

}

/// <summary>
/// 角色
/// </summary>
public class Character {
    public List<Buff> buffs;
    public List<Skill> skills;
}




// todo: 技能配置表
// todo: 技能归属表，分类表
/*
 
skill 记录变量：lastCastTime
update 逻辑:
if (needCast) {
    var 时间差 = min( now - lastCastTime, max(帧延迟, 技能cd) )
    var count = 时间差 / cd
    if (count > 0) {
       ...
        int i = 0;
        for( ; i <  count; ++i) {
            if (蓝不够扣) break;
            if (某些条件满足) {
                扣蓝
                cast logic
            }
            lastCastTime += 技能cd
        }
    }
}

这段逻辑的缺陷在于 蓝的恢复时机。循环内在模拟时间流逝，但此时蓝并没有回
另外，如果是多个技能并行施展，有依赖关系的话，也不对劲

所以应该尽量避开这种设计, 比如当前流行技能不耗兰，也没啥关联

*/
