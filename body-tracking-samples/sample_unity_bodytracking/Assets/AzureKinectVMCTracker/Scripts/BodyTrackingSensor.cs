using System;
using System.Collections.Generic;
using Microsoft.Azure.Kinect.BodyTracking;
using UnityEditor.IMGUI.Controls;
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
                {_leftHand, JointId.WristLeft},
                {_rightHand, JointId.WristRight},
                {_hip, JointId.Pelvis},
                {_leftFoot, JointId.AnkleLeft},
                {_rightFoot, JointId.AnkleRight}
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
                            // tracker.Key.localScale = new Vector3(1f, 1f, 1f);
                            tracker.Key.position = new Vector3(
                                -_mBackgroudData.Bodies[0].JointPositions3D[(int) tracker.Value]
                                    .X,
                                -_mBackgroudData.Bodies[0].JointPositions3D[(int) tracker.Value]
                                    .Y,
                                -_mBackgroudData.Bodies[0].JointPositions3D[(int) tracker.Value].Z
                            );

                            tracker.Key.localRotation = new Quaternion(
                                -_mBackgroudData.Bodies[0]
                                    .JointRotations[(int) tracker.Value].X,
                                _mBackgroudData.Bodies[0]
                                    .JointRotations[(int) tracker.Value].Y,
                                -_mBackgroudData.Bodies[0]
                                    .JointRotations[(int) tracker.Value].Z,
                                _mBackgroudData.Bodies[0]
                                    .JointRotations[(int) tracker.Value].W
                            ) * ApplyIndividualRotationBias(tracker.Value);
                        }
                    }
                }
            }
        }

        Quaternion ApplyIndividualRotationBias(JointId bone)
        {
            switch (bone)
            {
                case JointId.Pelvis : case JointId.Head: case JointId.AnkleLeft:
                    return Quaternion.AngleAxis(-90, new Vector3(1, 0, 0))
                           * Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
                default:
                    return Quaternion.identity;
            }
        }

        private void OnApplicationQuit()
        {
            _mBackgroudProvider?.StopClientThread();
        }
    }
}