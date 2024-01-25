using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AvatarBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _rootObject;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        BuildHumanAvatarWithBoneMapping(_rootObject, 
                                        _animator, 
                                        _boneMapping);
    }

    private static void BuildHumanAvatarWithBoneMapping(GameObject gameObject, 
                                                        Animator animator, 
                                                        IReadOnlyDictionary<string, string> boneMapping)
    {
        var humanDescription = new HumanDescription
        {
            human = boneMapping.Select(mapping =>
            {
                
                var bone = new HumanBone { 
                                            humanName = mapping.Key, 
                                            boneName = mapping.Value 
                                         };
                bone.limit.useDefaultValues = true;
                return bone;

            }).ToArray(),
        };

        animator.avatar = UnityEngine.AvatarBuilder.BuildHumanAvatar(gameObject, 
                                                                        humanDescription);
    }
    
    private readonly IReadOnlyDictionary<string, string> _boneMapping = new Dictionary<string, string>()
    {
        {"Chest", "Chest_Jnt"},
        {"Left Little Proximal", "Left_PinkyFinger1_Jnt"},
        {"RightLowerArm", "Right_LowerArm_Jnt"},
        {"Spine", "Spine_Jnt"},
        {"Left Thumb Intermediate", "Left_ThumbFinger2_Jnt"},
        {"UpperChest", "UpperChest_Jnt"},
        {"Left Ring Intermediate", "Left_RingFinger2_Jnt"},
        {"Left Ring Proximal", "Left_RingFinger1_Jnt"},
        {"Right Index Intermediate", "Right_IndexFinger2_Jnt"},
        {"Right Index Proximal", "Right_IndexFinger1_Jnt"},
        {"Head", "Head_Jnt"},
        {"LeftLowerLeg", "Left_LowerLeg_Jnt"},
        {"Left Middle Proximal", "Left_MiddleFinger1_Jnt"},
        {"Left Thumb Proximal", "Left_ThumbFinger1_Jnt"},
        {"LeftShoulder", "Left_Shoulder_Jnt"},
        {"Left Index Proximal", "Left_IndexFinger1_Jnt"},
        {"RightUpperLeg", "Right_UpperLeg_Jnt"},
        {"LeftUpperArm", "Left_UpperArm_Jnt"},
        {"Right Thumb Distal", "Right_ThumbFinger3_Jnt"},
        {"Left Little Intermediate", "Left_PinkyFinger2_Jnt"},
        {"Left Little Distal", "Left_PinkyFinger3_Jnt"},
        {"Hips", "Hips_Jnt"},
        {"Left Index Distal", "Left_IndexFinger3_Jnt"},
        {"Left Ring Distal", "Left_RingFinger3_Jnt"},
        {"Right Middle Proximal", "Right_MiddleFinger1_Jnt"},
        {"LeftUpperLeg", "Left_UpperLeg_Jnt"},
        {"Right Little Distal", "Right_PinkyFinger3_Jnt"},
        {"Right Index Distal", "Right_IndexFinger3_Jnt"},
        {"Right Thumb Intermediate", "Right_ThumbFinger2_Jnt"},
        {"Right Little Intermediate", "Right_PinkyFinger2_Jnt"},
        {"Right Middle Intermediate", "Right_MiddleFinger2_Jnt"},
        {"RightLowerLeg", "Right_LowerLeg_Jnt"},
        {"Left Thumb Distal", "Left_ThumbFinger3_Jnt"},
        {"Neck", "Neck_Jnt"},
        {"LeftLowerArm", "Left_LowerArm_Jnt"},
        {"Right Middle Distal", "Right_MiddleFinger3_Jnt"},
        {"Right Ring Intermediate", "Right_RingFinger2_Jnt"},
        {"Right Thumb Proximal", "Right_ThumbFinger1_Jnt"},
        {"Left Middle Distal", "Left_MiddleFinger3_Jnt"},
        {"RightUpperArm", "Right_UpperArm_Jnt"},
        {"RightFoot", "Right_Foot_Jnt"},
        {"RightHand", "Right_Hand_Jnt"},
        {"RightToes", "Right_Toes_Jnt"},
        {"Left Middle Intermediate", "Left_MiddleFinger2_Jnt"},
        {"Right Ring Proximal", "Right_RingFinger1_Jnt"},
        {"LeftFoot", "Left_Foot_Jnt"},
        {"Left Index Intermediate", "Left_IndexFinger2_Jnt"},
        {"LeftHand", "Left_Hand_Jnt"},
        {"LeftToes", "Left_Toes_Jnt"},
        {"Right Little Proximal", "Right_PinkyFinger1_Jnt"},
        {"RightShoulder", "Right_Shoulder_Jnt"},
        {"Right Ring Distal", "Right_RingFinger3_Jnt"}
    };
}
