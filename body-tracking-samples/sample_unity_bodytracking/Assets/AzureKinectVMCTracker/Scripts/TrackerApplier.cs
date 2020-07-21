using System;
using UnityEngine;

namespace AzureKinectVMCTracker
{
    public class TrackerApplier : MonoBehaviour
    {
        private Animator _avatar;

        [SerializeField] private Transform _head;
        [SerializeField] private Transform _leftHand;
        [SerializeField] private Transform _righthand;
        [SerializeField] private Transform _Pelvis;
        [SerializeField] private Transform _leftFoot;
        [SerializeField] private Transform _rightFoot;

        private void Start()
        {
            _avatar = GetComponent<Animator>();
        }

        private void Update()
        {
            SetBoneTransform(HumanBodyBones.Head, _head);
            SetBoneTransform(HumanBodyBones.LeftHand, _leftHand);
            SetBoneTransform(HumanBodyBones.RightHand, _righthand);
            SetBoneTransform(HumanBodyBones.Spine, _Pelvis);
            SetBoneTransform(HumanBodyBones.LeftFoot, _leftFoot);
            SetBoneTransform(HumanBodyBones.RightFoot, _rightFoot);
        }

        void SetIK(AvatarIKGoal goal, Transform transform)
        {
            // _avatar.SetIKPosition(goal, transform.position);
            // _avatar.SetIKPositionWeight(goal, 1.0f);
            _avatar.SetIKRotation(goal, transform.rotation);
            _avatar.SetIKRotationWeight(goal, 1.0f);
        }

        void SetBoneTransform(HumanBodyBones bone, Transform _transform)
        {
            _avatar.GetBoneTransform(bone).rotation = _transform.rotation;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            // SetIK(AvatarIKGoal.LeftHand, _leftHand);
            // SetIK(AvatarIKGoal.RightHand, _righthand);
            // SetIK(AvatarIKGoal.LeftFoot, _leftFoot);
            // SetIK(AvatarIKGoal.RightFoot, _rightFoot);
        }
    }
}