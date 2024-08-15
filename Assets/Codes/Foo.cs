using UnityEngine;

/// <summary>
/// 这个类的成员当前只支持 2 种数据类型：int, float，且需要用 Range 标注范围
/// </summary>
public class Foo {
    [Range(0, 100)]
    public int intValue = 12;

    [Range(0, 1)]
    public float floatValue = 0.345f;

    [Range(0, 100)]
    public int intValue1 = 12;

    [Range(0, 1)]
    public float floatValue1 = 0.345f;

    [Range(0, 100)]
    public int intValue2 = 12;

    [Range(0, 1)]
    public float floatValue2 = 0.345f;

    [Range(0, 100)]
    public int intValue3 = 12;

    [Range(0, 1)]
    public float floatValue3 = 0.345f;
}
