using HARD.Config;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace HARD.ViewModel.AutoD
{
    public class AutoD_ViewModel : INotifyPropertyChanged
    {
        #region [ Binding ]

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #region [ Control ]

        private string newContent = "New";
        public string NewContent
        {
            get { return newContent; }
            set
            {
                newContent = value;
                OnPropertyChanged(nameof(NewContent));
            }
        }

        private ObservableCollection<string> actionName;
        public ObservableCollection<string> ActionName
        {
            get { return actionName; }
            set
            {
                actionName = value;
                OnPropertyChanged(nameof(ActionName));
            }
        }

        private string selectedActionName;
        public string SelectedActionName
        {
            get { return selectedActionName; }
            set
            {
                actionList = new ObservableCollection<AutoD_Config.Action>();
                for (int i = 0; i < AutoD_Config.Instance.ConfigData.Count; i++)
                {
                    if (AutoD_Config.Instance.ConfigData[i].ActionName == value)
                    {
                        SelectedActionNameIndex = i;
                        for (int j = 0; j < AutoD_Config.Instance.ConfigData[i].Action.Count; j++)
                        {
                            var tmp = AutoD_Config.Instance.ConfigData[i].Action[j];
                            actionList.Add(new AutoD_Config.Action(tmp.X, tmp.Y, tmp.Text, tmp.Interval, tmp.Type));
                        }
                        break;
                    }
                }

                selectedActionName = value;
                OnPropertyChanged(nameof(SelectedActionName));
                OnPropertyChanged(nameof(ActionList));
            }
        }

        private ObservableCollection<AutoD_Config.Action> actionList;
        public ObservableCollection<AutoD_Config.Action> ActionList
        {
            get { return actionList; }
            set
            {
                actionList = value;
                OnPropertyChanged(nameof(ActionList));
            }
        }

        private AutoD_Config.Action selectedActionList;
        public AutoD_Config.Action SelectedActionList
        {
            get { return selectedActionList; }
            set
            {
                selectedActionList = value;
                OnPropertyChanged(nameof(SelectedActionList));
            }
        }

        private string saveFileName;
        public string SaveFileName
        {
            get { return saveFileName; }
            set
            {
                saveFileName = value;
                OnPropertyChanged(nameof(SaveFileName));
            }
        }

        #endregion

        #region [ Command ]

        private ICommand newCommand;
        public ICommand NewCommand
        {
            get
            {
                return (newCommand) ?? (newCommand = new DelegateComand(New));
            }
        }

        private ICommand startCommand;
        public ICommand StartCommand
        {
            get
            {
                return (startCommand) ?? (startCommand = new DelegateComand(Start));
            }
        }

        private ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return (deleteCommand) ?? (deleteCommand = new DelegateComand(Delete));
            }
        }

        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return (saveCommand) ?? (saveCommand = new DelegateComand(Save));
            }
        }

        private ICommand deleteActionCommand;
        public ICommand DeleteActionCommand
        {
            get
            {
                return (deleteActionCommand) ?? (deleteActionCommand = new DelegateComand(DeleteAction));
            }
        }

        #endregion

        #endregion

        #region [ Member ]

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);

        private const int MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        private const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */
        private const int MOUSEEVENTF_RIGHTUP = 0x0010; /* right button up */
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020; /* middle button down */
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040; /* middle button up */
        private const int MOUSEEVENTF_XDOWN = 0x0080; /* x button down */
        private const int MOUSEEVENTF_XUP = 0x0100; /* x button down */
        private const int MOUSEEVENTF_WHEEL = 0x0800; /* wheel button rolled */
        private const int MOUSEEVENTF_VIRTUALDESK = 0x4000; /* map to entire virtual desktop */
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000; /* absolute move */

        private SynchronizationContext context;
        private List<AutoD_Config.Action> newActionList;
        private Thread runActionThread;

        #endregion

        private int SelectedActionNameIndex;

        public AutoD_ViewModel()
        {
            context = SynchronizationContext.Current;
            newActionList = new List<AutoD_Config.Action>();

            var ActionName = AutoD_Config.Instance.ConfigData.Select(x => x.ActionName);
            this.ActionName = new ObservableCollection<string>(ActionName);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);

            return lpPoint;
        }

        public void RecoedSequence(Key key)
        {
            if (!isRecord) return;

            if (key == Key.C || key == Key.D || key == Key.R || key == Key.T)
            {
                var ct = new AutoD_Config.ClickType();
                switch (key)
                {
                    case Key.C:
                        ct = AutoD_Config.ClickType.LeftClick;
                        break;
                    case Key.D:
                        ct = AutoD_Config.ClickType.DoubleClick;
                        break;
                    case Key.R:
                        ct = AutoD_Config.ClickType.RightClick;
                        break;
                    case Key.T:
                        ct = AutoD_Config.ClickType.SendKeys;
                        break;
                }

                POINT p;
                GetCursorPos(out p);

                var X = p.X;
                var Y = p.Y;
                var Text = "";
                var Interval = 2;
                var Type = ct.ToString();

                var newAction = new AutoD_Config.Action(X, Y, Text, Interval, Type);
                newActionList.Add(newAction);

                ActionList = new ObservableCollection<AutoD_Config.Action>(newActionList);
            }
            else if (key == Key.S)
            {
                Start();
            }
            else if (key == Key.Escape)
            {
                Stop();
            }
        }

        private void RunAction()
        {
            foreach (var action in AutoD_Config.Instance.ConfigData[SelectedActionNameIndex].Action)
            {
                if (action.Type == nameof(AutoD_Config.ClickType.SendKeys))
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(WorkSendKeys), action);
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(WorkClick), action);
                }

                int tmpIntervl = action.Interval == 0 ? 0 : action.Interval * 1000 - 100;
                Thread.Sleep(tmpIntervl);
            }
        }

        private void WorkSendKeys(object state)
        {
            context.Send(new SendOrPostCallback(delegate (object _state)
            {
                var action = state as AutoD_Config.Action;
                SendKeys.SendWait(action.Text);
            }), state);
        }

        private void WorkClick(object state)
        {
            context.Send(new SendOrPostCallback(delegate (object _state)
            {
                var action = state as AutoD_Config.Action;
                SetCursorPos(action.X, action.Y);
                Thread.Sleep(100);
                if (action.Type == nameof(AutoD_Config.ClickType.LeftClick))
                {
                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                }
                else if (action.Type == nameof(AutoD_Config.ClickType.DoubleClick))
                {
                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                    Thread.Sleep(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                }
                else if (action.Type == nameof(AutoD_Config.ClickType.RightClick))
                {
                    mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                    mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                }
            }), state);
        }

        public void Stop()
        {
            NewContent = "New";
            isRecord = false;

            if (runActionThread == null) return;

            runActionThread.Abort();
        }

        public bool isRecord;
        public bool isSave;
        private void New()
        {
            isSave = false;
            newActionList = new List<AutoD_Config.Action>();

            isRecord = true;
            MessageBox.Show("Recoding 시작");
            NewContent = "Recording";
        }

        private void Start()
        {
            if (isRecord)
            {
                MessageBox.Show("Recording 중입니다");
                return;
            }
            if (runActionThread == null || !runActionThread.IsAlive)
            {
                runActionThread = new Thread(RunAction);
                runActionThread.Start();
            }
        }

        private void Delete()
        {
            if (isRecord) {
                MessageBox.Show("Recording 중입니다");
                return;
            }
            if (AutoD_Config.Instance.ConfigData.Count == 0) return;

            AutoD_Config.Instance.ConfigData.RemoveAt(SelectedActionNameIndex);

            var ActionName = AutoD_Config.Instance.ConfigData.Select(x => x.ActionName);
            this.ActionName = new ObservableCollection<string>(ActionName);
            SelectedActionName = ActionName.Last();
        }

        private void Save()
        {
            if (isRecord)
            {
                if (string.IsNullOrWhiteSpace(SaveFileName))
                {
                    MessageBox.Show("파일 이름을 입력하세요");
                    return;
                }
                if (newActionList.Count == 0)
                {
                    MessageBox.Show("저장할 리스트가 없습니다");
                    return;
                }

                var newActions = new AutoD_Config.Actions(SaveFileName, newActionList);
                AutoD_Config.Instance.ConfigData.Add(newActions);
                AutoD_Config.Instance.Save();

                var ActionName = AutoD_Config.Instance.ConfigData.Select(x => x.ActionName);
                this.ActionName = new ObservableCollection<string>(ActionName);
                SelectedActionName = ActionName.Last();
                InitData();

                NewContent = "New";
            }
            else
            {
                AutoD_Config.Instance.ConfigData[SelectedActionNameIndex].Action = ActionList.ToList();
                AutoD_Config.Instance.Save();
            }

            MessageBox.Show("저장 완료");
        }

        private void DeleteAction()
        {
            foreach (var item in ActionList)
            {
                if (item == SelectedActionList)
                {
                    ActionList.Remove(item);
                    break;
                }
            }

            ActionList = new ObservableCollection<AutoD_Config.Action>(ActionList);
        }

        private void InitData()
        {
            SaveFileName = "";
            newActionList = new List<AutoD_Config.Action>();
        }
    }
}
