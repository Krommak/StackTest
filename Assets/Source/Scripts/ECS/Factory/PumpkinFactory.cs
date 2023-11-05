using Game.Inject.Installers;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Ecs.Factory
{
    public class PumpkinFactory : IDisposable
    {
        private Dictionary<Transform, Pumpkin> _pumpkins;
        private GameObject[] _pumpkinsPrefabs;
        private Transform _containerForDisable;

        [Inject]
        public PumpkinFactory(PumpkinSpawnSettings spawnSettings)
        {
            _pumpkinsPrefabs = spawnSettings.PumpkinPrefabs;

            _pumpkins = new Dictionary<Transform, Pumpkin>();
            _containerForDisable = new GameObject("PoolOfPumpkins").transform;
        }

        public void SetPumpkin(Transform parent)
        {
            foreach (var item in _pumpkins)
            {
                if(!item.Value.IsActive)
                {
                    item.Value.Enable(parent);
                    return;
                }
            }
            CreateNewPumpkin(parent);
        }

        public void Return(Transform transform)
        {
            if(_pumpkins.ContainsKey(transform))
            {
                _pumpkins[transform].Disable(_containerForDisable);
            }
        }

        public void Dispose()
        {
            _pumpkins = null;
        }

        private void CreateNewPumpkin(Transform parent)
        {
            var newPumpkin = new Pumpkin(
                _pumpkinsPrefabs[UnityEngine.Random.Range(0, _pumpkinsPrefabs.Length)],
                parent);
            _pumpkins.Add(newPumpkin.Object.transform, newPumpkin);
        }

        private class Pumpkin
        {
            public GameObject Object { get; private set; }
            public bool IsActive => Object.activeInHierarchy;

            public Pumpkin(GameObject gameObject, Transform parent)
            {
                Object = GameObject.Instantiate(gameObject, parent);
                Enable(parent);
            }

            public void Enable(Transform parent)
            {
                Object.transform.parent = parent;
                Object.transform.position = Vector3.zero;
                Object.transform.localPosition = Vector3.zero;
                Object.SetActive(true);
            }

            public void Disable(Transform poolTransform)
            {
                Object.transform.parent = poolTransform;
                Object.transform.localScale = Vector3.one;
                Object.transform.position = Vector3.zero;
                Object.transform.localPosition = Vector3.zero;
                Object.transform.eulerAngles = Vector3.zero;

                Object.SetActive(false);
            }
        }
    }
}