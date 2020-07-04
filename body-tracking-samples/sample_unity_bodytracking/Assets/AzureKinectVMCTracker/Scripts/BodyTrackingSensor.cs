using System;
using UnityEngine;

namespace AzureKinectVMCTracker
{
    [RequireComponent(typeof(ConfigLoader))]
    public class BodyTrackingSensor : MonoBehaviour
    {
        private BackgroundDataProvider m_backgroudProvider;
        private BackgroundData m_backgroudData;

        [SerializeField] private Transform _head, _leftHand, _rightHand, _hip, _leftFoot, _rightFoot;

        private void Start()
        {
            SkeletalTrackingProvider m_skeletalTracking = new SkeletalTrackingProvider();

            const int TRACKING_ID = 0;
            m_skeletalTracking.StartClientThread(TRACKING_ID);
            m_backgroudProvider = m_skeletalTracking;
        }

        private void Update()
        {
            if (m_backgroudProvider.IsRunning)
            {
                if (m_backgroudProvider.GetCurrentFrameData(ref m_backgroudData))
                {
//                    m_backgroudData.Bodies[0].
                }
            }
        }

        private void OnApplicationQuit()
        {
            m_backgroudProvider?.StopClientThread();
        }
    }
}