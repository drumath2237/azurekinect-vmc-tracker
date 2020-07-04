using System;
using UnityEngine;

namespace AzureKinectVMCTracker
{
    [RequireComponent(typeof(ConfigLoader))]
    public class BodyTrackingSensor : MonoBehaviour
    {
        private BackgroundDataProvider _mBackgroudProvider;
        private BackgroundData _mBackgroudData = new BackgroundData();

        [SerializeField] private Transform _head, _leftHand, _rightHand, _hip, _leftFoot, _rightFoot;

        private void Start()
        {
            SkeletalTrackingProvider mSkeletalTracking = new SkeletalTrackingProvider();

            const int TRACKING_ID = 0;
            mSkeletalTracking.StartClientThread(TRACKING_ID);
            _mBackgroudProvider = mSkeletalTracking;
        }

        private void Update()
        {
            if (_mBackgroudProvider.IsRunning)
            {
                if (_mBackgroudProvider.GetCurrentFrameData(ref _mBackgroudData))
                {
                    if (_mBackgroudData.NumOfBodies != 0)
                    {
                        var v = new Vector3(
                            _mBackgroudData.Bodies[0].JointPositions3D[0].X,
                            _mBackgroudData.Bodies[0].JointPositions3D[0].Y,
                            _mBackgroudData.Bodies[0].JointPositions3D[0].Z
                        );
                        _head.position = v*10f;
                    }
                }
            }
        }

        private void OnApplicationQuit()
        {
            _mBackgroudProvider?.StopClientThread();
        }
    }
}