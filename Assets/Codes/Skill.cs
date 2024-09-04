/*
先分析一波 技能

技能实质是针对 场景内对象的一些创建行为，或是属性的一些短期或长期修改行为，可能会有后续连锁反应。可理简单解为 角色( 玩家, 怪, 机关... ) 能在游戏里做什么

技能需要有一个 “承载物”，可想象成 技能卷轴，放置于特殊的技能背包
技能的 “释放” 接口，需要传入 释放者，目标( 可能是具体角色，或是可供查询的条件，范围什么的 )
释放者本身的一些数据，会影响技能 强度，在某些设计中，技能释放成功后，还会修改释放者的一些数据。比如蓝
释放者可以是非玩家。比如机关. 可理解为，怪，机关，玩家，可能都具备相同基类，这个基类需要传递到技能，作为释放者使用
注意，就算游戏里开了 “自动攻击 自动施放”，也不会影响主动被动性质。因为这属于 外挂性质，是 AI逻辑在 帮忙手动触发

从分类来看，会有多种分法。
按 释放者 是否有好处来讲，可以是 增益 buff( 增加数值，通常作用于友方 ), 或 减益 debuff( 减少数值，通常作用于敌方 )
按 及时性 来讲 ，可以是 主动 active( 手动触发 )，被动 passive( 每帧自动触发 ) 
按 功能性质 来讲，可以是 直接加减血的，上 buff / dot( 周期 recurrent 影响属性强度 / 减血 ) 的，召唤( 创造新对象: item? buff? stat? ) 的，等等
按 使用场合 来讲，可以是 战斗 / 生活 / 建造 / 重铸 .... 等

这里先列举一些常见的典型的技能案例

受伤减血后脱离战斗了，血慢慢恢复
    效果： 周期加血( 可能每间隔几帧加一点直到加满? ) 每帧自动触发. 但会检查条件：脱离战斗后
    目标：释放者
    实现：target.OnCreate += { target.buffs.add( new 恢复buff( 永久, 执行条件：脱离战斗后 ) ) }

战斗中直接喝血瓶子，自己加了一截血
    效果：一次性加血
    目标：释放者
    实现：target.health += xxxx

砍怪一刀，怪减了一截血
    效果：一次性减血
    实现：target.health -= xxxx 可能会有联动 比如目标死亡？

召唤出一只骷髅怪
    效果：一次性召唤
    实现：scene.items.add( new 骷髅怪( owner = 释放者 ) )

发射一枚火球
    效果：一次性召唤
    目标：无( 就算是对着怪, 那也只是取其坐标，或令火球在飞行途中去跟随，不能算成当前的释放目标 )
    实现：scene.items.add( new 火球( owner = 释放者 ) )

用法术点燃怪，怪持续几秒都在掉血
    效果：一段时间内周期性减血
    实现：target.buffs.add( new 减血buff( 存活一段时间 ) )

开"无敌"数秒
    效果：一段时间内免伤，buff具备很高优先级( 不太容易被移除/取消 )
    实现：target.buffs.add( new 无敌buff( 存活数秒 ) )

使用xx药剂, 令某装备增加词条
    效果：一次性添加词条
    目标：装备
    实现：target.stats.add( new Stat( type = StatTypes.xxxx, value = random .... 永久生效 ) )

向目标使用禁言术
    效果：一段时间内一次性召唤, 创建出备排他性的特殊buff，会导致目标一段时间内很多行为不能( 行为条件可以是检查 buff 队列中是否存在这种东西 )
    实现：target.buffs.add( new 禁言buff( 存活一段时间 ) )

驱散/移除
    效果类似上面，走反向移除操作

目标走在地板上/墙边
    效果：另目标部分移动方向受限( 这里的释放者，是地板/墙 )
    实现：target.buffs.add( new 移动限制buff( 存活 1 帧 ) )

控制目标移动
    效果：令目标坐标发生变化. 可理解为是每一帧都去检测 输入设备( 也可能是AI决策 )状态，是否正在持续下达移动指令
    实现：target.buffs.add( new 移动buff( 存活 1 帧 ) )

令目标在场景里"存活"
    效果：初始化目标血量
    实现：target.OnCreate += { target.buffs.add( new 设置初始血量buff() ) }


items 队列操作有可能因为 "数量超出上限" 而失败
buffs, stats 队列操作有可能因为 "互斥" 而失败, 有可能因已存在而需要 "刷新/替换"( 视 优先级 / 强度 而定 )
buff, stat 本身和 item 之间，也有一个分类适配  的说法，通常不能乱挂。所以可能会存在 黑白名单配置表

stats 可理解为 "配置列表"
buffs 可理解为 "行为列表" / "指令表" / "timer表" ( 替代 update 的逻辑部分实现 )
stats 是 buffs 执行的参数或依据. 
buffs 执行, 也能直接修改 stats 的内容

class item_base {
    config; // 包含 stats buffs 黑白名单, 以及 常驻buff 配置( 体现天生本能 )
    stats;
    buffs1, buffs2;
    avaliableBuffs = buffs1;          // 当前可用于 insert 的 buffs
    update() {
        freeBuffs = buffs2;
        foreach( buff in buffs1 ) {  // 实际为倒循环
            execute( buff )
        }
        freeBuffs = buffs1;
        foreach( buff in buffs2 ) {  // 实际为倒循环
            execute( buff )
        }
        tryAddResidentSkills();     // 每轮都尝试恢复 本能skills
    }
}


*/


