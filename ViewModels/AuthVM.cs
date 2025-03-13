using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WriteErace.Models;

namespace WriteErace.ViewModels
{
    internal class AuthVM:ViewModelBase
    {
        User _currentUser;
        string _strCaptchaGenerated;
        int _countCheckData;
        DispatcherTimer timer = new DispatcherTimer();


        string _login;
        string _password;
        string _captchaStr;
        Canvas _captcha;
        List<User> users;
        bool _isVisibleCaptcha = false;
        bool _isEnabledTB = true;

        public string Login { get => _login; set => this.RaiseAndSetIfChanged(ref _login, value); }
        public string Password { get => _password; set => this.RaiseAndSetIfChanged(ref _password, value); }
        public string CaptchaStr { get => _captchaStr; set => this.RaiseAndSetIfChanged(ref _captchaStr, value); }
        public Canvas Captcha { get => _captcha; set => this.RaiseAndSetIfChanged( ref _captcha, value); }
        public List<User> Users { get => users; set => this.RaiseAndSetIfChanged(ref users, value); }
        public bool IsVisibleCaptcha { get => _isVisibleCaptcha; set => this.RaiseAndSetIfChanged(ref _isVisibleCaptcha, value); }
        public bool IsEnabledTB { get => _isEnabledTB; set => this.RaiseAndSetIfChanged(ref _isEnabledTB, value); }

        public AuthVM() {
            IsEnabledTB = true;
            IsVisibleCaptcha = false;
            Users = MainWindowViewModel.MyContext.Users.ToList();
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Tick += new EventHandler(timer_tick);
        }
        private void timer_tick(object sender, EventArgs e)
        {
            CreateCaptcha();
            IsEnabledTB = true;
            IsVisibleCaptcha = true;
            timer.Stop();
        }

        public async Task GoPageListProductsAsync() {
            
            if (CheckData()) MainWindowViewModel.Instance.PageContent = new ListProducts(_currentUser.Id);
            else await MessageBoxManager.GetMessageBoxStandard("Сообщение", "Проверьте логин или пароль", ButtonEnum.Ok).ShowAsync();
        }
        public void GoPageListProducts()
        {
           MainWindowViewModel.Instance.PageContent = new ListProducts(0);
        }


        public bool CheckData() {
            if (_countCheckData == 0) { 
                _currentUser = Users.FirstOrDefault(x => x.Password.Contains(Password) && x.Login.Contains(Login));
                if (_currentUser != null)
                {
                    return true;
                }
                else {

                    IsVisibleCaptcha = true;
                    CreateCaptcha();
                    _countCheckData++;
                    return false;
                } 
            }
            else if (_countCheckData >=1)
            {
                IsVisibleCaptcha = true;
                _currentUser = Users.FirstOrDefault(x => x.Password.Contains(Password) && x.Login.Contains(Login));
                if (_currentUser != null && CaptchaStr == _strCaptchaGenerated)
                {
                    return true;
                }
                else
                {
                    IsEnabledTB = false;
                    CreateCaptcha();
                    _countCheckData++;
                    timer.Start();
                    return false;
                }
            }
            return false;
        }

        public void CreateCaptcha() { 
            Random rnd = new Random();
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";
            _strCaptchaGenerated = "";

            Canvas cap = new Canvas() { 
                Width = 150,
                Height = 50,
                Background = new SolidColorBrush(Colors.White)
            };

            double startX = rnd.Next(5, 15);

            for (int i = 0; i < 4; i++) { 
                TextBlock symbol = new TextBlock();
                symbol.FontSize = 25;
                symbol.Foreground = new SolidColorBrush(Colors.Black);


                if (rnd.Next(10) % 2 == rnd.Next(0, 1))
                {
                    int resultNumb = rnd.Next(10);
                    symbol.Text = resultNumb.ToString();
                    _strCaptchaGenerated += Convert.ToString(resultNumb);
                }
                else
                {

                    //генерация рандомной буквы:
                    int index = rnd.Next(alphabet.Length);
                    char result = alphabet[index];
                    symbol.Text = result.ToString();
                    _strCaptchaGenerated += Convert.ToString(result);
                }

                Canvas.SetLeft(symbol, startX + (i * 35)); // Смещение по X
                Canvas.SetTop(symbol, rnd.Next(1, 20)); // Позиция по Y

                cap.Children.Add(symbol);
            }
            for (int i = 0; i < rnd.Next(10, 20); i++)
            {
                Line line = new Line()
                {
                    StartPoint = new Avalonia.Point(rnd.Next(140), rnd.Next(40)),
                    EndPoint = new Avalonia.Point(rnd.Next(140), rnd.Next(40)),
                    Stroke = new SolidColorBrush(Color.FromRgb(Convert.ToByte(rnd.Next(256)), Convert.ToByte(rnd.Next(255)), Convert.ToByte(rnd.Next(255)))),
                    StrokeThickness = rnd.Next(3),

                };
                cap.Children.Add(line);

            }
            Captcha = cap;
        }
    }
}
