using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class NoiseData : ScriptableObject{
    
    public int seed;
	public int octaves;

	[Range(0,1)]
	public float persistance;
	public float lacunarity;
    public float scale;

	public Vector2 offset;
}
