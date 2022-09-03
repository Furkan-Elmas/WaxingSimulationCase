using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using WaxingSimulation.Events;

namespace WaxingSimulation.Controllers
{
    public class WaxingController : MonoBehaviour
    {
        [SerializeField] float _speed = 5;
        [SerializeField] List<Vector3> _upperSideVertices;

        Mesh _waxMesh;
        Material _waxMaterial;
        Vector3[] _meshVerticesToChange;
        Vector3[] _waxMeshVertexPositions;
        float _maxDistanceForVertex = 50;
        int _selectedVerticeCount = 0;


        void Start()
        {
            _waxMesh = GetComponent<MeshFilter>().mesh;
            _waxMaterial = GetComponent<MeshRenderer>().material;

            _meshVerticesToChange = _waxMesh.vertices;
            _waxMeshVertexPositions = new Vector3[_meshVerticesToChange.Length];

            for (int i = 0; i < _meshVerticesToChange.Length; i++)
            {
                _waxMeshVertexPositions[i] = _meshVerticesToChange[i];
                _meshVerticesToChange[i] = new Vector3(_meshVerticesToChange[i].x, _meshVerticesToChange[i].y, _meshVerticesToChange[i].z + 6.5f);
            }
            _waxMesh.vertices = _meshVerticesToChange;

            _upperSideVertices = new List<Vector3>(new Vector3[_meshVerticesToChange.Length]);
            for (int i = 0; i < _upperSideVertices.Count; i++)
            {
                _upperSideVertices[i] = new Vector3(0, 0, 0);
            }
        }

        float GetTotalRate()
        {
            return (float)_selectedVerticeCount / (float)_upperSideVertices.Count;
        }
        public void DeformMeshAtPoint(Vector3 point)
        {
            point = transform.InverseTransformPoint(point);
            for (int i = 0; i < _meshVerticesToChange.Length; i++)
            {
                Vector3 pointToVertex = _meshVerticesToChange[i] - point;
                if (pointToVertex.sqrMagnitude < _maxDistanceForVertex && !Mathf.Approximately(_meshVerticesToChange[i].z, _waxMeshVertexPositions[i].z))
                {
                    _meshVerticesToChange[i] = Vector3.Lerp(_meshVerticesToChange[i], _waxMeshVertexPositions[i], _speed * Time.deltaTime);

                    if (!_upperSideVertices.Contains(_meshVerticesToChange[i]))
                    {
                        if (Mathf.Approximately(_meshVerticesToChange[i].z, _waxMeshVertexPositions[i].z))
                        {
                            float totalRate = GetTotalRate();
                            if (totalRate > 0.02f)
                            {
                                StartCoroutine(GameCompletedCor());
                            }

                            _upperSideVertices[i] = _meshVerticesToChange[i];
                            _selectedVerticeCount++;
                        }

                        _waxMesh.RecalculateNormals();
                    }
                }
            }
            _waxMesh.vertices = _meshVerticesToChange;
        }

        IEnumerator GameCompletedCor()
        {
            EventManager.WaxDrying();

            float timer = 0;
            while (_waxMaterial.color.a < 0.7f)
            {
                Color color = _waxMaterial.color;
                color.a += Time.fixedDeltaTime * 0.1f;
                _waxMaterial.color = color;
                yield return new WaitForFixedUpdate();
            }

            while (timer < 1.2f)
            {
                timer += Time.fixedDeltaTime;
                transform.position += Vector3.up * Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            EventManager.GameCompleted();
        }
    }
}