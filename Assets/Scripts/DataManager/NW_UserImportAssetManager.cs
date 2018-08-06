using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// 
/// </summary>
public class NW_UserImportAssetManager : MonoBehaviour {
    
    /// <summary>
    /// Key : 에셋의 이름(해당 에셋의 개발자가 명명), Value : (에셋의 사용처 / 에셋데이터) 딕셔너리
    /// </summary>
    public Dictionary<string, Dictionary<string, UserImportAssetInfo>> userImportAssetDic;
    public List<string> RootManifest;

    private void Awake() {
        this.Initialize_UserImportAssets();
    }

    public void Initialize_UserImportAssets() {
        //step 1. userImportAssetDic을 초기화한다.
        this.userImportAssetDic = new Dictionary<string, Dictionary<string, UserImportAssetInfo>>();

        //step 2. 유저에셋 폴더의 Root 매니패스트를 읽어서 userAssetRootFolderList를 생성한다.
        string json = File.ReadAllText(Path.Combine(GetUserImportAssetPath(), "UserImport.json"));
        RootManifest = NWJson.FromJsonToList<string>(json);

        //step 3. 유저에셋 폴더의 Root 매니패스트를 순회하며, 각 에셋들이 필요로 하는 파일을 로드한다.
        foreach (var assetName in RootManifest) {
            LoadUserImportAsset_Each(assetName);
        }

        //step 4. 에셋 충돌여부를 조사한다.
        bool isConflict = this.InvesticateConflict();
        if (isConflict) {
            Debug.LogError("에셋 충돌");
        } else {
            Debug.Log("에셋이 충돌하지 않음");
        }
    }
    private void LoadUserImportAsset_Each(string assetName) {
        //step 1. 해당 에셋의 루트경로를 확보한다.
        string assetRootPath = Path.Combine(GetUserImportAssetPath(), assetName);
        Debug.Log(assetRootPath);

        //step 2. 해당 에셋의 매니패스트를 읽는다.
        string json = File.ReadAllText(Path.Combine(assetRootPath, assetName + ".json"));
        Debug.Log(json);
        Dictionary<string, string> dic = NWJson.FromJsonToDictionary<string, string>(json);//key string은 assetUsageIdx, value string은 해당 에셋 파일의 경로

        //step 3. 해당 에셋의 매니패스트를 순회하여, 해당 에셋이 필요로하는 모든 파일을 로드한다.
        this.userImportAssetDic[assetName] = new Dictionary<string, UserImportAssetInfo>();
        foreach (KeyValuePair<string, string> kv in dic) {
            (this.userImportAssetDic[assetName])[kv.Key] = WWWLoadUserImportAsset(kv.Key, kv.Value);
        }
    }

    public void Refresh_UserImportAssets(bool unloadAllLoadedObjects) {
        //step 4. 에셋 충돌여부를 조사한다.
        bool isConflict = this.InvesticateConflict();
        if (isConflict) {
            Debug.LogError("에셋 충돌");
        } else {
            Debug.Log("에셋이 충돌하지 않음");
        }
    }

    /// <summary>
    /// 임포트된 에셋들 중에서 충돌하는 에셋이 존재하는지 확인합니다.
    /// 충돌하는 에셋들에는 isConflict속성을 true로 세팅하고, conflictList에 어떤 에셋과 충돌하는지 에셋의 이름을 나열합니다.
    /// </summary>
    /// <returns>충돌 여부</returns>
    public bool InvesticateConflict() {
        return false;
    }

    private static UserImportAssetInfo WWWLoadUserImportAsset(string assetUsageIdx, string path) {
        WWW www = new WWW("file://" + path);
        Debug.Log("www load texture.name : " + www.texture.name);
        return new UserImportAssetInfo(assetUsageIdx, www.texture);
    }
    private static string GetUserImportAssetPath() {
        string directory = Application.persistentDataPath;
        if (!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }
        return directory;
    }
}

public class UserImportAssetManifest {
    public List<string> userAssetRootFolderList = new List<string>();
}
public class UserImportAssetInfo {
    public string assetUsageIdx;
    public object asset;
    public UserImportAssetType type;
    public bool isConflict = false;
    public List<string> conflictList = new List<string>();

    public UserImportAssetInfo(string assetUsageIdx, Texture2D texture) {
        this.assetUsageIdx = assetUsageIdx;
        this.asset = texture;
        this.type = UserImportAssetType.Texture2D;
    }
    public UserImportAssetInfo(string assetUsageIdx, AudioSource audio) {
        this.assetUsageIdx = assetUsageIdx;
        this.asset = audio;
        this.type = UserImportAssetType.AudioSource;
    }
}
public enum UserImportAssetType {
    Texture2D,
    AudioSource,
}