using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Sort_Simulation {
    public static class Sort {
        // 버블 정렬 함수 코드
        public static IEnumerable<SortState> BubbleSort(int[] arr) {
            arr = (int[])arr.Clone();

            // 정렬 시작을 알리기 위한 반환
            yield return new SortState(SortStateType.Start, new List<ArrayState> { new ArrayState(arr, 0) });

            for (int i = arr.Length - 1; i > 0; i--) {
                for (int j = 0; j < i; j++) {

                    // 비교 상태를 보여주기 위한 반환
                    yield return new SortState(SortStateType.Compare, new List<ArrayState> {
                        new ArrayState(
                            arr,
                            0,
                            new List<SpecialStateValue> {
                                new SpecialStateValue(j + 1, Color.Red),
                                new SpecialStateValue(j, Color.Red)
                            }
                        )
                     });
                    
                    if (arr[j] > arr[j + 1]) {
                        Swap(ref arr[j], ref arr[j + 1]);

                        // 교환 상태를 보여주기 위한 반환
                        yield return new SortState(SortStateType.Swap, new List<ArrayState> {
                            new ArrayState(
                                arr,
                                0,
                                new List<SpecialStateValue> {
                                    new SpecialStateValue(j + 1, Color.Red),
                                    new SpecialStateValue(j, Color.Red)
                                }
                            )
                        });
                    }
                }
            }

            // 정렬 종료를 알리기 위한 반환
            yield return new SortState(SortStateType.Sorted, new List<ArrayState> { new ArrayState(arr, 0) });
        }

        // 선택 정렬 함수 코드
        public static IEnumerable<SortState> SelectionSort(int[] arr) {
            arr = (int[])arr.Clone();

            // 정렬 시작을 알리기 위한 반환
            yield return new SortState(SortStateType.Start, new List<ArrayState> { new ArrayState(arr, 0) });

            for (int i = 0; i < arr.Length; i++) {
                int index = i;

                for (int j = i + 1; j < arr.Length; j++) {

                    // 비교 상태를 보여주기 위한 반환
                    yield return new SortState(SortStateType.Compare, new List<ArrayState> {
                        new ArrayState(
                            arr,
                            0,
                            new List<SpecialStateValue> {
                                new SpecialStateValue(index, Color.Red),
                                new SpecialStateValue(j, Color.Red),
                                new SpecialStateValue(i, Color.Orange)
                            }
                        )
                     });

                    if (arr[index] > arr[j]) {
                        index = j;
                    }
                }

                Swap(ref arr[i], ref arr[index]);

                // 교환 상태를 보여주기 위한 반환
                yield return new SortState(SortStateType.Swap, new List<ArrayState> {
                    new ArrayState(
                        arr,
                        0,
                        new List<SpecialStateValue> {
                            new SpecialStateValue(index, Color.Red),
                            new SpecialStateValue(i, Color.Red)
                        }
                    )
                });
            }

            // 정렬 종료를 알리기 위한 반환
            yield return new SortState(SortStateType.Sorted, new List<ArrayState> { new ArrayState(arr, 0) });
        }

        // 삽입 정렬 함수 코드
        public static IEnumerable<SortState> InsertionSort(int[] arr) {
            arr = (int[])arr.Clone();

            // 정렬 시작을 알리기 위한 반환
            yield return new SortState(SortStateType.Start, new List<ArrayState> { new ArrayState(arr, 0) });

            for (int i = 1; i < arr.Length; i++) {
                for (int j = i; j > 0; j--) {

                    // 비교 상태를 보여주기 위한 반환
                    yield return new SortState(SortStateType.Compare, new List<ArrayState> {
                        new ArrayState(
                            arr,
                            0,
                            new List<SpecialStateValue> {
                                new SpecialStateValue(j - 1, Color.Red),
                                new SpecialStateValue(j, Color.Red)
                            }
                        )
                     });

                    if (arr[j - 1] > arr[j]) {
                        Swap(ref arr[j - 1], ref arr[j]);

                        // 교환 상태를 보여주기 위한 반환
                        yield return new SortState(SortStateType.Swap, new List<ArrayState> {
                            new ArrayState(
                                arr,
                                0,
                                new List<SpecialStateValue> {
                                    new SpecialStateValue(j - 1, Color.Red),
                                    new SpecialStateValue(j, Color.Red)
                                }
                            )
                        });

                    } else {
                        break;
                    }
                }
            }

            // 정렬 종료를 알리기 위한 반환
            yield return new SortState(SortStateType.Sorted, new List<ArrayState> { new ArrayState(arr, 0) });
        }

        // 병합 정렬 함수 코드
        public static IEnumerable<SortState> MergeSort(int[] arr) {
            arr = (int[])arr.Clone();

            // 정렬 시작을 알리기 위한 반환
            yield return new SortState(SortStateType.Start, new List<ArrayState> { new ArrayState(arr, 0) });

            foreach (SortState state in Merge(arr, 0, arr.Length - 1)) {
                yield return state;
            }

            // 정렬 종료을 알리기 위한 반환
            yield return new SortState(SortStateType.Sorted, new List<ArrayState> { new ArrayState(arr, 0) });
        }

        // 병합 정렬 재귀 진행을 위한 함수
        private static IEnumerable<SortState> Merge(int[] arr, int start, int end) {
            if (start == end) {
                yield break;
            }

            int mid = (start + end) / 2;

            foreach (SortState state in Merge(arr, start, mid)) {
                yield return state;
            }

            foreach (SortState state in Merge(arr, mid + 1, end)) {
                yield return state;
            }

            int i = 0;

            int[] temp = new int[end - start + 1];

            int left = start;
            int right = mid + 1;

            while (left <= mid && right <= end) {

                // 비교 상태를 보여주기 위한 반환
                yield return new SortState(SortStateType.Compare, new List<ArrayState> {
                    new ArrayState(
                        arr,
                        0,
                        new List<SpecialStateValue> {
                            new SpecialStateValue(left, Color.Red),
                            new SpecialStateValue(right, Color.Red)
                        }
                    ),
                    new ArrayState(
                        temp,
                        start,
                        new List<SpecialStateValue> {
                            new SpecialStateValue(i, Color.LightGreen),
                        }
                    )
                });

                if (arr[left] <= arr[right]) {
                    Swap(ref arr[left], ref temp[i]);

                    // 교환 상태를 보여주기 위한 반환
                    yield return new SortState(SortStateType.Swap, new List<ArrayState> {
                        new ArrayState(
                            arr,
                            0,
                            new List<SpecialStateValue> {
                                new SpecialStateValue(left, Color.Red),
                                new SpecialStateValue(right, Color.Red)
                            }
                        ),
                        new ArrayState(
                            temp,
                            start,
                            new List<SpecialStateValue> {
                                new SpecialStateValue(i, Color.LightGreen),
                            }
                        )
                    });

                    left++;
                    i++;
                } else {
                    Swap(ref arr[right], ref temp[i]);

                    // 교환 상태를 보여주기 위한 반환
                    yield return new SortState(SortStateType.Swap, new List<ArrayState> {
                        new ArrayState(
                            arr,
                            0,
                            new List<SpecialStateValue> {
                                new SpecialStateValue(left, Color.Red),
                                new SpecialStateValue(right, Color.Red)
                            }
                        ),
                        new ArrayState(
                            temp,
                            start,
                            new List<SpecialStateValue> {
                                new SpecialStateValue(i, Color.LightGreen),
                            }
                        )
                    });

                    right++;
                    i++;
                }
            }

            while (left <= mid) {
                Swap(ref arr[left], ref temp[i]);

                // 교환 상태를 보여주기 위한 반환
                yield return new SortState(SortStateType.Swap, new List<ArrayState> {
                        new ArrayState(
                            arr,
                            0,
                            new List<SpecialStateValue> {
                                new SpecialStateValue(left, Color.Red),
                            }
                        ),
                        new ArrayState(
                            temp,
                            start,
                            new List<SpecialStateValue> {
                                new SpecialStateValue(i, Color.LightGreen),
                            }
                        )
                    });

                left++;
                i++;
            }

            while (right <= end) {
                Swap(ref arr[right], ref temp[i]);

                // 교환 상태를 보여주기 위한 반환
                yield return new SortState(SortStateType.Swap, new List<ArrayState> {
                        new ArrayState(
                            arr,
                            0,
                            new List<SpecialStateValue> {
                                new SpecialStateValue(right, Color.Red),
                            }
                        ),
                        new ArrayState(
                            temp,
                            start,
                            new List<SpecialStateValue> {
                                new SpecialStateValue(i, Color.LightGreen),
                            }
                        )
                    });

                right++;
                i++;
            }

            temp.CopyTo(arr, start);
        }

        public static IEnumerable<SortState> HeapSort(int[] arr) {
            arr = (int[])arr.Clone();

            // 정렬 시작을 알리기 위한 반환
            yield return new SortState(SortStateType.Start, new List<ArrayState> { new ArrayState(arr, 0) });

            // 힙 정리
            for (int i = 1; i < arr.Length; i++) {
                int index = i;
                int parent = (index - 1) / 2;

                while (index != 0 && arr[index] > arr[parent]) {
                    // 비교 상태를 보여주기 위한 반환
                    yield return new SortState(SortStateType.Compare, new List<ArrayState> {
                        new ArrayState(
                            arr,
                            0,
                            new List<SpecialStateValue> {
                                new SpecialStateValue(index, Color.Red),
                                new SpecialStateValue(parent, Color.Red)
                            }
                        )
                    });

                    Swap(ref arr[index], ref arr[parent]);

                    // 교환 상태를 보여주기 위한 반환
                    yield return new SortState(SortStateType.Swap, new List<ArrayState> {
                        new ArrayState(
                            arr,
                            0,
                            new List<SpecialStateValue> {
                                new SpecialStateValue(index, Color.Red),
                                new SpecialStateValue(parent, Color.Red)
                            }
                        )
                    });

                    index = parent;
                    parent = (index - 1) / 2;
                }
            }

            // 정렬
            for (int i = arr.Length - 1; i >= 1; i--) {
                Swap(ref arr[0], ref arr[i]);

                // 교환 상태를 보여주기 위한 반환
                yield return new SortState(SortStateType.Swap, new List<ArrayState> {
                    new ArrayState(
                        arr,
                        0,
                        new List<SpecialStateValue> {
                            new SpecialStateValue(0, Color.Red),
                            new SpecialStateValue(i, Color.Red)
                        }
                    )
                });

                int index = 0;
                int child = index * 2 + 1;

                while (child < i) {
                    int temp;
                    if (child + 1 >= i) {
                        temp = child;
                    } else {
                        // 비교 상태를 보여주기 위한 반환
                        yield return new SortState(SortStateType.Compare, new List<ArrayState> {
                            new ArrayState(
                                arr,
                                0,
                                new List<SpecialStateValue> {
                                    new SpecialStateValue(child, Color.Red),
                                    new SpecialStateValue(child + 1, Color.Red)
                                }
                            )
                        });
                        temp = arr[child] < arr[child + 1] ? child + 1 : child;
                    }

                    // 비교 상태를 보여주기 위한 반환
                    yield return new SortState(SortStateType.Compare, new List<ArrayState> {
                        new ArrayState(
                            arr,
                            0,
                            new List<SpecialStateValue> {
                                new SpecialStateValue(index, Color.Red),
                                new SpecialStateValue(temp, Color.Red)
                            }
                        )
                    });

                    if (arr[index] < arr[temp]) {
                        Swap(ref arr[index], ref arr[temp]);

                        // 교환 상태를 보여주기 위한 반환
                        yield return new SortState(SortStateType.Swap, new List<ArrayState> {
                            new ArrayState(
                                arr,
                                0,
                                new List<SpecialStateValue> {
                                    new SpecialStateValue(index, Color.Red),
                                    new SpecialStateValue(temp, Color.Red)
                                }
                            )
                        });

                        index = temp;
                        child = index * 2 + 1;
                        continue;
                    }

                    break;
                }
            }

            // 정렬 종료을 알리기 위한 반환
            yield return new SortState(SortStateType.Sorted, new List<ArrayState> { new ArrayState(arr, 0) });
        }

        // 두 값 위치 교환 함수 코드
        private static void Swap(ref int a, ref int b) {
            int temp = a;
            a = b;
            b = temp;
        }
    }
}
