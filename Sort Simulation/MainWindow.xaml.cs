using Microsoft.Win32;
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
using System.IO;

namespace Sort_Simulation {
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window {
        Random random = new Random();

        int[] arr = { 0 };
        IEnumerator<SortState> sort;

        const int maxArrayCount = 1000000;
        int arrayCount = 10;

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

        int tickCountPerSec;
        const int maxTickCountPerSec = 1000000;

        double tickDelay;
        double tickTime;

        int TickCountPerSec {
            get => tickCountPerSec;
            set {
                tickCountPerSec = value;

                tickDelay = 65.0 / tickCountPerSec;
            }
        }

        const int ImageMaxSize = 1000;

        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow() {
            ChangeArrayCount(arrayCount);

            TickCountPerSec = 10;

            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += Tick;

            InitializeComponent();

            stepPerSec.Text = TickCountPerSec.ToString();
            arrayLength.Text = arrayCount.ToString();

            Draw(new SortState(SortStateType.None, new List<ArrayState> { new ArrayState(arr, 0) }));
        }

        private void Tick(object sender, EventArgs e) {
            if (tickDelay == 0) {
                return;
            }

            tickTime += 1;

            SortState? state = null;

            while (tickTime > tickDelay) {
                tickTime -= tickDelay;
                state = Step();
            }

            if (state != null) {
                Draw(state.Value);
            }
        }

        void ChangeArrayCount(int n) {
            if (arr.Length == n) {
                return;
            }

            arr = new int[n];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = i + 1;
            }
        }

        void Shuffle(int[] arr) {
            for (int i = 0; i < arr.Length; i++) {
                int index = random.Next(arr.Length);

                int temp = arr[index];
                arr[index] = arr[i];
                arr[i] = temp;
            }
        }

        void Draw(SortState state) {
            int size = arr.Length;
            double arrayToBitmap = 1;
            double bitmapToArray = 1;

            if (size > ImageMaxSize) {
                arrayToBitmap = (double)ImageMaxSize / size;
                bitmapToArray = (double)size / ImageMaxSize;
                size = ImageMaxSize;
            }

            Bitmap bitmap = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            using (Graphics gr = Graphics.FromImage(bitmap)) {
                gr.Clear(Color.Black);

                foreach (ArrayState arrayState in state.arrayStates) {
                    int[] array = arrayState.array;
                    int offset = arrayState.offset;

                    for (int i = 0; i < size; i++) {
                        int index = (int)(i * bitmapToArray) - offset;

                        if (index < 0 || index >= array.Length) {
                            continue;
                        }

                        Point point1 = new Point(i, (int)((arr.Length - array[index]) * arrayToBitmap));
                        Point point2 = new Point(i, size);
                        gr.DrawLine(new Pen(Color.White), point1, point2);
                    }

                    foreach (SpecialStateValue value in arrayState.specialStateValues) {
                        int x = (int)((value.index + offset) * arrayToBitmap);
                        Point point1 = new Point(x, (int)((arr.Length - array[value.index]) * arrayToBitmap));
                        Point point2 = new Point(x, size);
                        gr.DrawLine(new Pen(value.color), point1, point2);
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
                case 5:
                    sort = Sort.QuickSort(arr).GetEnumerator();
                    break;
                case 6:
                    sort = Sort.ShellSort(arr).GetEnumerator();
                    break;
                default:
                    return;
            }
            sort.MoveNext();

            next.IsEnabled = true;
            run.IsEnabled = false;
            load.IsEnabled = false;
            shuffle.IsEnabled = false;
            startPause.IsEnabled = true;

            Draw(sort.Current);
        }

        SortState Step() {
            if (sort.MoveNext()) {
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

            return sort.Current;
        }

        private void Next(object sender, RoutedEventArgs e) {
            Pause();
            SortState state = Step();
            Draw(state);
        }

        private void Reset(object sender, RoutedEventArgs e) {
            string fileName = (string)((ComboBoxItem)sortTypeBox.SelectedItem).Content + ".txt";

            string result = $"{SwapCount}\n{CompareCount}";

            File.WriteAllText(fileName, result);

            Pause();

            ChangeArrayCount(int.Parse(arrayLength.Text));

            run.IsEnabled = true;
            load.IsEnabled = true;
            shuffle.IsEnabled = true;
            next.IsEnabled = false;
            startPause.IsEnabled = false;

            SwapCount = 0;
            CompareCount = 0;

            Draw(new SortState(SortStateType.None, new List<ArrayState> { new ArrayState(arr, 0) }));
        }

        private void Load(object sender, RoutedEventArgs e) {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text File (*.txt;*.TXT)|*.txt;*.TXT";

            if (open.ShowDialog() != true) {
                return;
            }

            arr = File.ReadAllText(open.FileName).Split(' ').Select(int.Parse).ToArray();

            arrayLength.Text = arr.Length.ToString();

            Draw(new SortState(SortStateType.None, new List<ArrayState> { new ArrayState(arr, 0) }));
        }

        private void Shuffle(object sender, RoutedEventArgs e) {
            Shuffle(arr);
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

        private void stepPerSec_TextChanged(object sender, TextChangedEventArgs e) {
            if (stepPerSec.Text.Length == 0) {
                stepPerSec.Text = "1";
                stepPerSec.CaretIndex = 1;
            }
            try {
                TickCountPerSec = int.Parse(stepPerSec.Text);
                if (TickCountPerSec > maxTickCountPerSec) {
                    TickCountPerSec = maxTickCountPerSec;
                    stepPerSec.Text = maxTickCountPerSec.ToString();
                    stepPerSec.CaretIndex = stepPerSec.Text.Length;
                }
            } catch {
                int index = stepPerSec.CaretIndex - 1;

                stepPerSec.Text = TickCountPerSec.ToString();

                stepPerSec.CaretIndex = index;
            }
        }

        private void arrayLength_TextChanged(object sender, TextChangedEventArgs e) {
            if (arrayLength.Text.Length == 0) {
                arrayLength.Text = "1";
                arrayLength.CaretIndex = 1;
            }
            try {
                arrayCount = int.Parse(arrayLength.Text);
                if (arrayCount > maxArrayCount) {
                    arrayLength.Text = maxArrayCount.ToString();
                    arrayLength.CaretIndex = arrayLength.Text.Length;
                }
            } catch {
                int index = arrayLength.CaretIndex - 1;

                arrayLength.Text = arrayCount.ToString();

                arrayLength.CaretIndex = index;
            }
        }
    }
}