using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


[ExecuteInEditMode]
[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshCollider))]
public class TileMap : MonoBehaviour
{

	public static TileMap instance;
	private int size_x, size_z;
	private float tileSize = 1;
	private Mesh mesh;


	public Texture2D  grass,dirt,gravelstone,ice,wood,redwood,cottontan;
	private int tileResolution = 100;

 
    void Awake ()
	{
		if (instance != null) {
			Debug.LogError ("TileMap: More than one TileMap in the scene.");
		} else {
			instance = this;
		}
	}

	public TileMap GetInstance(){
		return instance;
	}
		

	Color[][] TileSlicing ()
	{
		Texture2D[] arr = new Texture2D[7];
		arr [0] = grass;
		arr [1] = dirt;
		arr [2] = gravelstone;
		arr [3] = ice;
		arr [4] = wood;
		arr [5] = redwood;
		arr [6] = cottontan;
		Color[][] tiles = new Color[arr.Length][];
		for (int i = 0; i < tiles.Length; i++) {

			tiles [i] = arr[i].GetPixels (0,0, 100, 100);	
			}	

		return tiles;
	}
	void BuildTexture (int level)
	{
		
			size_x = 24;
			size_z = 18;
			tileSize = 1;
			tileResolution = 100;
	
		DTileMap map = new DTileMap (size_z, size_x, level);
		Debug.Log ("Build texture level: "+level);
		int tWidth = size_x * 100;
		int tHeight = size_z * 100; 
		Debug.Log ("Mesh building successful.");
		Texture2D texture = new Texture2D (tWidth, tHeight);


		Color[][] tiles = TileSlicing ();


		for (int x = 0; x < size_x; x++) {
			for (int y = 0; y < size_z; y++) {

				Color[] p = tiles [map.GetTileAt (x, y)];
				texture.SetPixels (x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
			}
		}

		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply ();

		MeshRenderer mesh_renderer = GetComponent<MeshRenderer> ();
		mesh_renderer.sharedMaterials [0].mainTexture = texture;

		Debug.Log ("Texture applied successfully.");
	}

    public void BuildMesh(int level)
    {


        int numTiles = size_x * size_z;
        int numTris = numTiles * 2;

        int vsize_x = size_x + 1;
        int vsize_z = size_z + 1;
        int numVerts = vsize_x * vsize_z;

        // Generate the mesh data
        Vector3[] vertices = new Vector3[numVerts];
        Vector3[] normals = new Vector3[numVerts];
        Vector2[] uv = new Vector2[vertices.Length];

        int[] triangles = new int[numTris * 3];

        int x, z;
        for (z = 0; z < vsize_z; z++)
        {
            for (x = 0; x < vsize_x; x++)
            {
                vertices[z * vsize_x + x] = new Vector3(x * tileSize, 0, z * tileSize);
                normals[z * vsize_x + x] = Vector3.up;
                uv[z * vsize_x + x] = new Vector2((float)x / size_x, (float)z / size_z);
            }

        }
        Debug.Log("Done Verts!");

        for (z = 0; z < size_z; z++)
        {
            for (x = 0; x < size_x; x++)
            {
                int squareIndex = z * size_x + x;
                int triOffset = squareIndex * 6;
                triangles[triOffset + 0] = z * vsize_x + x + 0;

                triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 0;
                triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 1;

                triangles[triOffset + 3] = z * vsize_x + x + 0;
                triangles[triOffset + 4] = z * vsize_x + x + vsize_x + 1;
                triangles[triOffset + 5] = z * vsize_x + x + 1;

            }
        }

        Debug.Log("Done Triangles!");

        // Create a new Mesh and populate with the data
        mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        // Assign our mesh to our filter/renderer/collider
        MeshFilter mesh_filter = GetComponent<MeshFilter>();
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        MeshCollider mesh_collider = GetComponent<MeshCollider>();


        mesh_filter.mesh = mesh;
        mesh_collider.sharedMesh = mesh;
		Debug.Log ("Level: "+level);
        BuildTexture(level);
    }


	public Vector3[] GetTilePositions(){
		return mesh.vertices;
	}

}
