using Godot;
using System;

public class block_scr : RigidBody2D
{
    public bool SkipBlock = false;

    public override void _Process(float delta)
    {
        if (SkipBlock)
        {
            if (GlobalRotation > Mathf.Deg2Rad(60) || GlobalRotation < Mathf.Deg2Rad(-60))
            {
                Node mainNode = GetParent().GetParent();
                if (!(bool) mainNode.Get("_isFinal"))
                {
                    mainNode.Set("_isFinal", true);
                }
            }
        }
    }
}
