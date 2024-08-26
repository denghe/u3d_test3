using UnityEngine;

/// <summary>
/// 启动器. 附加到场景首个空节点上
/// </summary>
public class Scene1 : MonoBehaviour {
    public GameObject prefab_property;

    void Start() {
        // 前置检查
        Debug.Assert(prefab_property != null);  // EditorApplication.isPlaying = false;
        // ...

        // 初始化输入管理
        Inputs.Init();

        // 扫描场景内 特定名字的对象，附加组件
        GameObject.Find("Canvas").AddComponent<UICanvas>().Init(prefab_property, new Foo());
    }

    void Update() {
        // 更新输入管理
        Inputs.Update();
    }
}
