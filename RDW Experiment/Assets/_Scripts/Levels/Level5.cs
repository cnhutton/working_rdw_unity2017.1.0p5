using System;
using UnityEngine;

public class Level5 : MonoBehaviour
{
    private bool _turnLeft;
    private bool useIndividualized;
    private bool usePositive;
    private int count;
    private int totalCount;
    private Edge _startingEdge;
    private Edge _endingEdge;

    public delegate void EndpointReached();

    public static event EndpointReached Reached;

    private AlgorithmType algorithm; //
    private float positiveAvg;
    private float negativeAvg;
    private float positiveAlg;
    private float negativeAlg;
    private Feedback response;

    void Start()
    {
        Manager.Sound.SetIndex(12);
        FindObjectOfType<Controller>().SetGain(0);
        _startingEdge = LevelUtilities.ChooseRandomEdge();
        Manager.Spawn.PurpleFeet(_startingEdge);
        FeetObject.OnCollision += Feet;
        Pointer.Click += Touchpad;
        _turnLeft = LevelUtilities.GenerateRandomBool();
        useIndividualized = LevelUtilities.GenerateRandomBool();
        usePositive = LevelUtilities.GenerateRandomBool();
        Manager.Sound.PlayNextVoiceover(2.0f); //#13 position & follow path
        count = 0;
        totalCount = 0;
        Manager.Experiment.GetWalkthroughAlgorithm(out algorithm);
        Manager.Experiment.GetThreshold(algorithm, out positiveAlg, out negativeAlg);
        negativeAvg = -0.2f;
        positiveAvg = 0.4f;
        SetupFMS();
    }

    private void Feet()
    {
        FeetObject.OnCollision -= Feet;
        SetupPath();
    }

    private void Endpoint()
    {
        ++count;
        ++totalCount;
        EndpointObject.OnCollision -= Endpoint;

        if (Reached != null)
        {
            Reached();
        }

        Manager.Spawn.DiscernmentButtons(_endingEdge);
        Manager.Sound.PlaySpecificVoiceover(13);
    }

    private void SetupPath()
    {
        _turnLeft = !_turnLeft;

        if (count == 2)
        {
            useIndividualized = !useIndividualized;
            usePositive = !usePositive;
            count = 0;
        }
        else
        {
            usePositive = !usePositive;
        }

        float gain = 0;
        if (useIndividualized)
        {
            gain = usePositive ? positiveAlg : negativeAlg;
        }
        else
            gain = usePositive ? positiveAvg : negativeAvg;

        FindObjectOfType<Controller>().SetGain(gain);
        _endingEdge = LevelUtilities.EndpointEdge(_startingEdge, _turnLeft);
        Manager.Spawn.Path(_turnLeft, _startingEdge);
        Manager.Spawn.Endpoint(_endingEdge);
        EndpointObject.OnCollision += Endpoint;
    }

    private void Touchpad(ObjectType type)
    {
        bool isFinal;
        Manager.Experiment.WalkthroughStatus(out isFinal);
        switch (type)
        {
            case ObjectType.FMS:
                int value;
                Manager.Experiment.GetFMSResponse(out value);
                UpdateFMS(value);
                if (totalCount == 4 && !isFinal)
                {
                    Manager.SceneSwitcher.LoadNextScene(SceneName.Four);
                }
                else
                {

                }
                break;
            case ObjectType.SameButton:
                response = Feedback.Same;
                //Manager.Spawn.MotionSicknessUI();
                if (totalCount == 4 && !isFinal)
                {
                    Manager.SceneSwitcher.LoadNextScene(SceneName.Four);
                }
                break;
            case ObjectType.DifferentButton:
                response = Feedback.Different;
                //Manager.Spawn.MotionSicknessUI();
                if (totalCount == 4 && !isFinal)
                {
                    Manager.SceneSwitcher.LoadNextScene(SceneName.Four);
                }
                break;
            case ObjectType.ContinueButton:
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }

    private void SetupFMS()
    {
        System.DateTime now = System.DateTime.Now;
        string line = "\nWALKTHROUGH PHASE\n" +
                      "Algorithm: " + (algorithm == AlgorithmType.Staircase ? "Staircase" : "PEST") + "\n" +
                      "Start Time: " + now.Hour.ToString() + ":" + now.Minute.ToString() + "\n\n";
        Manager.Experiment.WriteToFMS(line);
    }

    private void UpdateFMS(int FMS)
    {
        float gain;
        FindObjectOfType<Controller>().GetGain(out gain);
        string line = "Individualized: " + (useIndividualized ? "Yes" : "No") + "\n" +
                      "Positive Threshold: " + (usePositive ? "Yes" : "No") + "\n" +
                      "Gain Applied: " + gain.ToString() + "\n" +
                      "Response: " + (response == Feedback.Same ? "Same" : "Different") + "\n" +
                      "FMS: " + FMS + "\n";
        Manager.Experiment.WriteToFMS(line);
    }

}
