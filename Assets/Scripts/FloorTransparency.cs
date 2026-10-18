using UnityEngine;

public class FloorTransparency : MonoBehaviour
{
    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor"); // URP
    static readonly int ColorId = Shader.PropertyToID("_Color");     // Built-in

    private Material transparentBase;
    private Renderer[] renderers;
    private Material[][] originals;
    private MaterialPropertyBlock block;
    private bool isTransparent;

    public void Init(Material transparentMaterial)
    {
        transparentBase = transparentMaterial;
        renderers = GetComponentsInChildren<Renderer>(true);
        originals = new Material[renderers.Length][];
        block = new MaterialPropertyBlock();
        for (int i = 0; i < renderers.Length; i++)
        {
            originals[i] = renderers[i].sharedMaterials;
        }
    }

    public void SetTransparent(bool on, float alpha)
    {
        if (renderers == null) return;
        if (!on && !isTransparent) return; // すでに不透明なら何もしない
        isTransparent = on;

        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer r = renderers[i];
            if (!on)
            {
                r.sharedMaterials = originals[i];
                r.SetPropertyBlock(null);
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                continue;
            }

            var mats = new Material[originals[i].Length];
            for (int j = 0; j < mats.Length; j++)
            {
                mats[j] = transparentBase;
            }
            r.sharedMaterials = mats;
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            for (int j = 0; j < originals[i].Length; j++)
            {
                Material src = originals[i][j];
                int id = src.HasProperty(BaseColorId) ? BaseColorId : ColorId;
                Color c = src.GetColor(id);
                c.a = alpha;
                block.Clear();
                block.SetColor(BaseColorId, c);
                block.SetColor(ColorId, c);
                r.SetPropertyBlock(block, j);
            }
        }
    }
}
