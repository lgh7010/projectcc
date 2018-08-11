using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using UnityEngine.UI;//only test

/// <summary>
/// 유저 임포트 텍스쳐는 오로지 일러스트에만 사용됩니다. 256*256 사이즈만 제공됩니다.
/// 그 외에는, UserImportAsset폴더가 아니라 Skins폴더에 존재하는 파일을 직접 수정하여 스킨으로 사용할 수 있습니다.
/// 여기 있는 파일은 아틀라스를 그대로 한장의 그림으로 익스포트 한 것으로, 그림 곳곳의 요소들을 수정하여 해당 오브젝트에 실제로 적용할 수 있습니다.
/// </summary>
public class NW_UserImportAssetManager : MonoBehaviour {
    /// <summary>
    /// 유저 에셋 폴더의 모든 하위폴더에 존재하는 모든 텍스쳐를 usage별로 나열합니다. 각 usage의 리스트에 2개 이상의 텍스쳐가 들어있는 경우, 해당 텍스쳐의 Usage가 충돌한 것입니다.
    /// </summary>
    public Dictionary<string, List<UserImportAssetInfo>> tempTextureDic = new Dictionary<string, List<UserImportAssetInfo>>();// [usage / texture]
    /// <summary>
    /// 유저 에셋 폴더의 모든 하위폴더에 존재하는 모든 오디오 클립을 usage별로 나열합니다. 각 usage의 리스트에 2개 이상의 오디오 클립이 들어있는 경우, 해당 오디오 클립의 Usage가 충돌한 것입니다.
    /// </summary>
    public Dictionary<string, List<UserImportAssetInfo>> tempAudioClipDic = new Dictionary<string, List<UserImportAssetInfo>>();// [usage / audioSource]

    /// <summary>
    /// 유저 에셋 폴더에서 임포트된 모든 usage별 텍스쳐
    /// </summary>
    public static Dictionary<string, UserImportAssetInfo> UserImportTextureDic = new Dictionary<string, UserImportAssetInfo>();
    public static Dictionary<string, UserImportAssetInfo> UserImportAudioClipDic = new Dictionary<string, UserImportAssetInfo>();

    private void Awake() {
        this.Initialize_UserImportAssets();

        //인게임 오브젝트 테스트 코드
        GameObject.Find("Cube1").GetComponent<MeshRenderer>().material.mainTexture = UserImportTextureDic["usage1"].texture;
        GameObject.Find("Cube2").GetComponent<MeshRenderer>().material.mainTexture = UserImportTextureDic["usage2"].texture;
        GameObject.Find("Cube3").GetComponent<MeshRenderer>().material.mainTexture = UserImportTextureDic["usage1"].texture;

        //UI 오브젝트 테스트 코드
        //GameObject.Find("Image (1)").GetComponent<Image>().material.mainTexture = this.finalUserImportTextureDic["usage1"];//이렇게 하면 디폴트 매터리얼이 적용되어, 전체 UI에 적용됨
        Rect rec = new Rect(0, 0, UserImportTextureDic["usage1"].texture.width, UserImportTextureDic["usage1"].texture.height);
        GameObject.Find("Image (1)").GetComponent<Image>().sprite = Sprite.Create(UserImportTextureDic["usage1"].texture, rec, new Vector2(0, 0), 1);
        GameObject.Find("Image (2)").GetComponent<Image>().sprite = Sprite.Create(UserImportTextureDic["usage1"].texture, rec, new Vector2(0, 0), 1);
        GameObject.Find("Image (3)").GetComponent<Image>().sprite = Sprite.Create(UserImportTextureDic["usage1"].texture, rec, new Vector2(0, 0), 1);
        GameObject.Find("Image (4)").GetComponent<Image>().sprite = Sprite.Create(UserImportTextureDic["usage1"].texture, rec, new Vector2(0, 0), 1);

        Rect rec2 = new Rect(0, 0, UserImportTextureDic["usage2"].texture.width, UserImportTextureDic["usage2"].texture.height);
        GameObject.Find("Image (5)").GetComponent<Image>().sprite = Sprite.Create(UserImportTextureDic["usage2"].texture, rec2, new Vector2(0, 0), 1);
        GameObject.Find("Image (6)").GetComponent<Image>().sprite = Sprite.Create(UserImportTextureDic["usage2"].texture, rec2, new Vector2(0, 0), 1);
        GameObject.Find("Image (7)").GetComponent<Image>().sprite = Sprite.Create(UserImportTextureDic["usage2"].texture, rec2, new Vector2(0, 0), 1);
        GameObject.Find("Image (8)").GetComponent<Image>().sprite = Sprite.Create(UserImportTextureDic["usage2"].texture, rec2, new Vector2(0, 0), 1);
        GameObject.Find("Image (9)").GetComponent<Image>().sprite = Sprite.Create(UserImportTextureDic["usage2"].texture, rec2, new Vector2(0, 0), 1);
    }

