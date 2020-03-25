using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TreeNode
{
    public enum ReturnResult {Fail = 0, Success = 1, Running = 2 }

    public List<TreeNode> refToChildren;
    public ReturnResult status;

    public abstract void Run(TheAI ai);
}
