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

        public MainWindow() {
            arr = new int[10];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = i + 1;
            }

            Shuffle(arr);

            InitializeComponent();

            Draw(new SortState(SortStateType.None));
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
                for (int i = 0; i < arr.Length; i++) {
                    gr.DrawLine(new Pen(Color.White), new Point(i, arr.Length - arr[i]), new Point(i, arr.Length));
                }

                if (state.specialStateValues != null) {
                    foreach (SpecialStateValue value in state.specialStateValues) {
                        gr.DrawLine(new Pen(value.color), new Point(value.index, arr.Length - value.value), new Point(value.index, arr.Length));
                    }
                }
            }

            image.Source = GetBitmapImage(bitmap);
        }

        private void Start(object sender, RoutedEventArgs e) {
            switch (sortTypeBox.SelectedIndex) {
                case 0:
                    sort = Sort.BubbleSort(arr);
                    break;
                case 1:
                    sort = Sort.SelectionSort(arr);
                    break;
                case 2:
                    sort = Sort.InsertionSort(arr);
                    break;
                case 3:
                    sort = Sort.BubbleSort(arr);
                    break;
                default:
                    return;
            }
            sort.MoveNext();
            next.IsEnabled = true;
            start.IsEnabled = false;
            Draw(sort.Current);
        }

        private void Next(object sender, RoutedEventArgs e) {
            if (sort.MoveNext()) {
                Draw(sort.Current);
                if (sort.Current.type == SortStateType.Sorted) {
                    next.IsEnabled = false;
                }
            } else {
                next.IsEnabled = false;
            }
        }

        private void Reset(object sender, RoutedEventArgs e) {
            ChangeArrayCount(int.Parse(arrayLength.Text));
            Shuffle(arr);
            start.IsEnabled = true;
            next.IsEnabled = false;

            Draw(new SortState(SortStateType.None));
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
    }
}
