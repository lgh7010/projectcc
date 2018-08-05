using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NW_UI_SHEET<K,T> : NW_UI<K,T> {

    public int maxColumn = 0;//0이면 제한 없음
    public int maxRaw = 0;//0이면 제한 없음
    public bool verticalFirstFill = false;
    public float columnSpaceRate = 0.05f;
    public float rawSpaceRate = 0.05f;

    public ScrollRect ScrollView;
    
    public override void RefreshSize_UI() {
        //step 0. 잘못 입력된 maxCloumn, maxRaw 처리. 적어도 둘중 하나는 0이어야 함.
        if(maxColumn * maxRaw != 0) {
            maxColumn = 0;//칼럼 수 제한을 강제로 해제한다.
        }


        //step 1. Element Size를 파악하기 위해, ELEMENT_SIZE_INDICATOR의 참조를 확보한다.
        RectTransform elementRectTr = ELEMENT_TEMPLATE.GetComponent<RectTransform>();
        if(elementRectTr == null) {
            Debug.LogError("NW_UI_SHEET's elementRectTr is null");
            return;
        }

        //step 2. Content 패널의 RectTransform참조를 확보한다.
        RectTransform contentRectTr = this.ScrollView.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        if (contentRectTr == null) {
            Debug.LogError("NW_UI_SHEET's contentRectTr is null");
            return;
        }

        //step 3. 파악된 Element size를 이용하여, Scroll View의 Content 패널의 크기를 결정하고 적용한다.
        float targetHeight;
        float targetWidth;
        int column;
        int raw;

        if (verticalFirstFill) {//세로 우선
            if (maxRaw == 0) {//세로 우선, 최대 행 무제한
                raw = contentRectTr.childCount;
                column = 1;
            } else {//세로 우선, 최대 행 제한
                if(maxColumn == 0) {//세로 우선, 최대 행 제한, 최대 열 무제한
                    if(contentRectTr.childCount > maxRaw) {//세로 우선, 최대 행 제한, 최대 열 무제한, 엘리먼트가 최대 행보다 많음
                        raw = maxRaw;
                    } else {//세로 우선, 최대 행 제한, 최대 열 무제한, 엘리먼트가 최대 행보다 적음
                        raw = contentRectTr.childCount;
                    }
                    column = contentRectTr.childCount / maxRaw + 1;
                } else {//세로 우선, 최대 행 제한, 최대 열 제한
                    //최대 행 제한 상황이므로 최대 열이 동시에 제한될 수는 없다.
                    Debug.LogError("NW_UI_SHEET's raw, column both limited!");
                    return;
                }
            }
        } else {//가로 우선
            if(maxColumn == 0) {//가로 우선, 최대 열 무제한
                raw = 1;
                column = contentRectTr.childCount;
            } else {//가로 우선, 최대 열 제한
                if(maxRaw == 0) {//가로 우선, 최대 열 제한, 최대 행 무제한
                    if(contentRectTr.childCount > maxColumn) {
                        column = maxColumn;
                    } else {
                        column = contentRectTr.childCount;
                    }
                    raw = contentRectTr.childCount / maxColumn + 1;
                } else {//가로 우선, 최대 열 제한, 최대 행 제한
                    //최대 열 제한 상황이므로 최대 행이 동시에 제한될 수는 없다.
                    Debug.LogError("NW_UI_SHEET's raw, column both limited!");
                    return;
                }
            }
        }

        GridLayoutGroup gd = contentRectTr.GetComponent<GridLayoutGroup>();
        targetHeight = raw * elementRectTr.rect.height + (elementRectTr.rect.height * this.columnSpaceRate * raw-1) + gd.padding.top + gd.padding.bottom;
        targetWidth = column * elementRectTr.rect.width + (elementRectTr.rect.width * this.rawSpaceRate * column-1) + gd.padding.left + gd.padding.right;

        Debug.Log("element height : " + elementRectTr.rect.height + ", element width : " + elementRectTr.rect.width);
        Debug.Log("raw : " + raw + ", column : " + column);
        Debug.Log("targetHeight : " + targetHeight + ", targetWidth : " + targetWidth);
        contentRectTr.sizeDelta = new Vector2(targetWidth, targetHeight);
        //size delta는 현재 앵커를 기준으로 한 것이다. 0,0을 주면 앵커의 크기와 동일하게 맞춰진다.
        //따라서 앵커를 좌상단 끝으로 해놓고 이렇게 주면, 정확히 이 크기가 된다.
        
        //step 4. ScrollView의 Content패널의 GridLayout의 정보들을 세팅한다.
        gd.cellSize = new Vector2(elementRectTr.rect.width, elementRectTr.rect.height);
        gd.spacing = new Vector2(elementRectTr.rect.width * this.rawSpaceRate, elementRectTr.rect.height * this.columnSpaceRate);
        if (verticalFirstFill) {
            gd.startAxis = GridLayoutGroup.Axis.Vertical;
        } else {
            gd.startAxis = GridLayoutGroup.Axis.Horizontal;
        }
    }
}