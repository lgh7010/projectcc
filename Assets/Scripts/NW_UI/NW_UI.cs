using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NW_UI<K, T> : MonoBehaviour {

    public GameObject ELEMENT_TEMPLATE;

    /// <summary>
    /// 해당 UI가 핸들링 할 콘텐츠의 딕셔너리입니다. NW_UI 클래스가 제공하는 공개함수를 통해 핸들링 할 수 있습니다.
    /// </summary>
    protected Dictionary<K, T> _contDic = new Dictionary<K, T>();
    protected Dictionary<K, NW_UI_ELEMENT> _viewDic = new Dictionary<K, NW_UI_ELEMENT>();//콘텐츠 패널에 콘텐츠 항목을 추가할때는, 항상 여기에 해당 콘텐츠 패널의 NW_UI_ELEMENT를 넣는다. 여기서 쉽게 해당 게임오브젝트에 접근 가능

    [ContextMenu("Refresh UI Size")]
    public void Manual_RefreshSize_UI() {
        RefreshSize_UI();
    }
    public virtual void RefreshSize_UI() { }

    /// <summary>
    /// 딕셔너리에 명시한 콘텐츠를 추가하고, 뷰에도 이러한 추가사항을 반영합니다.
    /// </summary>
    /// <param name="key">추가될 콘텐츠의 키</param>
    /// <param name="content">콘텐츠</param>
    /// <returns>성공 여부</returns>
    public virtual bool AddContent_Both(K key, T content) {
        Debug.LogError("Virtual AddContentAndRefresh 함수가 호출되었어!");
        return false;
    }

    /// <summary>
    /// 딕셔너리에서 명시한 콘텐츠를 제거하고, 뷰에도 이러한 삭제사항을 반영합니다.
    /// </summary>
    /// <param name="key">삭제할 콘텐츠의 키</param>
    /// <returns>성공 여부</returns>
    public virtual bool RemoveContent_Both(K key) {
        Debug.LogError("Virtual RemoveContentAndRefresh 함수가 호출되었어!");
        return false;
    }

    /// <summary>
    /// 뷰에 명시한 콘텐츠를 추가합니다. 딕셔너리에는 영향을 주지 않습니다.
    /// 이 함수는 많은 콘텐츠를 포함하는 뷰 패널의 랜더링이 오래걸리는 경우, 유저의 스크롤에 따라 뷰에는 콘텐츠의 일부만 표시하려고 할 때 사용합니다.
    /// </summary>
    /// <param name="key">추가할 콘텐츠의 키</param>
    /// <param name="content">콘텐츠</param>
    /// <returns>성공 여부</returns>
    public virtual bool AddContent_View(K key, T content) {
        Debug.LogError("Virtual AddContent_View 함수가 호출되었어!");
        return false;
    }

    /// <summary>
    /// 뷰에서 명시한 콘텐츠를 제거합니다. 딕셔너리에는 영향을 주지 않습니다.
    /// 이 함수는 많은 콘텐츠를 포함하는 뷰 패널의 랜더링이 오래걸리는 경우, 유저의 스크롤에 따라 뷰에는 콘텐츠의 일부만 표시하려고 할 때 사용합니다.
    /// </summary>
    /// <param name="key">삭제할 콘텐츠의 키</param>
    /// <returns>성공 여부</returns>
    public virtual bool RemoveContent_View(K key) {
        Debug.LogError("Virtual RemoveContent_View 함수가 호출되었어!");
        return false;
    }


    /// <summary>
    /// 딕셔너리에 json을 파싱하여 콘텐츠를 모두 생성하고, 뷰에도 이러한 변화를 반영합니다.
    /// 기존의 딕셔너리 정보는 모두 삭제되며, 뷰 또한 마찬가지로 전체가 삭제되고 새롭게 생성됩니다.
    /// </summary>
    /// <param name="json">콘텐츠 정보가 들어있는 json 스트링</param>
    /// <returns>성공 여부</returns>
    public virtual bool SetContent(string json) {
        Debug.LogError("Virtual SetContent 함수가 호출되었어!");
        return false;
    }

    #region 에러메시지 딕셔너리
    protected enum NW_UI_ERROR_TYPE : short {
        JSON_TO_DICTIONARY_PARSING_ERROR = 1,
    }
    protected readonly Dictionary<NW_UI_ERROR_TYPE, string> ERR_MSG = new Dictionary<NW_UI_ERROR_TYPE, string> {
        { NW_UI_ERROR_TYPE.JSON_TO_DICTIONARY_PARSING_ERROR, L.T("Json을 딕셔너리로 파싱하는 과정에서 에러가 발생하였습니다.") },
    };
    #endregion

}