using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour
{
    public InfiniteRotate infiniteRotate;
    public void Test()
    {
        int x = infiniteRotate.PieceOfRotate(4, 0);
        print("spine");
        switch (x)
        {
            case 0:
                print("blue");
                break;
            case 1:
                print("yellow");
                break;
            case 2:
                print("Red");
                break;
            case 3:
                print("green");
                break;
        }
    }
}
