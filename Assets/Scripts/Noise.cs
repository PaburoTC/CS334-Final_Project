using UnityEngine;
using System.Collections;

public static class Noise{

	public static float[,] GenerateNoiseMap(int mapLength, NoiseData noiseData) {
		float[,] noiseMap = new float[mapLength, mapLength];

		System.Random prng = new System.Random(noiseData.seed);
		Vector2[] octaveOffsets = new Vector2[noiseData.octaves];
		
		for (int i = 0; i < noiseData.octaves; i++) {
			float offsetX = prng.Next (-100000, 100000) + noiseData.offset.x;
			float offsetY = prng.Next (-100000, 100000) - noiseData.offset.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);
		}

		float maxLocalNoiseHeight = float.MinValue;
		float minLocalNoiseHeight = float.MaxValue;

		float halfLength = mapLength / 2f;

		for (int y = 0; y < mapLength; y++) {
			for (int x = 0; x < mapLength; x++) {

				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < noiseData.octaves; i++) {
					float sampleX = (x-halfLength + octaveOffsets[i].x) / noiseData.scale * frequency;
					float sampleY = (y-halfLength + octaveOffsets[i].y) / noiseData.scale * frequency;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= noiseData.persistance;
					frequency *= noiseData.lacunarity;
				}

				if (noiseHeight > maxLocalNoiseHeight) maxLocalNoiseHeight = noiseHeight;
				else if (noiseHeight < minLocalNoiseHeight) minLocalNoiseHeight = noiseHeight;

				noiseMap [x, y] = noiseHeight;
			}
		}

		for (int y = 0; y < mapLength; y++) {
			for (int x = 0; x < mapLength; x++) {
				noiseMap [x, y] = Mathf.InverseLerp (minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap [x, y]);
			}
		}

		return noiseMap;
	}

}
