using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRotate : MonoBehaviour
{

    public Transform rotateTransform;
    public float speed = 45;
    public bool IsActive = false;
    public Vector3 directionRotation = new Vector3(0.0f, 0.0f, 2.0f);
    public bool isRightRotate = true;
    int directionRotate = 1;
    [Header("Rotation Options")]
    [Tooltip("If true, rotation will be independent of Time.timeScale.")]
    public bool useUnscaledTime = false; // Thêm option này

    public void Init()
    {
        IsActive = true;
        rotateTransform.rotation = Quaternion.identity;
        if (isRightRotate) directionRotate = -1;
        else directionRotate = 1;
    }
    private void Start()
    {
        if (isRightRotate) directionRotate = -1;
        else directionRotate = 1;
    }
    void Update()
    {
        if (!IsActive)
            return;

        // Chọn deltaTime phù hợp dựa trên cờ useUnscaledTime
        float deltaTime = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

        rotateTransform.Rotate(directionRotation * speed * deltaTime * directionRotate);
    }

    public int PieceOfRotate(int numberPiece, float pointerOffsetAngle)
    {
        float angleOfAPiece = 360f / numberPiece;
        float currentRotationZ = rotateTransform.eulerAngles.z;
        float normalizedAngle = (currentRotationZ + pointerOffsetAngle) % 360;
        if (normalizedAngle < 0) normalizedAngle += 360;

        normalizedAngle = 360 - normalizedAngle;

        int sectionIndex = Mathf.FloorToInt(normalizedAngle / angleOfAPiece);

        return sectionIndex;
    }

}
