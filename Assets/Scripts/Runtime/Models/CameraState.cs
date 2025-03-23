using System;
using System.Numerics;
using Runtime.Enums;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Runtime.Models
{
    [Serializable]
    public class CameraState
    {
        public GameStates GameState;
        public Vector3 Position;
        public Quaternion Rotation;
        public float FieldOfView;
        public bool IsOrthographic;
        public float OrthographicSize;
    }
}