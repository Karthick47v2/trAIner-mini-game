using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TensorFlowLite;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(WebCamInput))]
public class Visual : MonoBehaviour{
    [SerializeField, FilePopup("*.tflite")]
    private string fileName = default;

    [SerializeField]
    private RawImage cameraView = null;

    [SerializeField, Range(0, 1)]
    public float threshold = 0.3f;

    public PoseModel poseModel;
    private readonly Vector3[] rtCorners = new Vector3[4];
    private PrimitiveDraw draw;
    public PoseModel.Result[] results;
    public GameObject interruptPanel;

    public float x = 0;
    public float conf = 0;


    private void Start(){
        poseModel = new PoseModel(fileName);
        draw = new PrimitiveDraw(Camera.main, gameObject.layer){
            color = Color.green,
        };

        var WebCamInput = GetComponent<WebCamInput>();
        WebCamInput.OnTextureUpdate.AddListener(OnTextureUpdate);
    }

    private void OnDestroy(){
        var WebCamInput = GetComponent<WebCamInput>();
        WebCamInput.OnTextureUpdate.RemoveListener(OnTextureUpdate);
        poseModel?.Dispose();
        draw?.Dispose();
    }

    private void Update(){
        DrawResult(results);
    }

    private void OnTextureUpdate(Texture texture){
        Invoke(texture);
    }

    private void Invoke(Texture texture){
        poseModel.Invoke(texture);
        results = poseModel.GetResults();
        cameraView.material = poseModel.transformMat;
    }

    private void DrawResult(PoseModel.Result[] results){
        if (results == null || results.Length == 0) return;

        var rect = cameraView.GetComponent<RectTransform>();
        rect.GetWorldCorners(rtCorners);
        Vector3 min = rtCorners[0];
        Vector3 max = rtCorners[2];

        var connections = poseModel.Connections;
        int len = connections.GetLength(0);

        bool allVisible = true;

        for(int i = 0; i < len; i++){
            if(results[(int)connections[i,0]].confidence < threshold || results[(int)connections[i,1]].confidence < threshold){
                allVisible = false;
                break;
            }
        }

        // if(!allVisible){
        //     interruptPanel.SetActive(true);
        // }
        // else{
        //    interruptPanel.SetActive(false);
            for (int i = 0; i < len; i++){
            var a = results[(int)connections[i, 0]];
            var b = results[(int)connections[i, 1]];
            if (a.confidence >= threshold && b.confidence >= threshold){
                draw.Line3D(
                    MathTF.Lerp(min, max, new Vector3(a.x, 1f - a.y, 0)),
                    MathTF.Lerp(min, max, new Vector3(b.x, 1f - b.y, 0)),
                    1
                );
            }
        }
        x = (results[(int)connections[0,0]]).y;
        conf = (results[(int)connections[0,0]]).confidence;
        // }
        draw.Apply();
    }
}
