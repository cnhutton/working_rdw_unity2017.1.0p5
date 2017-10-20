using System;
using JetBrains.Annotations;
using UnityEngine;
// ReSharper disable SpecifyACultureInStringConversionExplicitly
// ReSharper disable RedundantNameQualifier

public class Level4 : MonoBehaviour
{

    public GameObject Player;
    public GameObject Room;
    public GameObject NorthWall;
    public GameObject EastWall;
    public GameObject SouthWall;
    public GameObject WestWall;
    public Transform PurpleFeetSpawn;

    private bool _turnLeft;
    private bool _northArrow;
    private GameObject _painting;
    private GameObject _discernment;
    private GameObject _arrowNorth;
    private GameObject _arrowSouth;

    private AlgorithmType _algorithm;

    private int _turnCount;

    [UsedImplicitly]
    // ReSharper disable once ArrangeTypeMemberModifiers
    void Start()
    {
        Manager.Sound.SetIndex(10);
        FindObjectOfType<Controller>().SetGain(0);
        Manager.Spawn.PurpleFeet(PurpleFeetSpawn.position);
        FeetObject.OnCollision += Feet;

        AlgorithmManager.Complete += Completed;
        _turnLeft = LevelUtilities.GenerateRandomBool();
        Manager.Experiment.GetAlgorithm(out _algorithm);
        Debug.Log(_algorithm);
        Manager.Algorithm.Initialize(_algorithm);
        Manager.Sound.PlayNextVoiceover(); //#9 Experiment now begins, go to feet
        Pointer.Click += Touchpad;
        SetupInitialCalibration();
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void Feet()
    {
        FeetObject.OnCollision -= Feet;
    }

    private void SetupInitialCalibration()
    {
        _turnCount = 1;
        SetupFile();
        _northArrow = true;
        Manager.Spawn.ArrowButton(_turnLeft, out _arrowNorth);
        Manager.Spawn.ArrowButton(_turnLeft, out _arrowSouth);
        Manager.Spawn.Painting(_turnLeft ? Direction.West : Direction.East, out _painting);
        Manager.Spawn.DiscernmentButtons(_turnLeft ? Direction.West : Direction.East, out _discernment);
        _arrowNorth.transform.SetParent(NorthWall.transform);
        _arrowSouth.transform.SetParent(SouthWall.transform);
        _arrowSouth.SetActive(false);
        _painting.transform.SetParent(_turnLeft ? WestWall.transform : EastWall.transform);
        _discernment.transform.SetParent(_turnLeft ? WestWall.transform : EastWall.transform);
    }

    private void SetupCalibration()
    {
        _northArrow = !_northArrow;
        _arrowNorth.SetActive(_northArrow);
        _arrowSouth.SetActive(!_northArrow);
        _turnLeft = !_turnLeft;
        Manager.Algorithm.Run(_algorithm);
        ++_turnCount;
        UpdateFile();
        RotateRoom(!_turnLeft);
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
                      "Algorithm: " + (_algorithm == AlgorithmType.Staircase ? "Staircase" : "PEST") + "\n" +
                      "Start Time: " + now.Hour.ToString() + ":" + now.Minute.ToString() + "\n" +
                      "Negative Threshold: " + (negative ? "Yes" : "No") + "\n" +
                      "Turn direction: " + (_turnLeft ? "Left" : "Right") + "\n" +
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
                      "Turn direction: " + (_turnLeft ? "Left" : "Right") + "\n" +
                      "Turn count: " + _turnCount + "\n" +
                      "Current Gain: " + gain.ToString() + "\n";
        Manager.Experiment.WriteToFile(line);
    }

    //private void UpdateFMSFile()
    //{

    //}

    private void CompleteFile()
    {
        System.DateTime now = System.DateTime.Now;
        float pos, neg;
        Manager.Experiment.GetThreshold(_algorithm, out pos, out neg);
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
        float angle = turn ? 90f : -90f;
        Room.transform.RotateAround(Vector3.zero, axis, angle);
        SteamVR_Fade.Start(Color.clear, 1.2f);
    }
}
