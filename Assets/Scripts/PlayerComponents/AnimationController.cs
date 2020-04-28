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

        public AnimationController(Animator animator, Rigidbody rigidBody, Pickup pickup)
        {
            _animator = animator;
            _rigidBody = rigidBody;
            _pickup = pickup;
            _equippableItems.Add(ItemType.Hammer);
            _equippableItems.Add(ItemType.Sword);
        }

        public void Tick()
        {
            _animator.SetFloat("Speed", _rigidBody.velocity.magnitude);
            
            var isCarryingState = _animator.GetCurrentAnimatorStateInfo(1).IsTag("Carry");
            if (isCarryingState)
            {
                _animator.SetLayerWeight(1, _pickup.PickedUpItem == null ? 0 : 1);
            }

            if (_pickup.PickedUpItem != null)
            {
                var equippableItem = _equippableItems.Contains(_pickup.PickedUpItem.itemType);
                _animator.SetBool("Carrying", !equippableItem);
            }
        }

        public void Throw()
        {
            _animator.SetTrigger("Throw");
        }

        public void Interact()
        {
            _animator.SetTrigger("Interact");
        }
    }
}