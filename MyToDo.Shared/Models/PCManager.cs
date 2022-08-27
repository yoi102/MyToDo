using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyToDo.Shared.Models
{
    public class PCManager
    {



        #region Property
        /// <summary>
        /// disk used space
        /// </summary>
        private int _UsedSpace;
        public int UsedSpace
        {
            get { return _UsedSpace; }
            set { if (_UsedSpace != value) { _UsedSpace = value; } }
        }

        /// <summary>
        /// disk total space
        /// </summary>
        private int _TotalSpace;
        public int TotalSpace
        {
            get { return _TotalSpace; }
            set { if (_TotalSpace != value) { _TotalSpace = value; } }
        }

        /// <summary>
        /// disk used space ratio
        /// </summary>
        private double _UsedSpaceRatio;
        public double UsedSpaceRatio
        {
            get { return _UsedSpaceRatio; }
            set { if (_UsedSpaceRatio != value) { _UsedSpaceRatio = value; } }
        }

        /// <summary>
        /// disk used space ratio
        /// </summary>
        private bool _DiskCleanBusy;
        public bool DiskCleanBusy
        {
            get { return _DiskCleanBusy; }
            set { if (_DiskCleanBusy != value) { _DiskCleanBusy = value; } }
        }

        /// <summary>
        /// current disk drive
        /// </summary>
        private string _CurrentDrive;
        public string CurrentDrive
        {
            get { return _CurrentDrive; }
            set { if (_CurrentDrive != value) { _CurrentDrive = value; } }
        }

        #endregion

        #region Operations
        /// <summary>
        /// get disk info
        /// </summary>
        /// <returns></returns>
        public bool GetDiskSpaceInfo()
        {
            try
            {
                var s =new ManagementClass();


                string disk = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
                CurrentDrive = disk.Substring(0, 1);
                // Create a DriveInfo instance of current drive drive
                DriveInfo dDrive = new DriveInfo(CurrentDrive);

                // When the drive is accessible..
                if (dDrive.IsReady)
                {
                    TotalSpace = (int)(dDrive.TotalSize / Math.Pow(2, 30));
                    UsedSpace = TotalSpace - (int)(dDrive.AvailableFreeSpace / Math.Pow(2, 30));
                    // Calculate the percentage free space
                    UsedSpaceRatio = UsedSpace / (double)TotalSpace;

                    // Ouput drive information
                    //Console.WriteLine("Drive: {0} ({1}, {2})",
                    //    dDrive.Name, dDrive.DriveFormat, dDrive.DriveType);

                    //Console.WriteLine("\tFree space:\t{0}",
                    //    dDrive.AvailableFreeSpace);
                    //Console.WriteLine("\tTotal space:\t{0}",
                    //    dDrive.TotalSize);

                    //Console.WriteLine("\n\tPercentage used space: {0:0.00}%.",
                    //    UsedSpaceRatio);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

 






        #endregion

    }
}


