using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player.Animation
{
    public class PlayerAnimController : MonoBehaviour
    {
        private Animator _animator;
        private PlayerMovement _playerController;
        private string currentState;
        //Animation States
        const string PLAYER_IDLE = "idleAnim";


        public string PLAYER_RUN = "runAnim";

        const string PLAYER_JUMP = "jumpAnim";
        const string PLAYER_WALKBACKWARD = "walkBackwardAnim";
        const string PLAYER_WALK = "walkAnim";




        public void ChangeAnimationState(string newState)
        {
            if (currentState == newState) return;
            _animator.Play(newState);
            currentState = newState;

        }
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _playerController = GetComponent<PlayerMovement>();
        }
        private void Start()
        {

        }
        private void Update()
        {



        }

    }
}