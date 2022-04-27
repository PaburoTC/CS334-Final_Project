using UnityEngine;
using System.Collections;

public static class Noise{

	public static float[,] NoiseMap(int mapLength, NoiseData noiseData) {
		
		float maxHeight = float.MinValue;
		float minHeight = float.MaxValue;

		float halfLength = mapLength / 2f;
		float[,] noiseMap = new float[mapLength, mapLength];

		System.Random prng = new System.Random(noiseData.seed);
		Vector2[] octaveOffsets = new Vector2[noiseData.octaves];
		
		for (int i = 0; i < noiseData.octaves; i++) octaveOffsets [i] = new Vector2 (prng.Next(-100000, 100000) + noiseData.offset.x, prng.Next(-100000, 100000) - noiseData.offset.y);
		
		for (int y = 0; y < mapLength; y++) {
			for (int x = 0; x < mapLength; x++) {

				float amplitude = 1;
				float frequency = 1;
				float height = 0;

				for (int i = 0; i < noiseData.octaves; i++) {
					float perlinValue = Mathf.PerlinNoise ((x-halfLength + octaveOffsets[i].x) / noiseData.scale * frequency, (y-halfLength + octaveOffsets[i].y) / noiseData.scale * frequency) * 2 - 1;
					height += perlinValue * amplitude;

					amplitude *= noiseData.persistance;
					frequency *= noiseData.lacunarity;
				}

				if (height > maxHeight) maxHeight = height;
				else if (height < minHeight) minHeight = height;

				noiseMap [x, y] = height;
			}
		}

		for (int y = 0; y < mapLength; y++)
			for (int x = 0; x < mapLength; x++) noiseMap [x, y] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap [x, y]);
			
		return noiseMap;
	}

}