/*******************************************************************************************************************************************************************************/
/*******************************************************************************************************************************************************************************/

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

// 这里将 buff, skill, behavior ... 都同等对待

// 模拟 1只怪 不断移动，每隔 几帧 会 晕几帧

public enum BuffTypes : int {
    Move,
    Stun,
    // ...
    MaxValue
}

[StructLayout(LayoutKind.Sequential)]
public struct Buff {
    public BuffTypes type;
    public int _0, _1, _2;
}

[StructLayout(LayoutKind.Sequential)]
public struct Buff_Move {
    public BuffTypes type;
    public int speed, _1, _2;
}

[StructLayout(LayoutKind.Sequential)]
public struct Buff_Stun {
    public BuffTypes type;
    public int timeoutFrameNumber, _1, _2;
}

public class Scene {
    public int frameNumber;
    public Monster monster = new();
    public void Init() {
        monster.Init(this);
    }
    public int Update() {
        ++frameNumber;
        if (frameNumber % 3 == 0) {
            monster.AddBuff_Stun(5);        // simulate stun event every 3 frames
        }
        return monster.Update();
    }

}

public unsafe class Monster {
    public Scene scene;
    public int pos;
    // ...

    public ulong buffsFlags;
    public int buffsLen;
    public Buff[] buffsArray;
    public GCHandle buffsHandle;
    public Buff* buffs;

    public bool BuffsExists(BuffTypes bt) {
        return (buffsFlags & ((ulong)1 << (int)bt)) > 0;
    }
    public void BuffsSetFlag(BuffTypes bt) {
        Debug.Assert(!BuffsExists(bt));
        buffsFlags |= ((ulong)1 << (int)bt);
    }
    public void BuffsClearFlag(BuffTypes bt) {
        Debug.Assert(BuffsExists(bt));
        buffsFlags &= ~((ulong)1 << (int)bt);
    }

    public bool BuffsRemove(BuffTypes bt) {
        if (!BuffsExists(bt)) return false;
        buffsFlags &= ~((ulong)1 << (int)bt);
        for (int i = buffsLen - 1; i >= 0; --i) {
            if (buffsArray[i].type == bt) {
                buffsArray[i] = buffs[--buffsLen];
            }
        }
        return true;
    }

    public void Init(Scene scene_) {
        scene = scene_;

        var cap = 6;
        buffsArray = new Buff[cap];
        buffsHandle = GCHandle.Alloc(buffsArray, GCHandleType.Pinned);
        buffs = (Buff*)buffsHandle.AddrOfPinnedObject();

        TryAddBaseBuffs();
    }
    public void TryAddBaseBuffs() {
        AddBuff_Move(1);
    }

    public int Update() {
		var frameNumber = scene.frameNumber;
		for (int i = buffsLen - 1; i >= 0; --i) {
			var b = &buffs[i];
			switch (b->type) {
                case BuffTypes.Move: HandleBuff_Move((Buff_Move*)b, frameNumber, i); break;
                case BuffTypes.Stun: HandleBuff_Stun((Buff_Stun*)b, frameNumber, i); break;
                // ... more case
            }
		}
		TryAddBaseBuffs();
        return 0;
    }

    public void HandleBuff_Move(Buff_Move* o, int frameNumber, int index) {
        pos += o->speed;
    }

    public void HandleBuff_Stun(Buff_Stun* o, int frameNumber, int index) {
        if (o->timeoutFrameNumber < frameNumber) {
            buffsArray[index] = buffs[--buffsLen];
            BuffsClearFlag(BuffTypes.Stun);
        }
    }

    /*********************************************************************************************/

    public bool AddBuff_Move(int speed) {
        if (BuffsExists(BuffTypes.Move) || BuffsExists(BuffTypes.Stun)) return false;
        BuffsSetFlag(BuffTypes.Move);
        var o = (Buff_Move*)&buffs[buffsLen++];
        o->type = BuffTypes.Move;
        o->speed = speed;
        return true;
    }

    public bool AddBuff_Stun(int numFrames) {
        if (BuffsExists(BuffTypes.Stun)) return false;
        BuffsRemove(BuffTypes.Move);
        BuffsSetFlag(BuffTypes.Stun);
        var o = (Buff_Stun*)&buffs[buffsLen++];
        o->type = BuffTypes.Stun;
        o->timeoutFrameNumber = scene.frameNumber + numFrames;
        return true;
    }
}

public static class SceneTester {
    public static string Run() {
        var scene = new Scene();
        scene.Init();
        var sb = new StringBuilder();
#if false
		for (int i = 0; i < 20; i++) {
			scene.Update();
            sb.AppendLine(scene.frameNumber + "\tmonster.pos = " + scene.monster.pos);
		}
#else
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 100000000; i++) {
            scene.Update();
        }
        sb.AppendLine("secs = " + sw.ElapsedMilliseconds / 1000f);
        sb.AppendLine(scene.frameNumber + "\tmonster.pos = " + scene.monster.pos);
#endif
        return sb.ToString();
    }
}















/*******************************************************************************************************************************************************************************/
/*******************************************************************************************************************************************************************************/

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

///// <summary>
///// Buff 或 Debuff
///// </summary>
//public class Buff {

//}

///// <summary>
///// 角色
///// </summary>
//public class Character {
//    public List<Buff> buffs;
//    public List<Skill> skills;
//}




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
