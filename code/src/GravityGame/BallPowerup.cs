﻿using Engine;
using Engine.Assets;
using Engine.Audio;
using Engine.Physics;
using System;
using Veldrid.Assets;

namespace GravityGame
{
    public class BallPowerup : Component
    {
        private AudioSystem _audioSystem;
        private AssetSystem _assetSystem;

        public string PowerupType { get; set; } = string.Empty;
        public AssetRef<WaveFile> SoundEffect { get; set; }
        public float Volume { get; set; } = 1.0f;

        protected override void Attached(SystemRegistry registry)
        {
            _audioSystem = registry.GetSystem<AudioSystem>();
            _assetSystem = registry.GetSystem<AssetSystem>();
        }

        protected override void OnDisabled()
        {
            GameObject.GetComponent<Collider>().TriggerEntered -= OnTriggerEntered;
        }

        protected override void OnEnabled()
        {
            GameObject.GetComponent<Collider>().TriggerEntered += OnTriggerEntered;
        }

        private void OnTriggerEntered(Collider other)
        {
            if (other.GameObject.GetComponent<CharacterMarker>() != null)
            {
                other.GameObject.AddComponent(CreateBallComponent());
                if (!SoundEffect.ID.IsEmpty)
                {
                    _audioSystem.PlaySound(_assetSystem.Database.LoadAsset(SoundEffect), Volume);
                }
                GameObject.Destroy();
            }
        }

        private Component CreateBallComponent()
        {
            Type powerupType = Type.GetType(PowerupType);
            return (Component)Activator.CreateInstance(powerupType);
        }

        protected override void Removed(SystemRegistry registry)
        {
        }
    }
}
