using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全局环境容器，方便使用，减少传参
/// </summary>
public static class Env {
    public const float FPS = 60;                    // 逻辑帧率
    public const float frameDelay = 1f / FPS;       // 逻辑帧延迟
    public static int frameNumber;                  // 帧编号
    public static float time;                       // 帧编号 对应的 帧秒数时间
    public static float timePool;                   // 时间池( 用于稳定 update 并更新 time, timeSeconds )
}
