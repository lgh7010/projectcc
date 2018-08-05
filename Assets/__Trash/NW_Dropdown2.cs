//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class NW_Dropdown2 : Dropdown {
//    public InputField _inputField;
    
//    public override void OnPointerClick(PointerEventData eventData) {
//        base.OnPointerClick(eventData);

//        this._inputField.gameObject.SetActive(true);
//        this._inputField.Select();
//    }

//    public override void OnDeselect(BaseEventData eventData) {
//        base.OnDeselect(eventData);

//        this._inputField.gameObject.SetActive(false);
//    }

//    public void OnInputEditDone() {
//        this._inputField.gameObject.SetActive(false);
//        this.Select();
//    }
//    public void OnInputEditChange() {
//        Debug.Log("input change");
//        this.options.Add(new OptionData("testdata"));
//        this.RefreshShownValue();
//        this.Hide();
//        StartCoroutine(showRefreshedList());
//    }
//    IEnumerator showRefreshedList() {
//        yield return new WaitForSeconds(0.3f);
//        this.Show();
//        this._inputField.gameObject.SetActive(true);
//        this._inputField.Select();
//    }
//}
