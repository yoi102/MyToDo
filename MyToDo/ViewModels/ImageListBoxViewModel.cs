using OpenCvSharp;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MyToDo.Common.Models;
using OpenCvSharp.WpfExtensions;
using static System.Net.Mime.MediaTypeNames;

namespace MyToDo.ViewModels
{
    public class ImageListBoxViewModel : BindableBase
    {

        public ImageListBoxViewModel()
        {
            MyImages = new ObservableCollection<ListImage>();
            CreateImagesList();

            ImageBox = new uclImageBoxModel();

            using Mat mat = new Mat("Images/1.jpg");//MyToDo/Images/1.jpg
            //Cv2.CvtColor(mat, mat, ColorConversionCodes.RGB2GRAY);
            ImageBox.Image = mat.ToBitmapSource();
            ImageBox.Image = mat.ToWriteableBitmap();


        }







        private uclImageBoxModel _ImageBox;

        public uclImageBoxModel ImageBox
        {
            get { return _ImageBox; }
            set { _ImageBox = value; RaisePropertyChanged(); }
        }





        private ObservableCollection<ListImage> _MyImages;

        public ObservableCollection<ListImage> MyImages
        {
            get { return _MyImages; }
            set { _MyImages = value; RaisePropertyChanged(); }
        }




        void CreateImagesList()
        {
            for (int i = 0; i < 51; i++)
            {
                Mat mat = new Mat("Images/user.jpg");//MyToDo/Images/user.jpg

                Cv2.CvtColor(mat, mat, ColorConversionCodes.RGB2GRAY);
                Cv2.Threshold(mat, mat, 255 - i * 5, 255, ThresholdTypes.Binary);

                MyImages.Add(new ListImage() { MyImage = mat.ToBitmapSource() });
                mat.Dispose();
            }

        }
    





    }
    public class ListImage
    {
        public BitmapSource MyImage { get; set; }
    }


}
