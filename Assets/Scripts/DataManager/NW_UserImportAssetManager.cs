using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using UnityEngine.UI;//only test

/// <summary>
/// 
/// </summary>
public class NW_UserImportAssetManager : MonoBehaviour {

    /// <summary>
    /// 일단 지금 usage별로 파일들을 나눠야 하고, 서로 다른 usage의, 사용되는 텍스쳐들을 아틀라스로 묶어야 한다. 아틀라스로 묶는건 texture.PackTexture호출하면 끝인듯한데, 구조에 대한 고민이 더 문제다.
    /// https://docs.unity3d.com/2018.2/Documentation/ScriptReference/Texture2D.PackTextures.html
    /// </summary>

    public Dictionary<string, List<Texture2D>> userImportTextureDic = new Dictionary<string, List<Texture2D>>();// [usage / texture]
    public Dictionary<string, List<AudioClip>> userImportAudioClipDic = new Dictionary<string, List<AudioClip>>();// [usage / audioSource]

    public List<Texture2D> userFinalImportTextureList = new List<Texture2D>();
    public List<AudioClip> userFinalImportAudioClipList = new List<AudioClip>();

    private void Awake() {
        this.Initialize_UserImportAssets();

        //GameObject.Find("Cube1").GetComponent<MeshRenderer>().material.mainTexture = (userImportAssetDic["a"])["usage1"].asset as Texture2D;
        //GameObject.Find("Cube2").GetComponent<MeshRenderer>().material.mainTexture = (userImportAssetDic["b"])["usage2"].asset as Texture2D;
        //GameObject.Find("Cube3").GetComponent<MeshRenderer>().material.mainTexture = (userImportAssetDic["c"])["usage1"].asset as Texture2D;
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

        //step 4. 생성한 딕셔너리를 이용해, 실제 에셋들을 로드한다. 0번 에셋만 로드한다. (유저가 다른 에셋을 임포트하고자 원하면, 그것을 리스트의 0번에 놓는다.)텍스쳐의 경우 아틀라스로 묶는다.
        foreach(KeyValuePair<string, List<Texture2D>> kv in this.userImportTextureDic){
            this.userFinalImportTextureList.Add(kv.Value[0]);
        }
        foreach(KeyValuePair<string, List<AudioClip>> kv in this.userImportAudioClipDic) {
            this.userFinalImportAudioClipList.Add(kv.Value[0]);
        }

        //step 5. 위에서 생성한 배열들에 필요한 잔여 작업을 수행한다.
        List<Texture2D> atlasList = MakeAtlasFromTextureList(this.userFinalImportTextureList);
    }
    private void process_EachFolder(string manifestFileName) {
        string assetPath = Path.Combine(userImportAssetPath, manifestFileName);
        Debug.Log("process_EachFolder path : " + manifestFileName);

        //step 1. 각각의 에셋 폴더 하위에는 해당 폴더이름과 동일한 매니패스트 파일이 있다. 이 파일을 읽는다.
        string json = File.ReadAllText(Path.Combine(assetPath, manifestFileName + ".txt"));
        Dictionary<string, Dictionary<string, string>> manifestDic = NWJson.FromJsonToDictionary<string, Dictionary<string,string>>(json);

        Dictionary<string, string> tempDic;
        //step 2. 해당 에셋의 매니페스트가 명시하고 있는 텍스쳐 에셋을 userImportTextureDic에 넣는다.
        tempDic = manifestDic["Texture"];
        foreach(KeyValuePair<string, string> kv in tempDic) {
            if(!this.userImportTextureDic.ContainsKey(kv.Key)) {
                this.userImportTextureDic.Add(kv.Key, new List<Texture2D>());
            }
            this.userImportTextureDic[kv.Key].Add(WWWLoadUserImportAsset(kv.Key, Path.Combine(assetPath, kv.Value), UserImportAssetType.Texture2D) as Texture2D);
        }

        //step 3. 해당 에셋의 매니페스트가 명시하고 있는 사운드 에셋을 userImportAudioClipDic에 넣는다.
        tempDic = manifestDic["AudioClip"];
        foreach (KeyValuePair<string, string> kv in tempDic) {
            if (!this.userImportAudioClipDic.ContainsKey(kv.Key)) {
                this.userImportAudioClipDic.Add(kv.Key, new List<AudioClip>());
            }
            this.userImportAudioClipDic[kv.Key].Add(WWWLoadUserImportAsset(kv.Key, Path.Combine(assetPath, kv.Value), UserImportAssetType.AudioClip) as AudioClip);
        }
    }
    private bool DetermineConflict() {
        foreach(KeyValuePair<string, List<Texture2D>> kv in this.userImportTextureDic) {
            if(kv.Value.Count > 1) {
                return true;
            }
        }
        foreach(KeyValuePair<string, List<AudioClip>> kv in this.userImportAudioClipDic) {
            if(kv.Value.Count > 1) {
                return true;
            }
        }
        return false;
    }
    private List<Texture2D> MakeAtlasFromTextureList(List<Texture2D> list) {
        List<Texture2D> rst = new List<Texture2D>();

        Texture2D currentPackingAtlas = new Texture2D(4096, 4096);
        int accumulatedWidth = 0;
        int accumulatedHeight = 0;

        List<Texture2D> currentPackingAtlasCandidates = new List<Texture2D>();
        foreach (var texture in list) {
            if((accumulatedWidth + texture.width) < 4096 && (accumulatedHeight + texture.height) < 4096) {//현재 채우는 중인 아틀라스에 적당한 빈 공간이 있는 경우
                accumulatedWidth += texture.width;//생각해보니 이렇게 늘리면 안되네. 그럼 실제 들어갈 수 있는 것보다 훨씬 적게 들어가네. PackTextures할때 가로 우선인지 세로 우선인지 알아내서, 계산해주는 로직 필요할 듯.
                accumulatedHeight += texture.height;//더구나 '용도별'로 아틀라스를 묶어야지, 이렇게 마구잡이로 묶으면 안됨
                currentPackingAtlasCandidates.Add(texture);
            } else {
                currentPackingAtlas.PackTextures(currentPackingAtlasCandidates.ToArray(), 0);
                rst.Add(currentPackingAtlas);

                currentPackingAtlas = new Texture2D(4096, 4096);
                accumulatedHeight = 0;
                accumulatedWidth = 0;
            }
        }

        return rst;
    }

    private static object WWWLoadUserImportAsset(string assetUsageIdx, string path, UserImportAssetType type) {
        WWW www = new WWW("file://" + path);
        switch (type) {
            case UserImportAssetType.Texture2D: return www.texture;
            case UserImportAssetType.AudioClip: return www.GetAudioClip();
            default: Debug.LogError("WWWLoadUserImportAsset Error. type is unknown."); return null;
        }
    }
    private static string userImportAssetPath {
        get {
            string directory = Application.persistentDataPath;
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
            return directory;
        }
    }
}

public class UserImportAssetData {
    public object asset;
    public UserImportAssetType type;
    public UserImportAssetData(Texture2D texture) {
        this.asset = texture;
        this.type = UserImportAssetType.Texture2D;
    }
    public UserImportAssetData(AudioClip audioClip) {
        this.asset = audioClip;
        this.type = UserImportAssetType.AudioClip;
    }
}

public enum UserImportAssetType {
    Texture2D,
    AudioClip,
}