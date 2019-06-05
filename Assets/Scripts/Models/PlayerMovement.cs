﻿using Assets.Scripts.BaseScripts;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class PlayerMovement
    {
        public float Speed = 5f;

        public float RunSpeed = 10f;

        public float RotateSpeed = 15f;

        public float AimRotateSpeed = 120f;

        public float Gravity = -9.81f;

        public float JumpHeight = 10f;

        public float GroundRayDistanceOffset = 0.3f;

        public float MinimumFall = -3.0f;

        public float TerminalFall = -15.0f;

        public float RollDistance = 5f;

        public float RollSpeed = 5f;
    }
}
