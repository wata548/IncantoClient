using Extension;
using UnityEngine;

namespace InGame.Map {
    public class Map: MonoSingleton<Map> {
        protected override bool AllowAutoGen => false;
        protected override bool IsNarrowSingleton => true;

        [SerializeField] private Transform[] _playerPositions;

        public Vector3 GetPos(int pIdx) => _playerPositions[pIdx].transform.position;
    }
}