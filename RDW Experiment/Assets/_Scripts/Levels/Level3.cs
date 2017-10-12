using System;
using UnityEngine;

public class Level3 : MonoBehaviour
{

    public Transform Player;
    
    public Transform PurpleFeetSpawn;

    private GameObject painting;
    private GameObject discernment;
    private GameObject arrow;
    private GameObject proceed;

    private bool turnLeft;
    private bool readyToAdvance = false;
    private bool trainingComplete;
    public GameObject Room;
    public delegate void Reset();

    public static event Reset reset;

    void Start()
    {
        Manager.Sound.SetIndex(6);
        FindObjectOfType<Controller>().SetGain(0);
        Manager.Spawn.PurpleFeet(PurpleFeetSpawn.position, Quaternion.Euler(0, 180, 0));
        FeetObject.OnCollision += Feet;
        
        turnLeft = LevelUtilities.GenerateRandomBool();
        trainingComplete = false;
        Manager.Sound.PlayNextVoiceover(1.0f); //#6 position purple
    }

    private void Feet()
    {
        FeetObject.OnCollision -= Feet;
        Pointer.Click += Touchpad;
        Manager.Sound.PlayNextVoiceover(); //#7 calibration info
        SetupInitialCalibration();
        Manager.Sound.PlayNextVoiceover(5f); //#8 after turning
    }

    private void SetupInitialCalibration()
    {
        Manager.Spawn.ArrowButton(turnLeft, out arrow);
        Manager.Spawn.Painting(turnLeft ? Direction.West : Direction.East, out painting);
        Manager.Spawn.DiscernmentButtons(turnLeft ? Direction.West : Direction.East, out discernment);
        Manager.Spawn.ProceedButton(turnLeft ? Direction.West : Direction.East, out proceed);

        proceed.SetActive(false);
        
    }

    private void SetupCalibration()
    {
        
        turnLeft = !turnLeft;
        if (!trainingComplete)
        {
            Manager.Sound.PlayNextVoiceover(1.0f); //#9 practice until comfortable then continue
            trainingComplete = true;
        }
        proceed.SetActive(true);
        Manager.Spawn.CalibrationRotation(painting, turnLeft ? Direction.West : Direction.East);
        Manager.Spawn.CalibrationRotation(discernment, turnLeft ? Direction.West : Direction.East);
        Manager.Spawn.CalibrationRotation(proceed, turnLeft ? Direction.West : Direction.East);
        Manager.Spawn.ArrowRotation(arrow, turnLeft);
        RotateRoom(!turnLeft);
    }

    private void Touchpad(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.FMS:
                break;
            case ObjectType.SameButton:
                SetupCalibration();
                break;
            case ObjectType.DifferentButton:
                SetupCalibration();
                break;
            case ObjectType.ContinueButton:
                if (readyToAdvance)
                {
                    readyToAdvance = false;
                    Manager.SceneSwitcher.LoadNextScene(SceneName.Four);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }

    public void RotateRoom(bool turn)
    {
        SteamVR_Fade.Start(Color.black, 1f, true);
        Vector3 axis = new Vector3(0, 1, 0);
        float angle = turn ? -90f : 90f;
        Room.transform.RotateAround(Vector3.zero, axis, angle);
        SteamVR_Fade.Start(Color.clear, 1.2f);
        Debug.LogError("Room rotated successfully");
    }
}


