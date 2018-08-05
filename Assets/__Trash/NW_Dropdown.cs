//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;

///// <summary>
///// Dropdown을 상속받아 NW_Dropdown을 만들려고 했으나,
///// Dropdown을 상속받으면 options 리스트를 실시간으로 고칠 수 없는걸 확인했다.
///// (리스트 자체는 실시간으로 고쳐지나, 씬에 표시되는건 다른곳 클릭해야 고쳐짐)
///// 그래서 직접 만들었다.
///// </summary>

//public class NW_Dropdown : NW_UI {

//    #region public 변수
//    public InputField inputField;
//    public ScrollRect scrollRect;
//    public GameObject contentPanel;
//    public GameObject dropdownButton;
//    public bool useSearch = true;
//    public bool hideNonSearchItems = true;
//    public ToggleGroup toggleGroup;
//    public Canvas canvas;
//    #endregion

//    private void Start() {
//        //this._elementInfoList.Add(new NW_UI_ELEMENT("test1"));
//        //this._elementInfoList.Add(new NW_UI_ELEMENT("test2"));
//        //this._elementInfoList.Add(new NW_UI_ELEMENT("test3"));
//        //this._elementInfoList.Add(new NW_UI_ELEMENT("test4"));
//        //this._elementInfoList.Add(new NW_UI_ELEMENT("test5"));
//        //this._elementInfoList.Add(new NW_UI_ELEMENT("test6"));
//        //this._elementInfoList.Add(new NW_UI_ELEMENT("test7"));
//        //this._elementInfoList.Add(new NW_UI_ELEMENT("tesk1"));
//        //this._elementInfoList.Add(new NW_UI_ELEMENT("tesk2"));
//        //this._elementInfoList.Add(new NW_UI_ELEMENT("tesk3"));
//    }

//    #region 검색창 관련 함수
//    public void OnDropdownClick() {
//        this.openOptionList();
//        this.RefreshShownElements(null);
//    }

//    //검색창 무언가 키를 입력할 떄 마다 호출
//    public void OnInputValueChanged() {
//        if (hideNonSearchItems) {
//            this.RefreshShownElements(this.inputField.text);
//        }
//    }

//    //검색어 입력창을 활성화 한 이후에, 엔터를 치거나 마우스 좌/우클릭을 하거나, Esc키를 누르는 등 하여튼 편집을 중단하는 모든 경우 호출
//    public void OnInputEndEdit() {
//        if (Input.GetKeyDown(KeyCode.Return)) {//그 중에서 엔터를 입력한 경우에만, 값을 바꿀 수도 있는 행위를 한다.
//            for (int i = 0; i < this._viewList.Count; i++) {
//                //검색어와 일치하는게 있는 경우에만 값을 변경한다. 그 외에는 값을 변경하지 않는다.
//                //if (this._elementInfoList[i].text.Equals(this.inputField.text)) {
//                //    this._value = i;
//                //    this.closeOptionList();
//                //    break;
//                //}
//            }
//        }

//        //레이캐스트 등으로 오브젝트를 찾아서 클릭한 오브젝트가 자신의 스크롤바/Content패널이 아니면 그냥 리스트를 닫고
//        //그것들이면 값을 바꾸고 리스트를 닫는다.
//        if (Input.GetMouseButtonDown(0)) {
//            bool isTouchedDropdown = false;
//            List<RaycastResult> results = new List<RaycastResult>();
//            PointerEventData ped = new PointerEventData(null);
//            ped.position = Input.mousePosition;
//            canvas.GetComponent<GraphicRaycaster>().Raycast(ped, results);
//            if(results.Count != 0) {
//                if (results[0].gameObject.transform.IsChildOf(this.transform)) {//가장 높이 있는 오브젝트가 0번에 있음.
//                    isTouchedDropdown = true;
//                    Debug.Log("Drop down 내부가 터치되었습니다.");
//                }
//            }
//            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//            //RaycastHit hitInfo;
//            //Debug.Log("좌클릭");
//            //if (Physics.Raycast(ray, out hitInfo)) {
//            //    Debug.Log("hit!");
//            //    Debug.Log(hitInfo.transform.gameObject.name);
//            //} //UI에 레이캐스트를 쏠 때는 위와같이 쏘아야 한다.

//            if (isTouchedDropdown) {
//                for(int i = 0; i < results.Count; i++) {
//                    if (results[i].gameObject.transform.IsChildOf(contentPanel.transform)) {
//                        if(results[i].gameObject.transform.parent.GetComponent<NW_UI_ELEMENT>() != null) {
//                            Debug.Log("엘리먼트중 하나가 터치되었습니다.");
//                            break;
//                        }

