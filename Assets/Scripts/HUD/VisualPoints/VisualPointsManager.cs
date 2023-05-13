using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPointsManager : MonoBehaviour
{
    [SerializeField]
    private VisualPoints[] _visualPoints;

    private List<int> _generatedNumbers;

    private Vector3 _visualPointsStartPosition;
    private Vector3 _targetPosition;

    



    public void VisualizePoints(int value, Vector3 visualPointsStartPosition, Vector3 targetPosition, bool convertStartPositionToScreenSpace = true, bool convertTargetPositionToScreenSpace = true)
    {
        StartCoroutine(Execute(value, visualPointsStartPosition, targetPosition, convertStartPositionToScreenSpace, convertTargetPositionToScreenSpace));
    }

    private IEnumerator Execute(int value, Vector3 visualPointsStartPosition, Vector3 targetPosition, bool convertToScreenSpace, bool convertTargetPositionToScreenSpace)
    {
        _visualPointsStartPosition = convertToScreenSpace ? CameraSight.ScreenPoint(visualPointsStartPosition) : visualPointsStartPosition;

        _targetPosition = convertTargetPositionToScreenSpace ? CameraSight.ScreenPoint(targetPosition) : targetPosition;

        yield return StartCoroutine(GenerateRandomNumbers(Mathf.Abs(value)));

        yield return StartCoroutine(InitializeVisualPoints(isNegativeNumber: value < 0));
    }

    private IEnumerator GenerateRandomNumbers(int value)
    {
        _generatedNumbers = new List<int>();

        for (int i = 0; i < _visualPoints.Length; i++)
        {
            int minNum = _generatedNumbers.Count > 0 ? _generatedNumbers[_generatedNumbers.Count - 1] : 0;
            int maxNum = value + 1;
            int randomNum = Random.Range(minNum, maxNum);

            bool isWithinRange = randomNum > 0 && randomNum < value;
            bool isLastVisualPoint = i == _visualPoints.Length - 1;

            // Ensure that the '_generatedNumbers' list always contains stored values.
            // Calculate the value of the last visual point:
            //   - If random numbers were generated, it is 'value' minus the sum of previously generated numbers.
            //   - If no random numbers were generated, it is equal to the total value of 'value'.

            if (isLastVisualPoint)
            {
                _generatedNumbers.Add(_generatedNumbers.Count > 0 ? value - _generatedNumbers[_generatedNumbers.Count - 1] : value);

                print($"LastVisualPoint: {_generatedNumbers.Count}");

                continue;
            }

            // Add numbers to the '_generatedNumbers' list if they are within the range of greater than 0 and less than the total value.

            if (isWithinRange)
            {
                _generatedNumbers.Add(randomNum);

                print($"WithinRange: {_generatedNumbers.Count}");
            }
            else
                continue;

            yield return null;
        }
    }

    private IEnumerator InitializeVisualPoints(bool isNegativeNumber = false)
    {
        int index = 0;
        //int length = _generatedNumbers.Count > _visualPoints.Length ? _visualPoints.Length : _generatedNumbers.Count;

        while (index < _generatedNumbers.Count)
        {
            int num = isNegativeNumber ? -_generatedNumbers[index] : _generatedNumbers[index];

            _visualPoints[index].Initialize(num.ToString(), _visualPointsStartPosition, _targetPosition);

            index++;

            yield return new WaitForSeconds(0.1f);
        }
    }
}
