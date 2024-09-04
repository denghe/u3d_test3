using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Scene3 : MonoBehaviour {

    // map to ui components
    internal TextMeshProUGUI text_output;
    internal Button button_run;

    internal void GetComponentTo<T>(ref T tar, string name) {
        tar = GameObject.Find(name).GetComponent<T>();
        Debug.Assert(tar != null);
    }
    internal void GetGameObjectTo(ref GameObject tar, string name) {
        tar = GameObject.Find(name);
        Debug.Assert(tar != null);
    }

    void Start() {
        GetComponentTo<TextMeshProUGUI>(ref text_output, "Output");
        GetComponentTo<Button>(ref button_run, "Run");
        button_run.onClick.AddListener(() => {
            text_output.text = SceneTester.Run();
        });
    }
}
