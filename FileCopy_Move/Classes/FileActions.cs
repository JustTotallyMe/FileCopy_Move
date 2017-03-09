using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace FileCopy_Move.Classes
{
    class FileActions
    {
        string _destFolder, _destPathComp, _drivePath, _driveName;
        DirectoryInfo _sourceDirInfo;

        public FileActions()
        {
            _destFolder = ConfigurationManager.AppSettings["DestFolder"];
            _driveName = ConfigurationManager.AppSettings["DriveName"];
            _sourceDirInfo = new DirectoryInfo(ConfigurationManager.AppSettings["SourceFolder"]);
        }

        #region Public Methods
        public string RunMoveWorkflow()
        {
            try
            {
                var list = GetCreationDates_NoDoubles();
                CreateFoldersFromList(list);
                Copy_MoveFiles(true);

                return "DONE";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string RunCopyWorkflow()
        {
            try
            {
                var list = GetCreationDates_NoDoubles();
                CreateFoldersFromList(list);
                Copy_MoveFiles(false);

                return "DONE";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void SetDestPathComp(bool isDriveChecked)
        {
            if (isDriveChecked)
            {
                _destPathComp = _drivePath + _destFolder;
            }
            else
            {
                _destPathComp = _destFolder;
            }
        }

        public bool checkForSpecificDrive()
        {
            DriveInfo[] ad = DriveInfo.GetDrives();
            bool returnvalue = false;
            foreach (DriveInfo d in ad)
            {
                if (d.IsReady == true)
                {
                    if (d.VolumeLabel != null)
                    {
                        if (d.VolumeLabel == _driveName)
                        {
                            returnvalue = true;
                            _drivePath = d.Name;
                            break;
                        }
                    }
                }
            }
            return returnvalue;
        }
        #endregion

        #region Private Methods
        void Copy_MoveFiles(bool isMove)
        {
            foreach (FileInfo item in _sourceDirInfo.GetFiles())
            {
                if (isMove)
                {
                    File.Move(item.FullName, _destPathComp + "\\" + item.LastWriteTime.ToString("dd_MM_yyyy") + "\\" + item.Name);
                }
                else
                {
                    File.Copy(item.FullName, _destPathComp + "\\" + item.LastWriteTime.ToString("dd_MM_yyyy") + "\\" + item.Name);
                }
                Console.Write("=");
            }
            Console.WriteLine("");
            Console.WriteLine("Files moved to " + _destPathComp);
        }

        void CreateFoldersFromList(List<string> testing)
        {
            foreach (var item in testing)
            {
                if (!Directory.Exists($"{_destPathComp}\\{item}"))
                {
                    Directory.CreateDirectory(_destPathComp + "\\" + item);
                }
            }
        }

        List<string> GetCreationDates_NoDoubles()
        {
            List<string> testing = new List<string>();
            foreach (FileInfo item in _sourceDirInfo.GetFiles())
            {
                if (!testing.Contains(item.LastWriteTime.ToString("dd_MM_yyyy")))
                {
                    testing.Add(item.LastWriteTime.ToString("dd_MM_yyyy"));
                }
            }
            return testing;
        }
        #endregion
    }
}
