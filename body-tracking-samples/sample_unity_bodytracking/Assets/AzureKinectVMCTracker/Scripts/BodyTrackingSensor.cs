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

        [SerializeField] private float rotation_x_bias;
        [SerializeField] private float rotation_y_bias;
        [SerializeField] private float rotation_z_bias;
        [SerializeField] private float rotation_w_bias;


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
                                                       -_mBackgroudData.Bodies[0].JointPositions3D[(int) tracker.Value]
                                                           .X,
                                                       -_mBackgroudData.Bodies[0].JointPositions3D[(int) tracker.Value]
                                                           .Y,
                                                       -_mBackgroudData.Bodies[0].JointPositions3D[(int) tracker.Value].Z
                                                   ) * 10f;
                            
                            if (tracker.Value == JointId.HandLeft || tracker.Value == JointId.HandRight)
                            {
                                tracker.Key.rotation = Quaternion.LookRotation(tracker.Key.position - _hip.position);
                                tracker.Key.Rotate(new Vector3(0,0,1), 90f);
                                continue;
                            }

                            tracker.Key.rotation = new Quaternion(
                                                       _mBackgroudData.Bodies[0].JointRotations[(int) tracker.Value].X *
                                                       rotation_x_bias,
                                                       _mBackgroudData.Bodies[0].JointRotations[(int) tracker.Value].Y *
                                                       rotation_y_bias,
                                                       _mBackgroudData.Bodies[0].JointRotations[(int) tracker.Value].Z *
                                                       rotation_z_bias,
                                                       _mBackgroudData.Bodies[0].JointRotations[(int) tracker.Value].W *
                                                       rotation_w_bias
                                                   ) * Quaternion.AngleAxis(180, new Vector3(0, 1, 0));
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