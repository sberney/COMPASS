﻿using COMPASS.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static COMPASS.Tools.Enums;

namespace COMPASS.ViewModels
{
    public class FileBaseViewModel : BaseViewModel
    {
        public FileBaseViewModel(MainViewModel vm)
        {
            mainViewModel = vm;
            EditFileCommand = new BasicCommand(EditFile);
            OpenSelectedFileCommand = new RelayCommand<object>(OpenSelectedFile, CanOpenSelectedFile);
            OpenFileOnlineCommand = new RelayCommand<object>(OpenFileOnline,CanOpenFileOnline);
            DeleteFileCommand = new BasicCommand(DeleteFile);
        }

        #region Properties

        //MainViewModel
        private MainViewModel mainViewModel;
        public MainViewModel MVM
        {
            get { return mainViewModel; }
            set { SetProperty(ref mainViewModel, value); }
        }

        public ObservableCollection<MyFile> ActiveFiles
        {
            get { return MVM.FilterHandler.ActiveFiles; }
            set { }  
        }

        private MyFile selectedFile;
        public MyFile SelectedFile {
            get { return selectedFile; } 
            set { SetProperty(ref selectedFile, value); }
        }

        #endregion

        #region Functions and Commands

        //Open File Offline
        public RelayCommand<object> OpenSelectedFileCommand { get; private set; }
        public void OpenSelectedFile(object o = null)
        {
            MyFile ToOpen;
            if (o != null) ToOpen = (MyFile)o;
            else ToOpen = MVM.CurrentFileViewModel.SelectedFile;
            try
            {
                Process.Start(ToOpen.Path);
            }
            catch
            {
                MessageBox.Show("File Path Invalid");
            }
        }
        public bool CanOpenSelectedFile(object o = null)
        {
            MyFile ToOpen;
            if (o != null) ToOpen = (MyFile)o;
            else ToOpen = MVM.CurrentFileViewModel.SelectedFile;

            if (ToOpen == null) return false;
            if (ToOpen.Path == null) return false;
            if (!ToOpen.Path.Contains(".pdf")) return false;
            return true;
        }

        public RelayCommand<object> OpenFileOnlineCommand { get; private set; }
        public void OpenFileOnline(object o = null)
        {
            MyFile ToOpen;
            if (o != null) ToOpen = (MyFile)o;
            else ToOpen = MVM.CurrentFileViewModel.SelectedFile;
            try
            {
                Process.Start(ToOpen.SourceURL);
            }
            catch
            {
                MessageBox.Show("URL Invalid");
            }
            
        }
        public bool CanOpenFileOnline(object o = null)
        {
            MyFile ToOpen;
            if (o != null) ToOpen = (MyFile)o;
            else ToOpen = MVM.CurrentFileViewModel.SelectedFile;

            if (ToOpen == null) return false;
            if (ToOpen.SourceURL == null || ToOpen.SourceURL == "") return false;
            return true;
        }

        //Edit File
        public BasicCommand EditFileCommand { get; private set; }
        public void EditFile()
        {
            MVM.CurrentEditViewModel = new FileEditViewModel(MVM, MVM.CurrentFileViewModel.SelectedFile);
            FilePropWindow fpw = new FilePropWindow((FileEditViewModel)MVM.CurrentEditViewModel);
            fpw.Show();
        }

        public BasicCommand DeleteFileCommand { get; private set; }
        public void DeleteFile()
        {
            MVM.CurrentData.DeleteFile(MVM.CurrentFileViewModel.SelectedFile);
            MVM.FilterHandler.RemoveFile(MVM.CurrentFileViewModel.SelectedFile);
        }

        #endregion
    }
}
