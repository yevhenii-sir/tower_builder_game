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
                if (!(bool) mainNode.Get("_isStart"))
                {
                   /* var rope = mainNode.GetNode<Node2D>("Rope");
                    
                    var get_curBox = (Node) rope.Get("_currentBox");
                    GD.Print(rope, get_curBox, get_curBox.GetPath());
                    if (get_curBox != null)
                    {
                        if (!GetTree().Root.HasNode(get_curBox.GetPath()))
                        {
                            rope.Call("Skip");
                        }
                    }*/
                    
                    mainNode.Set("_isStart", true);
                    mainNode.Set("_startAnimationPlayBtn", true);

                    foreach (var child in mainNode.GetChildren())
                    {
                        Node tmpChild = (Node) child;

                        if (tmpChild.Name.Contains("box_"))
                            mainNode.RemoveChild(tmpChild);
                    }

                    mainNode.GetNode<Camera2D>("Camera").Call("GoToStartupPosition");
                    mainNode.Set("_score", 0);
                    mainNode.GetNode("Rope").Call("SetScoreLabel", false, 0);
                }
            }
        }
    }
}
