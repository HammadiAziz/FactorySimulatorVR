using UnityEngine;
using TMPro;
public class AnimationScript : MonoBehaviour
{
    public TMP_Text text;


    // Update is called once per frame
    void Update()
    {
        text.ForceMeshUpdate();
        var textinfo = text.textInfo;
        for (int i = 0; i < textinfo.characterCount; i++)
        {
            var charinfo = textinfo.characterInfo[i];
            if (!charinfo.isVisible)
            {
                continue;
            }
            var verts = textinfo.meshInfo[charinfo.materialReferenceIndex].vertices;
            for (int j = 0; j < 4; j++) 
            {
                var orig = verts[charinfo.vertexIndex + j];
                verts[charinfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time*1f+ orig.x * 0.1f)*0.1f, 0);

            }

        }
        for (int i = 0;i < textinfo.meshInfo.Length;i++)
        {
            var meshinfo = textinfo.meshInfo[i];
            meshinfo.mesh.vertices = meshinfo.vertices;
            text.UpdateGeometry(meshinfo.mesh,i);
        }
    }
}
