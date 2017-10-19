using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject Room;

    public GameObject PurpleFeetPrefab;
    public GameObject ContinueButtonPrefab;
    public GameObject CompleteButtonPrefab;
    public GameObject LeftArrowPrefab;
    public GameObject RightArrowPrefab;
    public GameObject PaintingPrefab;
    public GameObject MotionSicknessPrefab;
    public GameObject LeftPathPrefab;
    public GameObject RightPathPrefab;
    public GameObject EndpointPrefab;
    public GameObject DiscernmentButtonsPrefab;

    public Transform NorthWallSpawn;
    public Transform EastWallSpawn;
    public Transform SouthWallSpawn;
    public Transform WestWallSpawn;
    public Transform PathSpawn;
    public Transform NortheastSpawn;
    public Transform SoutheastSpawn;
    public Transform SouthwestSpawn;
    public Transform NorthwestSpawn;

    //Need completion: continue button, motion sickness, path
    public void PurpleFeet(Vector3 spawnPoint)
    {
        Instantiate(PurpleFeetPrefab, spawnPoint, Quaternion.identity);
    }

    public void PurpleFeet(Vector3 spawnPoint, Quaternion rotation)
    {
        Instantiate(PurpleFeetPrefab, spawnPoint, rotation);
    }

    public void PurpleFeet(Edge edge)
    {
        switch (edge)
        {
            case Edge.Northeast:
                Instantiate(PurpleFeetPrefab, NortheastSpawn.position, Quaternion.Euler(0, -135, 0));
                break;
            case Edge.Southeast:
                Instantiate(PurpleFeetPrefab, SoutheastSpawn.position, Quaternion.Euler(0, -45, 0));
                break;
            case Edge.Southwest:
                Instantiate(PurpleFeetPrefab, SouthwestSpawn.position, Quaternion.Euler(0, 45, 0));
                break;
            case Edge.Northwest:
                Instantiate(PurpleFeetPrefab, NorthwestSpawn.position, Quaternion.Euler(0, 135, 0));
                break;
            default:
                throw new ArgumentOutOfRangeException("edge", edge, null);
        }
    }

    public void ProceedButton(Direction wall, out GameObject button)
    {
        switch (wall)
        {
            case Direction.North:
                button = (GameObject)Instantiate(CompleteButtonPrefab, NorthWallSpawn.position, Quaternion.identity) as GameObject;
                break;
            case Direction.East:
                button = (GameObject)Instantiate(CompleteButtonPrefab, EastWallSpawn.position, Quaternion.Euler(0, 90, 0)) as GameObject;
                break;
            case Direction.South:
                button = (GameObject)Instantiate(CompleteButtonPrefab, SouthWallSpawn.position, Quaternion.Euler(0, 180, 0)) as GameObject;
                break;
            case Direction.West:
                button = (GameObject)Instantiate(CompleteButtonPrefab, WestWallSpawn.position, Quaternion.Euler(0, -90, 0)) as GameObject;
                break;
            default:
                throw new ArgumentOutOfRangeException("wall", wall, null);
        }
    }


    public void ContinueButton(Direction wall, out GameObject button)
    {
        switch (wall)
        {
            case Direction.North:
                button = (GameObject)Instantiate(ContinueButtonPrefab, NorthWallSpawn.position, Quaternion.identity) as GameObject;
                break;
            case Direction.East:
                button = (GameObject)Instantiate(ContinueButtonPrefab, EastWallSpawn.position, Quaternion.Euler(0, 90, 0)) as GameObject;
                break;
            case Direction.South:
                button = (GameObject)Instantiate(ContinueButtonPrefab, SouthWallSpawn.position, Quaternion.Euler(0, 180, 0)) as GameObject;
                break;
            case Direction.West:
                button = (GameObject)Instantiate(ContinueButtonPrefab, WestWallSpawn.position, Quaternion.Euler(0, -90, 0)) as GameObject;
                break;
            default:
                throw new ArgumentOutOfRangeException("wall", wall, null);
        }
    }

    public void ContinueButton(Edge edge)
    {
        switch (edge)
        {
            case Edge.Northeast:
                GameObject clone1 = (GameObject)Instantiate(ContinueButtonPrefab, NortheastSpawn.position, Quaternion.Euler(0, -135, 0)) as GameObject;
                break;
            case Edge.Southeast:
                GameObject clone2 = (GameObject)Instantiate(ContinueButtonPrefab, SoutheastSpawn.position, Quaternion.Euler(0, -45, 0)) as GameObject;
                break;
            case Edge.Southwest:
                GameObject clone3 = (GameObject)Instantiate(ContinueButtonPrefab, SouthwestSpawn.position, Quaternion.Euler(0, 45, 0)) as GameObject;
                break;
            case Edge.Northwest:
                GameObject clone4 = (GameObject)Instantiate(ContinueButtonPrefab, NorthwestSpawn.position, Quaternion.Euler(0, 135, 0)) as GameObject;
                break;
            default:
                throw new ArgumentOutOfRangeException("edge", edge, null);
        }
    }

    public void ArrowButton(bool spawnLeftArrow, out GameObject arrow)
    {
        arrow = Instantiate( LeftArrowPrefab, SouthWallSpawn.position, spawnLeftArrow ? Quaternion.identity : Quaternion.Euler(0, 180, 0));
    }

    public void Painting(Direction wall, out GameObject painting)
    {
        switch (wall)
        {
            case Direction.North:
                painting = (GameObject)Instantiate(PaintingPrefab, NorthWallSpawn.position, Quaternion.identity) as GameObject;
                break;
            case Direction.East:
                painting = (GameObject)Instantiate(PaintingPrefab, EastWallSpawn.position, Quaternion.Euler(0, 90, 0)) as GameObject;
                break;
            case Direction.South:
                painting = (GameObject)Instantiate(PaintingPrefab, SouthWallSpawn.position, Quaternion.Euler(0, 180, 0)) as GameObject;
                break;
            case Direction.West:
                painting = (GameObject)Instantiate(PaintingPrefab, WestWallSpawn.position, Quaternion.Euler(0, -90, 0)) as GameObject;
                break;
            default:
                throw new ArgumentOutOfRangeException("wall", wall, null);
        }
    }

    public void MotionSicknessUI()
    {
        Instantiate(MotionSicknessPrefab, NorthWallSpawn.position, Quaternion.identity);
    }

    public void Path(bool spawnLeftTurn, Edge startEdge)
    {
        switch (startEdge)
        {
            case Edge.Northeast:
                Instantiate(spawnLeftTurn ? LeftPathPrefab : RightPathPrefab, PathSpawn.position, Quaternion.identity);
                break;
            case Edge.Southeast:
                Instantiate(spawnLeftTurn ? LeftPathPrefab : RightPathPrefab, PathSpawn.position, Quaternion.Euler(0, 90, 0));
                break;
            case Edge.Southwest:
                Instantiate(spawnLeftTurn ? LeftPathPrefab : RightPathPrefab, PathSpawn.position, Quaternion.Euler(0, 180, 0));
                break;
            case Edge.Northwest:
                Instantiate(spawnLeftTurn ? LeftPathPrefab : RightPathPrefab, PathSpawn.position, Quaternion.Euler(0, -90, 0));
                break;
            default:
                throw new ArgumentOutOfRangeException("startEdge", startEdge, null);
        }
    }

    public void Endpoint(Edge edge)
    {
        switch (edge)
        {
            case Edge.Northeast:
                Instantiate(EndpointPrefab, NortheastSpawn.position, Quaternion.identity);
                break;
            case Edge.Southeast:
                Instantiate(EndpointPrefab, SoutheastSpawn.position, Quaternion.identity);
                break;
            case Edge.Southwest:
                Instantiate(EndpointPrefab, SouthwestSpawn.position, Quaternion.identity);
                break;
            case Edge.Northwest:
                Instantiate(EndpointPrefab, NorthwestSpawn.position, Quaternion.identity);
                break;
            default:
                throw new ArgumentOutOfRangeException("edge", edge, null);
        }
    }

    public void DiscernmentButtons(Direction wall, out GameObject buttons)
    {
        switch (wall)
        {
            case Direction.North:
                buttons = (GameObject)Instantiate(DiscernmentButtonsPrefab, NorthWallSpawn.position, Quaternion.identity) as GameObject;
                break;
            case Direction.East:
                buttons = (GameObject)Instantiate(DiscernmentButtonsPrefab, EastWallSpawn.position, Quaternion.Euler(0, 90, 0)) as GameObject;
                break;
            case Direction.South:
                buttons = (GameObject)Instantiate(DiscernmentButtonsPrefab, SouthWallSpawn.position, Quaternion.Euler(0, 180, 0)) as GameObject;
                break;
            case Direction.West:
                buttons = (GameObject)Instantiate(DiscernmentButtonsPrefab, WestWallSpawn.position, Quaternion.Euler(0, -90, 0)) as GameObject;
                break;
            default:
                throw new ArgumentOutOfRangeException("wall", wall, null);
        }
    }

    public void DiscernmentButtons(Edge edge)
    {
        switch (edge)
        {
            case Edge.Northeast:
                Instantiate(DiscernmentButtonsPrefab, NortheastSpawn.position, Quaternion.Euler(0, -135, 0));
                break;
            case Edge.Southeast:
                Instantiate(DiscernmentButtonsPrefab, SoutheastSpawn.position, Quaternion.Euler(0, -45, 0));
                break;
            case Edge.Southwest:
                Instantiate(DiscernmentButtonsPrefab, SouthwestSpawn.position, Quaternion.Euler(0, 45, 0));
                break;
            case Edge.Northwest:
                Instantiate(DiscernmentButtonsPrefab, NorthwestSpawn.position, Quaternion.Euler(0, 135, 0));
                break;
            default:
                throw new ArgumentOutOfRangeException("edge", edge, null);
        }
    }

    public void CalibrationRotation(GameObject g, Direction wall )
    {
        switch (wall)
        {
            case Direction.North:
                g.transform.position = NorthWallSpawn.position;
                g.transform.rotation = Quaternion.identity;
                break;
            case Direction.East:
                g.transform.position = EastWallSpawn.position;
                g.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case Direction.South:
                g.transform.position = SouthWallSpawn.position;
                g.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case Direction.West:
                g.transform.position = WestWallSpawn.position;
                g.transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            default:
                throw new ArgumentOutOfRangeException("wall", wall, null);
        }
        
    }

    public void ArrowRotation(GameObject g, bool left)
    {
        g.transform.rotation = left ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
    }

}
