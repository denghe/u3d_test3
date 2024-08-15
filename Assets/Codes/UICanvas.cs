using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

/// <summary>
/// 附加到场景的 Canvas UI上
/// </summary>
public class UICanvas : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler/*, IDragHandler, IPointerDownHandler*/ {
    private TextMeshProUGUI richText;    // RichText
    private Button button; // Button

    public void Init(GameObject prefab, System.Object data) {
        Helpers.GenUI_PropsTo("Content", prefab, data);
        richText = GameObject.Find("RichText").GetComponent<TextMeshProUGUI>();
        button = GameObject.Find("Button").GetComponent<Button>();

        for (int i = 0; i < transform.childCount; ++i) {
            var child = transform.GetChild(i);
            if (child.name.StartsWith("Panel")) {
                child.AddComponent<UIDragger>().Init();
            }
        }
        //image.gameObject.AddComponent<UIDragger>();

        button.onClick.AddListener(() => {
            gameObject.SetActive(false);
        });
    }

    // todo: 鼠标悬停时弹出提示？移走后消失？

    //public void OnDrag(PointerEventData eventData) {
    //    Debug.Log("MainScene OnDrag " + eventData.pointerDrag.name);
    //}

    public void OnPointerClick(PointerEventData eventData) {
        //Debug.Log(eventData.position);
        var idx = TMP_TextUtilities.FindIntersectingLink(richText, eventData.position, null);
        if (idx != -1) {
            var info = richText.textInfo.linkInfo[idx];
            Debug.Log(info.GetLinkID());

            // todo: 点击超链接之后 弹出临时的 info panel 可以多层, 点击 info panel 的关闭或外围来 关闭它  可级联关闭 也就是说 只要没有点中最上层 info panel 就关所有
        }
    }

    //public void OnPointerDown(PointerEventData eventData) {  
    //    Debug.Log("MainScene OnPointerDown " + eventData.position);
    //}

    public void OnPointerEnter(PointerEventData eventData) {
        Inputs.mouseCoveredUI = true;
        Debug.Log("enter " + eventData.pointerEnter.name);
    }

    public void OnPointerExit(PointerEventData eventData) {
        Inputs.mouseCoveredUI = false;
        Debug.Log("exit " + eventData.pointerEnter.name);
    }

}

