﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : MonoBehaviour
{
    private bool _turnLeft;

    private Corner _startingCorner;
    private Corner _endingCorner;

    private GameObject button;
    private bool _completed;

    public delegate void EndpointReached();

    public static event EndpointReached Reached;
    private bool readyToAdvance = false;
    void Start()
    {
        FindObjectOfType<Controller>().SetGain(0);
        Manager.Sound.SetIndex(2);
        _completed = false;

        _startingCorner = LevelUtilities.ChooseRandomCorner();
        Manager.Spawn.PurpleFeet(_startingCorner);
        FeetObject.OnCollision += Feet;
        Pointer.Click += Touchpad;
        _turnLeft = LevelUtilities.GenerateRandomBool();
        Manager.Sound.PlayNextVoiceover(2.0f);
    }

    private void Feet()
    {
        FeetObject.OnCollision -= Feet;
        SetupPath();
        Manager.Sound.PlayNextVoiceover(1.0f); //voiceover #3
    }

    private void Endpoint()
    {
        Debug.Log("Endpoint reached");
        EndpointObject.OnCollision -= Endpoint;
        if (Reached != null)
        {
            Reached();
        }
        if (_completed)
        {
            Manager.Spawn.ContinueButton(Direction.North, out button);
            Manager.Sound.PlayNextVoiceover(); //voiceover #5
            readyToAdvance = true;
        }
        else
        {
            FindObjectOfType<Controller>().SetGain(0);
            Manager.Sound.PlayNextVoiceover(); //voiceover #4 turn to center
            _turnLeft = !_turnLeft;
            _startingCorner = _endingCorner;
            _completed = true;
            SetupPath();
        }
    }

    private void SetupPath()
    {
        _endingCorner = LevelUtilities.EndpointCorner(_startingCorner, _turnLeft);
        Manager.Spawn.Path(_turnLeft, _startingCorner);
        Manager.Spawn.Endpoint(_endingCorner);
        EndpointObject.OnCollision += Endpoint;
        FindObjectOfType<Controller>().SetGain(_completed ? 0.5f : -0.5f);
    }

    private void Touchpad(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.FMS:
                break;
            case ObjectType.SameButton:
                break;
            case ObjectType.DifferentButton:
                break;
            case ObjectType.ContinueButton:
                if (readyToAdvance)
                {
                    readyToAdvance = false;
                    Manager.SceneSwitcher.LoadNextScene(SceneName.Three);
                }
                Debug.LogError("Scene 2 continue reached");
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }

}