using UnityEngine;

namespace InGame.Entity {
    public abstract class PlayerBase: MonoBehaviour {
        [SerializeField] private int _maxHp = 100;
        public EntityData Data { get; private set; }

        public void SetUp(int pId) {
            Data = new(pId, _maxHp, transform);
        }

        private void Awake() {
            SetUp(-1);
        }
    }
}