    public void Initialize_UserImportAssets() {
        //step 1. UserImportAsset 폴더 내부의 모든 하위 폴더 정보를 얻어낸다.
        DirectoryInfo rootDirectoryInfo = new DirectoryInfo(userImportAssetPath);
        DirectoryInfo[] eachAssetDirectoryInfo = rootDirectoryInfo.GetDirectories();

        //step 2. UserImportAsset 폴더 하위의 모든 폴더는 각각 하나의 유저 임포트 에셋번들을 의미한다. 각각의 폴더를 순회하며, userImportTextrueDic, userImportAudioDic에 넣는다.
        foreach(var dir in eachAssetDirectoryInfo) {
            this.process_EachFolder(dir.Name);
        }

        //step 3. 에셋 충돌을 조사한다. 에셋 타입을 통틀어 충돌을 감지한다.
        bool isConflict = DetermineConflict();
        Debug.Log("User Import Asset Conflict : " + isConflict);

        //step 4. 생성한 딕셔너리를 이용해, 실제 에셋들을 로드한다. 0번 에셋만 로드한다. (유저가 다른 에셋을 임포트하고자 원하면, 그것을 리스트의 0번에 놓는다.)텍스쳐의 경우 아틀라스로 묶는다.
        foreach (KeyValuePair<string, List<UserImportAssetInfo>> kv in this.tempTextureDic){
            UserImportTextureDic[kv.Key] = kv.Value[0];
            Debug.Log("texture added in FinalTextureList. key : " + kv.Key + ", name : " + kv.Value[0].texture.name);
        }
        foreach(KeyValuePair<string, List<UserImportAssetInfo>> kv in this.tempAudioClipDic) {
            UserImportAudioClipDic[kv.Key] = kv.Value[0];
            Debug.Log("audioclip added in FinalAudioClipList. key : " + kv.Key + ", name : " + kv.Value[0].texture.name);
        }

        //step 5. 위에서 생성한 배열들에 필요한 잔여 작업을 수행한다.
        List<Texture2D> atlasList = MakeAtlasFromTextureList(UserImportTextureDic);
    }
    private void process_EachFolder(string manifestFileName) {
        string assetPath = Path.Combine(userImportAssetPath, manifestFileName);
        Debug.Log("process_EachFolder path : " + manifestFileName);

        //step 1. 각각의 에셋 폴더 하위에는 해당 폴더이름과 동일한 매니패스트 파일이 있다. 이 파일을 읽는다.
        string json = File.ReadAllText(Path.Combine(assetPath, manifestFileName + ".json"));
        Debug.Log("User Import Asset Folder manifest loaded. " + json);
        Dictionary<string, Dictionary<string, string>> manifestDic = NWJson.FromJsonToDictionary<string, Dictionary<string, string>>(json);

        Dictionary<string, string> tempDic;
        //step 2. 해당 에셋의 매니페스트가 명시하고 있는 텍스쳐 에셋을 userImportTextureDic에 넣는다.
        try {
            tempDic = manifestDic["Texture"];
            foreach (KeyValuePair<string, string> kv in tempDic) {
                if (!this.tempTextureDic.ContainsKey(kv.Key)) {
                    this.tempTextureDic.Add(kv.Key, new List<UserImportAssetInfo>());
                }
                this.tempTextureDic[kv.Key].Add(new UserImportAssetInfo(manifestFileName, WWWLoadUserImportAsset(kv.Key, Path.Combine(assetPath, kv.Value), UserImportAssetType.Texture2D) as Texture2D));
            }
        } catch {
            Debug.LogWarning("There is no Texture in this asset : " + manifestFileName);
        }

        //step 3. 해당 에셋의 매니페스트가 명시하고 있는 사운드 에셋을 userImportAudioClipDic에 넣는다.
        try {
            tempDic = manifestDic["AudioClip"];
            foreach (KeyValuePair<string, string> kv in tempDic) {
                if (!this.tempAudioClipDic.ContainsKey(kv.Key)) {
                    this.tempAudioClipDic.Add(kv.Key, new List<UserImportAssetInfo>());
                }
                this.tempAudioClipDic[kv.Key].Add(new UserImportAssetInfo(manifestFileName, WWWLoadUserImportAsset(kv.Key, Path.Combine(assetPath, kv.Value), UserImportAssetType.AudioClip) as AudioClip));
            }
        } catch {
            Debug.LogWarning("There is no AudioClip in this asset : " + manifestFileName);
        }
    }
    private bool DetermineConflict() {
        foreach(KeyValuePair<string, List<UserImportAssetInfo>> kv in this.tempTextureDic) {
            if(kv.Value.Count > 1) {
                return true;
            }
        }
        foreach(KeyValuePair<string, List<UserImportAssetInfo>> kv in this.tempAudioClipDic) {
            if(kv.Value.Count > 1) {
                return true;
            }
        }
        return false;
    }
    private List<Texture2D> MakeAtlasFromTextureList(Dictionary<string, UserImportAssetInfo> dic) {
        //이제 캐릭터 일러스트, 아틀라스 크기가 모두 확정되었으므로, 이 함수 작성 가능 (텍스쳐의 사용처가 전부 캐릭터 일러이므로, 그냥 막 묶으면 됨)
        //언제 어떤 캐릭이 쓰일지 예측할 방법은 없음.
        //List<Texture2D> rst = new List<Texture2D>();

        //Texture2D currentPackingAtlas = new Texture2D(4096, 4096);
        //int accumulatedWidth = 0;
        //int accumulatedHeight = 0;

        //List<Texture2D> currentPackingAtlasCandidates = new List<Texture2D>();
        //foreach (var texture in list) {
        //    if ((accumulatedWidth + texture.width) < 4096 && (accumulatedHeight + texture.height) < 4096) {//현재 채우는 중인 아틀라스에 적당한 빈 공간이 있는 경우
        //        accumulatedWidth += texture.width;//생각해보니 이렇게 늘리면 안되네. 그럼 실제 들어갈 수 있는 것보다 훨씬 적게 들어가네. PackTextures할때 가로 우선인지 세로 우선인지 알아내서, 계산해주는 로직 필요할 듯.
        //        accumulatedHeight += texture.height;//더구나 '용도별'로 아틀라스를 묶어야지, 이렇게 마구잡이로 묶으면 안됨
        //        currentPackingAtlasCandidates.Add(texture);
        //    } else {
        //        currentPackingAtlas.PackTextures(currentPackingAtlasCandidates.ToArray(), 0);
        //        rst.Add(currentPackingAtlas);

        //        currentPackingAtlas = new Texture2D(4096, 4096);
        //        accumulatedHeight = 0;
        //        accumulatedWidth = 0;
        //    }
        //}

        //return rst;
        return null;
    }

