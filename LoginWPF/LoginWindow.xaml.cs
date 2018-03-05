using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;
using System.IO;
using System.Windows.Forms;
using System.Media;
using System.Threading;
using LoginWPF.Properties;

namespace LoginWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        private static ModiffyType modiffyType = ModiffyType.BlackToReName;
        private DispatcherTimer dispatcherTimer = null;
        //一堆控制参数
        private static bool isPaperUp = true;
        private static bool isToTop = false;
        private static bool isShow = true;
        private static bool firstTurn = true;
        private static int count = 0;

        private static ModiffyType tempModiffyType = ModiffyType.BlackToReName;

        public FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

        //MediaPlayer player = new MediaPlayer();
        SoundPlayer player = null;
        
        public LoginWindow()
        {
            InitializeComponent();

            //控制页面的Timer
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(OnTimedEvent);
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,2);
            
        }

        public enum ModiffyType
        {
            BlackToReName,
            BlackToReType,
            ReNameToReType,
            ReTypeToReName,
            OnMode,
            NoPath,
            NoChoose

        }

        public enum Type
        {
            File,
            Directory,
            FilesAndDirectory,
            AddType,
        }

        public enum SoundType 
        {
            Paper,
            PaperDown,
            Shake,
            Success,
            Error
        }

        public void Play(SoundType soundType) 
        {
            switch (soundType) 
            {
                case SoundType.Paper:
                    player = new SoundPlayer (LoginWPF.Properties.Resources.Paper);//LoginWPF.Properties.Resources.Paper
                    break;
                case SoundType.PaperDown:
                    player = new SoundPlayer(LoginWPF.Properties.Resources.PaperDown);
                    break;
                case SoundType.Shake:
                    player = new SoundPlayer(LoginWPF.Properties.Resources.Shake);
                    break;
                case SoundType.Success:
                    player = new SoundPlayer(LoginWPF.Properties.Resources.Success);
                    break;
                case SoundType.Error:
                    player = new SoundPlayer(LoginWPF.Properties.Resources.Error);
                    break;
            }

            player.Play();
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            switch (modiffyType) 
            {
                case ModiffyType.BlackToReName:
                    if (isPaperUp)PaperDown();
                    else 
                    {
                        if (ShowItems(gridReName))
                        {
                            radioButtonReName.IsEnabled = true;
                            radioButtonReType.IsEnabled = true;

                            dispatcherTimer.Stop();
                        }
                    }
                    break;
                case ModiffyType.BlackToReType:
                    if (isPaperUp) PaperDown();
                    else
                    {
                        if (ShowItems(gridReType))
                        {
                            radioButtonReName.IsEnabled = true;
                            radioButtonReType.IsEnabled = true;

                            dispatcherTimer.Stop();
                        }
                    }
                    break;
                case ModiffyType.ReNameToReType:
                    if (firstTurn)
                    {
                        if (isShow)
                        {
                            if (HideItems(gridReName))
                                isShow = false;
                        }
                        else if (!isPaperUp)
                        {
                            if (PaperUp())
                            {
                                isPaperUp = true;
                                firstTurn = false;
                            }
                        }
                    }
                    else 
                    {
                        if (isPaperUp)
                        {
                            if (PaperDown())
                                isPaperUp = false;
                        }
                        else 
                        { 
                            if(ShowItems(gridReType))
                            {
                                isShow = true;
                                firstTurn = true;

                                radioButtonReName.IsEnabled = true;
                                radioButtonReType.IsEnabled = true;

                                dispatcherTimer.Stop();
                            }
                        }
                    }

                        break;
                case ModiffyType.ReTypeToReName:
                        if (firstTurn)
                        {
                            if (isShow)
                            {
                                if (HideItems(gridReType))
                                    isShow = false;
                            }
                            else if (!isPaperUp)
                            {
                                if (PaperUp())
                                {
                                    isPaperUp = true;
                                    firstTurn = false;
                                }
                            }
                        }
                        else
                        {
                            if (isPaperUp)
                            {
                                if (PaperDown())
                                    isPaperUp = false;
                            }
                            else
                            {
                                if (ShowItems(gridReName))
                                {
                                    isShow = true;
                                    firstTurn = true;

                                    radioButtonReName.IsEnabled = true;
                                    radioButtonReType.IsEnabled = true;

                                    dispatcherTimer.Stop();
                                }
                            }
                        }
                    break;
                case ModiffyType.OnMode:
                    if (gridRadio.Margin == new Thickness(120, 101, 120, 0)) gridRadio.Margin = new Thickness(110, 101, 130, 0);
                    else if (gridRadio.Margin == new Thickness(110, 101, 130, 0)) gridRadio.Margin = new Thickness(121, 101, 119, 0);
                    else if (gridRadio.Margin == new Thickness(121, 101, 119, 0)) gridRadio.Margin = new Thickness(130, 101, 110, 0);
                    else if (gridRadio.Margin == new Thickness(130, 101, 110, 0)) gridRadio.Margin = new Thickness(120, 101, 120, 0);
                    if (count++ == 18)
                    {
                        count = 0;
                        gridRadio.Margin = new Thickness(120, 101, 120, 0);
                        modiffyType = tempModiffyType;
                        dispatcherTimer.Stop();
                        radioButtonReName.IsEnabled = true;
                        radioButtonReType.IsEnabled = true;
                    }
                    
                    break;
                case ModiffyType.NoPath:
                    if (tbPath.Margin == new Thickness(75, 54, 75, 0)) tbPath.Margin = new Thickness(65, 54, 85, 0);
                    else if (tbPath.Margin == new Thickness(65, 54, 85, 0)) tbPath.Margin = new Thickness(74, 54, 76, 0);
                    else if (tbPath.Margin == new Thickness(74, 54, 76, 0)) tbPath.Margin = new Thickness(85, 54, 65, 0);
                    else if (tbPath.Margin == new Thickness(85, 54, 65, 0)) tbPath.Margin = new Thickness(75, 54, 75, 0);
                    if (count++ == 18)
                    {
                        count = 0;
                        tbPath.Margin = new Thickness(75, 54, 75, 0);
                        modiffyType = tempModiffyType;
                        dispatcherTimer.Stop();
                        btnModiffyType.IsEnabled = true;
                        btnReName.IsEnabled = true;
                    }
                    break;
                case ModiffyType.NoChoose:
                    if (gridCheckBox.Margin == new Thickness(108, 96, 108, 0)) gridCheckBox.Margin = new Thickness(98, 96, 118, 0);
                    else if (gridCheckBox.Margin == new Thickness(98, 96, 118, 0)) gridCheckBox.Margin = new Thickness(109, 96, 107, 0);
                    else if (gridCheckBox.Margin == new Thickness(109, 96, 107, 0)) gridCheckBox.Margin = new Thickness(118, 96, 98, 0);
                    else if (gridCheckBox.Margin == new Thickness(118, 96, 98, 0)) gridCheckBox.Margin = new Thickness(108, 96, 108, 0);
                    if (count++ == 18)
                    {
                        count = 0;
                        gridCheckBox.Margin = new Thickness(108, 96, 108, 0);
                        modiffyType = tempModiffyType;
                        dispatcherTimer.Stop();
                        cbFile.IsEnabled = true;
                        cbFolder.IsEnabled = true;
                        btnReName.IsEnabled = true;
                    }
                    break;
            }
        }  

        public bool PaperDown()
        {
            //放下Grid
            if (!isToTop)
            {
                //没有到达顶点
                if (gridMain.Height < 210) gridMain.Height += 15;
                else isToTop = true;
                return false;
            }
            else
            {
                if (gridMain.Height > 195) gridMain.Height -= 5;
                else
                {
                    isPaperUp = false;
                    isToTop = false;
                    return true;
                }
                return false;
            }
        }

        public bool PaperUp()
        {
            //收起Grid
            if (gridMain.Height > 0) gridMain.Height -= 15;
            else
                return true;
            return false;
        }

        public bool ShowItems(Grid grid)
        {
            if (grid.Opacity < 1)
                grid.Opacity += 0.25;
            else 
            {
                grid.Visibility = System.Windows.Visibility.Visible;
                return true;
            }
            return false;
        }

        public bool HideItems(Grid grid)
        {
            if (grid.Opacity > 0)
                grid.Opacity -= 0.25;
            else
            {
                grid.Visibility = System.Windows.Visibility.Hidden;
                return true;
            }
            return false;
        }

        public bool ReName(string strDirectory, Type type, string strBefor, string strAfter)
        {
            DirectoryInfo directory = new DirectoryInfo(strDirectory);
            DirectoryInfo[] directoryArray = null;
            FileInfo[] fileInfoArray = null;
            try
            {
                directoryArray = directory.GetDirectories();
                fileInfoArray = directory.GetFiles();

                switch (type)
                {
                    case Type.File:
                        foreach (FileInfo tempFileInfo in fileInfoArray)
                        {
                            if (tempFileInfo.Name.Contains(strBefor))
                            {
                                File.Move(tbPath.Text + "\\" + tempFileInfo.Name, tbPath.Text + "\\" + tempFileInfo.Name.Replace(strBefor, strAfter));
                            }

                        }
                        return true;
                    case Type.Directory:
                        foreach (DirectoryInfo tempDirectory in directoryArray)
                        {
                            if (tempDirectory.Name.Contains(strBefor))
                            {
                                Directory.Move(tbPath.Text + "\\" + tempDirectory.Name, tbPath.Text + "\\" + tempDirectory.Name.Replace(strBefor, strAfter));
                            }

                        }
                        return true;
                    case Type.FilesAndDirectory:
                        foreach (FileInfo tempFileInfo in fileInfoArray)
                        {
                            if (tempFileInfo.Name.Contains(strBefor))
                            {
                                File.Move(tbPath.Text + "\\" + tempFileInfo.Name, tbPath.Text + "\\" + tempFileInfo.Name.Replace(strBefor, strAfter));
                            }

                        }
                        foreach (DirectoryInfo tempDirectory in directoryArray)
                        {
                            if (tempDirectory.Name.Contains(strBefor))
                            {
                                Directory.Move(tbPath.Text + "\\" + tempDirectory.Name, tbPath.Text + "\\" + tempDirectory.Name.Replace(strBefor, strAfter));
                            }

                        }
                        return true;
                    case Type.AddType:
                        foreach (FileInfo tempFileInfo in fileInfoArray)
                        {
                            File.Move(tbPath.Text + "\\" + tempFileInfo.Name, tbPath.Text + "\\" + tempFileInfo.Name + strAfter);
                        }
                        return true;
                }
            }

            catch (Exception)
            {
                return false;
            }

            return false;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void radioButtonReName_Click(object sender, RoutedEventArgs e)
        {
            lblChoose.Content = "";
            tbReNameBefor.Text = "";
            tbReNameAfter.Text = "";
            tblReNameShow.Text = "";
            cbFile.IsChecked = false;
            cbFolder.IsChecked = false;
             
            //空白到改名
            if (isPaperUp && modiffyType == ModiffyType.BlackToReName)
            {
                Play(SoundType.PaperDown);

                radioButtonReName.IsEnabled = false;
                radioButtonReType.IsEnabled = false;

                dispatcherTimer.Start();
            }
            //已处于改名状态
            else if (!isPaperUp && (modiffyType == ModiffyType.BlackToReName || modiffyType == ModiffyType.ReTypeToReName))
            {
                Play(SoundType.Shake);
                
                tempModiffyType = ModiffyType.BlackToReName;
                modiffyType = ModiffyType.OnMode;
                dispatcherTimer.Start();
            }
            //改类型到改名
            else if (!isPaperUp && (modiffyType == ModiffyType.BlackToReType || modiffyType == ModiffyType.ReNameToReType))
            {
                Play(SoundType.Paper);

                radioButtonReName.IsEnabled = false;
                radioButtonReType.IsEnabled = false;

                modiffyType = ModiffyType.ReTypeToReName;
                dispatcherTimer.Start();
            }
        }

        private void radioButtonReType_Click(object sender, RoutedEventArgs e)
        {
            lblChoose.Content = "";
            tbReType.Text = "";
            tblReTypeShow.Text = "";

            //空白到改类型
            if (isPaperUp && modiffyType == ModiffyType.BlackToReName)
            {
                Play(SoundType.PaperDown);

                radioButtonReName.IsEnabled = false;
                radioButtonReType.IsEnabled = false;

                modiffyType = ModiffyType.BlackToReType;
                dispatcherTimer.Start();
            }
            //已处于改类型状态
            else if (!isPaperUp && (modiffyType == ModiffyType.BlackToReType || modiffyType == ModiffyType.ReNameToReType))
            {
                Play(SoundType.Shake);
                tempModiffyType = ModiffyType.BlackToReType;
                modiffyType = ModiffyType.OnMode;
                radioButtonReName.IsEnabled = false;
                radioButtonReType.IsEnabled = false;
                dispatcherTimer.Start();
            }
            //改名到改类型
            else if (!isPaperUp && (modiffyType == ModiffyType.BlackToReName || modiffyType == ModiffyType.ReTypeToReName))
            {
                Play(SoundType.Paper);

                radioButtonReName.IsEnabled = false;
                radioButtonReType.IsEnabled = false;

                modiffyType = ModiffyType.ReNameToReType;
                dispatcherTimer.Start();
            }
        }

        private void tbPath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void tbPath_GotFocus(object sender, RoutedEventArgs e)
        {
            if(tbPath.Text == "      双击选取路径。也可直接输入文件或文件夹的父级路径")
            tbPath.Text = "";
        }

        private void tbPath_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbPath.Text == "") tbPath.Text = "      双击选取路径。也可直接输入文件或文件夹的父级路径";
        }

        private void btnModiffyType_Click(object sender, RoutedEventArgs e)
        {
            if (tbPath.Text == "      双击选取路径。也可直接输入文件或文件夹的父级路径")
            {
                Play(SoundType.Shake);
                tempModiffyType = modiffyType;
                modiffyType = ModiffyType.NoPath;
                btnModiffyType.IsEnabled = false;
                dispatcherTimer.Start();
            }
            else 
            {
                if (ReName(tbPath.Text, Type.AddType, "", tbReType.Text))
                {
                    Play(SoundType.Success);
                    
                    tblReTypeShow.Text = "添加成功";
                }
                else
                {
                    Play(SoundType.Error);
                    
                    tblReTypeShow.Text = "发生错误";
                }
            }
        }

        private void tbReType_GotFocus(object sender, RoutedEventArgs e)
        {
            tblReTypeShow.Text = "";
        }

        private void btnReName_Click(object sender, RoutedEventArgs e)
        {
            if (tbPath.Text == "      双击选取路径。也可直接输入文件或文件夹的父级路径")
            {
                Play(SoundType.Shake);
                tempModiffyType = modiffyType;
                modiffyType = ModiffyType.NoPath;
                btnReName.IsEnabled = false;
                dispatcherTimer.Start();
            }
            else
            {
                Type type;
                if ((bool)cbFile.IsChecked && (bool)cbFolder.IsChecked)
                {
                    type = Type.FilesAndDirectory;
                }
                else if ((bool)cbFile.IsChecked && !(bool)cbFolder.IsChecked)
                {
                    type = Type.File;
                }
                else if (!(bool)cbFile.IsChecked && (bool)cbFolder.IsChecked)
                {
                    type = Type.Directory;
                }
                else
                {
                    Play(SoundType.Shake);
                    cbFile.IsEnabled = false;
                    cbFolder.IsEnabled = false;
                    btnReName.IsEnabled = false;
                    tempModiffyType = modiffyType;
                    modiffyType = ModiffyType.NoChoose;
                    dispatcherTimer.Start();
                    return;
                }
                if (ReName(tbPath.Text, type, tbReNameBefor.Text, tbReNameAfter.Text))
                {
                    Play(SoundType.Success);
                    tblReNameShow.Text = "修改成功";
                }
                else
                {
                    Play(SoundType.Error);
                    tblReNameShow.Text = "发生错误";
                }
            
            }
        }

        private void tbReNameBefor_GotFocus(object sender, RoutedEventArgs e)
        {
            tblReNameShow.Text = "";
        }

        private void tbReNameAfter_GotFocus(object sender, RoutedEventArgs e)
        {
            tblReNameShow.Text = "";
        }

        private void cbFile_GotFocus(object sender, RoutedEventArgs e)
        {
            tblReNameShow.Text = "";
        }

        private void cbFolder_GotFocus(object sender, RoutedEventArgs e)
        {
            tblReNameShow.Text = "";
        }

        private void imageExit_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            imageExit.Source = new BitmapImage(new Uri("Images/MouseOnExit.png",UriKind.Relative));
        }

        private void imageExit_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            imageExit.Source = new BitmapImage(new Uri("Images/Exit.png", UriKind.Relative));
        }

        private void imageExit_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void imageContact_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            imageContact.Source = new BitmapImage(new Uri("Images/MouseOnContact.png", UriKind.Relative));
        }

        private void imageContact_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            imageContact.Source = new BitmapImage(new Uri("Images/Contact.png", UriKind.Relative));
        }

        private void imageContact_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore.exe", "http://mail.qq.com/cgi-bin/qm_share?t=qm_mailme&email=guH67q-h7e3uwuTt_u-j6_6s4e3v");
        }
    }
}
