using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBlock : MonoBehaviour
{
    private GameObject parentBlock;
    private Block parentBlockScript;
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(RepositionGhostBlock());
    }

    public void SetParent(GameObject _parentBlock)
    {
        parentBlock = _parentBlock;
        parentBlockScript = parentBlock.GetComponent<Block>();
    }


    private IEnumerator RepositionGhostBlock()
    {
        while (parentBlockScript.enabled)
        {
            PositionGhost();
            //Move at the end of grid
            MoveDown();
            yield return new WaitForSeconds(0.005f);
        }
        Destroy(gameObject);
        yield return null;
    }

    private void PositionGhost()
    {
        transform.position = parentBlock.transform.position;
        transform.rotation = parentBlock.transform.rotation;
    }

    private void MoveDown()
    {
        while (CheckValidMove())
        {
            transform.position += Vector3.down;
        }
        if (!CheckValidMove())
        {
            transform.position += Vector3.up;
        }
    }

    private bool CheckValidMove()
    {
        foreach (Transform childCube in transform)
        {
            Vector3 position = Grid.Instance.VectorRound(childCube.position);
            if (!Grid.Instance.IsInsideGrid(position))
            {
                return false;
            }
        }
        foreach (Transform childCube in transform)
        {
            Vector3 roundedPosition = Grid.Instance.VectorRound(childCube.position);
            Transform childTransform = Grid.Instance.GetTransformOnGridPosition(roundedPosition);
            if(childTransform!=null && childTransform.parent == parentBlock.transform)
            {
                return true;
            }
            if (childTransform != null && childTransform.parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}
