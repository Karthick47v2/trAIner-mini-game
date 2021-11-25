using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using Cysharp.Threading.Tasks;

namespace TensorFlowLite{
    public class PoseModel : BaseImagePredictor<sbyte>{
        public enum Part{
            NOSE, LEFT_EYE, RIGHT_EYE, LEFT_EAR, RIGHT_EAR, LEFT_SHOULDER, RIGHT_SHOULDER, LEFT_ELBOW, RIGHT_ELBOW, 
            LEFT_WRIST, RIGHT_WRIST, LEFT_HIP, RIGHT_HIP, LEFT_KNEE, RIGHT_KNEE, LEFT_ANKLE, RIGHT_ANKLE
        }

        public readonly Part[,] Connections = new Part[,]{
            { Part.NOSE, Part.LEFT_SHOULDER },
            { Part.NOSE, Part.RIGHT_SHOULDER },
            { Part.LEFT_HIP, Part.LEFT_SHOULDER },
            { Part.LEFT_ELBOW, Part.LEFT_SHOULDER },
            { Part.LEFT_ELBOW, Part.LEFT_WRIST },
            { Part.LEFT_HIP, Part.LEFT_KNEE },
            { Part.LEFT_KNEE, Part.LEFT_ANKLE },
            { Part.RIGHT_HIP, Part.RIGHT_SHOULDER },
            { Part.RIGHT_ELBOW, Part.RIGHT_SHOULDER },
            { Part.RIGHT_ELBOW, Part.RIGHT_WRIST },
            { Part.RIGHT_HIP, Part.RIGHT_KNEE },
            { Part.RIGHT_KNEE, Part.RIGHT_ANKLE },
            { Part.LEFT_SHOULDER, Part.RIGHT_SHOULDER },
            { Part.LEFT_HIP, Part.RIGHT_HIP }
        };

        [System.Serializable]
        public readonly struct Result{
            public readonly float x;
            public readonly float y;
            public readonly float confidence;

            public Result(float x, float y, float confidence){
                this.x = x;
                this.y = y;
                this.confidence = confidence;
            }
        }

        private readonly float[,] outputs;
        public readonly Result[] results;

        public PoseModel(string modelPath) : base(modelPath, false){
            var dim = interpreter.GetOutputTensorInfo(0).shape;

            Assert.AreEqual(1, dim[0]);
            Assert.AreEqual(1, dim[1]);
            Assert.AreEqual(17, dim[2]);
            Assert.AreEqual(3, dim[3]);

            outputs = new float[dim[2], dim[3]];
            results = new Result[dim[2]];
        }

        public override void Invoke(Texture inputTex){
            ToTensor(inputTex, input0);

            interpreter.SetInputTensorData(0, input0);
            interpreter.Invoke();
            interpreter.GetOutputTensorData(0, outputs);
        }

        public Result[] GetResults(){
            for (int i = 0; i < results.Length; i++){
                results[i] = new Result(
                    x: outputs[i, 1],
                    y: outputs[i, 0],
                    confidence: outputs[i, 2]
                );
            }
            return results;
        }
    }
}
