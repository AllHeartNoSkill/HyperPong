using System.Collections.Generic;
using UnityEngine;


public class GameplayAudio : MonoBehaviour
{
    [Header("Ball Events")]
    [SerializeField] private GameEvent _wallBounceEvent;
    [SerializeField] private GameEvent_PlayerType _playerBounceEvent;

    [Header("Ball Audio")]
    [SerializeField] private AudioSource _ballAudioSource;
    [SerializeField] private AudioClip _ballWallSound;
    [SerializeField] private AudioClip _ballPlayerSound;

    private void OnEnable()
    {
        _wallBounceEvent.AddListener(OnWallBounce);
        _playerBounceEvent.AddListener(OnPlayerBounce);
    }

    private void OnWallBounce()
    {
        _ballAudioSource.PlayOneShot(_ballWallSound);
    }

    private void OnPlayerBounce(PlayerType player)
    {
        _ballAudioSource.PlayOneShot(_ballPlayerSound);
    }
}
