using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;

public class Menu : MonoBehaviour{

    public int textureLayerIndex;
    
    public Dropdown drawModeDropdown;
    public Dropdown normalDropdown;
    public Dropdown textureLayerDropdown;

    public Slider lodSlider;
    public Slider heightMultiplierSlider;
    public Slider octavesSlider;
    public Slider persistanceSlider;
    public Slider lacunaritySlider;
    public Slider scaleSlider;
    public Slider seedSlider;
    public Slider xOffsetSlider;
    public Slider yOffsetSlider;
    public Slider textureScaleSlider;
    public Slider blendStrengthSlider;
    public Slider startHeightSlider;
    public Slider colorStrengthSlider;
    public Slider redColorSlider;
    public Slider greenColorSlider;
    public Slider blueColorSlider;
    
    public GameObject noiseOptions;
    public GameObject meshOptions;
    public GameObject textureOptions;

    public GameObject lodSliderText;
    public GameObject heightMultiplierSliderText;
    public GameObject octavesSliderText;
    public GameObject persistanceSliderText;
    public GameObject lacunaritySliderText;
    public GameObject scaleSliderText;
    public GameObject seedSliderText;
    public GameObject xOffsetSliderText;
    public GameObject yOffsetSliderText;
    public GameObject textureScaleSliderText;
    public GameObject blendStrengthSliderText;
    public GameObject startHeightSliderText;
    public GameObject colorStrengthSliderText;
    public GameObject redColorSliderText;
    public GameObject greenColorSliderText;
    public GameObject blueColorSliderText;

    public NoiseData noiseData;
	public TerrainData terrainData;
	public TextureData textureData;
	public DrawMode drawMode;

    public MapGenerator mapGenerator;
    
    void Start(){
        showMenus();

        textureLayerIndex = 0;

        lodSlider.onValueChanged.AddListener(LODSelected);
        heightMultiplierSlider.onValueChanged.AddListener(HeightMultiplierSelected);
        octavesSlider.onValueChanged.AddListener(OctavesSelected);
        persistanceSlider.onValueChanged.AddListener(PersistanceSelected);
        lacunaritySlider.onValueChanged.AddListener(LacunaritySelected);
        scaleSlider.onValueChanged.AddListener(ScaleSelected);
        seedSlider.onValueChanged.AddListener(SeedSelected);
        xOffsetSlider.onValueChanged.AddListener(XOffsetSelected);
        yOffsetSlider.onValueChanged.AddListener(YOffsetSelected);
        textureScaleSlider.onValueChanged.AddListener(TextureScaleSelected);
        blendStrengthSlider.onValueChanged.AddListener(BlendStrengthSelected);
        startHeightSlider.onValueChanged.AddListener(StartHeightSelected);
        colorStrengthSlider.onValueChanged.AddListener(ColorStrengthSelected);
        redColorSlider.onValueChanged.AddListener(RedColorSelected);
        greenColorSlider.onValueChanged.AddListener(GreenColorSelected);
        blueColorSlider.onValueChanged.AddListener(BlueColorSelected);


        lodSlider.value = terrainData.LOD;
        heightMultiplierSlider.value = terrainData.meshHeightMultiplier;
        octavesSlider.value = noiseData.octaves;
        persistanceSlider.value = noiseData.persistance;
        lacunaritySlider.value = noiseData.lacunarity;
        scaleSlider.value = noiseData.scale;
        seedSlider.value = noiseData.seed;
        xOffsetSlider.value = noiseData.offset.x;
        yOffsetSlider.value = noiseData.offset.y;

        PopulateDrawModeDropdown();
        PopulateNormalDropdown();
        PopulateTextureLayerDropdown();

        UpdateLayerData(textureData.layers[textureLayerIndex]);
    }

    void showMenus(){
        bool mesh = drawMode == DrawMode.Mesh;
        noiseOptions.SetActive(!mesh);
        meshOptions.SetActive(mesh);
        textureOptions.SetActive(mesh || drawMode == DrawMode.ColorMap);
    }

    void PopulateTextureLayerDropdown(){
        List<string> options = new List<String>();
        for(int i = 0; i < textureData.layers.Length; i++) options.Add(string.Format("Layer {0}: {1}", i, textureData.layers[i].name));
        textureLayerDropdown.AddOptions(options);
        textureLayerDropdown.value = textureLayerIndex;
    }

    void UpdateLayerData(Layer layer){
        textureScaleSlider.value = layer.textureScale;
        blendStrengthSlider.value = layer.blendStrength;
        startHeightSlider.value = layer.startHeight;
        colorStrengthSlider.value = layer.colorStrength;
        redColorSlider.value = layer.color.r;
        greenColorSlider.value = layer.color.g;
        blueColorSlider.value = layer.color.b;
    }