//                        if(results[i].gameObject.transform.parent.parent.GetComponent<NW_UI_ELEMENT>() != null) {
//                            Debug.Log("엘리먼트중 하나가 터치되었습니다.");
//                            //Debug.Log(results[i].gameObject.transform.parent.parent.GetComponent<NW_UI_ELEMENT>().text);
//                            break;
//                        }
//                    }
//                }
//            }
//            this.closeOptionList();
//        }
        
//    }

//    public void TEST() {
//        Debug.LogError("test");
//    }

//    public override void RefreshShownElements(string searchString) {
//        //step 1. Dropdown에 보이는 options를 전체 제거
//        for (int i = 0; i < this.contentPanel.transform.childCount; i++) {
//            this.RemoveViewElement(this.contentPanel.transform.GetChild(i).gameObject);
//        }

//        //step 2. _options의 적절한 항목을, 보여지는 목록에 추가
//        int optionCount = 0;
//        if (string.IsNullOrEmpty(searchString)) {
//            for (int i = 0; i < this._viewList.Count; i++) {
//                this.CreateViewElement(this._viewList[i]);
//            }
//            optionCount = this._viewList.Count;
//        } else {
//            for (int i = 0; i < this._viewList.Count; i++) {
//                //if (this._elementInfoList[i].text.Contains(searchString)) {
//                //    this.CreateViewElement(this._elementInfoList[i]);
//                //    optionCount++;
//                //}
//            }
//        }

//        //step 3. content패널의 크기를 조절한다.
//        RectTransform elementRectTr = ELEMENT_TEMPLATE.GetComponent<RectTransform>();
//        if (elementRectTr == null) {
//            Debug.LogError("NW_Dropdown's elementRectTr is null");
//            return;
//        }
//        RectTransform contentRectTr = this.contentPanel.GetComponent<RectTransform>();
//        if (contentRectTr == null) {
//            Debug.LogError("NW_Dropdown's contentRectTr is null");
//            return;
//        }
//        float targetHeight = optionCount * elementRectTr.rect.height;
//        contentRectTr.sizeDelta = new Vector2(0, targetHeight);//앵커를 상단에 맞춰놓고 이렇게 하면, 아래로만 커진다.
//    }
//    public override void CreateViewElement(NW_UI_ELEMENT elementInfo) {
//        GameObject newObj = GameObject.Instantiate(this.ELEMENT_TEMPLATE);
//        newObj.transform.SetParent(this.contentPanel.transform);
//        newObj.SetActive(true);
//        this.toggleGroup.RegisterToggle(newObj.GetComponent<Toggle>());
//    }
//    public override void RemoveViewElement(GameObject go) {
//        this.toggleGroup.UnregisterToggle(go.GetComponent<Toggle>());
//        GameObject.Destroy(go);
//    }
//    public override void RefreshSize_UI() {
//        //Interactor들은 굳이 이 함수를 사용할 일은 없다.
//    }

//    private void openOptionList() {
//        this.scrollRect.gameObject.SetActive(true);
//        if (this.useSearch) {
//            this.inputField.gameObject.SetActive(true);
//            if (this._viewList.Count > 0 && this._viewList[this._viewListValue] != null) {
//                //this.inputField.text = this._elementInfoList[this._value].text;
//            }
//            this.inputField.Select();
//        }
//        this.dropdownButton.SetActive(false);
//    }
//    private void closeOptionList() {
//        this.scrollRect.gameObject.SetActive(false);
//        if (this.useSearch) {
//            this.inputField.gameObject.SetActive(false);
//        }
//        this.dropdownButton.SetActive(true);
//    }
//    #endregion





//    #region 드롭다운 관련 함수
//    public void OnOptionValueChanged() {
//        //드롭다운 선택창에서 특정 옵션을 클릭하여 선택한 경우
//    }
//    #endregion

//    #region 정보 추가 제거
//    public void AddOption(NW_UI_ELEMENT elem) {
//        this._viewList.Add(elem);
//    }
//    public void AddOptionAt(NW_UI_ELEMENT elem, int index) {
//        this._viewList.Insert(index, elem);
//    }
//    public void AddOptionAt_Force(NW_UI_ELEMENT elem, int index) {
//        if (this._viewList[index] == null) {
//            Debug.LogError("NW_Dropdown AddOptionAt_Force. index is out of range");
//            return;
//        }
//        this._viewList[index] = elem;
//    }
//    public void RemoveOption(NW_UI_ELEMENT elem) {
//        this._viewList.Remove(elem);
//    }
//    public void RemoveOptionAt(int index) {
//        this._viewList.RemoveAt(index);
//    }
//    public void ClearOptions() {
//        this._viewList.Clear();
//    }
//    #endregion
//}