    private static object WWWLoadUserImportAsset(string assetUsageIdx, string path, UserImportAssetType type) {
        Debug.Log("WWW fild load try : " + path);
        WWW www = new WWW("file://" + path);
        switch (type) {
            case UserImportAssetType.Texture2D: return www.texture;
            case UserImportAssetType.AudioClip: return www.GetAudioClip();
            default: Debug.LogError("WWWLoadUserImportAsset Error. type is unknown."); return null;
        }
    }
    private static string userImportAssetPath {
        get {
            string directory = Path.Combine(Application.persistentDataPath, "UserImportAsset");
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
            return directory;
        }
    }
}

public enum UserImportAssetType {
    Texture2D,
    AudioClip,
}

public static partial class CONST_VALUES {
    public const int CHARECTOR_ILUST_TYPE1_SIZE_X = 256;
    public const int CHARECTOR_ILUST_TYPE1_SIZE_Y = 256;
    public const int CHARECTOR_ILUSE_ATLAS_MAX_SIZE = 2048;
}

public class UserImportAssetInfo {
    public object asset;
    public string assetName;
    public UserImportAssetType type;
    public UserImportAssetInfo(string name, Texture2D texture) {
        this.assetName = name;
        this.asset = texture;
        this.type = UserImportAssetType.Texture2D;
    }
    public UserImportAssetInfo(string name, AudioClip audioClip) {
        this.assetName = name;
        this.asset = audioClip;
        this.type = UserImportAssetType.AudioClip;
    }
    public Texture2D texture {
        get {
            return asset as Texture2D;
        }
    }
    public AudioClip audioClip {
        get {
            return asset as AudioClip;
        }
    }
}
