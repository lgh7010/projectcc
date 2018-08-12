using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaTileInfo : MonoBehaviour {
    /// <summary>
    /// 해당 타일의 좌표입니다. 씬의 좌표값이 아니라 HexaTile 좌표계의 좌표이며, 따라서 x, z 값이 모두 정수여야 합니다.
    /// </summary>
    public Vector2 CorePosition {
        get {
            return CorePosition;
        }
        set {
            CorePosition = value;
        }
    }
    /// <summary>
    /// 해당 타일의 사용처입니다.
    /// </summary>
    public string usage {
        get {
            return usage;
        }
        set {
            usage = value;
        }
    }
    /// <summary>
    /// 해당 타일의 종류입니다.
    /// </summary>
    public TileType type {
        get {
            return type;
        }
        set {
            type = value;
        }
    }
    public HexaTileInfo N;
    public HexaTileInfo NE;
    public HexaTileInfo SE;
    public HexaTileInfo S;
    public HexaTileInfo SW;
    public HexaTileInfo NW;
    /// <summary>
    /// 이 타일의 메시 FaceModel을 구성하게 될 Vertex중, 위에 드러나는 6개의 배열입니다.
    /// 0~5까지 각각 N과 NE, NE와 SE, SE와 S, S와 SW, SW와 NW, NW와 N의 중앙을 형성하는 Vertex를 의미합니다.
    /// FaceModel의 아래부분을 구성하는 나머지 6개의 Vertex는 적절한 값으로 자동 생성되며, BodyModel은 전체가 자동 생성됩니다.
    /// </summary>
    public Vector3[] vertexList = new Vector3[6];
    /// <summary>
    /// FaceModel의 아래부분을 구성하는 나머지 6개의 Vertex 배열입니다.
    /// </summary>
    private Vector3[] otherVertexList = new Vector3[6];
    private Mesh _mesh;
    public void MeshRendering() {
        //step 1. 메시의 버텍스 정보를 저장하는, 메시 필터의 참조를 확보한다.
        this._mesh = this.GetComponent<MeshFilter>().mesh;

        //step 2. FaceModel의 12개 버텍스 중, 상판을 구성하는 6개의 버텍스를 확보한다.
        vertexList[0] = GetInterpolationVector3(this.CorePosition, N.CorePosition, NE.CorePosition);
        vertexList[1] = GetInterpolationVector3(this.CorePosition, NE.CorePosition, SE.CorePosition);
        vertexList[2] = GetInterpolationVector3(this.CorePosition, SE.CorePosition, S.CorePosition);
        vertexList[3] = GetInterpolationVector3(this.CorePosition, S.CorePosition, SW.CorePosition);
        vertexList[4] = GetInterpolationVector3(this.CorePosition, SW.CorePosition, NW.CorePosition);
        vertexList[5] = GetInterpolationVector3(this.CorePosition, NW.CorePosition, N.CorePosition);

        //근데 생각해보면, 이렇게 그냥 삼각형 6개로 상판을 그려버리면.. 좀 이상해..
        //차라리 땅은 평평하고, 그냥 6개 버텍스 값만 들고있다가 이게 인접타일이랑 차이가 크면 거기에
        //절벽 오브젝트만 붙이거나(땅의 요철은 그냥 텍스쳐로 표현) 하는 방법도 있어.
        //베스트는, 유니티 터레인 시스템을 이용할 수 있는지 보자.

        //--> 이용할 수 있다. 버텍스로 직접 메시를 그리지 말고, 어떤 '터레인 헥사 타일'이 필요한지
        //그것만 판단해서, 그 타일로 대체하는 방식을 쓰도록 한다.
    }
    private Vector3 GetInterpolationVector3(Vector3 origin, Vector3 v1, Vector3 v2) {
        return new Vector3((origin.x + v1.x + v2.x)/3f, (origin.y + v1.y + v2.y)/3f, (origin.z + v1.z + v2.z)/3f);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public enum TileType {
    //월드뷰의 타일 종류
    Terrain_World,
    City,

    //도시뷰의 타일 종류
    Terrain_City,
    Building,
    Resource,
}
