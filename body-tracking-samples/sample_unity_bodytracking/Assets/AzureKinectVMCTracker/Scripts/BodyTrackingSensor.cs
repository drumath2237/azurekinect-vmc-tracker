using System;
using System.Collections.Generic;
using Microsoft.Azure.Kinect.BodyTracking;
using UnityEngine;

namespace AzureKinectVMCTracker
{
    [RequireComponent(typeof(ConfigLoader))]
    public class BodyTrackingSensor : MonoBehaviour
    {
        private BackgroundDataProvider _mBackgroudProvider;
        private BackgroundData _mBackgroudData = new BackgroundData();

        [SerializeField] private Transform _head, _leftHand, _rightHand, _hip, _leftFoot, _rightFoot;

        private Dictionary<Transform, JointId> _jointTable;

        private void Start()
        {
            SkeletalTrackingProvider mSkeletalTracking = new SkeletalTrackingProvider();

            const int TRACKING_ID = 0;
            mSkeletalTracking.StartClientThread(TRACKING_ID);
            _mBackgroudProvider = mSkeletalTracking;

            _jointTable = new Dictionary<Transform, JointId>()
            {
                {_head, JointId.Head},
                {_leftHand, JointId.HandLeft},
                {_rightHand, JointId.HandRight},
                {_hip, JointId.Pelvis},
                {_leftFoot, JointId.FootLeft},
                {_rightFoot, JointId.FootRight}
            };
        }

        private void Update()
        {
            if (_mBackgroudProvider.IsRunning)
            {
                if (_mBackgroudProvider.GetCurrentFrameData(ref _mBackgroudData))
                {
                    if (_mBackgroudData.NumOfBodies != 0)
                    {
                        foreach (var tracker in _jointTable)
                        {
                            tracker.Key.position = new Vector3(
                                _mBackgroudData.Bodies[0].JointPositions3D[(int) tracker.Value].X,
                                -_mBackgroudData.Bodies[0].JointPositions3D[(int) tracker.Value].Y,
                                _mBackgroudData.Bodies[0].JointPositions3D[(int) tracker.Value].Z
                            )*10f;
                        }
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