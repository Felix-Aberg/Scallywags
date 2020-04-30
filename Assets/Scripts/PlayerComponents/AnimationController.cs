using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class AnimationController
    {
        private Animator _animator;
        private Rigidbody _rigidBody;
        private Pickup _pickup;
        private List<ItemType> _equippableItems = new List<ItemType>();
        private Jump _jump;

        public AnimationController(Animator animator, Rigidbody rigidBody, Pickup pickup, Jump jump)
        {
            _animator = animator;
            _rigidBody = rigidBody;
            _pickup = pickup;
            _jump = jump;
            _equippableItems.Add(ItemType.Hammer);
            _equippableItems.Add(ItemType.Sword);
        }

        public void Tick()
        {
            _animator.SetFloat("Speed", _rigidBody.velocity.sqrMagnitude);
            
            if (_pickup.PickedUpItem != null)
            {
                var equippableItem = _equippableItems.Contains(_pickup.PickedUpItem.itemType);
                _animator.SetBool("Carrying", !equippableItem);
                _animator.SetLayerWeight(1, 1);
            }
            else
            {
                _animator.SetLayerWeight(1, 0);
            }

            _animator.SetBool("Grounded", _jump.IsGrounded());
        }

        public void Throw()
        {
            _animator.SetTrigger("Throw");
        }

        public void Interact(ItemType type)
        {
            switch (type)
            {
                case ItemType.Sword:
                    _animator.SetTrigger("Sword");
                    break;
                default:
                    _animator.SetTrigger("Interact");
                    break;
            }
        }
    }
}