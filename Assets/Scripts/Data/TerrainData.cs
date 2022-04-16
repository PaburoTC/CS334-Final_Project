using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainData : UpdatableData{
    public bool useFalloff;
    

    [Range(0,4)]
	public int LOD;
    public readonly int scale = 10;
    public float meshHeightMultiplier;
    
    public float minHeight {
        get {
            return scale * meshHeightMultiplier * meshHeightCurve.Evaluate(0);
        }
    }

    public float maxHeight {
        get {
            return scale * meshHeightMultiplier * meshHeightCurve.Evaluate(1);
        }
    }

	public AnimationCurve meshHeightCurve;
    public NormalMode normalMode;

}
