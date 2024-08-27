using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全局资源引用容器，在 scene 的 start 中填充，方便使用，减少传参
/// </summary>
public static class Res {
    public static Sprite[] sprites_bg;
    public static Sprite[] sprites_item;
}
