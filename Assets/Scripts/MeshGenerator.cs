using UnityEngine;
using System.Collections;

public static class MeshGenerator {

	public static MeshData GenerateTerrainMesh(float[,] heightMap, TerrainData terrainData) {
		

		int mapLength = heightMap.GetLength (0);
		int meshSimplificationIncrement = terrainData.LOD == 0 ? 1 : terrainData.LOD * 2;
		int verticesPerLine = (mapLength - 1) / meshSimplificationIncrement + 1;
		int meshVertexIndex = 0;

		float topLeftX = (mapLength - 1) / -2f;
		float topLeftZ = (mapLength - 1) / 2f;

		int[,] vertexIndicesMap = new int[mapLength,mapLength];
		
		MeshData meshData = new MeshData (verticesPerLine, terrainData.normalMode);
		
		for (int y = 0; y < mapLength; y += meshSimplificationIncrement) {
			for (int x = 0; x < mapLength; x += meshSimplificationIncrement) {
				vertexIndicesMap [x, y] = meshVertexIndex;
				meshVertexIndex++;
			}
		}

		for (int y = 0; y < mapLength; y += meshSimplificationIncrement) {
			for (int x = 0; x < mapLength; x += meshSimplificationIncrement) {
				int vertexIndex = vertexIndicesMap [x, y];
				Vector2 percent = new Vector2 ((x-meshSimplificationIncrement) / (float)mapLength, (y-meshSimplificationIncrement) / (float)mapLength);
				float height = terrainData.meshHeightCurve.Evaluate (heightMap [x, y]) * terrainData.meshHeightMultiplier;
				Vector3 vertexPosition = new Vector3 (topLeftX + percent.x * mapLength, height, topLeftZ - percent.y * mapLength);

				if( x < mapLength - 1 && y < mapLength - 1){
					int a = vertexIndicesMap [x, y];
					int b = vertexIndicesMap [x + meshSimplificationIncrement, y];
					int c = vertexIndicesMap [x, y + meshSimplificationIncrement];
					int d = vertexIndicesMap [x + meshSimplificationIncrement, y + meshSimplificationIncrement];
					meshData.AddTriangle (a,d,c);
					meshData.AddTriangle (d,a,b);
					meshData.AddVertex (vertexPosition, percent, vertexIndex);
				}
				vertexIndex++;
			}
		}
		meshData.Compile();
		return meshData;
	}
}

public class MeshData {
	Vector3[] vertices;
	Vector3[] normals;
	Vector2[] uvs;

	int[] triangles;

	int triangleIndex;

	NormalMode normalMode;

	public MeshData(int verticesPerLine, NormalMode normalMode) {
		vertices = new Vector3[verticesPerLine * verticesPerLine];
		uvs = new Vector2[verticesPerLine * verticesPerLine];
		triangles = new int[(verticesPerLine-1)*(verticesPerLine-1)*6];

		this.normalMode = normalMode;
	}

	public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex) {
		vertices [vertexIndex] = vertexPosition;
		uvs [vertexIndex] = uv;
	}

	public void AddTriangle(int a, int b, int c) {
		triangles [triangleIndex++] = a;
		triangles [triangleIndex++] = b;
		triangles [triangleIndex++] = c;
		
	}

	void FlatShading(){
		Vector3[] flatVertices = new Vector3[triangles.Length];
		Vector2[] flatUvs = new Vector2[triangles.Length];

		for(int i = 0; i< triangles.Length; i++){
			flatVertices[i] = vertices[triangles[i]];
			flatUvs[i] = uvs[triangles[i]];
			triangles[i] = i;
		}

		vertices = flatVertices;
		uvs = flatUvs;
	}

	Vector3[] CalculateNormals() {

		Vector3[] vertexNormals = new Vector3[vertices.Length];
		int triangleCount = triangles.Length / 3;
		for (int i = 0; i < triangleCount; i++) {
			int normalTriangleIndex = i * 3;
			int vertexIndexA = triangles [normalTriangleIndex];
			int vertexIndexB = triangles [normalTriangleIndex + 1];
			int vertexIndexC = triangles [normalTriangleIndex + 2];

			Vector3 triangleNormal = SurfaceNormalFromIndices (vertexIndexA, vertexIndexB, vertexIndexC);
			vertexNormals [vertexIndexA] += triangleNormal;
			vertexNormals [vertexIndexB] += triangleNormal;
			vertexNormals [vertexIndexC] += triangleNormal;
		}

		for (int i = 0; i < vertexNormals.Length; i++) vertexNormals [i].Normalize ();

		return vertexNormals;

	}

	Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC) {
		return Vector3.Cross (vertices[indexB] - vertices[indexA], vertices[indexC] - vertices[indexA]).normalized;
	}

	public void Compile(){
		switch(normalMode){
			case NormalMode.Smooth:
				BakedNormals();
				break;
			case NormalMode.Flat:
				FlatShading();
				break;
		}
	}


	void BakedNormals(){
		normals = CalculateNormals();
	}

	public Mesh CreateMesh() {
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;

		switch(normalMode){
			case NormalMode.Smooth:
				mesh.normals = normals;
				break;
			case NormalMode.Flat:
				mesh.RecalculateNormals();
				break;
		}
		
		return mesh;
	}

}

public enum NormalMode {Smooth, Flat};