    public void TextureLayerSelected(int index){
        textureLayerIndex = index;
        UpdateLayerData(textureData.layers[index]);
    }

    void PopulateNormalDropdown(){
        string[] enumOptions = Enum.GetNames(typeof(NormalMode));
        List<string> options = new List<String>(enumOptions);
        normalDropdown.AddOptions(options);
    }

    public void NormalDropdownSelected(int index){
        terrainData.normalMode = (NormalMode)index;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    void PopulateDrawModeDropdown(){
        string[] enumOptions = Enum.GetNames(typeof(DrawMode));
        List<string> options = new List<String>(enumOptions);
        drawModeDropdown.AddOptions(options);
    }

    public void DrawModeDropdownSelected(int index){
        drawMode = (DrawMode)index;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
        showMenus();
    }

    public void FallOffSelected(bool selected){
        terrainData.useFalloff ^= selected;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void LODSelected(float selected){
        int LOD = (int)selected;
        terrainData.LOD = LOD;
        lodSliderText.GetComponent<Text>().text = string.Format("Level of Detail: {0}", LOD);
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void HeightMultiplierSelected(float selected){
        int value = (int)selected;
        heightMultiplierSliderText.GetComponent<Text>().text = string.Format("Height Multiplier: {0}", value);
        terrainData.meshHeightMultiplier = value;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);;
    }

    public void OctavesSelected(float selected){
        int value = (int)selected;
        octavesSliderText.GetComponent<Text>().text = string.Format("Number of Octaves: {0}", value);
        noiseData.octaves = value;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void PersistanceSelected(float selected){
        persistanceSliderText.GetComponent<Text>().text = string.Format("Persistance: {0}", Math.Round(selected, 2));
        noiseData.persistance = selected;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void LacunaritySelected(float selected){
        lacunaritySliderText.GetComponent<Text>().text = string.Format("Lacunarity: {0}", Math.Round(selected, 2));
        noiseData.lacunarity = selected;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void ScaleSelected(float selected){
        scaleSliderText.GetComponent<Text>().text = string.Format("Scale: {0}", (int)selected);
        noiseData.scale = selected;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void SeedSelected(float selected){
        int value = (int)selected;
        seedSliderText.GetComponent<Text>().text = string.Format("Seed: {0}", value);
        noiseData.seed = value;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void XOffsetSelected(float selected){
        xOffsetSliderText.GetComponent<Text>().text = string.Format("X Offset: {0}", (int)selected);
        noiseData.offset.x = selected;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void YOffsetSelected(float selected){
        yOffsetSliderText.GetComponent<Text>().text = string.Format("Y Offset: {0}", (int)selected);
        noiseData.offset.y = selected;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void TextureScaleSelected(float selected){
        textureScaleSliderText.GetComponent<Text>().text = string.Format("Texture scale: {0}", (int)selected);
        textureData.layers[textureLayerIndex].textureScale = selected;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void BlendStrengthSelected(float selected){
        blendStrengthSliderText.GetComponent<Text>().text = string.Format("Blend Strength: {0}", Math.Round(selected, 3));
        textureData.layers[textureLayerIndex].blendStrength = selected;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void StartHeightSelected(float selected){
        startHeightSliderText.GetComponent<Text>().text = string.Format("Start Height: {0}", Math.Round(selected, 3));
        textureData.layers[textureLayerIndex].startHeight = selected;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void ColorStrengthSelected(float selected){
        colorStrengthSliderText.GetComponent<Text>().text = string.Format("Color Strength: {0}", Math.Round(selected, 3));
        textureData.layers[textureLayerIndex].colorStrength = selected;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void RedColorSelected(float selected){
        redColorSliderText.GetComponent<Text>().text = string.Format("Red: {0}", (int)(selected * 255));
        textureData.layers[textureLayerIndex].color.r = selected;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void GreenColorSelected(float selected){
        greenColorSliderText.GetComponent<Text>().text = string.Format("Green: {0}", (int)(selected * 255));
        textureData.layers[textureLayerIndex].color.g = selected;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void BlueColorSelected(float selected){
        blueColorSliderText.GetComponent<Text>().text = string.Format("Blue: {0}", (int)(selected * 255));
        textureData.layers[textureLayerIndex].color.b = selected;
        mapGenerator.SetData(noiseData, terrainData, textureData, drawMode);
    }

    public void QuitGame(){
        Application.Quit();
    }

}
