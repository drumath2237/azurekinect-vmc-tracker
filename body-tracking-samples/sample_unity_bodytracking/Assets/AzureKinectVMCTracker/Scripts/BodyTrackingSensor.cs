using System;
using UnityEngine;

namespace AzureKinectVMCTracker
{
    [RequireComponent(typeof(ConfigLoader))]
    public class BodyTrackingSensor : MonoBehaviour
    {
        private BackgroundDataProvider _mBackgroudProvider;
        private BackgroundData _mBackgroudData;

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
//                    m_backgroudData.Bodies[0].
                }
            }
        }

        private void OnApplicationQuit()
        {
            _mBackgroudProvider?.StopClientThread();
        }
    }
}