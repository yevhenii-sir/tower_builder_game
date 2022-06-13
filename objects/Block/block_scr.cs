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
                    var tempScr = (int) mainNode.Get("_score");
                    if (tempScr > (float)mainNode.Get("_maxScore"))
                       mainNode.Set("_maxScore", tempScr);

                    SaveGame(mainNode);
                   
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

    public void SaveGame(Node mainNode)
    {
        var saveGame = new File();
        saveGame.Open("user://savegame.save", File.ModeFlags.Write);

        if (!mainNode.HasMethod("Save"))
        {
            GD.Print("Skip Save");
            return;
        }
        
        var nodeData = mainNode.Call("Save");
        saveGame.StoreLine(JSON.Print(nodeData));

        saveGame.Close();
        
        GD.Print("Saved!");
    }
}
