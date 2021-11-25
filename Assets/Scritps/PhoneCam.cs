using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace TensorFlowLite{
    public class PhoneCam : MonoBehaviour{

        [System.Serializable]
        public class TextureUpdateEvent : UnityEvent<Texture> { }
        public TextureUpdateEvent OnTextureUpdate = new TextureUpdateEvent();
        private bool isCamAvail;
        private WebCamTexture frontCam; 
        private Texture defaultBG;
        public RawImage bg;
        public AspectRatioFitter fitter;
        private TextureResizer resizer;

        private void Start(){
            defaultBG = bg.texture;
            WebCamDevice[] devices = WebCamTexture.devices;
            resizer = new TextureResizer();

            if(devices.Length == 0){
                Debug.Log("No cam detected");
                isCamAvail = false;
                return;
            }

            for(int i = 0; i < devices.Length; i++){
                if(devices[i].isFrontFacing){
                    frontCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                }
            }

            if(frontCam == null){
                Debug.Log("Unable to find front cam");
                return;
            }

            frontCam.Play();
            bg.texture = frontCam;

            isCamAvail = true;
        }

        private void Update(){
            if(!isCamAvail) return;

            // bool isPortrait = IsPortrait(texture);
            // if (isPortrait)
            // {
            //     (cameraWidth, cameraHeight) = (cameraHeight, cameraWidth); // swap
            // }

            float ratio = (float) frontCam.width / (float) frontCam.height;
            fitter.aspectRatio = ratio;

            float scaleY = frontCam.videoVerticallyMirrored ? -1f : 1f;

            bg.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

            int orient =  -frontCam.videoRotationAngle;

            bg.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

            Matrix4x4 mtx;
            Vector4 uvRect;

            mtx = TextureResizer.GetVertTransform(orient, frontCam.videoVerticallyMirrored, true);
            uvRect = TextureResizer.GetTextureST(ratio, ratio, AspectMode.Fill);

            
            var tex = resizer.Resize(frontCam, frontCam.width, frontCam.height, false, mtx, uvRect);;
            OnTextureUpdate.Invoke(tex);
        }

        private void OnDestroy(){
            if (frontCam == null) return;
            frontCam.Stop();
            Destroy(frontCam);

            resizer?.Dispose();
        }

        // private bool IsPortrait(WebCamTexture texture){
        //     return texture.videoRotationAngle == 90 || texture.videoRotationAngle == 270;
        // }


    
    }

}