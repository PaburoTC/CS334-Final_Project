using UnityEngine;
using System.Collections;

public static class TextureGenerator {

	public static Texture2D FromColorMap(Color[] colourMap, int size) {
		Texture2D texture = new Texture2D (size, size);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels (colourMap);
		texture.Apply ();
		return texture;
	}


	public static Texture2D FromHeightMap(float[,] heightMap) {
		int size = heightMap.GetLength (0);

		Color[] colorMap = new Color[size * size];
		for (int y = 0; y < size; y++) {
			for (int x = 0; x < size; x++) {
				colorMap [(size -y -1) * size + x] = Color.Lerp (Color.black, Color.white, heightMap [x, y]);
			}
		}

		return FromColorMap (colorMap, size);
	}

	public static Texture2D ColorFromHeightMap(float[, ] heightMap, TextureData textureData){
		int size = heightMap.GetLength (0);

		Color[] colorMap = new Color[size * size];
		for (int y = size -1; y >= 0; y--) {
			for (int x = 0; x < size; x++) {
				bool found = false;
				for(int i = 0; i < textureData.layers.Length; i++){
					if(heightMap[x,y] < textureData.layers[i].startHeight){
						if (i == 0) colorMap[(size -y -1) * size + x] = new Color(0,0,0);
						else colorMap [(size -y -1) * size + x] = textureData.layers[i-1].color;
							
						found = true;
						break;
					}
				}
				if(!found) colorMap [(size -y -1) * size + x] = textureData.layers[textureData.layers.Length-1].color;
				
			}
		}

		return FromColorMap (colorMap, size);
	}

}
