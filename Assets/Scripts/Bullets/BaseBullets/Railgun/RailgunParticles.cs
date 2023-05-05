using System.Collections.Generic;
using UnityEngine;

public class RailgunParticles : BaseBulletParticles
{
    protected LineRenderer _trailLineRenderer;

    protected List<Vector3> _positions = new List<Vector3>();

    protected virtual Vector3 StartPosition { get; set; }
    protected virtual Vector3 RigidbodyPosition => _baseBulletController.RigidBody.position;






    protected override void Awake()
    {
        base.Awake();

        GetStartPosition();
    }

    protected virtual void FixedUpdate() => UpdateTrailLineRendererPosition();

    protected virtual void GetStartPosition() => StartPosition = transform.position;

    protected override void InstantiateTrail()
    {
        if (IsAssetReferenceTrailEmpty())
            return;

        _assetReferenceTrail.InstantiateAsync(transform).Completed += asset => 
        {
            bool isParentDestroyed = asset.Result.transform.parent == null;

            if (isParentDestroyed)
                return;

            GetTrailLineRenderer(asset.Result);

            SetTrailLineRendererPosition(0, StartPosition);
            SetTrailLineRendererPosition(1, StartPosition);

            SecondarySoundController.PlaySound(11, 0);
        };
    }

    protected virtual void GetTrailLineRenderer(GameObject trailObj) => _trailLineRenderer = Get<LineRenderer>.From(trailObj);

    protected virtual void SetTrailLineRendererPosition(int index, Vector3 position)
    {
        if (_trailLineRenderer == null)
            return;

        _trailLineRenderer.SetPosition(index, position);
    }

    protected virtual void UpdateTrailLineRendererPosition()
    {
        bool hasReachedPositionLimit = _positions.Count > 20;

        if (hasReachedPositionLimit)
        {
            SetTrailLineRendererPosition(0, _positions[0]);

            _positions.RemoveAt(0);
        }

        _positions.Add(RigidbodyPosition);

        SetTrailLineRendererPosition(1, RigidbodyPosition);
    }
}
