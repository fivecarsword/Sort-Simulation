using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Sort_Simulation {
    public static class Sort {
        // 버블 정렬 함수 코드
        public static IEnumerator<SortState> BubbleSort(int[] arr) {
            // 정렬 시작을 알리기 위한 반환
            yield return new SortState(SortStateType.Start);

            for (int i = arr.Length - 1; i > 0; i--) {
                for (int j = 0; j < i; j++) {

                    // 비교 상태를 보여주기 위한 반환
                    yield return new SortState(SortStateType.Compare, new List<SpecialStateValue>{ new SpecialStateValue(j + 1, arr[j + 1], Color.Red), new SpecialStateValue(j, arr[j], Color.Red) });
                    
                    if (arr[j] > arr[j + 1]) {
                        Swap(ref arr[j], ref arr[j + 1]);
                        
                        // 교환 상태를 보여주기 위한 반환
                        yield return new SortState(SortStateType.Swap, new List<SpecialStateValue> { new SpecialStateValue(j + 1, arr[j + 1], Color.Red), new SpecialStateValue(j, arr[j], Color.Red) });
                    }
                }
            }

            // 정렬 종료를 알리기 위한 반환
            yield return new SortState(SortStateType.Sorted);
        }

        // 선택 정렬 함수 코드
        public static IEnumerator<SortState> SelectionSort(int[] arr) {
            // 정렬 시작을 알리기 위한 반환
            yield return new SortState(SortStateType.Start);

            for (int i = 0; i < arr.Length; i++) {
                int index = i;

                for (int j = i + 1; j < arr.Length; j++) {

                    // 비교 상태를 보여주기 위한 반환
                    yield return new SortState(SortStateType.Compare, new List<SpecialStateValue> { new SpecialStateValue(j, arr[j], Color.Red), new SpecialStateValue(index, arr[index], Color.Red), new SpecialStateValue(i, arr[i], Color.Orange) });

                    if (arr[index] > arr[j]) {
                        index = j;
                    }
                }

                Swap(ref arr[i], ref arr[index]);

                // 교환 상태를 보여주기 위한 반환
                yield return new SortState(SortStateType.Swap, new List<SpecialStateValue> { new SpecialStateValue(index, arr[index], Color.Red), new SpecialStateValue(i, arr[i], Color.Red) });
            }

            // 정렬 종료를 알리기 위한 반환
            yield return new SortState(SortStateType.Sorted);
        }

        // 삽입 정렬 함수 코드
        public static IEnumerator<SortState> InsertionSort(int[] arr) {
            // 정렬 시작을 알리기 위한 반환
            yield return new SortState(SortStateType.Start);

            for (int i = 1; i < arr.Length; i++) {
                for (int j = i; j > 0; j--) {

                    // 비교 상태를 보여주기 위한 반환
                    yield return new SortState(SortStateType.Compare, new List<SpecialStateValue> { new SpecialStateValue(j - 1, arr[j - 1], Color.Red), new SpecialStateValue(j, arr[j], Color.Red) });

                    if (arr[j - 1] > arr[j]) {
                        Swap(ref arr[j - 1], ref arr[j]);

                        // 교환 상태를 보여주기 위한 반환
                        yield return new SortState(SortStateType.Swap, new List<SpecialStateValue> { new SpecialStateValue(j - 1, arr[j - 1], Color.Red), new SpecialStateValue(j, arr[j], Color.Red) });

                    } else {
                        break;
                    }
                }
            }

            // 정렬 종료를 알리기 위한 반환
            yield return new SortState(SortStateType.Sorted);
        }

        // 두 값 위치 교환 함수 코드
        private static void Swap(ref int a, ref int b) {
            int temp = a;
            a = b;
            b = temp;
        }
    }
}
