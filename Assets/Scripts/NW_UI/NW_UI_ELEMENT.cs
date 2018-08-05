using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NW_UI_ELEMENT : MonoBehaviour {

    public List<Text> TextComponent;
    public List<Image> ImageComponent;

    public int ContentPanelRawDiviedBy;
    public int ContentPanelColumnDiviedBy;

    public void ResizeElement() {
        if(ContentPanelColumnDiviedBy * ContentPanelRawDiviedBy == 0) {//둘중 하나라도 0이면 사용 불가
            Debug.LogError("이 Element를 Content패널에 가로/세로 몇개로 배치할지 값이 둘다 0입니다");
            return;
        }

        RectTransform contentRectTr = this.transform.parent.GetComponent<RectTransform>();
        GridLayoutGroup gd = contentRectTr.GetComponent<GridLayoutGroup>();
        float targetHeight = (contentRectTr.rect.height / ContentPanelColumnDiviedBy) - (gd.padding.top + gd.padding.bottom);
        float targetWidth = (contentRectTr.rect.width / ContentPanelRawDiviedBy) - (gd.padding.left + gd.padding.right);
        gd.cellSize = new Vector2(targetWidth, targetHeight);
    }
}
