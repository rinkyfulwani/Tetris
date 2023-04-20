using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPreview : MonoBehaviour
{
    public static BlockPreview currentInstance;
    public GameObject[] blocktsToPreview;

    private GameObject currentPreview;

    void Awake() {
        currentInstance = this;
    }

    public void AppearPreviewBlock(int i) {
        // first remove the current preview
        Destroy(currentPreview);
        
        currentPreview = Instantiate(blocktsToPreview[i], transform.position, transform.rotation);
    }

}
