using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;
using Point = System.Drawing.Point;
using Size = System.Windows.Size;

namespace Sort_Simulation {
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window {
        Random random = new Random();

        int[] arr = { 0 };
        IEnumerator<SortState> sort;

        int swapCount = 0;
        int compareCount = 0;

        int SwapCount {
            get => swapCount;
            set {
                swapCount = value;

                swapCountLabel.Content = $"Swap : {swapCount}";
            }
        }

        int CompareCount {
            get => compareCount;
            set {
                compareCount = value;

                compareCountLabel.Content = $"Compare : {compareCount}";
            }
        }

        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow() {
            arr = new int[10];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = i + 1;
            }

            Shuffle(arr);

            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += (sender, args) => {
                Step();
            };

            InitializeComponent();

            Draw(new SortState(SortStateType.None, new List<ArrayState> { new ArrayState(arr, 0) }));
        }

        void ChangeArrayCount(int n) {
            arr = new int[n];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = i + 1;
            }
        }

        void Shuffle(int[] arr) {
            List<int> copy = arr.ToList();

            for (int i = 0; i < arr.Length; i++) {
                int index = random.Next(copy.Count);
                arr[i] = copy[index];
                copy.RemoveAt(index);
            }
        }

        void Draw(SortState state) {
            Bitmap bitmap = new Bitmap(arr.Length, arr.Length);

            using (Graphics gr = Graphics.FromImage(bitmap)) {
                foreach (ArrayState arrayState in state.arrayStates) {
                    int[] array = arrayState.array;
                    int offset = arrayState.offset;

                    for (int i = 0; i < array.Length; i++) {
                        gr.DrawLine(new Pen(Color.White), new Point(i + offset, arr.Length - array[i]), new Point(i + offset, arr.Length));
                    }

                    foreach (SpecialStateValue value in arrayState.specialStateValues) {
                        gr.DrawLine(new Pen(value.color), new Point(value.index + offset, arr.Length - array[value.index]), new Point(value.index + offset, arr.Length));
                    }
                }
            }

            image.Source = GetBitmapImage(bitmap);
        }

        private void Run(object sender, RoutedEventArgs e) {
            switch (sortTypeBox.SelectedIndex) {
                case 0:
                    sort = Sort.BubbleSort(arr).GetEnumerator();
                    break;
                case 1:
                    sort = Sort.SelectionSort(arr).GetEnumerator();
                    break;
                case 2:
                    sort = Sort.InsertionSort(arr).GetEnumerator();
                    break;
                case 3:
                    sort = Sort.MergeSort(arr).GetEnumerator();
                    break;
                case 4:
                    sort = Sort.HeapSort(arr).GetEnumerator();
                    break;
                default:
                    return;
            }
            sort.MoveNext();

            next.IsEnabled = true;
            run.IsEnabled = false;
            startPause.IsEnabled = true;

            Draw(sort.Current);
        }

        void Step() {
            if (sort.MoveNext()) {
                Draw(sort.Current);

                switch (sort.Current.type) {
                    case SortStateType.Swap:
                        SwapCount++;
                        break;
                    case SortStateType.Compare:
                        CompareCount++;
                        break;
                    case SortStateType.Sorted:
                        next.IsEnabled = false;
                        break;
                }
            } else {
                next.IsEnabled = false;
                startPause.IsEnabled = false;
                Pause();
            }
        }

        private void Next(object sender, RoutedEventArgs e) {
            Pause();
            Step();
        }

        private void Reset(object sender, RoutedEventArgs e) {
            Pause();

            ChangeArrayCount(int.Parse(arrayLength.Text));
            Shuffle(arr);
            run.IsEnabled = true;
            next.IsEnabled = false;
            startPause.IsEnabled = false;

            SwapCount = 0;
            CompareCount = 0;

            Draw(new SortState(SortStateType.None, new List<ArrayState> { new ArrayState(arr, 0) }));
        }

        private BitmapImage GetBitmapImage(Bitmap bitmap) {
            using (MemoryStream memory = new MemoryStream()) {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e) {
            image.Width = canvas.RenderSize.Width;
            image.Height = canvas.RenderSize.Height;
        }

        private void Start() {
            startPause.Content = "Pause";
            timer.Start();
        }

        private void Pause() {
            startPause.Content = "Start";
            timer.Stop();
        }

        private void StartPause(object sender, RoutedEventArgs e) {
            if (timer.IsEnabled) {
                Pause();
            } else {
                Start();
            }
        }
    }
}
