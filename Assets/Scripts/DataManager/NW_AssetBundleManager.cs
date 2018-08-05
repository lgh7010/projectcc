using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// 여기서 에셋번들이라 함은, 유저가 임포트한 에셋을 말한다.
/// 유저는 캐릭터의 일러스트 등을 파일로 임포트해서, 인게임 에디터에서 에셋번들로 빌드할 수 있다. 이 에셋번들 파일을 스팀 등을 통해
/// 배포/다운받을 수 있고, 이 파일을 특정 경로에 넣으면 이용 가능하다.
/// 본 프로젝트는 모바일용이 아니고,
/// CDN다운로드도 제공하지 않으므로 어차피 모든 에셋은 빌드에 말아서 배포하면 그만이다.
/// 굳이 에셋번들을 도입하여 복잡하게 할 필요가 없다.
/// </summary>
public class NW_AssetBundleManager : MonoBehaviour {
    public class AssetBundleInfo {
        public AssetBundle assetBundle;

        public AssetBundleInfo(AssetBundle assetBundle) {
            this.assetBundle = assetBundle;
        }
    }

    public Dictionary<string, AssetBundleInfo> assetBundleDic = new Dictionary<string, AssetBundleInfo>();
    public List<string> ManifestList = null;

    public void Initialize_UserImportAssets(bool unloadAllLoadedObjects) {
        //step 1. 모든 에셋번들을 언로드한다.(혹시 잔여로 남은게 있으면 안됨)
        foreach (KeyValuePair<string, AssetBundleInfo> kv in this.assetBundleDic) {
            kv.Value.assetBundle.Unload(unloadAllLoadedObjects);
            this.assetBundleDic.Remove(kv.Key);
        }

        //step 2. 매니패스트 파일을 읽는다.
        this.ManifestList = ReadMenifestFile();

        //step 3. 에셋번들을 불러온다.
        foreach (var assetName in this.ManifestList) {
            this.assetBundleDic[assetName] = new AssetBundleInfo(AssetBundle.LoadFromFile(Path.Combine(GetUserImportAssetPath(), assetName)));
        }
    }
    public void Refresh_UserImportAssets(bool unloadAllLoadedObjects) {
        //step 1. 매니패스트 파일을 읽는다.(아마도 매니패스트가 처음 initialize했을 때와 달라졌을 것이다)
        this.ManifestList = ReadMenifestFile();

        //step 2. 매니패스트 파일에 존재하지 않는데 로드되어있는 에셋을 언로드한다.
        foreach (KeyValuePair<string, AssetBundleInfo> kv in this.assetBundleDic) {
            kv.Value.assetBundle.Unload(unloadAllLoadedObjects);
        }

        //step 3. 매니페스트에 존재하는데 로드되어있지 않으면 로드한다.
        foreach (var assetName in this.ManifestList) {
            if (!this.assetBundleDic.ContainsKey(assetName) || this.assetBundleDic[assetName] == null) {
                this.assetBundleDic[assetName] = new AssetBundleInfo(AssetBundle.LoadFromFile(Path.Combine(GetUserImportAssetPath(), assetName)));
            }
        }
    }

    private static string GetUserImportAssetPath() {
        string directory = Application.persistentDataPath;
        if (!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }
        return directory;
    }
    private static string GetUserImportAssetManifestFileName() {
        return "UserImport.json";//이건 어차피 유저가 임포트한 에셋번들의 목록을 의미할 뿐이다. 굳이 암호화 할 필요도 없다.
    }
    private static List<string> ReadMenifestFile() {
        string json = System.IO.File.ReadAllText(Path.Combine(GetUserImportAssetPath(), GetUserImportAssetManifestFileName()));
        return NWJson.FromJsonToList<string>(json);
    }
}
