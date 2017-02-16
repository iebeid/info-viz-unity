
namespace UALR_EAC
{
    using UnityEngine;
    using System.Collections;

    class InterleavedStereo : MonoBehaviour
    {
        public  Shader   InterleaveShader;
        private Material stereoMat_;

        private Camera camL_;
        private Camera camR_;

        void Awake()
        {
            if(!InterleaveShader)
            {
                Debug.Log("No 'InterleaveShader' set, using default.");
                InterleaveShader = Shader.Find("Hidden/InterleavedStereoShader");
            }

            stereoMat_ = new Material(InterleaveShader);
        }

        void Start()
        {
            RenderTexture rtLeft  = new RenderTexture(Screen.width / 2, Screen.height, 24);
            RenderTexture rtRight = new RenderTexture(Screen.width / 2, Screen.height, 24);

            camL_ = GameObject.Find("LeftEyeCamera").GetComponent<Camera>();
            camR_ = GameObject.Find("RightEyeCamera").GetComponent<Camera>();

            camL_.targetTexture = rtLeft;
            camR_.targetTexture = rtRight;

            stereoMat_.SetTexture("_LeftEye",  camL_.targetTexture);
            stereoMat_.SetTexture("_RightEye", camR_.targetTexture);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            GL.PushMatrix();
            GL.LoadOrtho();

            RenderTexture.active = destination;
            stereoMat_.SetPass(0);

            GL.Begin(GL.TRIANGLES);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3  (0.0f, 0.0f, 0.1f);

            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3  (1.0f, 0.0f, 0.1f);

            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3  (1.0f, 1.0f, 0.1f);

            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3  (1.0f, 1.0f, 0.1f);

            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3  (0.0f, 1.0f, 0.1f);

            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3  (0.0f, 0.0f, 0.1f);
            GL.End();

            GL.PopMatrix();

            camL_.targetTexture.DiscardContents();
            camR_.targetTexture.DiscardContents();
        }
    }

} // namespace UALR_EAC