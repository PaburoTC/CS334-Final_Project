using UnityEngine;
using System.Collections;

public static class FalloffGenerator {

	public static float[,] GenerateFalloffMap(int size) {
		float[,] map = new float[size,size];
		float x, y;

		for (int i = 0; i < size; i++) {
			x = i / (float)size * 2 - 1;
			for (int j = 0; j < size; j++) {
				
				y = j / (float)size * 2 - 1;
				map [i, j] = Sigmoid(Mathf.Max (Mathf.Abs (x), Mathf.Abs (y)));
			}
		}

		return map;
	}

	static float Sigmoid(float value) {
		float a = 3;
		float b = 2.2f;

		return Mathf.Pow (value, a) / (Mathf.Pow (value, a) + Mathf.Pow (b - b * value, a));
	}
}
