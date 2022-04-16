using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour {

	public GameObject plane;
	public GameObject mesh;
	
	public void DrawTexture(Texture2D texture) {
		Renderer renderer = plane.GetComponent<Renderer>();
		renderer.sharedMaterial.mainTexture = texture;
		renderer.transform.localScale = new Vector3 (texture.width, 1, texture.height);
		
		showPlane(true);
	}

	public void DrawMesh(MeshData meshData) {
		Mesh meshInfo = meshData.CreateMesh();
		MeshFilter filter = mesh.GetComponent<MeshFilter>();
		MeshRenderer renderer = mesh.GetComponent<MeshRenderer>();
		MeshCollider collider = mesh.GetComponent<MeshCollider>();
		MapGenerator mapGenerator = FindObjectOfType<MapGenerator>();

		float scale = mapGenerator.terrainData.normalMode == NormalMode.Smooth ? 10 : 25;
		
		filter.sharedMesh = meshInfo;
		filter.transform.localScale = new Vector3(scale, 10, scale);
		collider.sharedMesh = meshInfo;
		
		showPlane(false);
	}

	void showPlane(bool showPlane){
		plane.SetActive(showPlane);
		mesh.SetActive(!showPlane);
	}

}
