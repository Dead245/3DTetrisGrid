using GridSystem.Core;
using GridSystem.Items;
using System;
using UnityEngine;

namespace GridSystem.PickupLogic {
    public class Pickup : MonoBehaviour {
        #region Variables
        [SerializeField]
        private float itemLerpSpeed = 1f;
        [SerializeField]
        private float maxRayDistance = 4f;
        [SerializeField, Tooltip("The space between where the item is held and the point of contact")]
        private float raySpaceBuffer = 0.2f;
        //Position the item is being held at.
        public Vector3 itemPos { get; private set; }

        public bool isItemGrabbed;
        public GameObject grabbedItem;
        public Rigidbody itemRB;

        public GridManager interactingGridManager = null;
        private Vector3Int snappedCell;
        public GameObject GrabbedItem => grabbedItem;
        #endregion

        private void FixedUpdate() {
            //For moving an item around once grabbed
            if (grabbedItem != null) {
                //Item Rotation Handling
                grabbedItem.transform.rotation = Quaternion.Lerp(
                        grabbedItem.transform.rotation,
                        grabbedItem.GetComponent<ItemManager>().rotation,
                        itemLerpSpeed * Time.fixedDeltaTime);

                //Item Movement Handling

                itemRB.linearVelocity = UpdateHeldItemPosition() / Time.fixedDeltaTime;
            }
        }
        private Vector3 UpdateHeldItemPosition() {

            Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit[] hits = new RaycastHit[3];
            Physics.RaycastNonAlloc(ray, hits, maxRayDistance);

            if (hits.Length == 0) {
                return ray.GetPoint(maxRayDistance) - grabbedItem.transform.position;
            }

            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            Collider triggerHit = null;
            Collider solidHit  = null;
            Vector3? solidHitPos = null;
            Collider itemInTriggerHit = null;

            foreach (RaycastHit hit in hits) {
                if (!hit.transform) {
                    continue;
                }

                if (hit.collider.gameObject.Equals(grabbedItem)) {
                    continue;
                }
                
                if (hit.collider.isTrigger) {
                    //Hit a trigger
                    triggerHit = hit.collider;
                    continue;
                } else if (triggerHit) {
                    //Hit object in a trigger collider
                    if (triggerHit.bounds.Contains(hit.point)) {
                        itemInTriggerHit = hit.collider;
                        break;
                    }
                } else {
                    //Hit object is not in trigger
                    solidHit = hit.collider;
                    solidHitPos = hit.point;
                    break;
                }
            }

            if (itemInTriggerHit) {
                //Hit something IN a trigger volume (a grid)
                return itemInTriggerHit.transform.position - grabbedItem.transform.position;
            } else if (triggerHit && solidHit) {
                //The ray passed through the trigger and hit something behind it
                return GetCellLocation(ray.GetPoint(maxRayDistance), ray, triggerHit);
            } else if (triggerHit && !solidHit) {
                if (triggerHit.bounds.Contains(ray.GetPoint(maxRayDistance))) {
                    //Ray ended inside
                    return GetCellLocation(ray.GetPoint(maxRayDistance), ray, triggerHit);
                } else {
                    //Ray past a trigger
                    return GetCellLocation(ray.GetPoint(maxRayDistance), ray, triggerHit);
                }

            } else if (solidHit) {
                //Hit only a solid object
                Vector3 direction = (Vector3)solidHitPos - (ray.direction * raySpaceBuffer);
                return direction - grabbedItem.transform.position;
            }

            return ray.GetPoint(maxRayDistance) - grabbedItem.transform.position;
        }

        private Vector3 GetCellLocation(Vector3 hitPoint, Ray mainRay, Collider triggerCollider) {
            itemPos = mainRay.GetPoint(maxRayDistance);
            Vector3? nearestSnapPoint;
            if (interactingGridManager == null) {
                nearestSnapPoint = null;
            } else {
                ItemManager itemMang = grabbedItem.GetComponent<ItemManager>();
                nearestSnapPoint = interactingGridManager.GetNearestEmptyCell(itemMang, itemPos);
            }
            //if in grid = GRID CELL POS OF END OF RAY
            if (nearestSnapPoint != null) {
                itemPos = mainRay.GetPoint(maxRayDistance);
                ItemManager itemMang = grabbedItem.GetComponent<ItemManager>();
                nearestSnapPoint = interactingGridManager.GetNearestEmptyCell(itemMang, itemPos);
                return (Vector3)nearestSnapPoint - grabbedItem.transform.position;
            } else {
                //if past grid = GET BACK FACE & GRID CELL POS OF BACK FACE
                Ray backwardsRay = new(hitPoint, -mainRay.direction);
                RaycastHit backwardsRayHit;
                if (triggerCollider.Raycast(backwardsRay, out backwardsRayHit, maxRayDistance)) {
                    itemPos = backwardsRayHit.point + backwardsRay.direction * 0.1f;
                    backwardsRayHit.collider.TryGetComponent<GridManager>(out interactingGridManager);
                    if (interactingGridManager == null) {
                        Debug.LogWarning($"Failed to get GridManager from {backwardsRayHit.collider.name}");
                        return itemPos - grabbedItem.transform.position;
                    }

                    ItemManager itemMang = grabbedItem.GetComponent<ItemManager>();
                    nearestSnapPoint = interactingGridManager.GetNearestEmptyCell(itemMang, itemPos);

                    if (nearestSnapPoint == null) {
                        Debug.LogWarning("ClosestSnapPoint still NULL");
                        return itemPos - grabbedItem.transform.position;
                    }
                    return ((Vector3)nearestSnapPoint - grabbedItem.transform.position);
                } else {
                    Debug.LogWarning("Backwards Ray didn't hit anything!");
                    return itemPos - grabbedItem.transform.position;
                }
            }
        }
    }
}
