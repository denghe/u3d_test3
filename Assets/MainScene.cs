using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour {

    public GameObject prefab_property;

    void Start() {
        var canvas = GameObject.Find("Canvas");
        var o = Instantiate(prefab_property);
        var t = (RectTransform)o.transform;
        t.SetParent(canvas.transform);
        t.anchoredPosition = new Vector2(0, 0);
    }


    void Update() {

    }
}

//var gs = gameObject.scene.GetRootGameObjects();
//foreach (var g in gs) {
//    Debug.Log(g.name + " " + g.GetType());
//}

//void Start() {
//    // 获取当前GameObject的所有直接子节点
//    Transform[] directChildren = transform.GetComponentsInChildren<Transform>();
//    foreach (Transform child in directChildren) {
//        Debug.Log("直接子节点: " + child.name);
//    }

//    // 递归遍历整个对象树
//    TraverseObjectTree(transform);
//}

//void TraverseObjectTree(Transform parent) {
//    foreach (Transform child in parent) {
//        Debug.Log("子节点: " + child.name);
//        // 递归遍历当前子节点的子节点
//        TraverseObjectTree(child);
//    }
//}
