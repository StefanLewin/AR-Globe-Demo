using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARGlobe
{
        public class CubeController : MonoBehaviour
        {
                private List<InteractionCube> _cubes;
                private float _cubeEdgeLength;
                private Vector3[] _positions;
                private Color[] _flagColors;

                private void Start()
                {
                        _cubes = new List<InteractionCube>();
                        foreach (Transform cube in this.transform)
                        {
                                var newCube = new InteractionCube(cube.gameObject, cube.transform.position, false);
                                _cubes.Add(newCube);
                        }
                        _flagColors = new Color[_cubes.Count];
                        _positions = new Vector3[_cubes.Count];
                }

                private void Update()
                {
                        GetPositions();
                }

                Vector3[] GetPositions()
                {
                        for (int i = 0; i < _positions.Length; i++)
                        {
                                _positions[i] = _cubes[i].GetPosition();
                        }
                
                        return _positions;
                }
        
                Direction GetAlignment()
                {
                        float minX = _positions.Min(v => v.x);
                        float maxX = _positions.Max(v => v.x);
                
                        return Mathf.Abs(maxX - minX) < _cubes[0].EdgeLength ? Direction.VERTICAL : Direction.HORIZONTAL;
                }
        

                int[] GetCubeOrder()
                {
                        var dir = GetAlignment();
                        var selections = _positions
                                .Select((v, i) => new { Value = (dir == Direction.VERTICAL) ? v.z : v.x, Index = i })
                                .OrderBy(v => v.Value);

                        var minIndex = (dir == Direction.VERTICAL) ? selections.Last().Index : selections.First().Index;
                        var maxIndex = (dir == Direction.VERTICAL) ? selections.First().Index : selections.Last().Index;

                        var middleIndex = 3 - (maxIndex + minIndex);
                
                        return new int[] { minIndex, middleIndex, maxIndex };
                }

                public int GetNumberOfEnteredCubes()
                {
                        return _cubes.Sum(cube => cube.EnteredCard ? 1 : 0);
                }

                public void CompareCubes(GameObject otherCube, bool entered)
                {
                        foreach (var cube in _cubes)
                        {
                                if (!cube.Cube.Equals(otherCube)) continue;
                                if(cube.EnteredCard && entered || !cube.EnteredCard && !entered) return;

                                Debug.Log(entered
                                        ? "Object entered: " + otherCube.gameObject.name
                                        : "Object exited: " + otherCube.gameObject.name);
                        
                                cube.EnteredCard = entered;
                                return;
                        }
                        Debug.Log("No cubes found.");
                
                }

                public bool ValidateColors()
                {
                        var indices = GetCubeOrder();
                        var firstColor = _cubes[indices[0]].GetColor();
                        var secondColor = _cubes[indices[1]].GetColor();
                        var thirdColor = _cubes[indices[2]].GetColor();
                
                        Debug.Log(PrintCubes());
                
                        return (firstColor == _flagColors[0] && 
                                secondColor == _flagColors[1] && 
                                thirdColor == _flagColors[2]);
                }

                public void ResetCubes()
                {
                        foreach (var cube in _cubes)
                        {
                                cube.ResetPosition();
                        }
                }

                public void AssignColors(CountryFlag flag)
                {
                        _flagColors = flag.FlagColours;
                        var randColors = flag.FlagColours;
                        randColors = randColors.OrderBy(x => Random.Range(0f, 1f)).ToArray();

                        for (var i = 0; i < _cubes.Count; i++)
                        {
                                _cubes[i].SetColor(randColors[i]);
                        }
                }

                string PrintCubes()
                {
                        var dir = GetAlignment();
                        var indices = GetCubeOrder();
                
                        if (dir == Direction.VERTICAL)
                        {
                                return  $"(VER) Oben: {_cubes[indices[0]].Cube.name} | " +
                                        $"Mitte: {_cubes[indices[1]].Cube.name} | " +
                                        $"Unten: {_cubes[indices[2]].Cube.name}";
                        }
                        else
                        {
                                return  $"(HOR) inks: {_cubes[indices[0]].Cube.name} | " +
                                        $"Mitte: {_cubes[indices[1]].Cube.name} | " +
                                        $"Rechts: {_cubes[indices[2]].Cube.name}";
                        }
                }
        }

        public class InteractionCube
        {
                public readonly GameObject Cube;
                public bool EnteredCard;
                public float EdgeLength;
        
                private readonly Vector3 _initPos;
                private readonly MeshRenderer _cubeRenderer;


                public InteractionCube(GameObject cube, Vector3 initPos, bool enteredCard)
                {
                        Cube = cube;
                        _initPos = initPos;
                        EnteredCard = enteredCard;
                        _cubeRenderer = cube.GetComponent<MeshRenderer>();
                        EdgeLength = _cubeRenderer.bounds.size.x;
                }

                public Color GetColor()
                {
                        return _cubeRenderer.material.color;
                }

                public void SetColor(Color c)
                {
                        _cubeRenderer.material.color = c;
                }

                public Vector3 GetPosition()
                {
                        return Cube.transform.position;
                }

                public void ResetPosition()
                {
                        Cube.GetComponent<Rigidbody>().MovePosition(_initPos);
                }
        

        }
}