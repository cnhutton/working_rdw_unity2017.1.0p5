using System;
using UnityEngine;

public class Level3 : MonoBehaviour
{

    public Transform Player;
    public GameObject Room;
    public GameObject NorthWall;
    public GameObject EastWall;
    public GameObject SouthWall;
    public GameObject WestWall;
    public Transform PurpleFeetSpawn;

    private GameObject _painting;
    private GameObject _discernment;
    private GameObject _arrowNorth;
    private GameObject _arrowSouth;
    private GameObject _proceed;

    private bool _turnLeft;
    private bool _trainingComplete;

    private bool _northArrow;

    void Start()
    {
        Manager.Sound.SetIndex(6);
        FindObjectOfType<Controller>().SetGain(0);
        Manager.Spawn.PurpleFeet(PurpleFeetSpawn.position);
        FeetObject.OnCollision += Feet;
        _turnLeft = LevelUtilities.GenerateRandomBool();
        _trainingComplete = false;
        Manager.Sound.PlayNextVoiceover(1.0f); //#6 position purplex
    }

    private void Feet()
    {
        FeetObject.OnCollision -= Feet;
        Pointer.Click += Touchpad;
        Manager.Sound.PlayNextVoiceover(); //#7 calibration info
        SetupInitialCalibration();
        Manager.Sound.PlayNextVoiceover(6f); //#8 after turning
    }

    private void SetupInitialCalibration()
    {
        _northArrow = true;
        Manager.Spawn.ArrowButton(_turnLeft, out _arrowNorth);
        Manager.Spawn.ArrowButton(_turnLeft, out _arrowSouth);
        Manager.Spawn.Painting(_turnLeft ? Direction.West : Direction.East, out _painting);
        Manager.Spawn.DiscernmentButtons(_turnLeft ? Direction.West : Direction.East, out _discernment);
        Manager.Spawn.ProceedButton(_turnLeft ? Direction.West : Direction.East, out _proceed);
        _arrowNorth.transform.SetParent(NorthWall.transform);
        _arrowSouth.transform.SetParent(SouthWall.transform);
        _painting.transform.SetParent(_turnLeft ? WestWall.transform : EastWall.transform);
        _discernment.transform.SetParent(_turnLeft ? WestWall.transform : EastWall.transform);
        _proceed.transform.SetParent(_turnLeft ? WestWall.transform : EastWall.transform);

        _arrowSouth.SetActive(false);
        _proceed.SetActive(false);
        
    }

    private void SetupCalibration()
    {
        _turnLeft = !_turnLeft;
        if (!_trainingComplete)
        {
            Manager.Sound.PlayNextVoiceover(1.0f); //#9 practice until comfortable then continue
            _trainingComplete = true;
        }
        _proceed.SetActive(true);
        _northArrow = !_northArrow;
        _arrowNorth.SetActive(_northArrow);
        _arrowSouth.SetActive(!_northArrow);
        RotateRoom(!_turnLeft);
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
                Manager.SceneSwitcher.LoadNextScene(SceneName.Four);
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
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


