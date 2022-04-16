using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	static int mapChunkSize = 241;

	float[,] heightMap;
	float[,] falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);

	public NoiseData noiseData;
	public TerrainData terrainData;
	public TextureData textureData;
	public DrawMode drawMode;
	public Material terrainMat;

	public void SetData(NoiseData noiseData, TerrainData terrainData, TextureData textureData, DrawMode drawMode){
		this.noiseData = noiseData;
		this.terrainData = terrainData;
		this.textureData = textureData;
		this.drawMode = drawMode;
		mapChunkSize = this.terrainData.normalMode == NormalMode.Flat && this.drawMode == DrawMode.Mesh ? 97 : 241;
		this.textureData.ApplyToMaterial(terrainMat);
		DrawMap();

	}

	public void DrawMap() {
		heightMap = GenerateNoiseMap(Vector2.zero);
		MapDisplay display = FindObjectOfType<MapDisplay> ();
		switch (drawMode){
			case DrawMode.NoiseMap:
				display.DrawTexture(TextureGenerator.TextureFromHeightMap(heightMap));
				break;
			case DrawMode.ColorMap:
				display.DrawTexture(TextureGenerator.TextureColorFromHeightMap(heightMap, textureData));
				break;
			case DrawMode.Mesh:
				display.DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap, terrainData));
				break;
			case DrawMode.FallOffMap:
				display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize)));
				break;
		}
	}

	float[,] GenerateNoiseMap(Vector2 centre) {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, noiseData);
		if (terrainData.useFalloff){
			falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
		
			for (int y = 0; y < mapChunkSize; y++) {
				for (int x = 0; x < mapChunkSize; x++) {
					noiseMap [x, y] = Mathf.Clamp01(noiseMap [x, y] - falloffMap [x, y]);	
				}
			}
		}
		textureData.UpdateMeshHeights(terrainMat, terrainData.minHeight, terrainData.maxHeight);

		return noiseMap;
	}
}

public enum DrawMode {NoiseMap, ColorMap, FallOffMap, Mesh};
