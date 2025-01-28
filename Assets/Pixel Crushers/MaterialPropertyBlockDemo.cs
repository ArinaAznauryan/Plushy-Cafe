using UnityEngine;
using UnityEngine.Rendering;

//[ExecuteAlways] // this allows for animated properties preview in Timeline when in Edit Mode
[AddComponentMenu("Demo/Material Property Block")]
public class MaterialPropertyBlockDemo : MonoBehaviour
{
    [SerializeField, ColorUsage(true, true)] Color _OutlineColor;
    [SerializeField] float _OutlineWidth = 1f;

    Renderer _renderer;
    MaterialPropertyBlock _materialPropertyBlock;
    Material _material;

    int _outlineColor_id, _outlineWidth_id;

    public void Start() {
        Init();
    }

    public void Enable()
    {
        UpdateMaterialProperties();
    }

    public void Disable() 
    {
        ResetMaterialProperties();
    }

    void ResetMaterialProperties()
    {
        _materialPropertyBlock.SetColor(_outlineColor_id, new Color32(149, 23, 86, 255));
        _materialPropertyBlock.SetFloat(_outlineWidth_id, .15f);

        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }

    void UpdateMaterialProperties()
    {
        _materialPropertyBlock.SetColor(_outlineColor_id, new Color32(98, 255, 40, 255));
        _materialPropertyBlock.SetFloat(_outlineWidth_id, 5f);

        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }

    private void OnValidate()
    {
        Init();
        UpdateMaterialProperties();
    }

    private void Reset()
    {
        Init();
        _material = _renderer.material;
        UpdateMaterialProperties();
    }


    private void Init()
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
            if (_materialPropertyBlock == null)
                _materialPropertyBlock = new MaterialPropertyBlock();

        // Using property identifiers is more efficient than passing strings to all material property functions.
        // https://docs.unity3d.com/ScriptReference/Shader.PropertyToID.html

        _outlineColor_id = Shader.PropertyToID("_OutlineColor");
        _outlineWidth_id = Shader.PropertyToID("_OutlineWidth");

        ResetMaterialProperties();
    }
}
