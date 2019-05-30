﻿using Assets.Scripts.BaseScripts;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class PlayerMovement
    {
        public float Speed = 5f;

        public float RunSpeed = 10f;

        public float RotateSpeed = 15f;

        public float AimRotateSpeed = 20f;

        public float Gravity = -9.81f;

        public float JumpHeight = 10f;

        public float GroundRayDistance = 1.3f;

        public float MinimumFall = -1.0f;

        public float TerminalFall = -15.0f;
    }
}