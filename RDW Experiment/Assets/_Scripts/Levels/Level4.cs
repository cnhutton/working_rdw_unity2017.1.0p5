using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4 : MonoBehaviour
{

    public GameObject Player;
    public GameObject Room;
    public Transform PurpleFeetSpawn;

    private bool turnLeft;
    private GameObject painting;
    private GameObject discernment;
    private GameObject arrow;
    private GameObject proceed;
    public delegate void Reset();

    public static event Reset reset;

    private AlgorithmType algorithm;

    private int _turnCount;
    void Start()
    {
       
        Manager.Sound.SetIndex(10);
        FindObjectOfType<Controller>().SetGain(0);
        Manager.Spawn.PurpleFeet(PurpleFeetSpawn.position, Quaternion.Euler(0, 180, 0));
        //FeetObject.OnCollision += Feet;

        AlgorithmManager.Complete += Completed;
        turnLeft = LevelUtilities.GenerateRandomBool();
        Manager.Experiment.GetAlgorithm(out algorithm);
        Debug.Log(algorithm);
        Manager.Algorithm.Initialize(algorithm);
        Manager.Sound.PlayNextVoiceover(); //#9 Experiment now begins, go to feet
        Pointer.Click += Touchpad;
        SetupInitialCalibration();
    }

    private void Feet()
    {
        FeetObject.OnCollision -= Feet;
        
    }

    private void SetupInitialCalibration()
    {
        _turnCount = 1;
        SetupFile();
        Manager.Spawn.ArrowButton(turnLeft, out arrow);
        Manager.Spawn.Painting(turnLeft ? Direction.West : Direction.East, out painting);
        Manager.Spawn.DiscernmentButtons(turnLeft ? Direction.West : Direction.East, out discernment);
    }

    private void SetupCalibration()
    {
        turnLeft = !turnLeft;
        Manager.Spawn.CalibrationRotation(painting, turnLeft ? Direction.West : Direction.East);
        Manager.Spawn.CalibrationRotation(discernment, turnLeft ? Direction.West : Direction.East);
        Manager.Spawn.ArrowRotation(arrow, turnLeft);
        Manager.Algorithm.Run(algorithm);
        ++_turnCount;
        UpdateFile();
        RotateRoom(!turnLeft);
    }

    private void Touchpad(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.FMS:
                break;
            case ObjectType.SameButton:
                Manager.Algorithm.Response = Feedback.Same;
                SetupCalibration();
                break;
            case ObjectType.DifferentButton:
                Manager.Algorithm.Response = Feedback.Different;
                SetupCalibration();
                break;
            case ObjectType.ContinueButton:
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }

    private void Completed()
    {
        CompleteFile();
        Manager.SceneSwitcher.LoadNextScene(SceneName.Five);
        //Manager.Spawn.MotionSicknessUI();
    }

    private void SetupFile()
    {
        System.DateTime now = System.DateTime.Now;
        float gain;
        bool negative;
        Manager.Algorithm.GetData(out gain, out negative);
        string line = "CALIBRATION PHASE \n" +
                      "Algorithm: " + (algorithm == AlgorithmType.Staircase ? "Staircase" : "PEST") + "\n" +
                      "Start Time: " + now.Hour.ToString() + ":" + now.Minute.ToString() + "\n" +
                      "Negative Threshold: " + (negative ? "Yes" : "No") + "\n" +
                      "Turn direction: " + (turnLeft ? "Left" : "Right") + "\n" +
                      "Turn count: " + _turnCount + "\n" +
                      "Current Gain: " + gain.ToString() + "\n";
        Manager.Experiment.WriteToFile(line);
    }

    private void UpdateFile()
    {
        System.DateTime now = System.DateTime.Now;
        float gain;
        bool negative;
        Manager.Algorithm.GetData(out gain, out negative);
        string line = "Time: " + now.Hour.ToString() + ":" + now.Minute.ToString() + "\n" +
                      "Previous response: " + (Manager.Algorithm.Response == Feedback.Same ? "Same" : "Different") + "\n" +
                      "Negative Threshold: " + (negative ? "Yes" : "No") + "\n" +
                      "Turn direction: " + (turnLeft ? "Left" : "Right") + "\n" +
                      "Turn count: " + _turnCount + "\n" +
                      "Current Gain: " + gain.ToString() + "\n";
        Manager.Experiment.WriteToFile(line);
    }

    private void UpdateFMSFile()
    {

    }

    private void CompleteFile()
    {
        System.DateTime now = System.DateTime.Now;
        float pos, neg;
        Manager.Experiment.GetThreshold(algorithm, out pos, out neg);
        string line = "Time: " + now.Hour.ToString() + ":" + now.Minute.ToString() + "\n" +
                      "Previous response: " + (Manager.Algorithm.Response == Feedback.Same ? "Same" : "Different") + "\n" +
                      "Total turn count: " + _turnCount + "\n" +
                      "Final gain, positive: " + pos.ToString() + "\n" +
                      "Final gain, negative: " + neg.ToString() + "\n\n";
        Manager.Experiment.WriteToFile(line);
    }

    public void RotateRoom(bool turn)
    {
        SteamVR_Fade.Start(Color.black, 1f, true);
        Vector3 axis = new Vector3(0, 1, 0);
        float angle = turn ? -90f : 90f;
        Room.transform.RotateAround(Vector3.zero, axis, angle);
        SteamVR_Fade.Start(Color.clear, 1.2f);
        //Debug.LogError("Room rotated successfully");
    }
}
