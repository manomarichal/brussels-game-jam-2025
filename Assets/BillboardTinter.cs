using System.Collections;
using UnityEngine;


[ExecuteInEditMode]
public class BillboardTinter : MonoBehaviour
{
    private MeshRenderer m_Renderer;
    public Color Color = Color.white;

    public AnimationCurve Flashing;
    private void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
    }



    private void SetColor(Color colour)
    {
        m_Renderer.material.SetColor("_tint", colour);
    }

    public void FlashRed()
    {
        StartCoroutine(_flashRed());
    }

    private IEnumerator _flashRed()
    {
        float timePass = 0;
        while (timePass < Flashing.length)
        {

            timePass += Time.deltaTime;
            SetColor(Color.Lerp(Color.white, Color.red, Flashing.Evaluate(timePass)));
            yield return null;
        }
        SetColor(Color);
    } 
}
