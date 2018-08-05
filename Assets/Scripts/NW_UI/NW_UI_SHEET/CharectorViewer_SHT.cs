using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이 클래스는 개발 테스트용입니다. 이런식으로 NW_UI_SHEET를 상속받아 사용할 수 있습니다.
/// NW_UI_SHEET로 대부분의 UI를 만들 수 있을것으로 기대하기 때문에, 대부분 이러한 개발 방식을 사용하게 될 것입니다.
/// </summary>
public class CharectorViewer_SHT : NW_UI_SHEET<string, CharectorInfoForTest> {
    public override bool AddContent_Both(string key, CharectorInfoForTest content) {
        return false;
    }
    public override bool RemoveContent_Both(string key) {
        return false;
    }
    public override bool AddContent_View(string key, CharectorInfoForTest content) {
        return false;
    }
    public override bool RemoveContent_View(string key) {
        return false;
    }
    public override bool SetContent(string json) {
        try {
            this._contDic = NWJson.FromJsonToDictionary<string, CharectorInfoForTest>(json);
        } catch {
            Debug.LogError(ERR_MSG[NW_UI_ERROR_TYPE.JSON_TO_DICTIONARY_PARSING_ERROR]);
            return false;
        }
        return (this._contDic != null);
    }
}

/// <summary>
/// 개발용 테스트케이스. 지금은 그냥 NW_DATA를 상속받았는데,
/// '캐릭터'이런 클래스도 만들어서 캐릭터를 상속받아 여러 필요한 클래스를 만드는 방식을 사용하면 됩니다.
/// </summary>
public class CharectorInfoForTest : NW_DATA {
    public string name;
    public int age;
    public int strong;
    public int inteligence;
}
