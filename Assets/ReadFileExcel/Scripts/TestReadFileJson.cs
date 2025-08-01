using ReadFileJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestReadFileJson : MonoBehaviour
{

    public TextAsset fileJson;
    public WrapperArray<InformationData> wrapper;
    // Start is called before the first frame update
    void Start()
    {
        string newJson = "{ \"Data\": " + fileJson.text + "}";
        wrapper = JsonUtility.FromJson<WrapperArray<InformationData>>(newJson);
        print(wrapper.Data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
